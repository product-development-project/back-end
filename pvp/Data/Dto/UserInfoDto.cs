namespace pvp.Data.Dto
{
    public record UserInfoDto(string id, string Name, string Email);
    public record CreateUserInfoDto(string id, string Name, string Email);
    public record UpdateUserInfoDto(string id, string Name, string Email);
}
