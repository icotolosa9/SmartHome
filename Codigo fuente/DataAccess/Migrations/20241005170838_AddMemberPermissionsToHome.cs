using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddMemberPermissionsToHome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemberIds",
                table: "Homes");

            migrationBuilder.CreateTable(
                name: "HomeOwnerPermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HomeOwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Permission = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeOwnerPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomeOwnerPermissions_Homes_HomeId",
                        column: x => x.HomeId,
                        principalTable: "Homes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HomeOwnerPermissions_HomeId",
                table: "HomeOwnerPermissions",
                column: "HomeId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeOwnerPermissions_HomeOwnerId",
                table: "HomeOwnerPermissions",
                column: "HomeOwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HomeOwnerPermissions");

            migrationBuilder.AddColumn<string>(
                name: "MemberIds",
                table: "Homes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Homes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
