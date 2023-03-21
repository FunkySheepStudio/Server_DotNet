using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Server_Dotnet.Pages.Auth
{
    public class CustomSignalrAuthRequirement : AuthorizationHandler<CustomSignalrAuthRequirement, HubInvocationContext>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomSignalrAuthRequirement requirement, HubInvocationContext resource)
        {
            Console.WriteLine(resource.Context.GetHttpContext().Request.Query["token"]);
            //context.Fail();

            return Task.CompletedTask;
        }
    }
}
