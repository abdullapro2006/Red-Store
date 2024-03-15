using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RedStore.Migrations
{
    public partial class Seed_Admin_User_Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "Email", "Password" },
                values: new object[] { "superadmin@gmail.com", "123321am" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "Email", "Password" },
                values: new object[] { null, null });
        }
    }
}
