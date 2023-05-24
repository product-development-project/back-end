using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pvp.Auth.Models;
using pvp.Data.Dto;
using pvp.Data.Entities;
using pvp.Data.Repositories;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
namespace pvp.Controllers
{
    [ApiController]
    [AllowAnonymous]
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

            var tokenHandler = new JwtSecurityTokenHandler();
            var authHeader = Request.Headers["Authorization"].ToString();
            if (authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                string name = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
                if (name != UserName)
                {
                    return Forbid();
                }

            }
            if (user == null) { return NotFound(); }
            
            return new UserInfoDto(user.UserName, user.Email, user.PhoneNumber);
        }

        [HttpPut]
        [Route("{UserName}")]
        [Authorize(Roles = UserRoles.Alll)]
        public async Task<ActionResult<UserInfoDto>> Update(string UserName, UpdateUserInfoDto updateUserInfoDto)
        {
            var user = await _userInfoRepositry.GetAsync(UserName);
            if (user == null) { return NotFound(); }

            var tokenHandler = new JwtSecurityTokenHandler();
            var authHeader = Request.Headers["Authorization"].ToString();
            if (authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                string name = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
                if (name != UserName)
                {
                    return Forbid();
                }

            }
            if (user == null) { return NotFound(); }

            user.UserName = updateUserInfoDto.Name;
            user.Email = updateUserInfoDto.Email;
            user.PhoneNumber = updateUserInfoDto.PhoneNumber;

            await _userInfoRepositry.UpdateAsync(user);
            return Ok(new UserInfoDto(user.UserName, user.Email, user.PhoneNumber));
        }

        [HttpDelete]
        [Route("{UserName}")]
        [Authorize(Roles = UserRoles.Alll)]
        public async Task<ActionResult<UserInfoDto>> Delete(string UserName)
        {
            var user = await _userInfoRepositry.GetAsync(UserName);
            if (user == null) { return NotFound(); }
            var tokenHandler = new JwtSecurityTokenHandler();
            var authHeader = Request.Headers["Authorization"].ToString();
            if (authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                string name = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
                if (name != UserName)
                {
                    return Forbid();
                }

            }
            await _userInfoRepositry.DeleteAsync(user);
            return NoContent();
        }

    }
}
