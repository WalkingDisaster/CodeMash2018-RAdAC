using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace LittleIdea.Radac.Policies.Requirements
{
    public abstract class ClaimVerifierRequirement : IAuthorizationRequirement
    {
        private readonly string _claim;
        private readonly string _issuer;

        protected ClaimVerifierRequirement(string issuer, string claim)
        {
            (_issuer, _claim) = (issuer, claim);
        }

        public bool VerifyClaim(AuthorizationHandlerContext context)
        {
            return context.User.HasClaim(c => c.Type == _claim && c.Issuer == _issuer);
        }
    }
}