using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using pvp.Data.Auth;
using pvp.Data.Entities;

namespace pvp.Data
{
    public class SystemDbContext : IdentityDbContext<RestUsers>
    {
        public DbSet<ParinktosUzduotys> parinktosUzduotys { get; set; }
        public DbSet<Prisijunge> prisijunges { get; set; }
        public DbSet<Rezultatai> rezultatais { get; set; }
        public DbSet<Skelbimas> skelbimas { get; set;}
        public DbSet<Sprendimas> sprendimas { get; set; }
        public DbSet<Tipas> tipas { get; set; }
        public DbSet<Uzduotys> uzduotys { get; set;}
        public DbSet<Help> help { get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseMySQL("Data Source=127.0.0.1;port=3306;Initial Catalog=mydb;User Id=root;Password=root;SslMode=none;Convert Zero Datetime=True;");
            optionsBuilder.UseMySQL("Data Source=127.0.0.1;port=3306;Initial Catalog=mydb;User Id=root;Password=;SslMode=none;Convert Zero Datetime=True;");
        }
    }
}
