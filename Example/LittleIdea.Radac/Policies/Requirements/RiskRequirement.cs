using Microsoft.AspNetCore.Authorization;

namespace LittleIdea.Radac.Policies.Requirements
{
    public class RiskRequirement : IAuthorizationRequirement
    {
        public double MaximumRisk { get; }

        public RiskRequirement(double maximumRisk)
        {
            MaximumRisk = maximumRisk;
        }
    }
}