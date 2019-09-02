using System.Collections.Generic;

namespace EmployeeManagement.ViewModels
{
    public class UserClaimsViewModel
    {
        public string UserId { get; set; }
        public List<UserClaim> Claims { get; set; } = new List<UserClaim>();
    }
}