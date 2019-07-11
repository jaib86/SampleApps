﻿using System;
using EmployeeManagement.DataAccess;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(this.configuration.GetConnectionString("EmployeeDBConnection")));
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 2;
            }).AddEntityFrameworkStores<AppDbContext>();

            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlSerializerFormatters();
            services.AddScoped<IEmployeeRepository, SqlEmployeeRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                var developerExceptionPageOptions = new DeveloperExceptionPageOptions
                {
                    SourceCodeLineCount = 10
                };
                app.UseDeveloperExceptionPage(developerExceptionPageOptions);
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            // It will log the information in Output window,
            // Choose 'ASP.NET Core Web Server' or 'Debug' from 'Show output from:' drop down
            logger.LogWarning($"MyKey value is: {this.configuration["MyKey"]}");
            logger.LogWarning($"Environment: {env.EnvironmentName}{Environment.NewLine}WebRootPath: {env.WebRootPath}{Environment.NewLine}ContentRootPath: {env.ContentRootPath}{Environment.NewLine}ApplicationName: {env.ApplicationName}{Environment.NewLine}ContentRootFileProvider: {env.ContentRootFileProvider}{Environment.NewLine}WebRootFileProvider: {env.WebRootFileProvider}");

            if (env.IsDevelopment())
            {
                var defaultFilesOptions = new DefaultFilesOptions();
                defaultFilesOptions.DefaultFileNames.Clear();
                defaultFilesOptions.DefaultFileNames.Add("foo.html");
                app.UseDefaultFiles();
                app.UseStaticFiles();
            }
            else
            {
                var fileServerOptions = new FileServerOptions();
                fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
                fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("foo.html");
                app.UseFileServer();
            }

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute("Jack", "techjp/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}