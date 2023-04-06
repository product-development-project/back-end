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
using pvp.Data.Repositories;
using pvp.Data.Dto;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace pvp.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api")]
    public class AnalysisController : ControllerBase
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
        public AnalysisController(IAdRepository adRepository, ILoggedRepository loggedRepository, IResultRepository resultRepository, ISelectedTaskRepository selectedTaskRepository, ISolutionRepository solutionRepository, ITaskRepository taskRepository, ITypeRepository typeRepository, IUserInfoRepositry userInfoRepositry, IAuthorizationService authorizationService) 
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

        [HttpGet]
        [Route("Ratings")]
        //[Authorize(Roles = UserRoles.Alll)]
        public async Task<IEnumerable<RatingsDto>> GetManyRatings()
        {
            var loggedUsers = await _loggedRepository.GetManyAsync();
            var users = await _userInfoRepositry.GetManyAsync();
            var solutions = await _solutionRepository.GetManyAsync();


            var ratings = solutions.Join(loggedUsers, s => s.Prisijunge_id, p => p.Id, (s, p) => new { Solution = s, LoggedUser = p })
                       .Join(users, sp => sp.LoggedUser.UserId, u => u.Id, (sp, u) => new { sp.Solution, User = u })
                       .GroupBy(sp => sp.User.UserName)
                       .Select(g => new
                       {
                           UserName = g.Key,
                           TeisingumasTaskai = g.Sum(sp => sp.Solution.Teisingumas),
                           ProgramosLaikasTaskai = g.Sum(sp => sp.Solution.ProgramosLaikasTaskai),
                           ResursaiTaskai = g.Sum(sp => sp.Solution.ResursaiTaskai),
                           TotalPoints = g.Sum(sp => sp.Solution.Teisingumas + sp.Solution.ProgramosLaikasTaskai + sp.Solution.ResursaiTaskai)
                       })
                       .OrderByDescending(r => r.TotalPoints)
                       .ToList();


            return ratings.Select(o => new RatingsDto(o.UserName, o.TeisingumasTaskai, o.ProgramosLaikasTaskai, o.ResursaiTaskai,o.TotalPoints));
        }
        [HttpGet]
        [Route("RatingsOrderByRecources")]
        //[Authorize(Roles = UserRoles.Alll)]
        public async Task<IEnumerable<RatingsDto>> GetManyRatingsByRecourses()
        {
            var loggedUsers = await _loggedRepository.GetManyAsync();
            var users = await _userInfoRepositry.GetManyAsync();
            var solutions = await _solutionRepository.GetManyAsync();


            var ratings = solutions.Join(loggedUsers, s => s.Prisijunge_id, p => p.Id, (s, p) => new { Solution = s, LoggedUser = p })
                       .Join(users, sp => sp.LoggedUser.UserId, u => u.Id, (sp, u) => new { sp.Solution, User = u })
                       .GroupBy(sp => sp.User.UserName)
                       .Select(g => new
                       {
                           UserName = g.Key,
                           TeisingumasTaskai = g.Sum(sp => sp.Solution.Teisingumas),
                           ProgramosLaikasTaskai = g.Sum(sp => sp.Solution.ProgramosLaikasTaskai),
                           ResursaiTaskai = g.Sum(sp => sp.Solution.ResursaiTaskai),
                           TotalPoints = g.Sum(sp => sp.Solution.Teisingumas + sp.Solution.ProgramosLaikasTaskai + sp.Solution.ResursaiTaskai)
                       })
                       .OrderByDescending(r => r.ResursaiTaskai)
                       .ToList();


            return ratings.Select(o => new RatingsDto(o.UserName, o.TeisingumasTaskai, o.ProgramosLaikasTaskai, o.ResursaiTaskai, o.TotalPoints));
        }
        [HttpGet]
        [Route("RatingsOrderByTime")]
        //[Authorize(Roles = UserRoles.Alll)]
        public async Task<IEnumerable<RatingsDto>> GetManyRatingsByTime()
        {
            var loggedUsers = await _loggedRepository.GetManyAsync();
            var users = await _userInfoRepositry.GetManyAsync();
            var solutions = await _solutionRepository.GetManyAsync();


            var ratings = solutions.Join(loggedUsers, s => s.Prisijunge_id, p => p.Id, (s, p) => new { Solution = s, LoggedUser = p })
                       .Join(users, sp => sp.LoggedUser.UserId, u => u.Id, (sp, u) => new { sp.Solution, User = u })
                       .GroupBy(sp => sp.User.UserName)
                       .Select(g => new
                       {
                           UserName = g.Key,
                           TeisingumasTaskai = g.Sum(sp => sp.Solution.Teisingumas),
                           ProgramosLaikasTaskai = g.Sum(sp => sp.Solution.ProgramosLaikasTaskai),
                           ResursaiTaskai = g.Sum(sp => sp.Solution.ResursaiTaskai),
                           TotalPoints = g.Sum(sp => sp.Solution.Teisingumas + sp.Solution.ProgramosLaikasTaskai + sp.Solution.ResursaiTaskai)
                       })
                       .OrderByDescending(r => r.ProgramosLaikasTaskai)
                       .ToList();


            return ratings.Select(o => new RatingsDto(o.UserName, o.TeisingumasTaskai, o.ProgramosLaikasTaskai, o.ResursaiTaskai, o.TotalPoints));
        }
        [HttpGet]
        [Route("RatingsOrderByCorrectness")]
        //[Authorize(Roles = UserRoles.Alll)]
        public async Task<IEnumerable<RatingsDto>> GetManyRatingsByCorrectness()
        {
            var loggedUsers = await _loggedRepository.GetManyAsync();
            var users = await _userInfoRepositry.GetManyAsync();
            var solutions = await _solutionRepository.GetManyAsync();


            var ratings = solutions.Join(loggedUsers, s => s.Prisijunge_id, p => p.Id, (s, p) => new { Solution = s, LoggedUser = p })
                       .Join(users, sp => sp.LoggedUser.UserId, u => u.Id, (sp, u) => new { sp.Solution, User = u })
                       .GroupBy(sp => sp.User.UserName)
                       .Select(g => new
                       {
                           UserName = g.Key,
                           TeisingumasTaskai = g.Sum(sp => sp.Solution.Teisingumas),
                           ProgramosLaikasTaskai = g.Sum(sp => sp.Solution.ProgramosLaikasTaskai),
                           ResursaiTaskai = g.Sum(sp => sp.Solution.ResursaiTaskai),
                           TotalPoints = g.Sum(sp => sp.Solution.Teisingumas + sp.Solution.ProgramosLaikasTaskai + sp.Solution.ResursaiTaskai)
                       })
                       .OrderByDescending(r => r.TeisingumasTaskai)
                       .ToList();


            return ratings.Select(o => new RatingsDto(o.UserName, o.TeisingumasTaskai, o.ProgramosLaikasTaskai, o.ResursaiTaskai, o.TotalPoints));
        }
    }
}
