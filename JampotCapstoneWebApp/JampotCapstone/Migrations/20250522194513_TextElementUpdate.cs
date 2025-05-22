using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JampotCapstone.Migrations
{
    /// <inheritdoc />
    public partial class TextElementUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PagePositionId",
                table: "TextElements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TextElements_PagePositionId",
                table: "TextElements",
                column: "PagePositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TextElements_PagePosition_PagePositionId",
                table: "TextElements",
                column: "PagePositionId",
                principalTable: "PagePosition",
                principalColumn: "PagePositionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TextElements_PagePosition_PagePositionId",
                table: "TextElements");

            migrationBuilder.DropIndex(
                name: "IX_TextElements_PagePositionId",
                table: "TextElements");

            migrationBuilder.DropColumn(
                name: "PagePositionId",
                table: "TextElements");
        }
    }
}
