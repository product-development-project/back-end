using Microsoft.AspNetCore.Mvc;
using pvp.Data.Dto;
using pvp.Data.Entities;
using pvp.Data.Repositories;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Security.Claims;
using pvp.Auth.Models;

namespace pvp.Controllers
{
    [ApiController]
    [Route("api/TaskType")]
    public class TaskTypeController : ControllerBase
    {
        private readonly ITypeRepository _typeRepository;
        private readonly IAuthorizationService _authorizationService;
        public TaskTypeController(ITypeRepository taskRepository, IAuthorizationService authorizationService)
        {
            _typeRepository = taskRepository;
            _authorizationService = authorizationService;
        }
        [HttpGet]
        //[Authorize(Roles = UserRoles.Alll)]
        public async Task<IEnumerable<TypeDto>> GetMany()
        {
            var tasks = await _typeRepository.GetManyAsync();
            return tasks.Select(o => new TypeDto(o.id, o.Pavadinimas, o.Aprasymas));
        }

        [HttpGet]
        [Route("{taskId}")]
        //[Authorize(Roles = UserRoles.Alll)]
        public async Task<ActionResult<TypeDto>> Get(int taskId)
        {
            var task = await _typeRepository.GetAsync(taskId);
            if (task == null) { return NotFound(); }

            return new TypeDto(task.id, task.Pavadinimas, task.Aprasymas);
        }
    }
}
