using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InstPlusEntityFr.Migrations
{
    /// <inheritdoc />
    public partial class TagiUzytk_Uzytk1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TagiUzytkownikow");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TagiUzytkownikow",
                columns: table => new
                {
                    TagUzytkownikaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagPostuId = table.Column<int>(type: "int", nullable: false),
                    UzytkownikId = table.Column<int>(type: "int", nullable: false),
                    Wystapienia = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagiUzytkownikow", x => x.TagUzytkownikaId);
                    table.ForeignKey(
                        name: "FK_TagiUzytkownikow_Uzytkownicy_UzytkownikId",
                        column: x => x.UzytkownikId,
                        principalTable: "Uzytkownicy",
                        principalColumn: "UzytkownikId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TagiUzytkownikow_UzytkownikId",
                table: "TagiUzytkownikow",
                column: "UzytkownikId");
        }
    }
}
