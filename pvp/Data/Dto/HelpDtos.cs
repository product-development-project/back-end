namespace pvp.Data.Dto
{
    //public int Id { get; set; }
    //public string Name { get; set; }
    //public string Description { get; set; }
    //public string Phone { get; set; }
    //public string EmailAddress { get; set; }
    //public string Country { get; set; }
    //public bool Solved { get; set; }



    public record HelpDto(int id, string Name, string Description, string Phone, string EmailAddress, string Country, bool Solved);
    public record HelpCreateDto(int id, string Name, string Description, string Phone, string EmailAddress, string Country, bool Solved);
}
