using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AsyncBooks.API
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // Throttle the thread pool (set available threads to amount of processors
            // WebSurge to test load
            //System.ThreadingThreadPool.SetMaxThreads(System.Environment.ProcessorCount, System.Environment.ProcessorCount)

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}