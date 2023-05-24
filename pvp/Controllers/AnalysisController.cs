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
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using pvp.Data.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

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
        [HttpGet]
        [Route("TaskCount/{UserName}")]
        public async Task<ActionResult<TaskCountDto>> GetCount(string UserName)
        {
            var user = await _userInfoRepositry.GetAsync(UserName);
            if (user == null) { return NotFound(); }
            var solutions = await _solutionRepository.GetManyAsync();
            var loggedUsers = await _loggedRepository.GetManyAsync();
            var task = await _taskRepository.GetManyAsync();
            loggedUsers = loggedUsers.Where(lu => lu.UserId == user.Id).ToList();

            solutions = solutions.Join(loggedUsers, s => s.Prisijunge_id, p => p.Id, (s, p) => new { Solution = s, LoggedUser = p })
                .Select(x => x.Solution).Where(x => x.Teisingumas >= 80).ToList();
            loggedUsers = solutions.Join(loggedUsers, s => s.Prisijunge_id, p => p.Id, (s, p) => new { Solution = s, LoggedUser = p })
                .Select(x => x.LoggedUser).ToList();
            var count = task.Join(loggedUsers, s=> s.id, p => p.Uzduotys_id, (s, p) => new { task = s, LoggedUser = p})
                .Select(x => x.task).Where(x => x.Mokomoji == true).Count();
            

            return new TaskCountDto(count);

        }
        [HttpGet]
        [Route("TaskAnalisisByUser/{taskId}")]
        public async Task<ActionResult<TaskAnalysisDto>> GetTaskAnalysis(int taskId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var authHeader = Request.Headers["Authorization"].ToString();
            if (!authHeader.StartsWith("Bearer "))
            {
                return Forbid();
            }
            var token = authHeader.Substring("Bearer ".Length).Trim();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            string name = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
            var user = await _userInfoRepositry.GetAsync(name);
            if (user == null) { return NotFound(); }
            var solutions = await _solutionRepository.GetManyAsync();
            var loggedUsers = await _loggedRepository.GetManyAsync();
            var task = await _taskRepository.GetAsync(taskId);
            if (task == null){return NotFound();}

            loggedUsers = loggedUsers.Where(x => x.Uzduotys_id == task.id).ToList();
            var loggedUser = loggedUsers.FirstOrDefault(x => x.UserId == user.Id);
            if (loggedUser == null)
            {
                return NotFound();
            }
            var usersSolution = solutions.FirstOrDefault(x => x.Prisijunge_id == loggedUser.Id);
            if (usersSolution == null)
            {
                return NotFound();
            }

            solutions = solutions.Join(loggedUsers, s => s.Prisijunge_id, p => p.Id, (s, p) => new { Solution = s, LoggedUser = p })
                .Select(x => x.Solution).ToList();

            var Teisingumas = 0;
            var laikastaskai = 0;
            var resursaitaskai = 0;
            var kiekis = solutions.Count();
            foreach (var item in solutions)
            {
                Teisingumas += item.Teisingumas;
                laikastaskai += item.Teisingumas;
                resursaitaskai += item.ResursaiTaskai;
            }
            var total = Teisingumas + laikastaskai + resursaitaskai;

            Teisingumas = Teisingumas / kiekis;
            laikastaskai = laikastaskai / kiekis;
            resursaitaskai = resursaitaskai / kiekis;
            total = total / kiekis;
            var procentasteisingumas = 0;
            var procentaslaikas = 0;
            var procentasresursai = 0;
            var procentaitotal = 0;
            procentaitotal = (int)(((total - (usersSolution.Teisingumas + usersSolution.ProgramosLaikasTaskai + usersSolution.ResursaiTaskai)) * 100) / ((usersSolution.Teisingumas + usersSolution.ProgramosLaikasTaskai + usersSolution.ResursaiTaskai)));
            procentasteisingumas = (int)(((Teisingumas - usersSolution.Teisingumas) * 100) / usersSolution.Teisingumas);
            procentaslaikas = (int)(((laikastaskai - usersSolution.ProgramosLaikasTaskai) * 100) / usersSolution.ProgramosLaikasTaskai);
            procentasresursai = (int)(((resursaitaskai - usersSolution.ResursaiTaskai) * 100) / usersSolution.ResursaiTaskai);
            return new TaskAnalysisDto(procentasteisingumas, procentaslaikas, procentasresursai, procentaitotal);
        }

        [HttpGet]
        [Route("PointsAd/{adId}/{UserName}")]
        //[Authorize(Roles = UserRoles.Alll)]
        public async Task<ActionResult<RatingsDto>> GetPointsForAd(int adId, string UserName)
        {
            var user = await _userInfoRepositry.GetAsync(UserName);
            if (user == null) { return NotFound(); }


            var loggedUsers = await _loggedRepository.GetManyAsync();
            loggedUsers = loggedUsers.Where(o => o.UserId == user.Id).ToList();
            loggedUsers = loggedUsers.Where(o => o.Skelbimas_id == adId).ToList();
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
                       }).FirstOrDefault();

            if (ratings == null)
            {
                return new RatingsDto(UserName, 0, 0, 0, 0); // Return appropriate response if ratings is null
            }
            return  new RatingsDto(ratings.UserName, ratings.TeisingumasTaskai, ratings.ProgramosLaikasTaskai, ratings.ResursaiTaskai, ratings.TotalPoints);
        }
    }
}
