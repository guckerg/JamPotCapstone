using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JampotCapstone.Migrations
{
    /// <inheritdoc />
    public partial class ProductTypeUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductProductType");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ProductType",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductIngredients",
                table: "Products",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ProductType_ProductId",
                table: "ProductType",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductType_Products_ProductId",
                table: "ProductType",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductType_Products_ProductId",
                table: "ProductType");

            migrationBuilder.DropIndex(
                name: "IX_ProductType_ProductId",
                table: "ProductType");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductType");

            migrationBuilder.AlterColumn<string>(
                name: "ProductIngredients",
                table: "Products",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProductProductType",
                columns: table => new
                {
                    ProductCategoryTypeId = table.Column<int>(type: "int", nullable: false),
                    ProductsProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductProductType", x => new { x.ProductCategoryTypeId, x.ProductsProductId });
                    table.ForeignKey(
                        name: "FK_ProductProductType_ProductType_ProductCategoryTypeId",
                        column: x => x.ProductCategoryTypeId,
                        principalTable: "ProductType",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductProductType_Products_ProductsProductId",
                        column: x => x.ProductsProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ProductProductType_ProductsProductId",
                table: "ProductProductType",
                column: "ProductsProductId");
        }
    }
}
