using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TokenBasedAuthentication.Data;
using TokenBasedAuthentication.Models;

namespace TokenBasedAuthentication.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AuthController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var user = await this.userManager.FindByNameAsync(loginModel.Username);

            if (user != null && await this.userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                //var token  = JwtRegisteredClaimNames
            }

            return Unauthorized();
        }
    }
}