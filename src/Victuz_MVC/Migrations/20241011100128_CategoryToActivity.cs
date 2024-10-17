using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Victuz_MVC.Migrations
{
    /// <inheritdoc />
    public partial class CategoryToActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Activity");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Activity",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ActivityCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityCategory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activity_CategoryId",
                table: "Activity",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_ActivityCategory_CategoryId",
                table: "Activity",
                column: "CategoryId",
                principalTable: "ActivityCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_ActivityCategory_CategoryId",
                table: "Activity");

            migrationBuilder.DropTable(
                name: "ActivityCategory");

            migrationBuilder.DropIndex(
                name: "IX_Activity_CategoryId",
                table: "Activity");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Activity");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Activity",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
