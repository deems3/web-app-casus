using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Victuz_MVC.Migrations
{
    /// <inheritdoc />
    public partial class AddBlacklistedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Blacklisted",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Blacklisted",
                table: "AspNetUsers");
        }
    }
}
