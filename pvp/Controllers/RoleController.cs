using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pvp.Auth.Models;
using pvp.Data.Auth;

namespace pvp.Controllers
{
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    [Route("api")]
    public class RoleController : ControllerBase
    {
        private readonly UserManager<RestUsers> _useUserManager;
        public RoleController(UserManager<RestUsers> useUserManager) 
        {
            _useUserManager = useUserManager;
        }
        [HttpPut]
        [Route("ChangeRoleCompany/{userName}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult> UpdateToCompany(string userName)
        {
            var user = await _useUserManager.FindByNameAsync(userName);
            if (user == null) 
            {
                return NotFound(); 
            }
            await _useUserManager.RemoveFromRoleAsync(user, UserRoles.User);
            await _useUserManager.RemoveFromRoleAsync(user, UserRoles.Admin);
            await _useUserManager.RemoveFromRoleAsync(user, UserRoles.Company);
            await _useUserManager.AddToRoleAsync(user, UserRoles.Company);
            return Ok();
        }
        [HttpPut]
        [Route("ChangeRoleUser/{userName}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult> UpdateToUser(string userName)
        {
            var user = await _useUserManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound();
            }
            await _useUserManager.RemoveFromRoleAsync(user, UserRoles.User);
            await _useUserManager.RemoveFromRoleAsync(user, UserRoles.Admin);
            await _useUserManager.RemoveFromRoleAsync(user, UserRoles.Company);
            await _useUserManager.AddToRoleAsync(user, UserRoles.User);
            return Ok();
        }
        [HttpPut]
        [Route("ChangeRoleAdmin/{userName}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult> UpdateToAdmin(string userName)
        {
            var user = await _useUserManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound();
            }
            await _useUserManager.RemoveFromRoleAsync(user, UserRoles.User);
            await _useUserManager.RemoveFromRoleAsync(user, UserRoles.Admin);
            await _useUserManager.RemoveFromRoleAsync(user, UserRoles.Company);
            await _useUserManager.AddToRoleAsync(user, UserRoles.Admin);
            return Ok();
        }
    }
}
