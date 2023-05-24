namespace pvp.Data.Dto
{
    public record CompanyDto(int id, string svetaine, string pavadinimas, string adresas, string email, string telefonas);
    public record CreateCompanyDto(int id, string svetaine, string pavadinimas, string adresas, string email, string telefonas);
    public record UpdateCompanyDto(int id, string svetaine, string pavadinimas, string adresas, string email, string telefonas);
}
