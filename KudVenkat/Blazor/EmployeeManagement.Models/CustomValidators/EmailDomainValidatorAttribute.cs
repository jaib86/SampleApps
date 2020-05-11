using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models.CustomValidators
{
    public class EmailDomainValidatorAttribute : ValidationAttribute
    {
        public string AllowedDomain { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string[] strings = value.ToString().Split('@');

            if (strings[1].ToUpper() == AllowedDomain.ToUpper()) return null;

            string errorMessage = this.ErrorMessage ?? $"Domain must be {AllowedDomain}";

            return new ValidationResult(errorMessage, new[] { validationContext.MemberName });
        }
    }
}