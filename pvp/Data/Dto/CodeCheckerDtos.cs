namespace pvp.Data.Dto
{
    public record CodeChekcerDto(string language, string type, string name, string code, int taskId, int? adId, string userId);
    public record CodeResultDto(string[] passed, string[] failed, double runTime, double memoryUsage, int taskPoints, int memoryUsagePoints, int runTimePoints);
}
