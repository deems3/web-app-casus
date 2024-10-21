using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Victuz_MVC.Migrations
{
    /// <inheritdoc />
    public partial class FixManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountActivity_Activity_ActivityId",
                table: "AccountActivity");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountActivity_AspNetUsers_AccountId",
                table: "AccountActivity");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Activity_ActivityId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ActivityId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountActivity",
                table: "AccountActivity");

            migrationBuilder.DropIndex(
                name: "IX_AccountActivity_AccountId",
                table: "AccountActivity");

            migrationBuilder.DropIndex(
                name: "IX_AccountActivity_ActivityId",
                table: "AccountActivity");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AccountActivity");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "AccountActivity");

            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                table: "AccountActivity");

            migrationBuilder.RenameColumn(
                name: "ActivityId",
                table: "AccountActivity",
                newName: "ActivitiesId");

            migrationBuilder.AddColumn<string>(
                name: "HostsId",
                table: "AccountActivity",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountActivity",
                table: "AccountActivity",
                columns: new[] { "ActivitiesId", "HostsId" });

            migrationBuilder.CreateIndex(
                name: "IX_AccountActivity_HostsId",
                table: "AccountActivity",
                column: "HostsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountActivity_Activity_ActivitiesId",
                table: "AccountActivity",
                column: "ActivitiesId",
                principalTable: "Activity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountActivity_AspNetUsers_HostsId",
                table: "AccountActivity",
                column: "HostsId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountActivity_Activity_ActivitiesId",
                table: "AccountActivity");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountActivity_AspNetUsers_HostsId",
                table: "AccountActivity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountActivity",
                table: "AccountActivity");

            migrationBuilder.DropIndex(
                name: "IX_AccountActivity_HostsId",
                table: "AccountActivity");

            migrationBuilder.DropColumn(
                name: "HostsId",
                table: "AccountActivity");

            migrationBuilder.RenameColumn(
                name: "ActivitiesId",
                table: "AccountActivity",
                newName: "ActivityId");

            migrationBuilder.AddColumn<int>(
                name: "ActivityId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "AccountActivity",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "AccountId",
                table: "AccountActivity",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDate",
                table: "AccountActivity",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountActivity",
                table: "AccountActivity",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ActivityId",
                table: "AspNetUsers",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountActivity_AccountId",
                table: "AccountActivity",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountActivity_ActivityId",
                table: "AccountActivity",
                column: "ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountActivity_Activity_ActivityId",
                table: "AccountActivity",
                column: "ActivityId",
                principalTable: "Activity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountActivity_AspNetUsers_AccountId",
                table: "AccountActivity",
                column: "AccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Activity_ActivityId",
                table: "AspNetUsers",
                column: "ActivityId",
                principalTable: "Activity",
                principalColumn: "Id");
        }
    }
}
