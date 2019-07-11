using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}