using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Models
{
    public static class Utility
    {
        public static string GetControllerRoutingName(string controllerClassName)
        {
            if (string.IsNullOrWhiteSpace(controllerClassName))
            {
                return string.Empty;
            }
            else
            {
                return controllerClassName.Replace(nameof(Controller), string.Empty);
            }
        }
    }
}