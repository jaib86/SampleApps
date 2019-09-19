using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EmployeeManagement.Security
{
    public class CanEditOnlyOtherAdminRolesAndClaimsHandler : AuthorizationHandler<ManageAdminRolesAndClaimsRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageAdminRolesAndClaimsRequirement requirement)
        {
            var authFilterContext = context.Resource as AuthorizationFilterContext;

            if (authFilterContext == null)
            {
                return Task.CompletedTask;
            }
            else
            {
                string loggedInAdminId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

                string adminIdBeingEdited = authFilterContext.HttpContext.Request.Query["userId"];

                if (context.User.IsInRole("Admin")
                    && context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true")
                    && !adminIdBeingEdited.Equals(loggedInAdminId, StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Succeed(requirement);
                }

                // If any of the handler return Fail, policy fails even if other handlers return Success.
#if ExcludedCode
                else
                {
                    context.Fail();
                }
#endif

                return Task.CompletedTask;
            }
        }
    }
}