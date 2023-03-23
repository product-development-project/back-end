using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pvp.Auth.Models;
using pvp.Data.Dto;
using pvp.Data.Repositories;
using System.Data;

namespace pvp.Controllers
{
    [ApiController]
    [Route("api/User")]
    public class AccountController : ControllerBase
    {
        private readonly IUserInfoRepositry _userInfoRepositry;
        private readonly IAuthorizationService _authorizationService;
        public AccountController(IUserInfoRepositry userInfoRepositry, IAuthorizationService authorizationService)
        {
            _userInfoRepositry = userInfoRepositry;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [Route("{UserName}")]
        [Authorize(Roles = UserRoles.Alll)]
        public async Task<ActionResult<UserInfoDto>> Get(string UserName)
        {
            var user = await _userInfoRepositry.GetAsync(UserName);
            if (User.Identity.Name != UserName)
            {
                return Forbid();
            }

            if (user == null) { return NotFound(); }

            return new UserInfoDto(user.Id, user.UserName, user.Email);
        }

        [HttpPut]
        [Route("{UserName}")]
        [Authorize(Roles = UserRoles.Alll)]
        public async Task<ActionResult<UserInfoDto>> Update(string UserName, UpdateUserInfoDto updateUserInfoDto)
        {
            var user = await _userInfoRepositry.GetAsync(UserName);
            if (user == null) { return NotFound(); }

            var authr = await _authorizationService.AuthorizeAsync(User, user, PolicyNames.ResourceOwner);
            if (!authr.Succeeded || User.Identity.Name != UserName)
            {
                return Forbid();
            }

            user.UserName = updateUserInfoDto.Name;
            user.Email = updateUserInfoDto.Email;

            await _userInfoRepositry.UpdateAsync(user);
            return Ok(new UserInfoDto(user.Id, user.UserName, user.Email));
        }

        [HttpDelete]
        [Route("{UserName}")]
        [Authorize(Roles = UserRoles.Alll)]
        public async Task<ActionResult<UserInfoDto>> Delete(string userName)
        {
            var userInfo = await _userInfoRepositry.GetAsync(userName);
            if (userInfo == null) { return NotFound(); }
            await _userInfoRepositry.DeleteAsync(userInfo);
            return NoContent();
        }

    }
}
