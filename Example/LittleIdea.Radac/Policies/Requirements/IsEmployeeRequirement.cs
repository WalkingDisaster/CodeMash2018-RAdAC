namespace LittleIdea.Radac.Policies.Requirements
{
    public class IsEmployeeRequirement : ClaimVerifierRequirement
    {
        public IsEmployeeRequirement(string issuer) : base(issuer, Claims.Employee) {}
    }
}