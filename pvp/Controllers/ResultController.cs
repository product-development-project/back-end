using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pvp.Data.Dto;
using pvp.Data.Repositories;
using pvp.Auth.Models;
using MySqlX.XDevAPI.Common;
using pvp.Data.Entities;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace pvp.Controllers
{
    [ApiController]
    [Route("api/Task/{taskId}/Result")]
    public class ResultController : ControllerBase
    {
        private readonly IResultRepository _resultRepository;
        private readonly IAuthorizationService _authorizationService;

        public ResultController(IResultRepository resultRepository, IAuthorizationService authorizationService)
        {
            _resultRepository = resultRepository;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.Alll)]
        public async Task<IEnumerable<ResultDto>> GetManyByTask(int taskId)
        {
            var results = await _resultRepository.GetManyAsyncByTask(taskId);
            return results.Select(o => new ResultDto(o.Id, o.Duomenys, o.Rezultatas, o.Pavyzdine, o.Uzduotis_id));
        }

        [HttpGet]
        [Route("resultId")]
        [Authorize(Roles = UserRoles.Alll)]
        public async Task<ActionResult<ResultDto>> Get(int resultId)
        {
            var result = await _resultRepository.GetAsync(resultId);
            if (result == null) { return NotFound(); }

            return new ResultDto(result.Id, result.Duomenys, result.Rezultatas, result.Pavyzdine, result.Uzduotis_id);
        }

        
        [HttpPost]
        [Authorize(Roles = UserRoles.User)]
        public async Task<ActionResult<ResultDto>> Create(CreateResultDto createResultDto)
        {
            var result = new Rezultatai
            {
                Duomenys = createResultDto.Data,
                Rezultatas = createResultDto.Result,
                Pavyzdine = createResultDto.Example,
                Uzduotis_id = createResultDto.Task_id,
                UserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
            };

            await _resultRepository.CreateAsync(result);
            return Created("", new ResultDto(result.Id, result.Duomenys, result.Rezultatas, result.Pavyzdine, result.Uzduotis_id));
        }
        

        [HttpPut]
        [Route("{resultId}")]
        [Authorize(Roles = UserRoles.CompanyAndAdmin)]
        public async Task<ActionResult<AdDto>> Update(int resultId, UpdateResultDto updateResultDto)
        {
            var result = await _resultRepository.GetAsync(resultId);
            if (result == null) { return NotFound(); }
 
            var authr = await _authorizationService.AuthorizeAsync(User, result, PolicyNames.ResourceOwner);
            if (!authr.Succeeded)
            {
                return Forbid();
            }
            
            result.Duomenys = updateResultDto.Data;
            result.Rezultatas = updateResultDto.Result;
            result.Pavyzdine = updateResultDto.Example;
           
            await _resultRepository.UpdateAsync(result);
            return Ok(new ResultDto(result.Id, result.Duomenys, result.Rezultatas, result.Pavyzdine, result.Uzduotis_id));
        }

        [HttpDelete]
        [Route("{resultId}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult> Remove(int resultId)
        {
            var result = await _resultRepository.GetAsync(resultId);
            if (result == null) { return NotFound();}
            await _resultRepository.DeleteAsync(result);
            return NoContent();
        }
    }
}
