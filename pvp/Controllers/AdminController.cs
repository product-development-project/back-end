using pvp.Data.Entities;
using pvp.Data.Repositories;
using System.Data;
using pvp.Auth;
using pvp.Auth.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pvp.Data.Dto;
using pvp.Data.Auth;
using Microsoft.AspNetCore.Identity;

namespace pvp.Controllers
{
    [ApiController]
    [Route("api/Admin")]
    public class AdminController : ControllerBase
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
        private readonly UserManager<RestUsers> _useUserManager;
        private readonly IHelpRepository _helpRepository;
        public AdminController(IAdRepository adRepository, ILoggedRepository loggedRepository, IResultRepository resultRepository, ISelectedTaskRepository selectedTaskRepository, ISolutionRepository solutionRepository, ITaskRepository taskRepository, ITypeRepository typeRepository, IUserInfoRepositry userInfoRepositry, IAuthorizationService authorizationService, UserManager<RestUsers> useUserManager, IHelpRepository helpRepository)
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
            _useUserManager = useUserManager;
            _helpRepository = helpRepository;
        }

        [HttpGet]
        [Route("Company")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IEnumerable<UserInfoDto>> GetManyApproveCompany()
        {
            var users = await _useUserManager.GetUsersInRoleAsync(UserRoles.User);
            return users.Select(o => new UserInfoDto(o.UserName, o.Email, o.PhoneNumber));
        }

        [HttpGet]
        [Route("Task")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IEnumerable<TaskDto>> GetManyApproveTask()
        {
            var tasks = await _taskRepository.GetManyAsync();
            tasks = tasks.Where(o => o.Patvirtinta == false).ToList();
            return tasks.Select(o => new TaskDto(o.id, o.Pavadinimas, o.Problema, o.Sudetingumas, o.Patvirtinta, o.Mokomoji, o.Data, o.Tipas_id));
        }

        //[HttpGet]
        //[Route("Ads")]
        //[Authorize(Roles = UserRoles.Admin)]
        //public async Task<IEnumerable<AdDto>> GetManyApproveAds()
        //{

        //}

        [HttpGet]
        [Route("Help")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IEnumerable<HelpDto>> GetManyHelpSection()
        {
            var helps = await _helpRepository.GetManyAsync();
            helps = helps.Where(o => o.Solved == false).ToList();

            return helps.Select(o => new HelpDto(o.Id, o.Name, o.Description, o.Phone, o.EmailAddress, o.Country, o.Solved));
        }

        //[HttpGet]
        //[Route("Ad/{id}")]
        //[Authorize(Roles = UserRoles.Admin)]
        //public async Task<ActionResult<AdDto>> GetAdApprove(int adId)
        //{
        //}

        [HttpGet]
        [Route("Help/{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<HelpDto>> GetHelpSection(int id)
        {
            var help = await _helpRepository.GetAsync(id);
            if (help == null)
            {
                return NotFound();
            }
            return new HelpDto(help.Id, help.Name, help.Description, help.Phone, help.EmailAddress, help.Country, help.Solved);
        }


        [HttpPut]
        [Route("Task/{Id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<TaskDto>> UpdateTask(int Id)
        {
            var task = await _taskRepository.GetAsync(Id);
            if (task == null) { return NotFound(); }

            task.Patvirtinta = true;
            await _taskRepository.UpdateAsync(task);
            return new TaskDto(task.id, task.Pavadinimas, task.Problema, task.Sudetingumas, task.Patvirtinta, task.Mokomoji, task.Data, task.Tipas_id);
        }

        [HttpPut]
        [Route("Help/{Id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<HelpDto>> UpdateHelp(int Id)
        {
            var help = await _helpRepository.GetAsync(Id);
            if (help == null)
            {
                return NotFound();
            }

            help.Solved = true;
            await _helpRepository.UpdateAsync(help);
            return new HelpDto(help.Id, help.Name, help.Description, help.Phone, help.EmailAddress, help.Country, help.Solved);
        }
    }
}
