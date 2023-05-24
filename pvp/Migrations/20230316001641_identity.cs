using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pvp.Migrations
{
    public partial class identity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "prisijunges",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.CreateIndex(
                name: "IX_prisijunges_UserId",
                table: "prisijunges",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_prisijunges_User_UserId",
                table: "prisijunges",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_prisijunges_User_UserId",
                table: "prisijunges");

            migrationBuilder.DropIndex(
                name: "IX_prisijunges_UserId",
                table: "prisijunges");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "prisijunges",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)");
        }
    }
}
