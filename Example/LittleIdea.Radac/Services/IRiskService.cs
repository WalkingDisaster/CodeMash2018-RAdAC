using System;
using System.Threading.Tasks;

namespace LittleIdea.Radac.Services
{
    public interface IRiskService
    {
        Task<double> DetermineRisk(
            string user,
            string role,
            string mandatory,
            string discretionary
        );
    }
}