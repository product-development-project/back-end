namespace pvp.Data.Dto
{
    public record UserInfoDto(string Name, string Email, string PhoneNumber);
    public record CreateUserInfoDto(string id, string Name, string Email);
    public record UpdateUserInfoDto(string Name, string Email, string PhoneNumber);
}
