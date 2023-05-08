namespace pvp.Data.Dto
{
    public record CodeChekcerDto(string language, string type, string name, string code, int taskId);
    public record CodeResultDto(string[] passed, string[] failed, double runTime, double memoryUsage);
}
