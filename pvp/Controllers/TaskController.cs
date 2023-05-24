using Microsoft.AspNetCore.Mvc;
using pvp.Data.Dto;
using pvp.Data.Entities;
using pvp.Data.Repositories;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Security.Claims;
using pvp.Auth.Models;
using MySql.Data.MySqlClient;
using System.Text;

namespace pvp.Controllers 
{
    [ApiController]
    [Route("api/Task")]
    public class TaskController :ControllerBase
    {
        private readonly IAdRepository _adRepository;
        private readonly ILoggedRepository _loggedRepository;
        private readonly IResultRepository _resultRepository;
        private readonly ISelectedTaskRepository _selectedTaskRepository;
        private readonly ISolutionRepository _solutionRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ITypeRepository _typeRepository;
        private readonly IUserInfoRepositry _userInfoRepositry;
        private readonly IAuthorizationService _authorizationService;
        public TaskController(IAdRepository adRepository, ILoggedRepository loggedRepository, IResultRepository resultRepository, ISelectedTaskRepository selectedTaskRepository, ISolutionRepository solutionRepository, ITaskRepository taskRepository, ITypeRepository typeRepository, IUserInfoRepositry userInfoRepositry, IAuthorizationService authorizationService)
        {
            _adRepository = adRepository;
            _loggedRepository = loggedRepository;
            _resultRepository = resultRepository;
            _selectedTaskRepository = selectedTaskRepository;
            _solutionRepository = solutionRepository;
            _taskRepository = taskRepository;
            _typeRepository = typeRepository;
            _userInfoRepositry = userInfoRepositry;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.CompanyAndAdmin)]
        public async Task<ActionResult<TaskDto>> Create(CreateTaskDto createTaskDto) 
        {
            var codeInBytes = Encoding.UTF8.GetBytes(createTaskDto.Problem);
            var task = new Uzduotys
            {
                Pavadinimas = createTaskDto.Name,
                Problema = codeInBytes,
                Sudetingumas = createTaskDto.Difficulty,
                Patvirtinta = createTaskDto.Confirmed,
                Mokomoji = createTaskDto.Educational,
                Data = DateTime.UtcNow,
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

        //NEKISTI METODO IÐ VISO BE MANO LEIDIMO
        //NEKISTI METODO IÐ VISO BE MANO LEIDIMO
        //NEKISTI METODO IÐ VISO BE MANO LEIDIMO
        [HttpGet]
        [Route("{difficulty}/{typeId}")]
        //[Authorize(Roles = UserRoles.Alll)]
        public async Task<IEnumerable<TaskDto>> GetManyFiltered(string difficulty, int typeId)
        {
            var tasks = await _taskRepository.GetManyAsync();
            //NEKISTI METODO IÐ VISO BE MANO LEIDIMO
            //NEKISTI METODO IÐ VISO BE MANO LEIDIMO
            if (difficulty != "Choose" && typeId == 0)
            {
                tasks = tasks.Where(o => o.Sudetingumas == difficulty).ToList();
            }
            else if (difficulty == "Choose" && typeId != 0)
            {
                tasks = tasks.Where(o => o.Tipas_id == typeId).ToList();
            }
            else if (difficulty != "Choose" && typeId != 0)
            {
                tasks = tasks.Where(o => o.Sudetingumas == difficulty && o.Tipas_id == typeId).ToList();
            }
            //NEKISTI METODO IÐ VISO BE MANO LEIDIMO
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

        [HttpGet]
        [Route("Competition/{competitionId}")]
        public async Task<IEnumerable<TaskDto>> GetManyCompetition(int competitionId)
        {
            var tasks = await _taskRepository.GetManyAsync();
            var selected = await _selectedTaskRepository.GetManyAsync();
            selected = selected.Where(o => o.Skelbimas_id == competitionId).ToList();

            var result = tasks.Join(selected, task => task.id, p => p.Uzduotys_id, (task, id) => task).ToList();
            
            return result.Select(o => new TaskDto(o.id, o.Pavadinimas, o.Problema, o.Sudetingumas, o.Patvirtinta, o.Mokomoji, o.Data, o.Tipas_id));
        }

        [HttpPost]
        [Route("AddTaskCompetition/{taskId}/{competitionId}")]
        public async Task<ActionResult<TaskDto>> AddTask(int taskId, int competitionId)
        {
            var competition = await _adRepository.GetAsync(competitionId);
            var task = await _taskRepository.GetAsync(taskId);
            if(competition == null) { return NotFound(); }
            if (task == null)
            {
                return NotFound();
            }
            var authr = await _authorizationService.AuthorizeAsync(User, competition, PolicyNames.ResourceOwner);
            if (!authr.Succeeded)
            {
                return Forbid();
            }

            var added = new ParinktosUzduotys
            {
                Skelbimas_id = competitionId,
                Uzduotys_id = taskId
            };
            await _selectedTaskRepository.CreateAsync(added);
            return Ok();
        }
    }
}