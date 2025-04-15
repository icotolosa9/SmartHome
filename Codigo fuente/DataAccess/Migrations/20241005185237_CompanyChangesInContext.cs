using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CompanyChangesInContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Users_CompanyOwnerId",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "CompanyOwnerId",
                table: "Companies",
                newName: "OwnerId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Companies",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Companies_CompanyOwnerId",
                table: "Companies",
                newName: "IX_Companies_OwnerId");

            migrationBuilder.AddColumn<string>(
                name: "LogoURL",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Rut",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Users_OwnerId",
                table: "Companies",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Users_OwnerId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "LogoURL",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Rut",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Companies",
                newName: "CompanyOwnerId");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Companies",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Companies_OwnerId",
                table: "Companies",
                newName: "IX_Companies_CompanyOwnerId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Homes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Users_CompanyOwnerId",
                table: "Companies",
                column: "CompanyOwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
