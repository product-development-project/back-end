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
    [Route("api/Task")]
    public class TaskController :ControllerBase
    {
        private readonly ITaskRepository _taskRepository; 
        private readonly IAuthorizationService _authorizationService;
        public TaskController(ITaskRepository taskRepository, IAuthorizationService authorizationService)
        {
            _taskRepository = taskRepository;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.CompanyAndAdmin)]
        public async Task<ActionResult<TaskDto>> Create(CreateTaskDto createTaskDto) 
        {
            var task = new Uzduotys
            {
                Pavadinimas = createTaskDto.Name,
                Problema = createTaskDto.Problem,
                Sudetingumas = createTaskDto.Difficulty,
                Patvirtinta = createTaskDto.Confirmed,
                Mokomoji = createTaskDto.Educational,
                Data = createTaskDto.Date,
                Tipas_id = createTaskDto.Type_id,
                UserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
            };
            await _taskRepository.CreateAsync(task);
            return Created("", new TaskDto(task.id, task.Pavadinimas, task.Problema, task.Sudetingumas, task.Patvirtinta, task.Mokomoji, task.Data, task.Tipas_id));
        }
        [HttpGet]
        //[Authorize(Roles = UserRoles.Alll)]
        public async Task<IEnumerable<TaskDto>> GetMany()
        {
            var tasks = await _taskRepository.GetManyAsync();
            return tasks.Select(o => new TaskDto(o.id, o.Pavadinimas, o.Problema, o.Sudetingumas, o.Patvirtinta, o.Mokomoji, o.Data, o.Tipas_id));
        }

        [HttpGet]
        [Route("{taskId}")]
        //[Authorize(Roles = UserRoles.Alll)]
        public async Task<ActionResult<TaskDto>> Get(int taskId)
        {
            var task = await _taskRepository.GetAsync(taskId);
            if (task == null) { return NotFound(); }

            return new TaskDto(task.id, task.Pavadinimas, task.Problema, task.Sudetingumas, task.Patvirtinta, task.Mokomoji, task.Data, task.Tipas_id);
        }
        [HttpDelete]
        [Route("{taskId}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult> Remove(int taskId)
        {
            var task = await _taskRepository.GetAsync(taskId);
            if (task == null) { return NotFound();}
            await _taskRepository.DeleteAsync(task);
            return NoContent();
        }
    }
}