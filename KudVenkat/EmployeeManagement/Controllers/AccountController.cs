using System.Threading.Tasks;
using EmployeeManagement.DataAccess;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync().ConfigureAwait(false);
            return this.RedirectToAction(nameof(HomeController.Index), Utility.GetControllerRoutingName(nameof(HomeController)));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (this.ModelState.IsValid)
            {
                var result = await this.signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false).ConfigureAwait(false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && this.Url.IsLocalUrl(returnUrl))
                    {
                        return this.Redirect(returnUrl);
                    }
                    else
                    {
                        return this.RedirectToAction(nameof(HomeController.Index), Utility.GetControllerRoutingName(nameof(HomeController)));
                    }
                }
                else
                {
                    this.ModelState.AddModelError("", "Invalid login attempt");
                }
            }

            return this.View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, City = model.City };
                var result = await this.userManager.CreateAsync(user, model.Password).ConfigureAwait(false);

                if (result.Succeeded)
                {
                    if (this.signInManager.IsSignedIn(this.User) && this.User.IsInRole("Admin") && this.User.IsInRole("User"))
                    {
                        return this.RedirectToAction(nameof(AdministrationController.ListUsers), Utility.GetControllerRoutingName(nameof(AdministrationController)));
                    }
                    else
                    {
                        await this.signInManager.SignInAsync(user, isPersistent: false).ConfigureAwait(false);
                        return this.RedirectToAction(nameof(HomeController.Index), Utility.GetControllerRoutingName(nameof(HomeController)));
                    }
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError("", error.Description);
                }
            }

            return this.View(model);
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await this.userManager.FindByEmailAsync(email).ConfigureAwait(false);

            if (user == null)
            {
                return this.Json(user == null);
            }
            else
            {
                return this.Json($"Email '{email}' is already taken.");
            }
        }
    }
}