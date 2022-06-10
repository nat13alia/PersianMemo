using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PersianMemo.Migrations
{
    public partial class AddedExerciseTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "EF",
                table: "Words",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Words",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exercises_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExercisesWords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    WordId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExercisesWords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExercisesWords_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExercisesWords_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EF", "Status" },
                values: new object[] { 2.5, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_UserId",
                table: "Exercises",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExercisesWords_ExerciseId",
                table: "ExercisesWords",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExercisesWords_WordId",
                table: "ExercisesWords",
                column: "WordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExercisesWords");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropColumn(
                name: "EF",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Words");
        }
    }
}
