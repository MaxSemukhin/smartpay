using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPay.Migrations
{
    public partial class FavoriteMerchants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MerchantUser",
                columns: table => new
                {
                    FavoriteMerchantsId = table.Column<int>(type: "INTEGER", nullable: false),
                    UsersId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantUser", x => new { x.FavoriteMerchantsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_MerchantUser_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MerchantUser_Merchants_FavoriteMerchantsId",
                        column: x => x.FavoriteMerchantsId,
                        principalTable: "Merchants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MerchantUser_UsersId",
                table: "MerchantUser",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MerchantUser");
        }
    }
}
