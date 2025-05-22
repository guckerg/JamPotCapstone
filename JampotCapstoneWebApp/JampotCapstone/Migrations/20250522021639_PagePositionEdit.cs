using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JampotCapstone.Migrations
{
    /// <inheritdoc />
    public partial class PagePositionEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FAQ",
                table: "PagePosition",
                newName: "FAQs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FAQs",
                table: "PagePosition",
                newName: "FAQ");
        }
    }
}
