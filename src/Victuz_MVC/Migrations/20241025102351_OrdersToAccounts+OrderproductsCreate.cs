using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Victuz_MVC.Migrations
{
    /// <inheritdoc />
    public partial class OrdersToAccountsOrderproductsCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Order",
                columns: new[] { "Id", "AccountId", "AccountId1", "TotalPrice" },
                values: new object[] { 1, 1, null, 60.00m });

            migrationBuilder.InsertData(
                table: "OrderProduct",
                columns: new[] { "Id", "OrderId", "ProductAmount", "ProductId" },
                values: new object[] { 1, 1, 6, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrderProduct",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Order",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
