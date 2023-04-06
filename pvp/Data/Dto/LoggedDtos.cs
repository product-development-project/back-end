namespace pvp.Data.Dto
{
    //public int Id { get; set; }
    //public string UserId { get; set; }
    //public int Skelbimas_id { get; set; }
    //public int Uzduotys_id { get; set; }
    public record LoggedDto(int id, int? Ad_id, int? Task_id);
    public record CreateLoggedDto(int id, int? Ad_id, int? Task_id);
    public record UpdateLoggedDto(int id, int? Ad_id, int? Task_id);
}
