using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_CreateTime",
                table: "Users",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ModifyTime",
                table: "Users",
                column: "ModifyTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_CreateTime",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ModifyTime",
                table: "Users");
        }
    }
}
