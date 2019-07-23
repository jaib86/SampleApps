using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EmployeeManagement.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate nextDelegate;

        public LoggingMiddleware(RequestDelegate next)
        {
            this.nextDelegate = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var requestInfo = $"Path: {context.Request.Path}, QueryString: {context.Request.QueryString}, Headers: {context.Request.Headers}";

            // Logic here
            Debug.WriteLine("=== Logging Before ====\t\t" + requestInfo);
            await this.nextDelegate.Invoke(context).ConfigureAwait(false);
            Debug.WriteLine("=== Logging After ====\t\t" + requestInfo);
        }
    }
}