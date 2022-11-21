using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_City",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Address_Country",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Address_State",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Birthday",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Address_Street",
                table: "Users",
                newName: "Password");

            migrationBuilder.AddColumn<int>(
                name: "UserInfoId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    Birthday = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CardId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AddressStreet = table.Column<string>(name: "Address_Street", type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AddressCity = table.Column<string>(name: "Address_City", type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AddressCountry = table.Column<string>(name: "Address_Country", type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AddressState = table.Column<string>(name: "Address_State", type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifyTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserInfoId",
                table: "Users",
                column: "UserInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInfo_CreateTime",
                table: "UserInfo",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_UserInfo_ModifyTime",
                table: "UserInfo",
                column: "ModifyTime");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserInfo_UserInfoId",
                table: "Users",
                column: "UserInfoId",
                principalTable: "UserInfo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserInfo_UserInfoId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UserInfo");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserInfoId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserInfoId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "Address_Street");

            migrationBuilder.AddColumn<string>(
                name: "Address_City",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_Country",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_State",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Birthday",
                table: "Users",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}
