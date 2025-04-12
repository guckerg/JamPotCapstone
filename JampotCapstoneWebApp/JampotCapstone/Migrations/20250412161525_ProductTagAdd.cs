using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JampotCapstone.Migrations
{
    /// <inheritdoc />
    public partial class ProductTagAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductTag_ProductTag_TagsTagID",
                table: "ProductProductTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductTag",
                table: "ProductTag");

            migrationBuilder.RenameTable(
                name: "ProductTag",
                newName: "ProductTags");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductTags",
                table: "ProductTags",
                column: "TagID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductTag_ProductTags_TagsTagID",
                table: "ProductProductTag",
                column: "TagsTagID",
                principalTable: "ProductTags",
                principalColumn: "TagID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductTag_ProductTags_TagsTagID",
                table: "ProductProductTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductTags",
                table: "ProductTags");

            migrationBuilder.RenameTable(
                name: "ProductTags",
                newName: "ProductTag");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductTag",
                table: "ProductTag",
                column: "TagID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductTag_ProductTag_TagsTagID",
                table: "ProductProductTag",
                column: "TagsTagID",
                principalTable: "ProductTag",
                principalColumn: "TagID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
