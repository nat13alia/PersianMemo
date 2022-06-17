using Microsoft.EntityFrameworkCore.Migrations;

namespace PersianMemo.Migrations
{
    public partial class EditedWordsEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Words",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Words_UserId",
                table: "Words",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Words_AspNetUsers_UserId",
                table: "Words",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Words_AspNetUsers_UserId",
                table: "Words");

            migrationBuilder.DropIndex(
                name: "IX_Words_UserId",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Words");
        }
    }
}
