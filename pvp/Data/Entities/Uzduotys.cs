namespace pvp.Data.Entities
{
    public class Uzduotys
    {
        public int id { get; set; }
        public string Pavadinimas { get; set; }
        public string Aprasymas { get; set; }
        public int Sudetingumas { get; set; }
        public bool Patvirtinta { get; set; }
        public bool Mokomoji { get; set; }
        public DateTime Data { get; set; }
        public string UserId { get; set; }
        public int Tipas_id { get; set; }
    }
}
