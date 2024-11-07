using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Victuz_MVC.Migrations
{
    /// <inheritdoc />
    public partial class AddedDateTimeToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_AccountId1",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_AccountId1",
                table: "Order");

            migrationBuilder.DeleteData(
                table: "Order",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "AccountId1",
                table: "Order");

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "Order",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "Order",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Order",
                type: "VARCHAR(255)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Order_AccountId",
                table: "Order",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_AccountId",
                table: "Order",
                column: "AccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_AccountId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_AccountId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Order");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Order",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "AccountId1",
                table: "Order",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Order",
                columns: new[] { "Id", "AccountId", "AccountId1", "TotalPrice" },
                values: new object[] { 1, 1, null, 60.00m });

            migrationBuilder.CreateIndex(
                name: "IX_Order_AccountId1",
                table: "Order",
                column: "AccountId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_AccountId1",
                table: "Order",
                column: "AccountId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
