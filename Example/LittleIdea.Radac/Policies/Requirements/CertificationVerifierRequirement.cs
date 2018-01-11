using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using StackExchange.Redis;

namespace LittleIdea.Radac.Policies.Requirements
{
    public class CertificationVerifierRequirement : IAuthorizationRequirement
    {
        private readonly string _issuer;
        private readonly string _certificateClaim;

        public CertificationVerifierRequirement(string issuer, string certificateClaim)
        {
            _issuer = issuer;
            _certificateClaim = certificateClaim;
        }

        public bool VerifyCertificate(AuthorizationHandlerContext context, DateTime currentDateTimeUtc)
        {
            var claims = context.User.FindAll(c => c.Issuer == _issuer
                                                   && c.Type == _certificateClaim
                                                   && c.ValueType == typeof(DateTime).ToString());
            return claims.Any(c => VerifyCertificateClaim(c, currentDateTimeUtc));
        }

        private bool VerifyCertificateClaim(Claim claim, DateTime currentDateTimeUtc)
        {
            if (DateTime.TryParse(claim.Value, out var certificateDateTimeUtc))
            {
                return certificateDateTimeUtc > currentDateTimeUtc;
            }

            return false;
        }
    }
}