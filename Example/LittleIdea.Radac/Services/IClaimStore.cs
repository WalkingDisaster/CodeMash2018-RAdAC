using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;

namespace LittleIdea.Radac.Services
{
    public interface IClaimStore
    {
        IEnumerable<Claim> GetClaimsForUser(string userName);
    }
}