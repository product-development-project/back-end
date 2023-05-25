using Microsoft.AspNetCore.Mvc;
using pvp.Data.Dto;
using pvp.Data.Entities;
using pvp.Data.Repositories;
using pvp.Auth;
using pvp.Auth.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Identity;
using pvp.Data.Auth;

namespace pvp.Controllers
{
    [ApiController]
    [Route("api/Logged")]
    public class LoggedController : ControllerBase
    {
        private readonly ILoggedRepository _LoggedRepository;
        private readonly IAuthorizationService _AuthorizationService;
        private readonly UserManager<RestUsers> _useUserManager;
        public LoggedController(ILoggedRepository LoggedRepository, IAuthorizationService authorizationService, UserManager<RestUsers> useUserManager)
        {
            _LoggedRepository = LoggedRepository;
            _AuthorizationService= authorizationService;
            _useUserManager = useUserManager;
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.Alll)]
        public async Task<IEnumerable<LoggedDto>> GetMany()
        {
            var Loggeds = await _LoggedRepository.GetManyAsync();
            return Loggeds.Select(o => new LoggedDto(o.Id, o.Skelbimas_id, o.Uzduotys_id));
        }

        [HttpGet]
        [Route("{LoggedId}")]
        [Authorize(Roles = UserRoles.Alll)]
        public async Task<ActionResult<LoggedDto>> Get(int LoggedId)
        {
            var Logged = await _LoggedRepository.GetAsync(LoggedId);
            if (Logged == null) { return NotFound(); }

            return new LoggedDto(Logged.Id, Logged.Skelbimas_id, Logged.Uzduotys_id);
        }

        [HttpGet]
        [Route("ad/{adId}/user/{userId}")]
        public async Task<ActionResult<LoggedDto>> GetLoggedUser(string userId, int adId)
        {
            var result = await _LoggedRepository.GetAsyncByUserIdAdId(userId, adId);
            if (result == null) { return NotFound(); }
            return new LoggedDto(result.Id, result.Skelbimas_id, result.Uzduotys_id);
        }

        [HttpGet]
        [Route("user/{userName}")]
        public async Task<IEnumerable<LoggedDto>> GetUserResults(string userName)
        {
            var user = await _useUserManager.FindByNameAsync(userName);
            var result = await _LoggedRepository.GetAsyncByUserId(user.Id);
            var result2 = result.Where(x => x.Uzduotys_id != null && x.Skelbimas_id == null).ToList();

            return result2.Select(o => new LoggedDto(o.Id, o.Skelbimas_id, o.Uzduotys_id));
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.User)]
        public async Task<ActionResult<LoggedDto>> Create(CreateLoggedDto createLoggedDto)
        {
            //var Logged = await _LoggedRepository.GetAsyncByUserId(User.FindFirstValue(JwtRegisteredClaimNames.Sub));
            //if (Logged != null) { return NotFound(); }

            var newLogged = new Prisijunge
            {
                Skelbimas_id = createLoggedDto.Ad_id,
                Uzduotys_id = createLoggedDto.Task_id,
                UserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
            };

            await _LoggedRepository.CreateAsync(newLogged);
            return Created("", new LoggedDto(newLogged.Id, newLogged.Skelbimas_id, newLogged.Uzduotys_id));
        }

        //[HttpPut]
        //[Route("{LoggedId}")]
        //public async Task<ActionResult<LoggedDto>> Update(int LoggedId, UpdateLoggedDto updateLoggedDto)
        //{
        //    var Logged = await _LoggedRepository.GetAsync(LoggedId);
        //    if (Logged == null) { return NotFound(); }

        //    Logged.PavLoggedinimas = updateLoggedDto.Name;
        //    Logged.Aprasymas = updateLoggedDto.Description;
        //    Logged.PrLoggedzia = updateLoggedDto.Start;
        //    Logged.Pabaiga = updateLoggedDto.End;

        //    await _LoggedRepository.UpdateAsync(Logged);
        //    return Ok(new LoggedDto(o.Id, o.Skelbimas_id, o.Uzduotys_id));
        //}

        [HttpDelete]
        [Route("{LoggedId}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult> Remove(int LoggedId)
        {
            var Logged = await _LoggedRepository.GetAsync(LoggedId);
            if (Logged == null) { return NotFound(); }
            await _LoggedRepository.DeleteAsync(Logged);
            return NoContent();
        }
    }
}
