using Microsoft.EntityFrameworkCore.Migrations;

namespace ETicket.Repository.Migrations
{
    public partial class Sekundo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieInShoppingCarts",
                table: "MovieInShoppingCarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieInOrder",
                table: "MovieInOrder");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "MovieInOrder",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieInShoppingCarts",
                table: "MovieInShoppingCarts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieInOrder",
                table: "MovieInOrder",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MovieInShoppingCarts_MovieId",
                table: "MovieInShoppingCarts",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieInOrder_MovieId",
                table: "MovieInOrder",
                column: "MovieId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieInShoppingCarts",
                table: "MovieInShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_MovieInShoppingCarts_MovieId",
                table: "MovieInShoppingCarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieInOrder",
                table: "MovieInOrder");

            migrationBuilder.DropIndex(
                name: "IX_MovieInOrder_MovieId",
                table: "MovieInOrder");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "MovieInOrder");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieInShoppingCarts",
                table: "MovieInShoppingCarts",
                columns: new[] { "MovieId", "ShoppingCartId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieInOrder",
                table: "MovieInOrder",
                columns: new[] { "MovieId", "OrderId" });
        }
    }
}
