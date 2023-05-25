using pvp.Auth.Models;
using System.ComponentModel.DataAnnotations;

namespace pvp.Data.Entities
{
    public class Kompanija : IUserOwnedResources
    {
        public int id { get; set; }
        public string svetaine { get; set;}
        public string pavadinimas { get; set; }
        public string adresas { get; set; }
        public string email { get; set; }
        public string PhoneNumber { get; set; }

        [Required]
        public string UserId { get; set; }
        public User user { get; set; }
    }
}
