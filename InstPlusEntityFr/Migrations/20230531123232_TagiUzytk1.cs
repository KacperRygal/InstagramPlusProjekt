using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InstPlusEntityFr.Migrations
{
    /// <inheritdoc />
    public partial class TagiUzytk1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagiUzytkownikow_Uzytkownicy_UzytkownikId",
                table: "TagiUzytkownikow");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TagiUzytkownikow",
                table: "TagiUzytkownikow");

            migrationBuilder.DropColumn(
                name: "Nazwa",
                table: "TagiUzytkownikow");

            migrationBuilder.RenameColumn(
                name: "Counter",
                table: "TagiUzytkownikow",
                newName: "Wystapienia");

            migrationBuilder.RenameColumn(
                name: "TagId",
                table: "TagiUzytkownikow",
                newName: "TagPostuId");

            migrationBuilder.AlterColumn<int>(
                name: "UzytkownikId",
                table: "TagiUzytkownikow",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TagPostuId",
                table: "TagiUzytkownikow",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "TagUzytkownikaId",
                table: "TagiUzytkownikow",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TagiUzytkownikow",
                table: "TagiUzytkownikow",
                column: "TagUzytkownikaId");

            migrationBuilder.AddForeignKey(
                name: "FK_TagiUzytkownikow_Uzytkownicy_UzytkownikId",
                table: "TagiUzytkownikow",
                column: "UzytkownikId",
                principalTable: "Uzytkownicy",
                principalColumn: "UzytkownikId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagiUzytkownikow_Uzytkownicy_UzytkownikId",
                table: "TagiUzytkownikow");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TagiUzytkownikow",
                table: "TagiUzytkownikow");

            migrationBuilder.DropColumn(
                name: "TagUzytkownikaId",
                table: "TagiUzytkownikow");

            migrationBuilder.RenameColumn(
                name: "Wystapienia",
                table: "TagiUzytkownikow",
                newName: "Counter");

            migrationBuilder.RenameColumn(
                name: "TagPostuId",
                table: "TagiUzytkownikow",
                newName: "TagId");

            migrationBuilder.AlterColumn<int>(
                name: "UzytkownikId",
                table: "TagiUzytkownikow",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TagId",
                table: "TagiUzytkownikow",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Nazwa",
                table: "TagiUzytkownikow",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TagiUzytkownikow",
                table: "TagiUzytkownikow",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_TagiUzytkownikow_Uzytkownicy_UzytkownikId",
                table: "TagiUzytkownikow",
                column: "UzytkownikId",
                principalTable: "Uzytkownicy",
                principalColumn: "UzytkownikId");
        }
    }
}
