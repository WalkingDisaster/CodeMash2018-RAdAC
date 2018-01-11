namespace LittleIdea.Radac.Policies.Requirements
{
    public class HipaaCertificateRequirement : CertificationVerifierRequirement
    {
        public HipaaCertificateRequirement(string issuer) : base(issuer, Claims.Certificates.Hipaa)
        {
        }
    }
}