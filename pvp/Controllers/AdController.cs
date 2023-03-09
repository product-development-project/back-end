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
        private readonly IAuthorizationService _authorizationService;
        public AdController(IAdRepository adRepository, IAuthorizationService authorizationService) 
        {
            _adRepository = adRepository;
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
