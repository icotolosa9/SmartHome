using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CompanyIdInCompanyOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Users_OwnerId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_OwnerId",
                table: "Companies");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("52f77595-d638-473c-8ae7-4607e64ad36d"));

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreationDate", "Email", "FirstName", "LastName", "Password", "Role" },
                values: new object[] { new Guid("3ba83bbc-78a5-41d9-9508-4639b101bdab"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@smarthome.com", "Admin", "User", "Password123!", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_CompanyId",
                table: "Users",
                column: "CompanyId",
                unique: true,
                filter: "[CompanyId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CompanyId",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3ba83bbc-78a5-41d9-9508-4639b101bdab"));

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreationDate", "Email", "FirstName", "LastName", "Password", "Role" },
                values: new object[] { new Guid("52f77595-d638-473c-8ae7-4607e64ad36d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@smarthome.com", "Admin", "User", "Password123!", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_OwnerId",
                table: "Companies",
                column: "OwnerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Users_OwnerId",
                table: "Companies",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
