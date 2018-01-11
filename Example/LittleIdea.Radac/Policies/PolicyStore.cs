    using System.Collections.Generic;
    using LittleIdea.Radac.Policies.Handlers;
using LittleIdea.Radac.Policies.Requirements;
using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.DependencyInjection;

namespace LittleIdea.Radac.Policies
{
    public static class PolicyStore
    {
        public const string AnyEmployee = "policy:://user?type=employee";
        public const string UnmaskPhi = "policy://certifications/phi/unmask";
        public const string DataRisk = "policy://risk/data/phi";

        public static void ConfigurePolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AnyEmployee, policy =>
                    policy.Requirements.Add(new IsEmployeeRequirement(Issuers.Self)));
                options.AddPolicy(UnmaskPhi, policy =>
                    policy.Requirements.Add(new HipaaCertificateRequirement(Issuers.Self)));
                options.AddPolicy(DataRisk, policy => 
                    policy.Requirements.Add(new RiskRequirement(.8)));
            });

            services.AddSingleton<IAuthorizationHandler, IsEmployeeHandler>();
            services.AddSingleton<IAuthorizationHandler, HipaaCertificateHandler>();
            services.AddSingleton<IAuthorizationHandler, RiskHandler>();
        }

        public static (string mandatory, string discretionary) GetAuthorizationScheme(RouteData routeData)
        {
            var routes = new Dictionary<(string controller, string route), (string mandatory, string discretionary)>
            {
                {("Codemash", "Unmask"), ("View::ePHI", "Patient Appointment")}
            };
            var key = (routeData.Values["controller"].ToString(), routeData.Values["action"].ToString());
            if (routes.ContainsKey(key))
            {
                return routes[key];
            }

            return (null, null);
        }
    }
}