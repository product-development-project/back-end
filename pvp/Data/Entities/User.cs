using Microsoft.AspNetCore.Identity;

namespace pvp.Data.Entities
{
    public class User : IdentityUser
    {
        [PersonalData]
        public string? Authen { get; set; }
    }
}
