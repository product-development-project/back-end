namespace pvp.Data.Dto
{
    public record RatingsDto(string UserName, int CorrectnesPoints, int TimePoints, int RecourcesPoints, int TotalPoints);
    public record TaskCountDto(int Count);
    /// <summary>
    /// ABOVE AVERAGE
    /// </summary>
    /// <param name="CorrectnesPointsPercentage"></param>
    /// <param name="TimePointsPercentage"></param>
    /// <param name="ResourcesPointsPercentage"></param>
    /// <param name="TotalPointsPercentage"></param>
    public record TaskAnalysisDto(int CorrectnesPointsPercentage, int TimePointsPercentage, int ResourcesPointsPercentage, int TotalPointsPercentage);
}
