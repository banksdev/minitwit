using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Minitwit.DataAccessLayer.Migrations
{
    public partial class initial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_UserId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_UserId",
                table: "Messages");

            migrationBuilder.AddColumn<Guid>(
                name: "TestKey",
                table: "Messages",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_TestKey",
                table: "Messages",
                column: "TestKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_TestKey",
                table: "Messages",
                column: "TestKey",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_TestKey",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_TestKey",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "TestKey",
                table: "Messages");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserId",
                table: "Messages",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_UserId",
                table: "Messages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
