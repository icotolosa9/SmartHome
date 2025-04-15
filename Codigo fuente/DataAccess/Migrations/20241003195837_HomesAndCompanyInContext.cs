using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class HomesAndCompanyInContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Home1_HomeId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_HomeId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Home1",
                table: "Home1");

            migrationBuilder.DropColumn(
                name: "HomeId",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Home1",
                newName: "Homes");

            migrationBuilder.AddColumn<Guid>(
                name: "HomeOwnerId",
                table: "Homes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Homes",
                table: "Homes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyOwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_Users_CompanyOwnerId",
                        column: x => x.CompanyOwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Homes_HomeOwnerId",
                table: "Homes",
                column: "HomeOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CompanyOwnerId",
                table: "Companies",
                column: "CompanyOwnerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Homes_Users_HomeOwnerId",
                table: "Homes",
                column: "HomeOwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homes_Users_HomeOwnerId",
                table: "Homes");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Homes",
                table: "Homes");

            migrationBuilder.DropIndex(
                name: "IX_Homes_HomeOwnerId",
                table: "Homes");

            migrationBuilder.DropColumn(
                name: "HomeOwnerId",
                table: "Homes");

            migrationBuilder.RenameTable(
                name: "Homes",
                newName: "Home1");

            migrationBuilder.AddColumn<Guid>(
                name: "HomeId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Home1",
                table: "Home1",
                column: "Id");

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
    }
}
