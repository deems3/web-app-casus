using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Victuz_MVC.Migrations
{
    /// <inheritdoc />
    public partial class SeedDbWithActivities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ActivityCategory",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Sport" },
                    { 2, "Programming" },
                    { 3, "AI" }
                });

            migrationBuilder.InsertData(
                table: "Activity",
                columns: new[] { "Id", "ActivityCategoryId", "DateTime", "Description", "Limit", "Name", "Status" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 11, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Versla je medestudenten", 30, "Voetbal toernooi", 1 },
                    { 2, 2, new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bouw & programmeer je eigen BattleBot en versla je medestudenten", 30, "BattleBots", 1 },
                    { 3, 3, new DateTime(2024, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Leer een eigen AI applicatie te bouwen", 13, "Creatief met AI", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Activity",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Activity",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Activity",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ActivityCategory",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ActivityCategory",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ActivityCategory",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
