namespace LittleIdea.Radac.Policies
{
    public static class Claims
    {
        public const string Employee = "https://littleidea.com/claims/user/employee";
        public const string Consultant = "https://littleidea.com/claims/user/consultant";

        public static class Certificates
        {
            public const string Hipaa = "https://littleidea.com/certificates/hipaa";
        }

        public static class Discretionary
        {
            public const string Claim = "policy://user/certifications?type=discretionary";
        }
    }
}