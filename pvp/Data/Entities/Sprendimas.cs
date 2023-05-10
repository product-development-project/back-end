using System.Reflection.Metadata;

namespace pvp.Data.Entities
{
    public class Sprendimas
    {
        public int id { get; set; }
        public Byte[]? Programa { get; set; }
        public int Teisingumas { get; set; }
        public int ProgramosLaikasTaskai { get; set; }
        public int ResursaiTaskai { get; set; }
        public double ProgramosLaikas { get; set; }
        public double RamIsnaudojimas { get; set; }
        public int Prisijunge_id { get; set; }
        public int? ParinktosUzduotys_id { get; set; }
    }
}
