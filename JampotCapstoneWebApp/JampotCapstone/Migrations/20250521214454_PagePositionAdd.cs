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
            migrationBuilder.AddColumn<int>(
                name: "PagePositionId",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PagePosition",
                columns: table => new
                {
                    PagePositionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Home = table.Column<int>(type: "int", nullable: false),
                    Catering = table.Column<int>(type: "int", nullable: false),
                    Menu = table.Column<int>(type: "int", nullable: false),
                    About = table.Column<int>(type: "int", nullable: false),
                    FAQ = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagePosition", x => x.PagePositionId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Files_PagePositionId",
                table: "Files",
                column: "PagePositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_PagePosition_PagePositionId",
                table: "Files",
                column: "PagePositionId",
                principalTable: "PagePosition",
                principalColumn: "PagePositionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_PagePosition_PagePositionId",
                table: "Files");

            migrationBuilder.DropTable(
                name: "PagePosition");

            migrationBuilder.DropIndex(
                name: "IX_Files_PagePositionId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "PagePositionId",
                table: "Files");
        }
    }
}
