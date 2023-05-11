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
using System.IdentityModel.Tokens.Jwt;

namespace pvp.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/Company")]
    public class CompanyController : ControllerBase
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
        private readonly ICompanyRepository _companyRepository;
        public CompanyController(IAdRepository adRepository, ILoggedRepository loggedRepository, IResultRepository resultRepository, ISelectedTaskRepository selectedTaskRepository, ISolutionRepository solutionRepository, ITaskRepository taskRepository, ITypeRepository typeRepository, IUserInfoRepositry userInfoRepositry, IAuthorizationService authorizationService, ICompanyRepository companyRepository)
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
            _companyRepository = companyRepository;
        }

        [HttpGet]
        [Route("{UserName}")]
        [Authorize(Roles = UserRoles.Alll)]
        public async Task<ActionResult<CompanyDto>> Get(string UserName)
        {
            var user = await _userInfoRepositry.GetAsync(UserName);

            var tokenHandler = new JwtSecurityTokenHandler();
            var authHeader = Request.Headers["Authorization"].ToString();

            if (authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                string name = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
                if (name != UserName)
                {
                    return Forbid();
                }
            }
            if (user == null) { return NotFound(); }

            var company = await _companyRepository.GetAsync(user.Id);

            return new CompanyDto(company.id, company.svetaine, company.pavadinimas, company.adresas, company.email, company.telefonas);
        }
    }
}
