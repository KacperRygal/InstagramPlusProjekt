using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InstPlusEntityFr.Migrations
{
    /// <inheritdoc />
    public partial class Tabele2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Obserwowani");

            migrationBuilder.DropTable(
                name: "Obserwujacy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Obserwowani",
                columns: table => new
                {
                    ObserwatorId = table.Column<int>(type: "int", nullable: false),
                    ObserwowanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obserwowani", x => new { x.ObserwatorId, x.ObserwowanyId });
                });

            migrationBuilder.CreateTable(
                name: "Obserwujacy",
                columns: table => new
                {
                    ObserwatorId = table.Column<int>(type: "int", nullable: false),
                    ObserwowanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obserwujacy", x => new { x.ObserwatorId, x.ObserwowanyId });
                });
        }
    }
}
