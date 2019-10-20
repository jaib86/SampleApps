using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EmployeeManagement.DataAccess;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<AccountController> logger;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 ILogger<AccountController> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync().ConfigureAwait(false);
            return this.RedirectToAction(nameof(HomeController.Index), Utility.GetControllerRouteName<HomeController>());
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var loginViewMode = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            return this.View(loginViewMode);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            model.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (this.ModelState.IsValid)
            {
                if (await this.userManager.FindByEmailAsync(model.Email) is var user
                    && !user.EmailConfirmed
                    && await this.userManager.CheckPasswordAsync(user, model.Password))
                {
                    this.ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                    return this.View(model);
                }

                var result = await this.signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false).ConfigureAwait(false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && this.Url.IsLocalUrl(returnUrl))
                    {
                        return this.Redirect(returnUrl);
                    }
                    else
                    {
                        return this.RedirectToAction(nameof(HomeController.Index), Utility.GetControllerRouteName<HomeController>());
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
                    var token = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = this.Url.Action(nameof(ConfirmEmail), Utility.GetControllerRouteName<AccountController>(), new { userId = user.Id, token }, this.Request.Scheme);

                    this.logger.Log(LogLevel.Warning, confirmationLink);

                    if (this.signInManager.IsSignedIn(this.User) && this.User.IsInRole("Admin") && this.User.IsInRole("User"))
                    {
                        return this.RedirectToAction(nameof(AdministrationController.ListUsers), Utility.GetControllerRouteName<AdministrationController>());
                    }
                    else
                    {
                        //await this.signInManager.SignInAsync(user, isPersistent: false).ConfigureAwait(false)
                        //return this.RedirectToAction(nameof(HomeController.Index), Utility.GetControllerRouteName<HomeController>())

                        return this.RedirectToError("Registration successful", "Before you can Login, please confirm your email, by clicking on the confirmation link we have emailed you");
                    }
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError("", error.Description);
                }
            }

            return this.View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return this.RedirectToAction(nameof(HomeController.Index), Utility.GetControllerRouteName<HomeController>());
            }
            else if (!(await this.userManager.FindByIdAsync(userId) is var user))
            {
                return this.RedirectToNotFound($"The User ID {userId} is invalid");
            }
            else if (await this.userManager.ConfirmEmailAsync(user, token) is IdentityResult result && result.Succeeded)
            {
                return this.View();
            }
            else
            {
                return this.RedirectToError("Email cannot be confirmed", string.Empty);
            }
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

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = this.Url.Action(nameof(ExternalLoginCallback), Utility.GetControllerRouteName<AccountController>(), new { ReturnUrl = returnUrl });

            var properties = this.signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");

            var loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                this.ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");

                return this.View(nameof(Login), loginViewModel);
            }
            else if (!(await this.signInManager.GetExternalLoginInfoAsync() is ExternalLoginInfo info))
            {
                this.ModelState.AddModelError(string.Empty, "Error loading external login information.");

                return this.View(nameof(Login), loginViewModel);
            }
            else if (info.Principal.FindFirstValue(ClaimTypes.Email) is string email)
            {
                var user = await this.userManager.FindByEmailAsync(email);

                if (user != null && !user.EmailConfirmed)
                {
                    this.ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                    return this.View(nameof(Login), loginViewModel);
                }
                else if (await this.signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true) is var signInResult && signInResult.Succeeded)
                {
                    return this.LocalRedirect(returnUrl);
                }

                // Create User if not already there
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                    await this.userManager.CreateAsync(user);

                    var token = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = this.Url.Action(nameof(ConfirmEmail), Utility.GetControllerRouteName<AccountController>(), new { userId = user.Id, token }, this.Request.Scheme);

                    this.logger.Log(LogLevel.Warning, confirmationLink);

                    return this.RedirectToError("Registration successful", "Before you can login, please confirm your email, by clicking on the confirmation link we have emailed you");
                }
                else
                {
                    await this.userManager.AddLoginAsync(user, info);
                    await this.signInManager.SignInAsync(user, isPersistent: false);

                    return this.LocalRedirect(returnUrl);
                }
            }
            else
            {
                return this.RedirectToError(errorTitle: $"Email claim not received from: {info.LoginProvider}",
                                            errorMessage: "Please contact support on jack@techjp.in");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this.userManager.FindByEmailAsync(model.Email);

                if (user != null && await this.userManager.IsEmailConfirmedAsync(user))
                {
                    var token = await this.userManager.GeneratePasswordResetTokenAsync(user);

                    var passwordResetLink = this.Url.Action("ResetPassword", Utility.GetControllerRouteName<AccountController>(), new { email = model.Email, token }, this.Request.Scheme);

                    this.logger.Log(LogLevel.Warning, passwordResetLink);
                }

                return this.View("ForgotPasswordConfirmation");
            }

            return this.View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string email, string token)
        {
            if (token == null || email == null)
            {
                this.ModelState.AddModelError(string.Empty, "Invalid password reset token");
            }
            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                if (await this.userManager.FindByEmailAsync(model.Email) is var user)
                {
                    var result = await this.userManager.ResetPasswordAsync(user, model.Token, model.Password);

                    if (result.Succeeded)
                    {
                        return this.View("ResetPasswordConfirmation");
                    }

                    foreach (var error in result.Errors)
                    {
                        this.ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return this.View(model);
                }

                return this.View("ResetPasswordConfirmation");
            }
            else
            {
                return this.View(model);
            }
        }
    }
}