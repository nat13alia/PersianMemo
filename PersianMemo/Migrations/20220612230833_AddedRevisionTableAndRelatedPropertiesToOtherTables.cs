using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PersianMemo.Migrations
{
    public partial class AddedRevisionTableAndRelatedPropertiesToOtherTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "NextRevision",
                table: "Words",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "RevisionInterval",
                table: "Words",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RevisionsCount",
                table: "Words",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Revisions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    WordId = table.Column<int>(type: "int", nullable: false),
                    WriteAnswer = table.Column<int>(type: "int", nullable: false),
                    RevisionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Revisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Revisions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Revisions_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 1,
                column: "RevisionInterval",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Revisions_UserId",
                table: "Revisions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Revisions_WordId",
                table: "Revisions",
                column: "WordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Revisions");

            migrationBuilder.DropColumn(
                name: "NextRevision",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "RevisionInterval",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "RevisionsCount",
                table: "Words");
        }
    }
}
