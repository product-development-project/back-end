using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pvp.Data.Auth;
using pvp.Data;
using pvp.Auth;
using pvp.Auth.Models;

namespace pvp.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<RestUsers> _useUserManager;
        private readonly IJwtTokenService1 _jwtTokenService1;

        public AuthController(UserManager<RestUsers> useUserManager, IJwtTokenService1 jwtTokenService1)
        {
            _useUserManager = useUserManager;
            _jwtTokenService1 = jwtTokenService1;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
        {
            var user = await _useUserManager.FindByNameAsync(registerUserDto.UserName);
            if (user != null) 
            {
                return BadRequest("User already exist");
            }

            var newUser = new RestUsers
            {
                Email = registerUserDto.Email,
                UserName = registerUserDto.UserName
            };
            var createUser = await _useUserManager.CreateAsync(newUser, registerUserDto.Password);
            if (!createUser.Succeeded) 
            {
                return BadRequest("Could not create a user");
            }

            await _useUserManager.AddToRoleAsync(newUser, UserRoles.User);
            return CreatedAtAction(nameof(Register), new UserDto(newUser.Id.ToString(), newUser.UserName, newUser.Email));
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _useUserManager.FindByNameAsync(loginDto.UserName);
            if (user == null) 
            {
                return BadRequest("User name  is invalid");
            }

            var isPasswordValid = await _useUserManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
            {
                return BadRequest("User name or password is invalid");
            }
            var roles = await _useUserManager.GetRolesAsync(user);
            var accessToken = _jwtTokenService1.CreateAccessToken(user.UserName, user.Id.ToString(), roles);
            return Ok(new SuccessDto(accessToken));
        }
    }
}
