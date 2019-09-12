﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NLog.Web;

namespace Library.API
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
                        WebHost.CreateDefaultBuilder(args)
                               .UseNLog()
                               .UseStartup<Startup>();
                               
    }
}