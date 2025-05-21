using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JampotCapstone.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePageFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "FilePage",
                columns: table => new
                {
                    FilesFileID = table.Column<int>(type: "int", nullable: false),
                    PagesPageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilePage", x => new { x.FilesFileID, x.PagesPageId });
                    table.ForeignKey(
                        name: "FK_FilePage_Files_FilesFileID",
                        column: x => x.FilesFileID,
                        principalTable: "Files",
                        principalColumn: "FileID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilePage_Pages_PagesPageId",
                        column: x => x.PagesPageId,
                        principalTable: "Pages",
                        principalColumn: "PageId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_FilePage_PagesPageId",
                table: "FilePage",
                column: "PagesPageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilePage");

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
    }
}
