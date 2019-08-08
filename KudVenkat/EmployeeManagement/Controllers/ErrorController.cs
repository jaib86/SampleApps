using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }

        [Route("/Error/{statusCode}")]
        [AllowAnonymous]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = this.HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (statusCode)
            {
                case 404:
                    this.ViewBag.ErrorMessage = "Sorry, the resource you requested could not be found";
                    this.logger.LogWarning($"404 Error Occurred. Path = {statusCodeResult.OriginalPath} and QueryString = {statusCodeResult.OriginalQueryString}");
                    return this.View("NotFound");

            }

            return this.View();
        }

        [Route("/Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            var exceptionDetails = this.HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            this.ViewBag.ExceptionPath = exceptionDetails.Path;
            this.ViewBag.ExceptionMessage = exceptionDetails.Error.Message;
            this.ViewBag.StackTrace = exceptionDetails.Error.StackTrace;

            this.logger.LogError($"The path {exceptionDetails.Path} threw an exception {exceptionDetails.Error}");

            return this.View("Error");
        }
    }
}