using pvp.Auth.Models;
using System.ComponentModel.DataAnnotations;

namespace pvp.Data.Entities
{
    public class Skelbimas : IUserOwnedResources
    {
        public int id { get; set; }
        public string Pavadinimas { get; set; }
        public string Aprasymas { get; set; }
        public DateTime Pradzia { get; set; }
        public DateTime Pabaiga { get; set; }
        [Required]
        public string UserId { get; set; }
        public User user { get; set; }

    }
}
