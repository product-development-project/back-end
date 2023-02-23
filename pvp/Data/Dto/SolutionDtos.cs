namespace pvp.Data.Dto
{
    //public int id { get; set; }
    //public Byte[] Programa { get; set; }
    //public int Teisingumas { get; set; }
    //public int ProgramosLaikasTaskai { get; set; }
    //public int ResursaiTaskai { get; set; }
    //public int ProgramosLaikasSek { get; set; }
    //public int CpuIsnaudojimas { get; set; }
    //public int RamIsnaudojimas { get; set; }
    //public int Prisijunge_id { get; set; }
    //public int ParinktosUzduotys_id { get; set; }

    public record SolutionDto(int id, Byte[] Program, int Correctness, int ProgramTimePoints, int ResourcesPoints, int ProgramTimeSec, int CpuUsage, int RamUsage, int Logged_id, int SelectedTask_id);
    public record CreateSolutionDto(int id, Byte[] Program, int Correctness, int ProgramTimePoints, int ResourcesPoints, int ProgramTimeSec, int CpuUsage, int RamUsage, int Logged_id, int SelectedTask_id);
    public record UpdateSolutionDto(int id, Byte[] Program, int Correctness, int ProgramTimePoints, int ResourcesPoints, int ProgramTimeSec, int CpuUsage, int RamUsage, int Logged_id, int SelectedTask_id);
}
