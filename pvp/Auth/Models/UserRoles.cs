namespace pvp.Auth.Models
{
    public class UserRoles
    {
        public const string Admin = nameof(Admin);
        public const string User = nameof(User);
        public const string Company = nameof(Company);
        public const string Alll = Admin + "," + User + "," + Company;
        public const string CompanyAndAdmin = Admin + "," + Company;
        public static readonly IReadOnlyCollection<string> All = new[] { Admin, User, Company };
    }
}
