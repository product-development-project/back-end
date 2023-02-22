using Microsoft.EntityFrameworkCore;
using pvp.Data.Entities;

namespace pvp.Data
{
    public class SystemDbContext : DbContext
    {
        public DbSet<ParinktosUzduotys> parinktosUzduotys { get; set; }
        public DbSet<Prisijunge> prisijunges { get; set; }
        public DbSet<Rezultatai> rezultatais { get; set; }
        public DbSet<Skelbimas> skelbimas { get; set;}
        public DbSet<Sprendimas> sprendimas { get; set; }
        public DbSet<Tipas> tipas { get; set; }
        public DbSet<Uzduotys> uzduotys { get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("Data Source=127.0.0.1;port=3306;Initial Catalog=mydb;User Id=root;Password=;SslMode=none;Convert Zero Datetime=True;");
        }
    }
}
