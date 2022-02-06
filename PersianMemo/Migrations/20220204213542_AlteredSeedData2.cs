using Microsoft.EntityFrameworkCore.Migrations;

namespace PersianMemo.Migrations
{
    public partial class AlteredSeedData2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PersianWord", "PhotoPath", "PronunciationPath", "Translation" },
                values: new object[] { "ایران", "bac074b4 - 83ad - 4ec5 - 8b8a - e498fe02e048_Iran.png", "d4838478-2fba-4c1c-864d-21c8cc6f941e_pronunciation_fa_ایران.mp3", "Iran" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PersianWord", "PhotoPath", "PronunciationPath", "Translation" },
                values: new object[] { "خونه", null, null, "Dom" });

            migrationBuilder.InsertData(
                table: "Words",
                columns: new[] { "Id", "Difficulty", "PersianWord", "PhotoPath", "PronunciationPath", "Translation" },
                values: new object[] { 2, 1, "ایران", null, null, "Iran" });
        }
    }
}
