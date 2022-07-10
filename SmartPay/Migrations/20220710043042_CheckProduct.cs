using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPay.Migrations
{
    public partial class CheckProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckProduct");

            migrationBuilder.CreateTable(
                name: "ChecksProducts",
                columns: table => new
                {
                    CheckUid = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    Cost = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecksProducts", x => new { x.CheckUid, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ChecksProducts_Checks_CheckUid",
                        column: x => x.CheckUid,
                        principalTable: "Checks",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChecksProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChecksProducts_ProductId",
                table: "ChecksProducts",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChecksProducts");

            migrationBuilder.CreateTable(
                name: "CheckProduct",
                columns: table => new
                {
                    ChecksUid = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckProduct", x => new { x.ChecksUid, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_CheckProduct_Checks_ChecksUid",
                        column: x => x.ChecksUid,
                        principalTable: "Checks",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckProduct_ProductsId",
                table: "CheckProduct",
                column: "ProductsId");
        }
    }
}
