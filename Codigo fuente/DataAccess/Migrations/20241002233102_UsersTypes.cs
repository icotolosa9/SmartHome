using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UsersTypes : Migration
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
                name: "UserType",
                table: "Users",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Home1",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Home1", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_HomeId",
                table: "Users",
                column: "HomeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Home1_HomeId",
                table: "Users",
                column: "HomeId",
                principalTable: "Home1",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Home1_HomeId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Home1");

            migrationBuilder.DropIndex(
                name: "IX_Users_HomeId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HomeId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "Users");
        }
    }
}
