using pvp.Auth.Models;
using System.ComponentModel.DataAnnotations;

namespace pvp.Data.Entities
{
    public class Rezultatai : IUserOwnedResources
    {
        public int Id { get; set; }
        public string Duomenys { get; set; }
        public string Rezultatas { get; set; }
        public bool Pavyzdine { get; set; }
        [Required]
        public string UserId { get; set; }
        public User user { get; set; }
        public int Uzduotis_id { get; set; }
    }
}
