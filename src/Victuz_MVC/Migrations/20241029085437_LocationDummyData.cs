using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Victuz_MVC.Migrations
{
    /// <inheritdoc />
    public partial class LocationDummyData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Activity",
                keyColumn: "Id",
                keyValue: 1,
                column: "Location",
                value: "Sportveld");

            migrationBuilder.UpdateData(
                table: "Activity",
                keyColumn: "Id",
                keyValue: 2,
                column: "Location",
                value: "Lokaal 1");

            migrationBuilder.UpdateData(
                table: "Activity",
                keyColumn: "Id",
                keyValue: 3,
                column: "Location",
                value: "Lokaal 2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Activity",
                keyColumn: "Id",
                keyValue: 1,
                column: "Location",
                value: "");

            migrationBuilder.UpdateData(
                table: "Activity",
                keyColumn: "Id",
                keyValue: 2,
                column: "Location",
                value: "");

            migrationBuilder.UpdateData(
                table: "Activity",
                keyColumn: "Id",
                keyValue: 3,
                column: "Location",
                value: "");
        }
    }
}
