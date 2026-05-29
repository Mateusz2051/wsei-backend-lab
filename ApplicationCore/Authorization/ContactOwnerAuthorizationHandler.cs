using System.Security.Claims;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace ApplicationCore.Authorization;

public static class ContactOperations
{
    public static readonly OperationAuthorizationRequirement Create = new() { Name = nameof(Create) };
    public static readonly OperationAuthorizationRequirement Read = new() { Name = nameof(Read) };
    public static readonly OperationAuthorizationRequirement Update = new() { Name = nameof(Update) };
    public static readonly OperationAuthorizationRequirement Delete = new() { Name = nameof(Delete) };
}

public class ContactOwnerAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Contact>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        OperationAuthorizationRequirement requirement, 
        Contact resource)
    {
        if (context.User == null)
        {
            return Task.CompletedTask;
        }


        if (context.User.IsInRole(UserRole.Administrator.ToString()))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }


        if (requirement.Name == ContactOperations.Update.Name || 
            requirement.Name == ContactOperations.Delete.Name)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null && resource.OwnerId == userId)
            {
                context.Succeed(requirement);
            }
        }
        else
        {

            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
