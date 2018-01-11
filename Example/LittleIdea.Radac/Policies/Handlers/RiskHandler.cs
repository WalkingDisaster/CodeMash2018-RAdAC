using System.Security.Claims;
using System.Threading.Tasks;
using LittleIdea.Radac.Policies.Requirements;
using LittleIdea.Radac.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LittleIdea.Radac.Policies.Handlers
{
    public class RiskHandler : AuthorizationHandler<RiskRequirement>
    {
        private readonly IRiskService _riskService;

        public RiskHandler(IRiskService riskService)
        {
            _riskService = riskService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            RiskRequirement requirement
        )
        {
            if (context.Resource is AuthorizationFilterContext mvcContext)
            {
                var user = context.User.FindFirstValue("name");
                var role = context.User.FindFirstValue(ClaimTypes.Role);
                var (mandatory, discretionary) = PolicyStore.GetAuthorizationScheme(mvcContext.RouteData);
                var riskScore = await _riskService.DetermineRisk(user, role, mandatory, discretionary);
                mvcContext.HttpContext.Items["risk"] = riskScore;
                if (riskScore < requirement.MaximumRisk)
                {
                    context.Succeed(requirement);
                    return;
                }
            }
            context.Fail();
        }
    }
}