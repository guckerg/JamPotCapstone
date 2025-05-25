using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JampotCapstone.Migrations
{
    /// <inheritdoc />
    public partial class PagePositionAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilePage");

            migrationBuilder.CreateTable(
                name: "PagePositions",
                columns: table => new
                {
                    PagePositionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PageId = table.Column<int>(type: "int", nullable: false),
                    FileId = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagePositions", x => x.PagePositionId);
                    table.ForeignKey(
                        name: "FK_PagePositions_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "FileID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PagePositions_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "PageId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_PagePositions_FileId",
                table: "PagePositions",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_PagePositions_PageId",
                table: "PagePositions",
                column: "PageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PagePositions");

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
    }
}
