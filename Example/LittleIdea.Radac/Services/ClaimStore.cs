using System;
using System.Collections.Generic;
using System.Security.Claims;
using LittleIdea.Radac.Policies;

namespace LittleIdea.Radac.Services
{
    public class ClaimStore : IClaimStore
    {
        public IEnumerable<Claim> GetClaimsForUser(string userName)
        {
            if (userName == "Dr. Curly Howard")
            {
                return new[]
                {
                    new Claim(ClaimTypes.Role, Roles.Doctor, typeof(string).ToString(), Issuers.Self),
                    new Claim(Claims.Employee, true.ToString(), typeof(bool).ToString(), Issuers.Self),
                    new Claim(Claims.Certificates.Hipaa, DateTime.UtcNow.AddYears(1).ToString("O"), typeof(DateTime).ToString(), Issuers.Self)
                };

            }
            if (userName == "Dr. Larry Fine")
            {
                return new[]
                {
                    new Claim(ClaimTypes.Role, Roles.Doctor, typeof(string).ToString(), Issuers.Self),
                    new Claim(Claims.Employee, true.ToString(), typeof(bool).ToString(), Issuers.Self),
                    new Claim(Claims.Certificates.Hipaa, DateTime.UtcNow.AddDays(-1).ToString("O"), typeof(DateTime).ToString(), Issuers.Self)
                };
            }
            if (userName == "Dr. Shemp Howard")
            {
                return new[]
                {
                    new Claim(ClaimTypes.Role, Roles.Doctor, typeof(string).ToString(), Issuers.Self),
                    new Claim(Claims.Consultant, true.ToString(), typeof(bool).ToString(), Issuers.Self),
                    new Claim(Claims.Certificates.Hipaa, DateTime.UtcNow.AddYears(1).ToString("O"), typeof(DateTime).ToString(), Issuers.Self)
                };
            }
            else
            {
                return new Claim[] { };
            }
        }
    }
}