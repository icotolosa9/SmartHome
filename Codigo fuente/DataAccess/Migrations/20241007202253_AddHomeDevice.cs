using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddHomeDevice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HomeDevices",
                columns: table => new
                {
                    HardwareId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Connected = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeDevices", x => x.HardwareId);
                    table.ForeignKey(
                        name: "FK_HomeDevices_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HomeDevices_Homes_HomeId",
                        column: x => x.HomeId,
                        principalTable: "Homes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HomeDevices_DeviceId",
                table: "HomeDevices",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeDevices_HomeId",
                table: "HomeDevices",
                column: "HomeId");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HomeDevices");

            migrationBuilder.DropTable(
                name: "HomeOwnerPermissions");

            migrationBuilder.AddColumn<string>(
                name: "MemberIds",
                table: "Homes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
