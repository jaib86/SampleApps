﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.DataAccess;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    [Authorize(Roles = "User")]
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
                    Claims = userClaims.Select(c => c.Value).ToList(),
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