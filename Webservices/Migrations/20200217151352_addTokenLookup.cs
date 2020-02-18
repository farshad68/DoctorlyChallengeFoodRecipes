using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Webservices.Migrations
{
    public partial class addTokenLookup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecipeTokenLookUP",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<Guid>(nullable: false),
                    RecipeID = table.Column<bool>(nullable: false),
                    RecipeID1 = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeTokenLookUP", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RecipeTokenLookUP_Recipe_RecipeID1",
                        column: x => x.RecipeID1,
                        principalTable: "Recipe",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeTokenLookUP_RecipeID1",
                table: "RecipeTokenLookUP",
                column: "RecipeID1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeTokenLookUP");
        }
    }
}
