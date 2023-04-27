using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pvp.Data.Dto;
using pvp.Data.Entities;
using pvp.Data.Repositories;
using System.Data;
using pvp.Auth;
using pvp.Auth.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace pvp.Controllers
{
    [ApiController]
    [Route("api/Ad")]
    public class AdController :ControllerBase
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
        public AdController(IAdRepository adRepository, ILoggedRepository loggedRepository, IResultRepository resultRepository, ISelectedTaskRepository selectedTaskRepository, ISolutionRepository solutionRepository, ITaskRepository taskRepository, ITypeRepository typeRepository, IUserInfoRepositry userInfoRepositry, IAuthorizationService authorizationService) 
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
        //[Authorize(Roles = UserRoles.Alll)]
        public async Task<IEnumerable<AdDto>> GetMany()
        {
            var ads = await _adRepository.GetManyAsync();
            return ads.Select(o => new AdDto(o.id, o.Pavadinimas, o.Aprasymas, o.Pradzia, o.Pabaiga));
        }

        [HttpGet]
        [Route("CompanyAds")]
        [Authorize(Roles = UserRoles.CompanyAndAdmin)]
        public async Task<IEnumerable<AdDto>> GetManyCompany()
        {
            var ads = await _adRepository.GetManyAsync();
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            ads = ads.Where(o => o.UserId == userId).ToList();
            return ads.Select(o => new AdDto(o.id, o.Pavadinimas, o.Aprasymas, o.Pradzia, o.Pabaiga));
        }

        [HttpGet]
        [Route("CompanyAds/{adId}/Logged")]
        [Authorize(Roles = UserRoles.CompanyAndAdmin)]
        public async Task<IEnumerable<RatingsDto>> GetUsers(int adId)
        {
            var loggedUsers = await _loggedRepository.GetManyAsync();
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
                       })
                       .OrderByDescending(r => r.TotalPoints)
                       .ToList();

            return ratings.Select(o => new RatingsDto(o.UserName, o.TeisingumasTaskai, o.ProgramosLaikasTaskai, o.ResursaiTaskai, o.TotalPoints));
        }

        [HttpGet]
        [Route("{adId}")]
        //[Authorize(Roles = UserRoles.Alll)]
        public async Task<ActionResult<AdDto>> Get(int adId)
        {
            var ad = await _adRepository.GetAsync(adId);
            if (ad == null) { return NotFound(); }

            return new AdDto(ad.id, ad.Pavadinimas, ad.Aprasymas, ad.Pradzia, ad.Pabaiga);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Company)]
        public async Task<ActionResult<AdDto>> Create(CreateAdDto createAdDto)
        {
            var ad = new Skelbimas
            {
                Pavadinimas = createAdDto.Name,
                Aprasymas = createAdDto.Description,
                Pradzia = createAdDto.Start,
                Pabaiga = createAdDto.End,
                UserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
            };
            await _adRepository.CreateAsync(ad);
            return Created("", new AdDto(ad.id, ad.Pavadinimas, ad.Aprasymas, ad.Pradzia, ad.Pabaiga));
        }

        [HttpPut]
        [Route("{adId}")]
        [Authorize(Roles = UserRoles.Company)]
        public async Task<ActionResult<AdDto>> Update(int adId,UpdateAdDto updateAdDto)
        {
            var ad = await _adRepository.GetAsync(adId); 
            if (ad == null) { return NotFound();}

            var authr = await _authorizationService.AuthorizeAsync(User, ad, PolicyNames.ResourceOwner);
            if (!authr.Succeeded)
            {
                return Forbid();
            }

            ad.Pavadinimas = updateAdDto.Name;
            ad.Aprasymas = updateAdDto.Description;
            ad.Pradzia = updateAdDto.Start;
            ad.Pabaiga = updateAdDto.End;

            await _adRepository.UpdateAsync(ad);
            return Ok(new AdDto(ad.id, ad.Pavadinimas, ad.Aprasymas, ad.Pradzia, ad.Pabaiga));
        }

        [HttpDelete]
        [Route("{adId}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult> Remove(int adId)
        {
            var ad = await _adRepository.GetAsync(adId);
            if (ad == null) { return NotFound();}
            await _adRepository.DeleteAsync(ad);
            return NoContent();
        }
    }
}
