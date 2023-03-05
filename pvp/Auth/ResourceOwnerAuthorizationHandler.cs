using Microsoft.AspNetCore.Authorization;
using pvp.Auth.Models;
using System.Data;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace pvp.Auth
{
    public class ResourceOwnerAuthorizationHandler : AuthorizationHandler<ResourceOwnerRequirement, IUserOwnedResources>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOwnerRequirement requirement, IUserOwnedResources resource)
        {
            if (context.User.IsInRole(UserRoles.Admin) || context.User.FindFirstValue(JwtRegisteredClaimNames.Sub) == resource.UserId)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
    public record ResourceOwnerRequirement : IAuthorizationRequirement;
}
