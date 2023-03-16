using pvp.Auth.Models;
using System.ComponentModel.DataAnnotations;

namespace pvp.Data.Entities
{
    public class Prisijunge : IUserOwnedResources
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public User user { get; set; }
        public int Skelbimas_id { get; set; }
        public int Uzduotys_id { get; set; }

    }
}
