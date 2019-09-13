using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EmployeeManagement.DataAccess;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Controllers
{
    //[Authorize(Roles = "Admin")]
    //[Authorize(Roles = "User")]
    //[Authorize(Policy = "AdminRolePolicy")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<AdministrationController> logger;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ILogger<AdministrationController> logger)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return this.View();
        }

        [HttpGet]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> ManageUserClaims(string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return this.RedirectToNotFound($"User with Id = {userId} cannot be found");
            }
            else
            {
                var existingUserClaims = await this.userManager.GetClaimsAsync(user);

                var model = new UserClaimsViewModel
                {
                    UserId = userId
                };

                foreach (Claim claim in ClaimsStore.AllClaims)
                {
                    // If the user has the claim, set IsSelected property to true, so the CheckBox
                    // next to the claim is checked on the UI
                    model.Claims.Add(new UserClaim
                    {
                        ClaimType = claim.Type,
                        IsSelected = existingUserClaims.Any(c => c.Type == claim.Type && c.Value == "true")
                    });
                }

                return this.View(model);
            }
        }

        [HttpPost]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model)
        {
            var user = await this.userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                return this.NotFound($"User with Id = {model.UserId} cannot be found");
            }
            else
            {
                // Remove Existing Claims
                var claims = await this.userManager.GetClaimsAsync(user);
                var result = await this.userManager.RemoveClaimsAsync(user, claims);

                if (!result.Succeeded)
                {
                    this.ModelState.AddModelError("", "Cannot remove user's existing claims");
                    return this.View();
                }
                else
                {
                    // Add Selected Claims
                    result = await this.userManager.AddClaimsAsync(user, model.Claims.Select(c => new Claim(c.ClaimType, c.IsSelected ? "true" : "false")));

                    if (!result.Succeeded)
                    {
                        this.ModelState.AddModelError("", "Cannot add selected claims to user");
                        return this.View();
                    }
                    else
                    {
                        return this.RedirectToAction(nameof(EditUser), new { Id = model.UserId });
                    }
                }
            }
        }

        [HttpGet]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> ManagerUserRoles(string userId)
        {
            this.ViewBag.userId = userId;

            var user = await this.userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return this.RedirectToNotFound($"User with Id = {userId} cannot be found");
            }
            else
            {
                var model = new List<UserRolesViewModel>();

                foreach (var role in this.roleManager.Roles)
                {
                    model.Add(new UserRolesViewModel
                    {
                        RoleId = role.Id,
                        RoleName = role.Name,
                        IsSelected = await this.userManager.IsInRoleAsync(user, role.Name)
                    });
                }

                return this.View(model);
            }
        }

        [HttpPost]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> ManagerUserRoles(List<UserRolesViewModel> model, string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return this.RedirectToNotFound($"User with Id = {userId} cannot be found");
            }
            else
            {
                var roles = await this.userManager.GetRolesAsync(user);
                var result = await this.userManager.RemoveFromRolesAsync(user, roles);

                if (!result.Succeeded)
                {
                    this.ModelState.AddModelError("", "Cannot remove user from existing roles");
                    return this.View(model);
                }

                result = await this.userManager.AddToRolesAsync(user, model.Where(r => r.IsSelected).Select(r => r.RoleName));

                if (result.Succeeded)
                {
                    return this.RedirectToAction(nameof(EditUser), new { Id = userId });
                }
                else
                {
                    this.ModelState.AddModelError("", "Cannot add user to selected roles");
                    return this.View(model);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                this.ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return this.View("NotFound");
            }
            else
            {
                var result = await this.userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return this.RedirectToAction(nameof(ListUsers));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        this.ModelState.AddModelError("", error.Description);
                    }

                    return this.View(nameof(ListUsers));
                }
            }
        }

        [HttpPost]
        [Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await this.roleManager.FindByIdAsync(id);

            if (role == null)
            {
                this.ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return this.View("NotFound");
            }
            else
            {
                try
                {
                    var result = await this.roleManager.DeleteAsync(role);

                    if (result.Succeeded)
                    {
                        return this.RedirectToAction(nameof(ListRoles));
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            this.ModelState.AddModelError("", error.Description);
                        }

                        return this.View(nameof(ListRoles));
                    }
                }
                catch (DbUpdateException ex)
                {
                    this.logger.LogError(ex, $"Error deleting role {role.Name}: {Environment.NewLine}'{ex}'");

                    return this.RedirectToError(errorTitle: $"'{role.Name}' role is in use",
                                                errorMessage: $"'{role.Name}' role cannot be deleted as there are users in this role. If you want to delete this role, please remove the users from the role and then try to delete");
                }
            }
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            return this.View(this.userManager.Users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                this.ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return this.View("NotFound");
            }
            else
            {
                var userClaims = await this.userManager.GetClaimsAsync(user);
                var userRoles = await this.userManager.GetRolesAsync(user);

                var model = new EditUserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    City = user.City,
                    Claims = userClaims.Select(c => $"{c.Type} - {c.Value}").ToList(),
                    Roles = userRoles
                };

                return this.View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await this.userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                this.ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return this.View("NotFound");
            }
            else
            {
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.City = model.City;

                var result = await this.userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return this.RedirectToAction(nameof(ListUsers));
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
        [Authorize(Policy = "EditRolePolicy")]
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
        [Authorize(Policy = "EditRolePolicy")]
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

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            this.ViewBag.RoleId = roleId;

            var role = await this.roleManager.FindByIdAsync(roleId).ConfigureAwait(false);

            if (role == null)
            {
                this.ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return this.View("NotFound");
            }
            else
            {
                var model = new List<UserRoleViewModel>();

                foreach (var user in this.userManager.Users)
                {
                    model.Add(new UserRoleViewModel
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        IsSelected = await this.userManager.IsInRoleAsync(user, role.Name).ConfigureAwait(false)
                    });
                }

                return this.View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await this.roleManager.FindByIdAsync(roleId).ConfigureAwait(false);

            if (role == null)
            {
                this.ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return this.View("NotFound");
            }
            else
            {
                for (int i = 0; i < model.Count; i++)
                {
                    var user = await this.userManager.FindByIdAsync(model[i].UserId).ConfigureAwait(false);

                    if (model[i].IsSelected && !await this.userManager.IsInRoleAsync(user, role.Name))
                    {
                        await this.userManager.AddToRoleAsync(user, role.Name).ConfigureAwait(false);
                    }
                    else if (!model[i].IsSelected && await this.userManager.IsInRoleAsync(user, role.Name))
                    {
                        await this.userManager.RemoveFromRoleAsync(user, role.Name).ConfigureAwait(false);
                    }
                }

                return this.RedirectToAction(nameof(this.EditRole), new { Id = roleId });
            }
        }
    }
}