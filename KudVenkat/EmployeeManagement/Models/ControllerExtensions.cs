using EmployeeManagement.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Models
{
    public static class ControllerExtensions
    {
        public static IActionResult RedirectToError(this Controller controller, string errorTitle, string errorMessage)
        {
            controller.ViewBag.ErrorTitle = errorTitle;
            controller.ViewBag.ErrorMessage = errorMessage;
            return controller.View(nameof(ErrorController.Error));
        }

        public static IActionResult RedirectToNotFound(this Controller controller, string errorMessage)
        {
            controller.ViewBag.ErrorMessage = errorMessage;
            return controller.View("NotFound");
        }
    }
}