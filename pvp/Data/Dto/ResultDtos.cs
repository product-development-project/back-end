namespace pvp.Data.Dto
{
    //public int Id { get; set; }
    //public string Duomenys { get; set; }
    //public string Rezultatas { get; set; }
    //public bool Pavyzdine { get; set; }
    //public string UserId { get; set; }
    //public int Uzduotis_id { get; set; }

    public record ResultDto(int id, string Data, string Result, bool Example, int Task_id);
    public record CreateResultDto(int id, string Data, string Result, bool Example, int Task_id);
    public record UpdateResultDto(int id, string Data, string Result, bool Example, int Task_id);
}
