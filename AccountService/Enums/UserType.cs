namespace AccountService.Enums
{
    public static class UserType
    {
        public static string Company { get; } = "Company";
        public static string Doctor { get; } = "Doctor";
        public static string Patient { get; } = "Patient";

        public static IList<string> GetList()
        {
            return new List<string>() { Company, Doctor, Patient };
        }

        public static bool IsUserTypeValid(string userType)
        {
            return GetList().Contains(userType);
        }
    }
}
