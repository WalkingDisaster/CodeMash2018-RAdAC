using System;
using System.Threading.Tasks;
using LittleIdea.Radac.Policies.Requirements;
using LittleIdea.Radac.Services;
using Microsoft.AspNetCore.Authorization;

namespace LittleIdea.Radac.Policies.Handlers
{
    public class HipaaCertificateHandler : AuthorizationHandler<HipaaCertificateRequirement>
    {
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context
            , HipaaCertificateRequirement requirement
        )
        {
            await Task.Run(() =>
            {
                var now = DateTime.UtcNow;
                if (requirement.VerifyCertificate(context, now))
                    context.Succeed(requirement);
                else
                    context.Fail();
            });
        }
    }
}