using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Utilities
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class ValidEmailDomainAttribute : ValidationAttribute
    {
        private readonly string allowedDomain;

        public ValidEmailDomainAttribute(string allowedDomain)
        {
            this.allowedDomain = allowedDomain;
        }

        public override bool IsValid(object value)
        {
            string[] strings = value.ToString().Split('@');
            return string.Equals(strings[1], this.allowedDomain, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}