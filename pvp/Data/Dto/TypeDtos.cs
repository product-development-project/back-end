namespace pvp.Data.Dto
{
    //public int id { get; set; }
    //public string Pavadinimas { get; set; }
    //public byte[] Problema { get; set; }


    public record TypeDto(int id, string Name, byte[] Problema);
    public record CreateTypeDto(int id, string Name, byte[] Problema);
    public record UpdateTypeDto(int id, string Name, byte[] Problema);
}
