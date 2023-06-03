using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InstPlusEntityFr.Migrations
{
    /// <inheritdoc />
    public partial class Tabele : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Zdjecie",
                table: "Uzytkownicy",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Komentarze",
                columns: table => new
                {
                    KomentarzId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tresc = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UzytkownikId = table.Column<int>(type: "int", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Komentarze", x => x.KomentarzId);
                });

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
                name: "PolubieniaKomentarzy",
                columns: table => new
                {
                    KomentarzId = table.Column<int>(type: "int", nullable: false),
                    UzytkownikId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolubieniaKomentarzy", x => new { x.KomentarzId, x.UzytkownikId });
                });

            migrationBuilder.CreateTable(
                name: "PolubieniaPostow",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false),
                    UzytkownikId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolubieniaPostow", x => new { x.PostId, x.UzytkownikId });
                });

            migrationBuilder.CreateTable(
                name: "Posty",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Opis = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Zdjecie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UzytkownikId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posty", x => x.PostId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Komentarze");

            migrationBuilder.DropTable(
                name: "Obserwowani");

            migrationBuilder.DropTable(
                name: "Obserwujacy");

            migrationBuilder.DropTable(
                name: "PolubieniaKomentarzy");

            migrationBuilder.DropTable(
                name: "PolubieniaPostow");

            migrationBuilder.DropTable(
                name: "Posty");

            migrationBuilder.AlterColumn<string>(
                name: "Zdjecie",
                table: "Uzytkownicy",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
