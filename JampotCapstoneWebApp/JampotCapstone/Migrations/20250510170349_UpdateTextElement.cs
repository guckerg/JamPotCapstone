using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JampotCapstone.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTextElement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "TextElements");

            migrationBuilder.AddColumn<int>(
                name: "PageId",
                table: "TextElements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TextElements_PageId",
                table: "TextElements",
                column: "PageId");

            migrationBuilder.AddForeignKey(
                name: "FK_TextElements_Pages_PageId",
                table: "TextElements",
                column: "PageId",
                principalTable: "Pages",
                principalColumn: "PageId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TextElements_Pages_PageId",
                table: "TextElements");

            migrationBuilder.DropIndex(
                name: "IX_TextElements_PageId",
                table: "TextElements");

            migrationBuilder.DropColumn(
                name: "PageId",
                table: "TextElements");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "TextElements",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
