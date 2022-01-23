using Microsoft.EntityFrameworkCore.Migrations;

namespace PersianMemo.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersianWord = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Translation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Difficulty = table.Column<int>(type: "int", nullable: false),
                    PhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Words",
                columns: new[] { "Id", "Difficulty", "PersianWord", "PhotoPath", "Translation" },
                values: new object[] { 1, 1, "خونه", null, "House" });

            migrationBuilder.InsertData(
                table: "Words",
                columns: new[] { "Id", "Difficulty", "PersianWord", "PhotoPath", "Translation" },
                values: new object[] { 2, 1, "ایران", null, "Iran" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Words");
        }
    }
}
