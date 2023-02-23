namespace pvp.Data.Entities
{
    public class Skelbimas
    {
        public int id { get; set; }
        public string Pavadinimas { get; set; }
        public string Aprasymas { get; set; }
        public DateTime Pradzia { get; set; }
        public DateTime Pabaiga { get; set; }
        public string UserId { get; set; }
    }
}
