using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace EmployeeManagement.Models
{
    public static class HostingEnvironmentExtensions
    {
        public static string ImagesWebRootPath(this IHostingEnvironment hostingEnvironment)
        {
            return Path.Combine(hostingEnvironment.WebRootPath, "images");
        }
    }
}