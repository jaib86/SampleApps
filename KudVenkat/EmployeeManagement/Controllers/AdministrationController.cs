using System.Threading.Tasks;
using EmployeeManagement.DataAccess;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var identityRole = new IdentityRole { Name = model.RoleName };

                var result = await this.roleManager.CreateAsync(identityRole).ConfigureAwait(false);

                if (result.Succeeded)
                {
                    return this.RedirectToAction(nameof(AdministrationController.ListRoles), Utility.GetControllerRoutingName(nameof(AdministrationController)));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        this.ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return this.View(model);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = this.roleManager.Roles;
            return this.View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await this.roleManager.FindByIdAsync(id).ConfigureAwait(false);

            if (role == null)
            {
                this.ViewBag.Error = $"Role with Id = {id} cannot be found";
                return this.View("NotFound");
            }
            else
            {
                var model = new EditRoleViewModel
                {
                    Id = role.Id,
                    RoleName = role.Name
                };

                foreach (var user in this.userManager.Users)
                {
                    if (await this.userManager.IsInRoleAsync(user, role.Name).ConfigureAwait(false))
                    {
                        model.Users.Add(user.UserName);
                    }
                }

                return this.View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await this.roleManager.FindByIdAsync(model.Id).ConfigureAwait(false);

            if (role == null)
            {
                this.ViewBag.Error = $"Role with Id = {model.Id} cannot be found";
                return this.View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;

                var result = await this.roleManager.UpdateAsync(role).ConfigureAwait(false);

                if (result.Succeeded)
                {
                    return this.RedirectToAction(nameof(AdministrationController.ListRoles));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        this.ModelState.AddModelError("", error.Description);
                    }

                    return this.View(model);
                }
            }
        }
    }
}