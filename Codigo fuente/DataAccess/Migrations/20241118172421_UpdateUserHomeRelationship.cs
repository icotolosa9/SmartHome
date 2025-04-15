using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserHomeRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homes_Users_HomeOwnerId",
                table: "Homes");

            migrationBuilder.DropIndex(
                name: "IX_Users_HomeId",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("98522381-6473-4f52-8183-11604796ac78"));

            migrationBuilder.DropColumn(
                name: "HomeId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "HomeUser",
                columns: table => new
                {
                    HomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeUser", x => new { x.HomeId, x.UserId });
                    table.ForeignKey(
                        name: "FK_HomeUser_Homes_HomeId",
                        column: x => x.HomeId,
                        principalTable: "Homes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HomeUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreationDate", "Email", "FirstName", "LastName", "Password", "Role" },
                values: new object[] { new Guid("24ade9fd-19e8-4fab-b4f7-5234d16bc7e2"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@smarthome.com", "Admin", "User", "Password123!", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_HomeUser_UserId",
                table: "HomeUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Homes_Users_HomeOwnerId",
                table: "Homes",
                column: "HomeOwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homes_Users_HomeOwnerId",
                table: "Homes");

            migrationBuilder.DropTable(
                name: "HomeUser");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("24ade9fd-19e8-4fab-b4f7-5234d16bc7e2"));

            migrationBuilder.AddColumn<Guid>(
                name: "HomeId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreationDate", "Email", "FirstName", "HomeId", "LastName", "Password", "Role" },
                values: new object[] { new Guid("98522381-6473-4f52-8183-11604796ac78"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@smarthome.com", "Admin", null, "User", "Password123!", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_HomeId",
                table: "Users",
                column: "HomeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Homes_Users_HomeOwnerId",
                table: "Homes",
                column: "HomeOwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Homes_HomeId",
                table: "Users",
                column: "HomeId",
                principalTable: "Homes",
                principalColumn: "Id");
        }
    }
}
