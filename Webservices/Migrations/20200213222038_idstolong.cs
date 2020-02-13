using Microsoft.EntityFrameworkCore.Migrations;

namespace Webservices.Migrations
{
    public partial class idstolong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_Category_CategoryID1",
                table: "Recipe");

            migrationBuilder.DropIndex(
                name: "IX_Recipe_CategoryID1",
                table: "Recipe");

            migrationBuilder.DropColumn(
                name: "CategoryID1",
                table: "Recipe");

            migrationBuilder.AlterColumn<long>(
                name: "CategoryID",
                table: "Recipe",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_CategoryID",
                table: "Recipe",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_Category_CategoryID",
                table: "Recipe",
                column: "CategoryID",
                principalTable: "Category",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_Category_CategoryID",
                table: "Recipe");

            migrationBuilder.DropIndex(
                name: "IX_Recipe_CategoryID",
                table: "Recipe");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryID",
                table: "Recipe",
                type: "int",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "CategoryID1",
                table: "Recipe",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_CategoryID1",
                table: "Recipe",
                column: "CategoryID1");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_Category_CategoryID1",
                table: "Recipe",
                column: "CategoryID1",
                principalTable: "Category",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
