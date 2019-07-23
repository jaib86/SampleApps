using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace EmployeeManagement.Utilities
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class IsMobileAttribute : Attribute, IActionConstraint
    {
        public IsMobileAttribute(int order)
        {
            this.Order = order;
        }

        public int Order { get; }

        public bool Accept(ActionConstraintContext context)
        {
            return context.RouteContext.HttpContext.Request
                   .Headers["User-Agent"].Any(x => x.Contains("Android"));
        }
    }
}