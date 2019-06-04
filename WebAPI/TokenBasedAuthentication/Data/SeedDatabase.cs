using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace TokenBasedAuthentication.Data
{
    public static class SeedDatabase
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = serviceProvider.GetRequiredService<ApplicationDbContext>())
            {
                using (var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>())
                {
                    context.Database.EnsureCreated();

                    if (!context.Users.Any())
                    {
                        var user = new ApplicationUser
                        {
                            Email = "a@b.com",
                            SecurityStamp = Guid.NewGuid().ToString(),
                            UserName = "jai"
                        };
                        var output = userManager.CreateAsync(user, "Jaiprakash@123");
                        if (output?.Result != null)
                        {

                        }
                    }
                }
            }
        }
    }
}