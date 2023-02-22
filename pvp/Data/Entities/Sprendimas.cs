using System.Reflection.Metadata;

namespace pvp.Data.Entities
{
    public class Sprendimas
    {
        public int id { get; set; }
        public Blob Programa { get; set; }
        public int Teisingumas { get; set; }
        public int ProgramosLaikasTaskai { get; set; }
        public int ResursaiTaskai { get; set; }
        public int ProgramosLaikasSek { get; set; }
        public int CpuIsnaudojimas { get; set; }
        public int RamIsnaudojimas { get; set; }
        public int Prisijunge_id { get; set; }
        public int ParinktosUzduotys_id { get; set; }
    }
}
