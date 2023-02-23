namespace pvp.Data.Dto
{
    //public int id { get; set; }
    //public string Pavadinimas { get; set; }
    //public string Aprasymas { get; set; }
    //public DateTime Pradzia { get; set; }
    //public DateTime Pabaiga { get; set; }
    //public string UserId { get; set; }

    public record AdDto(int id, string Name, string Description, DateTime Start, DateTime End);
    public record CreateAdDto(int id, string Name, string Description, DateTime Start, DateTime End);
    public record UpdateAdDto(int id, string Name, string Description, DateTime Start, DateTime End);
}
