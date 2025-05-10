using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JampotCapstone.Migrations
{
    /// <inheritdoc />
    public partial class AddPageFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PageId",
                table: "Files",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Files_PageId",
                table: "Files",
                column: "PageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Pages_PageId",
                table: "Files",
                column: "PageId",
                principalTable: "Pages",
                principalColumn: "PageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Pages_PageId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_PageId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "PageId",
                table: "Files");
        }
    }
}
