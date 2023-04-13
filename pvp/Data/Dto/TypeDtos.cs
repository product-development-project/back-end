namespace pvp.Data.Dto
{
    //public int id { get; set; }
    //public string Pavadinimas { get; set; }
    //public string Aprasymas { get; set; }


    public record TypeDto(int id, string Name, string Description);
    public record CreateTypeDto(int id, string Name, string Description);
    public record UpdateTypeDto(int id, string Name, string Description);
}
