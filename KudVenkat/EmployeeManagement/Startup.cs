using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement
{
    public class Startup
    {
        private IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (context, next) =>
            {
                logger.LogInformation("MW1: Incoming Request " + DateTime.Now);
                await next();
                logger.LogInformation("MW1: Outgoing Response " + DateTime.Now);
            });

            app.Use(async (context, next) =>
            {
                logger.LogInformation("MW2: Incoming Request " + DateTime.Now);
                await next();
                logger.LogInformation("MW2: Outgoing Response " + DateTime.Now);
            });

            // Second middleware will not execute, first middleware is not allow to process next middleware in pipeline.
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("MW3: Request handled and response produced " + DateTime.Now);
                logger.LogInformation("MW3: Request handled and response produced " + DateTime.Now);
            });
        }
    }
}
