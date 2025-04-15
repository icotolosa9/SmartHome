using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ModelValidatorStringInCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("24ade9fd-19e8-4fab-b4f7-5234d16bc7e2"));

            migrationBuilder.AddColumn<string>(
                name: "ValidatorModel",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreationDate", "Email", "FirstName", "LastName", "Password", "Role" },
                values: new object[] { new Guid("ba9c4dca-2859-416b-a7bc-608dd5beceff"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@smarthome.com", "Admin", "User", "Password123!", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ba9c4dca-2859-416b-a7bc-608dd5beceff"));

            migrationBuilder.DropColumn(
                name: "ValidatorModel",
                table: "Companies");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreationDate", "Email", "FirstName", "LastName", "Password", "Role" },
                values: new object[] { new Guid("24ade9fd-19e8-4fab-b4f7-5234d16bc7e2"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@smarthome.com", "Admin", "User", "Password123!", "admin" });
        }
    }
}
