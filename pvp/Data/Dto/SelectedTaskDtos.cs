namespace pvp.Data.Dto
{
    //public int id { get; set; }
    //public int Skelbimas_id { get; set; }
    //public int Uzduotys_id { get; set; }

    public record SelectedTaskDto(int id, int Ad_id, int Task_id);
    public record CreateSelectedTaskDto(int id, int Ad_id, int Task_id);
    public record UpdateSelectedTaskDto(int id, int Ad_id, int Task_id);
}
