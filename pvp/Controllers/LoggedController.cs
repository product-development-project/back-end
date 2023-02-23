using Microsoft.AspNetCore.Mvc;
using pvp.Data.Dto;
using pvp.Data.Entities;
using pvp.Data.Repositories;

namespace pvp.Controllers
{
    [ApiController]
    [Route("api/Logged")]
    public class LoggedController : ControllerBase
    {
        private readonly ILoggedRepository _LoggedRepository;
        public LoggedController(ILoggedRepository LoggedRepository)
        {
            _LoggedRepository = LoggedRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<LoggedDto>> GetMany()
        {
            var Loggeds = await _LoggedRepository.GetManyAsync();
            return Loggeds.Select(o => new LoggedDto(o.Id, o.Skelbimas_id, o.Uzduotys_id));
        }

        [HttpGet]
        [Route("{LoggedId}")]
        public async Task<ActionResult<LoggedDto>> Get(int LoggedId)
        {
            var Logged = await _LoggedRepository.GetAsync(LoggedId);
            if (Logged == null) { return NotFound(); }

            return new LoggedDto(Logged.Id, Logged.Skelbimas_id, Logged.Uzduotys_id);
        }

        [HttpPost]
        public async Task<ActionResult<LoggedDto>> Create(CreateLoggedDto createLoggedDto)
        {
            var Logged = new Prisijunge
            {
                Skelbimas_id = createLoggedDto.Ad_id,
                Uzduotys_id = createLoggedDto.Task_id,
                UserId = "1111"
            };
            await _LoggedRepository.CreateAsync(Logged);
            return Created("", new LoggedDto(Logged.Id, Logged.Skelbimas_id, Logged.Uzduotys_id));
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
        public async Task<ActionResult> Remove(int LoggedId)
        {
            var Logged = await _LoggedRepository.GetAsync(LoggedId);
            if (Logged == null) { return NotFound(); }
            await _LoggedRepository.DeleteAsync(Logged);
            return NoContent();
        }
    }
}
