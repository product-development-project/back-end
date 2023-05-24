using System.ComponentModel.DataAnnotations;

namespace pvp.Data.Auth
{
     public record RegisterUserDto([Required] string UserName, [EmailAddress][Required] string Email, [Required] string Password, [Required] string PhoneNumber);
}
