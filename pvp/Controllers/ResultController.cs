using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pvp.Data.Dto;
using pvp.Data.Repositories;
using pvp.Auth.Models;
using MySqlX.XDevAPI.Common;
using pvp.Data.Entities;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.Identity;
using pvp.Data.Auth;

namespace pvp.Controllers
{
    [ApiController]
    [Route("api/Task/{taskId}/Result")]
    public class ResultController : ControllerBase
    {
        private readonly IResultRepository _resultRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly ITaskRepository _taskRepository;

        public ResultController(IResultRepository resultRepository, IAuthorizationService authorizationService, ITaskRepository taskRepository)
        {
            _resultRepository = resultRepository;
            _authorizationService = authorizationService;
            _taskRepository = taskRepository;
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
        [Authorize(Roles = UserRoles.Company)]
        [Route("Company")]
        public async Task<ActionResult<ResultDto>> CreateCompany(CreateResultDto createResultDto, int taskId)
        {
            var task = await _taskRepository.GetAsync(taskId);
            var authr = await _authorizationService.AuthorizeAsync(User, task, PolicyNames.ResourceOwner);
            if (!authr.Succeeded)
            {
                return Forbid();
            }

            var result = new Rezultatai
            {
                Duomenys = createResultDto.Data,
                Rezultatas = createResultDto.Result,
                Pavyzdine = createResultDto.Example,
                Uzduotis_id = taskId,
                UserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
            };

            await _resultRepository.CreateAsync(result);
            return Created("", new ResultDto(result.Id, result.Duomenys, result.Rezultatas, result.Pavyzdine, result.Uzduotis_id));
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("Admin")]
        public async Task<ActionResult<ResultDto>> CreateAdmin(CreateResultDto createResultDto, int taskId)
        {

            var result = new Rezultatai
            {
                Duomenys = createResultDto.Data,
                Rezultatas = createResultDto.Result,
                Pavyzdine = createResultDto.Example,
                Uzduotis_id = taskId,
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
            if (result == null) { return NotFound(); }
            await _resultRepository.DeleteAsync(result);
            return NoContent();
        }

        [HttpDelete]
        [Route("{resultId}/Company")]
        [Authorize(Roles = UserRoles.Company)]
        public async Task<ActionResult> RemoveCompany(int resultId)
        {
            var result = await _resultRepository.GetAsync(resultId);
            var authr = await _authorizationService.AuthorizeAsync(User, result, PolicyNames.ResourceOwner);
            if (!authr.Succeeded)
            {
                return Forbid();
            }
            if (result == null) { return NotFound(); }
            await _resultRepository.DeleteAsync(result);
            return NoContent();
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.Company)]
        [Route("GetManyByTaskForCompany")]
        public async Task<ActionResult<IEnumerable<ResultDto>>> GetManyByTaskForCompany(int taskId)
        {
            var task = await _taskRepository.GetAsync(taskId);
            var authr = await _authorizationService.AuthorizeAsync(User, task, PolicyNames.ResourceOwner);
            if (!authr.Succeeded)
            {
                return Forbid();
            }
            var results = await _resultRepository.GetManyAsyncByTask(taskId);
            return Ok(results.Select(o => new ResultDto(o.Id, o.Duomenys, o.Rezultatas, o.Pavyzdine, o.Uzduotis_id)));
        }
    }
}
