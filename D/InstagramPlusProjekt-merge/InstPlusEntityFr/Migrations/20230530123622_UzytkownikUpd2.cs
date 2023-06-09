using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InstPlusEntityFr.Migrations
{
    /// <inheritdoc />
    public partial class UzytkownikUpd2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpisProfilu",
                table: "Uzytkownicy");

            migrationBuilder.AddColumn<string>(
                name: "Opis",
                table: "Uzytkownicy",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Opis",
                table: "Uzytkownicy");

            migrationBuilder.AddColumn<string>(
                name: "OpisProfilu",
                table: "Uzytkownicy",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                defaultValue: "");
        }
    }
}
