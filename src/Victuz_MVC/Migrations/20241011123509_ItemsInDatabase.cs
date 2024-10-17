using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Victuz_MVC.Migrations
{
    /// <inheritdoc />
    public partial class ItemsInDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProductCategory",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Testproductcategorie 1" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "Name", "Price" },
                values: new object[] { 1, 1, "Het eerste testproduct1", "Testproduct 1", 19.99m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
