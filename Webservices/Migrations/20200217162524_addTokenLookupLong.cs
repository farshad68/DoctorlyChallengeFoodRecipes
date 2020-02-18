using Microsoft.EntityFrameworkCore.Migrations;

namespace Webservices.Migrations
{
    public partial class addTokenLookupLong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeTokenLookUP_Recipe_RecipeID1",
                table: "RecipeTokenLookUP");

            migrationBuilder.DropIndex(
                name: "IX_RecipeTokenLookUP_RecipeID1",
                table: "RecipeTokenLookUP");

            migrationBuilder.DropColumn(
                name: "RecipeID1",
                table: "RecipeTokenLookUP");

            migrationBuilder.AlterColumn<long>(
                name: "RecipeID",
                table: "RecipeTokenLookUP",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeTokenLookUP_RecipeID",
                table: "RecipeTokenLookUP",
                column: "RecipeID");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeTokenLookUP_Recipe_RecipeID",
                table: "RecipeTokenLookUP",
                column: "RecipeID",
                principalTable: "Recipe",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeTokenLookUP_Recipe_RecipeID",
                table: "RecipeTokenLookUP");

            migrationBuilder.DropIndex(
                name: "IX_RecipeTokenLookUP_RecipeID",
                table: "RecipeTokenLookUP");

            migrationBuilder.AlterColumn<bool>(
                name: "RecipeID",
                table: "RecipeTokenLookUP",
                type: "bit",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "RecipeID1",
                table: "RecipeTokenLookUP",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeTokenLookUP_RecipeID1",
                table: "RecipeTokenLookUP",
                column: "RecipeID1");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeTokenLookUP_Recipe_RecipeID1",
                table: "RecipeTokenLookUP",
                column: "RecipeID1",
                principalTable: "Recipe",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
