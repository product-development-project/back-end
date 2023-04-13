using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace pvp.Migrations
{
    public partial class initialmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "parinktosUzduotys",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Skelbimas_id = table.Column<int>(type: "int", nullable: false),
                    Uzduotys_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parinktosUzduotys", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "prisijunges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "longtext", nullable: false),
                    Skelbimas_id = table.Column<int>(type: "int", nullable: false),
                    Uzduotys_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prisijunges", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "rezultatais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Duomenys = table.Column<string>(type: "longtext", nullable: false),
                    Rezultatas = table.Column<string>(type: "longtext", nullable: false),
                    Pavyzdine = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UserId = table.Column<string>(type: "longtext", nullable: false),
                    Uzduotis_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rezultatais", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "skelbimas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Pavadinimas = table.Column<string>(type: "longtext", nullable: false),
                    Aprasymas = table.Column<string>(type: "longtext", nullable: false),
                    Pradzia = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Pabaiga = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserId = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_skelbimas", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sprendimas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Programa = table.Column<byte[]>(type: "longblob", nullable: false),
                    Teisingumas = table.Column<int>(type: "int", nullable: false),
                    ProgramosLaikasTaskai = table.Column<int>(type: "int", nullable: false),
                    ResursaiTaskai = table.Column<int>(type: "int", nullable: false),
                    ProgramosLaikasSek = table.Column<int>(type: "int", nullable: false),
                    CpuIsnaudojimas = table.Column<int>(type: "int", nullable: false),
                    RamIsnaudojimas = table.Column<int>(type: "int", nullable: false),
                    Prisijunge_id = table.Column<int>(type: "int", nullable: false),
                    ParinktosUzduotys_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sprendimas", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tipas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Pavadinimas = table.Column<string>(type: "longtext", nullable: false),
                    Aprasymas = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipas", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "uzduotys",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Pavadinimas = table.Column<string>(type: "longtext", nullable: false),
                    Problema = table.Column<byte[]>(type: "longblob", nullable: false),
                    Sudetingumas = table.Column<int>(type: "int", nullable: false),
                    Patvirtinta = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Mokomoji = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserId = table.Column<string>(type: "longtext", nullable: false),
                    Tipas_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uzduotys", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "parinktosUzduotys");

            migrationBuilder.DropTable(
                name: "prisijunges");

            migrationBuilder.DropTable(
                name: "rezultatais");

            migrationBuilder.DropTable(
                name: "skelbimas");

            migrationBuilder.DropTable(
                name: "sprendimas");

            migrationBuilder.DropTable(
                name: "tipas");

            migrationBuilder.DropTable(
                name: "uzduotys");
        }
    }
}
