using System.Threading.Tasks;
using LittleIdea.Radac.Policies.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace LittleIdea.Radac.Policies.Handlers
{
    public class IsEmployeeHandler : AuthorizationHandler<IsEmployeeRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsEmployeeRequirement requirement)
        {
            await Task.Run(() =>
            {
                if (requirement.VerifyClaim(context))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            });
        }
    }
}