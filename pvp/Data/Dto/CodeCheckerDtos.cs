namespace pvp.Data.Dto
{
    public record CodeChekcerDto(string language, string type, string name, string code);
    public record ResultDto(string[] passed, string[] failed);
}
