using Microsoft.AspNetCore.Identity;
using pvp.Auth.Models;
using pvp.Data.Auth;
using System.Data;

namespace pvp.Data
{
    public class AuthDbSeeder
    {
        private readonly UserManager<RestUsers> userManager1;
        private readonly RoleManager<IdentityRole> roleManager1;
        public AuthDbSeeder(UserManager<RestUsers> userManager, RoleManager<IdentityRole> role)
        {
            userManager1 = userManager;
            roleManager1 = role;
        }

        public async Task SeedAsync()
        {
            await AddDefaultRoles();
            await AddAdminUser();
        }
        private async Task AddAdminUser()
        {
            var admin = new RestUsers
            {
                UserName = "admin",
                Email = "admin@admin.com"
            };

            var exist = await userManager1.FindByNameAsync(admin.UserName);
            if (exist == null)
            {
                var createAdmin = await userManager1.CreateAsync(admin, "12345");
                if (createAdmin.Succeeded)
                {
                    await userManager1.AddToRolesAsync(admin, UserRoles.All);
                }
            }
        }
        private async Task AddDefaultRoles()
        {
            foreach (var item in UserRoles.All)
            {
                var roleExist = await roleManager1.RoleExistsAsync(item);
                if (!roleExist)
                {
                    await roleManager1.CreateAsync(new IdentityRole(item));
                }
            }
        }

    }
}
