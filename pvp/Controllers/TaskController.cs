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
                Aprasymas = createTaskDto.Description,
                Sudetingumas = createTaskDto.Difficulty,
                Patvirtinta = createTaskDto.Confirmed,
                Mokomoji = createTaskDto.Educational,
                Data = createTaskDto.Date,
                Tipas_id = createTaskDto.Type_id,
                UserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
            };
            await _taskRepository.CreateAsync(task);
            return Created("", new TaskDto(task.id, task.Pavadinimas, task.Aprasymas, task.Sudetingumas, task.Patvirtinta, task.Mokomoji, task.Data, task.Tipas_id));
        }
    }
}