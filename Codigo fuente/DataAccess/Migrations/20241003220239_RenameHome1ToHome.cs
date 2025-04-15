using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RenameHome1ToHome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "HomeId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Homes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Homes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Homes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Users_HomeId",
                table: "Users",
                column: "HomeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_HomeId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HomeId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Homes");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Homes");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Homes");
        }
    }
}
