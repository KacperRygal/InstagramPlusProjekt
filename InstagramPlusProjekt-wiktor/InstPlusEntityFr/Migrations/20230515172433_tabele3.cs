using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InstPlusEntityFr.Migrations
{
    /// <inheritdoc />
    public partial class tabele3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Obserwowani",
                columns: table => new
                {
                    ObserwowanyId = table.Column<int>(type: "int", nullable: false),
                    ObserwatorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obserwowani", x => new { x.ObserwatorId, x.ObserwowanyId });
                });

            migrationBuilder.CreateTable(
                name: "Obserwujacy",
                columns: table => new
                {
                    ObserwowanyId = table.Column<int>(type: "int", nullable: false),
                    ObserwatorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obserwujacy", x => new { x.ObserwatorId, x.ObserwowanyId });
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    TagId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Counter = table.Column<int>(type: "int", nullable: false),
                    UzytkownikId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.TagId);
                    table.ForeignKey(
                        name: "FK_Tag_Uzytkownicy_UzytkownikId",
                        column: x => x.UzytkownikId,
                        principalTable: "Uzytkownicy",
                        principalColumn: "UzytkownikId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tag_UzytkownikId",
                table: "Tag",
                column: "UzytkownikId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Obserwowani");

            migrationBuilder.DropTable(
                name: "Obserwujacy");

            migrationBuilder.DropTable(
                name: "Tag");
        }
    }
}
