using System.Linq;
using AspNetCoreRateLimit;
using CacheCow.Server.Core.Mvc;
using Library.API.Entities;
using Library.API.Helpers;
using Library.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace Library.API
{
    public class Startup
    {
        private readonly string AllowSpecificOrigins = "allowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;

                // Add XML Output Formatter
                var xmlDataContractSerializerOutputFormatter = new XmlDataContractSerializerOutputFormatter();
                xmlDataContractSerializerOutputFormatter.SupportedMediaTypes.Add("application/vnd.nagarro.hateoas+xml");
                setupAction.OutputFormatters.Add(xmlDataContractSerializerOutputFormatter);

                // Add XML Input Formatter
                var xmlDataContractSerializerInputFormatter = new XmlDataContractSerializerInputFormatter(setupAction);
                xmlDataContractSerializerInputFormatter.SupportedMediaTypes.Add("application/vnd.nagarro.authorwithdateofdeath.full+xml");
                setupAction.InputFormatters.Add(new XmlDataContractSerializerInputFormatter(setupAction));

                // Custom JSON Output Formatters Media Types
                if (setupAction.OutputFormatters.OfType<JsonOutputFormatter>().FirstOrDefault() is JsonOutputFormatter jsonOutputFormatter)
                {
                    jsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.nagarro.hateoas+json");
                }

                // Custom JSON Input Formatters Media Types
                if (setupAction.InputFormatters.OfType<JsonInputFormatter>().FirstOrDefault() is JsonInputFormatter jsonInputFormatter)
                {
                    jsonInputFormatter.SupportedMediaTypes.Add("application/vnd.nagarro.author.full+json");
                    jsonInputFormatter.SupportedMediaTypes.Add("application/vnd.nagarro.authorwithdateofdeath.full+json");
                }
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            // register the DbContext on the container, getting the connection string from
            // appSettings (note: use this during development; in a production environment,
            // it's better to store the connection string in an environment variable)
            var connectionString = this.Configuration.GetConnectionString("libraryDBConnectionString");
            services.AddDbContextPool<LibraryContext>(o => o.UseSqlServer(connectionString));

            // register the repository
            services.AddScoped<ILibraryRepository, LibraryRepository>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(implementationFactory =>
            {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });

            services.AddCors(options =>
            {
                options.AddPolicy(this.AllowSpecificOrigins, builder => builder.WithOrigins("http://localhost:4200"));
            });

            services.AddTransient<IPropertyMappingService, PropertyMappingService>();
            services.AddTransient<ITypeHelperService, TypeHelperService>();

            // Configure Caching
            services.AddHttpCachingMvc();
            services.AddResponseCaching();

            // Configure Memory Cache
            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(options =>
            {
                options.GeneralRules = new System.Collections.Generic.List<RateLimitRule>
                {
                    new RateLimitRule { Endpoint = "*", Limit = 1000, Period = "1m" },
                    new RateLimitRule { Endpoint = "*", Limit = 200, Period = "10s" }
                };
            });
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, LibraryContext libraryContext, ILoggerFactory loggerFactory)
        {
#if ExcludedCode
            loggerFactory.AddProvider(new NLogLoggerProvider());
            loggerFactory.AddNLog();
#endif

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (exceptionHandlerFeature != null)
                        {
                            var logger = loggerFactory.CreateLogger("Global exception logger");
                            logger.LogError(500, exceptionHandlerFeature.Error, exceptionHandlerFeature.Error.Message);
                        }
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened, Try again later.");
                    });
                });
                app.UseHsts();
            }

            // Initialize Mapper
            AutoMapper.Mapper.Initialize(config =>
            {
                config.CreateMap<Author, Models.AuthorDto>()
                      .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                      .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.GetCurrentAge(src.DateOfDeath)));

                config.CreateMap<Book, Models.BookDto>();

                config.CreateMap<Models.AuthorForCreationDto, Author>();
                config.CreateMap<Models.AuthorForCreationWithDateOfDeatchDto, Author>();
                config.CreateMap<Models.AuthorForUpdateDto, Author>();

                config.CreateMap<Models.BookForCreationDto, Book>();
                config.CreateMap<Models.BookForUpdateDto, Book>();
                config.CreateMap<Book, Models.BookForUpdateDto>();
            });

            libraryContext.EnsureSeedDataForContext();

            // Rate Limiting Middleware - Before other Request Pipeline
            app.UseIpRateLimiting();

            // Configure CORS
            app.UseCors(this.AllowSpecificOrigins);

            // Response Caching
            app.UseResponseCaching();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}