using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ETicket.Repository.Migrations
{
    public partial class Sedond : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieInOrder_Movies_MovieId",
                table: "MovieInOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieInOrder_Order_OrderId",
                table: "MovieInOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_UserId",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieInOrder",
                table: "MovieInOrder");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "MovieInOrder",
                newName: "ProductInOrders");

            migrationBuilder.RenameIndex(
                name: "IX_Order_UserId",
                table: "Orders",
                newName: "IX_Orders_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieInOrder_OrderId",
                table: "ProductInOrders",
                newName: "IX_ProductInOrders_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieInOrder_MovieId",
                table: "ProductInOrders",
                newName: "IX_ProductInOrders_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductInOrders",
                table: "ProductInOrders",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "EmailMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MailTo = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailMessages", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInOrders_Orders_MovieId",
                table: "ProductInOrders",
                column: "MovieId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInOrders_Movies_OrderId",
                table: "ProductInOrders",
                column: "OrderId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductInOrders_Orders_MovieId",
                table: "ProductInOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductInOrders_Movies_OrderId",
                table: "ProductInOrders");

            migrationBuilder.DropTable(
                name: "EmailMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductInOrders",
                table: "ProductInOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "ProductInOrders",
                newName: "MovieInOrder");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Order");

            migrationBuilder.RenameIndex(
                name: "IX_ProductInOrders_OrderId",
                table: "MovieInOrder",
                newName: "IX_MovieInOrder_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductInOrders_MovieId",
                table: "MovieInOrder",
                newName: "IX_MovieInOrder_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_UserId",
                table: "Order",
                newName: "IX_Order_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieInOrder",
                table: "MovieInOrder",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieInOrder_Movies_MovieId",
                table: "MovieInOrder",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieInOrder_Order_OrderId",
                table: "MovieInOrder",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_UserId",
                table: "Order",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
