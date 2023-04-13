using pvp.Auth.Models;
using System.ComponentModel.DataAnnotations;

namespace pvp.Data.Entities
{
    public class Uzduotys : IUserOwnedResources
    {
        public int id { get; set; }
        public string Pavadinimas { get; set; }
        public byte[] Problema { get; set; }
        public int Sudetingumas { get; set; }
        public bool Patvirtinta { get; set; }
        public bool Mokomoji { get; set; }
        public DateTime Data { get; set; }
        [Required]
        public string UserId { get; set; }
        public User user { get; set; }
        public int Tipas_id { get; set; }
    }
}
