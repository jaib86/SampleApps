using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Models
{
    public static class Utility
    {
        public static string GetControllerRouteName<T>()
            where T : Controller
        {
            return typeof(T).Name.Replace(nameof(Controller), string.Empty);
        }
    }
}