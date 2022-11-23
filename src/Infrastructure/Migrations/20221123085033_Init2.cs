using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserInfo_UserInfoId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CreateTime",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ModifyTime",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserInfo",
                table: "UserInfo");

            migrationBuilder.DropIndex(
                name: "IX_UserInfo_CreateTime",
                table: "UserInfo");

            migrationBuilder.DropIndex(
                name: "IX_UserInfo_ModifyTime",
                table: "UserInfo");

            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ModifyTime",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "UserInfo");

            migrationBuilder.DropColumn(
                name: "ModifyTime",
                table: "UserInfo");

            migrationBuilder.RenameTable(
                name: "UserInfo",
                newName: "UserInfos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserInfos",
                table: "UserInfos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserInfos_UserInfoId",
                table: "Users",
                column: "UserInfoId",
                principalTable: "UserInfos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserInfos_UserInfoId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserInfos",
                table: "UserInfos");

            migrationBuilder.RenameTable(
                name: "UserInfos",
                newName: "UserInfo");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "Users",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifyTime",
                table: "Users",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "UserInfo",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifyTime",
                table: "UserInfo",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserInfo",
                table: "UserInfo",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreateTime",
                table: "Users",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ModifyTime",
                table: "Users",
                column: "ModifyTime");

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
    }
}
