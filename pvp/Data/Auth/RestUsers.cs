using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace pvp.Data.Auth
{
    public class RestUsers : IdentityUser
    {
        [PersonalData]
        public string? Authen { get; set; }
    }
}
