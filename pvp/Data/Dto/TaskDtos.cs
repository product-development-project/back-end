namespace pvp.Data.Dto
{
    //public int id { get; set; }
    //public string Pavadinimas { get; set; }
    //public string Aprasymas { get; set; }
    //public int Sudetingumas { get; set; }
    //public bool Patvirtinta { get; set; }
    //public bool Mokomoji { get; set; }
    //public DateTime Data { get; set; }
    //public string UserId { get; set; }
    //public int Tipas_id { get; set; }

    public record TaskDto(int id, string Name, string Description, int Difficulty, bool Confirmed, bool Educational, DateTime Date, int Type_id);
    public record CreateTaskDto(int id, string Name, string Description, int Difficulty, bool Confirmed, bool Educational, DateTime Date, int Type_id);
    public record UpdateTaskDto(int id, string Name, string Description, int Difficulty, bool Confirmed, bool Educational, DateTime Date, int Type_id);
}
