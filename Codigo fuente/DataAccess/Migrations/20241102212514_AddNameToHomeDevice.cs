using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddNameToHomeDevice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("acfef33f-af33-468f-85df-b58c16d71e4c"));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "HomeDevices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreationDate", "Email", "FirstName", "HomeId", "LastName", "Password", "Role" },
                values: new object[] { new Guid("9056eee7-c7e8-4f60-bf34-3271c81df1d6"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@smarthome.com", "Admin", null, "User", "Password123!", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("9056eee7-c7e8-4f60-bf34-3271c81df1d6"));

            migrationBuilder.DropColumn(
                name: "Name",
                table: "HomeDevices");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreationDate", "Email", "FirstName", "HomeId", "LastName", "Password", "Role" },
                values: new object[] { new Guid("acfef33f-af33-468f-85df-b58c16d71e4c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@smarthome.com", "Admin", null, "User", "Password123!", "admin" });
        }
    }
}
