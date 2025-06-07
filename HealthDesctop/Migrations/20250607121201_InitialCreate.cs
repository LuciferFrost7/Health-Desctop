using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthDesctop.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryColors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Red = table.Column<byte>(type: "tinyint", nullable: false),
                    Green = table.Column<byte>(type: "tinyint", nullable: false),
                    Blue = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryColors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Calories = table.Column<int>(type: "int", nullable: false),
                    Proteins = table.Column<int>(type: "int", nullable: false),
                    Fats = table.Column<int>(type: "int", nullable: false),
                    Carbohydrates = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fk_Color = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_CategoryColors_Fk_Color",
                        column: x => x.Fk_Color,
                        principalTable: "CategoryColors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListOfProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_ProductId = table.Column<int>(type: "int", nullable: false),
                    Fk_CategoryId = table.Column<int>(type: "int", nullable: false),
                    tbl_CategoryId = table.Column<int>(type: "int", nullable: true),
                    tbl_ProductsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListOfProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListOfProducts_Categories_Fk_CategoryId",
                        column: x => x.Fk_CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListOfProducts_Categories_tbl_CategoryId",
                        column: x => x.tbl_CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ListOfProducts_Products_Fk_ProductId",
                        column: x => x.Fk_ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListOfProducts_Products_tbl_ProductsId",
                        column: x => x.tbl_ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Fk_Color",
                table: "Categories",
                column: "Fk_Color");

            migrationBuilder.CreateIndex(
                name: "IX_ListOfProducts_Fk_CategoryId",
                table: "ListOfProducts",
                column: "Fk_CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ListOfProducts_Fk_ProductId",
                table: "ListOfProducts",
                column: "Fk_ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ListOfProducts_tbl_CategoryId",
                table: "ListOfProducts",
                column: "tbl_CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ListOfProducts_tbl_ProductsId",
                table: "ListOfProducts",
                column: "tbl_ProductsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListOfProducts");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "CategoryColors");
        }
    }
}
