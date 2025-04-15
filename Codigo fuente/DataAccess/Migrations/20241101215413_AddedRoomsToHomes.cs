using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedRoomsToHomes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("6dde67df-c250-4f56-be8e-493e08d807b6"));

            migrationBuilder.AddColumn<Guid>(
                name: "RoomId",
                table: "HomeDevices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Room_Homes_HomeId",
                        column: x => x.HomeId,
                        principalTable: "Homes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreationDate", "Email", "FirstName", "HomeId", "LastName", "Password", "Role" },
                values: new object[] { new Guid("acfef33f-af33-468f-85df-b58c16d71e4c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@smarthome.com", "Admin", null, "User", "Password123!", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_HomeDevices_RoomId",
                table: "HomeDevices",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Room_HomeId",
                table: "Room",
                column: "HomeId");

            migrationBuilder.AddForeignKey(
                name: "FK_HomeDevices_Room_RoomId",
                table: "HomeDevices",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HomeDevices_Room_RoomId",
                table: "HomeDevices");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropIndex(
                name: "IX_HomeDevices_RoomId",
                table: "HomeDevices");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("acfef33f-af33-468f-85df-b58c16d71e4c"));

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "HomeDevices");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreationDate", "Email", "FirstName", "HomeId", "LastName", "Password", "Role" },
                values: new object[] { new Guid("6dde67df-c250-4f56-be8e-493e08d807b6"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@smarthome.com", "Admin", null, "User", "Password123!", "admin" });
        }
    }
}
