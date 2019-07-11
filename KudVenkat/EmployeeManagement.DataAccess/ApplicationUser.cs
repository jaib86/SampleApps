using Microsoft.AspNetCore.Identity;

namespace EmployeeManagement.DataAccess
{
    public class ApplicationUser : IdentityUser
    {
        public string City { get; set; }
    }
}