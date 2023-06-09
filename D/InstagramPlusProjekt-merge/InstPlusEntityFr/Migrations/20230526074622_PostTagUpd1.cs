using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InstPlusEntityFr.Migrations
{
    /// <inheritdoc />
    public partial class PostTagUpd1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagiPostow_Posty_PostId",
                table: "TagiPostow");

            migrationBuilder.DropIndex(
                name: "IX_TagiPostow_PostId",
                table: "TagiPostow");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "TagiPostow");

            migrationBuilder.CreateTable(
                name: "PostTagPostu",
                columns: table => new
                {
                    PostyPostId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTagPostu", x => new { x.PostyPostId, x.TagId });
                    table.ForeignKey(
                        name: "FK_PostTagPostu_Posty_PostyPostId",
                        column: x => x.PostyPostId,
                        principalTable: "Posty",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostTagPostu_TagiPostow_TagId",
                        column: x => x.TagId,
                        principalTable: "TagiPostow",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostTagPostu_TagId",
                table: "PostTagPostu",
                column: "TagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostTagPostu");

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "TagiPostow",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TagiPostow_PostId",
                table: "TagiPostow",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_TagiPostow_Posty_PostId",
                table: "TagiPostow",
                column: "PostId",
                principalTable: "Posty",
                principalColumn: "PostId");
        }
    }
}
