using Microsoft.EntityFrameworkCore.Migrations;

namespace PersianMemo.Migrations
{
    public partial class AddedAnswerAndListenProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DidListen",
                table: "ExercisesWords",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "WriteAnswer",
                table: "ExercisesWords",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DidListen",
                table: "ExercisesWords");

            migrationBuilder.DropColumn(
                name: "WriteAnswer",
                table: "ExercisesWords");
        }
    }
}
