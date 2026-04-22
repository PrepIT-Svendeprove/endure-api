using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Endure.Data.Migrations
{
    /// <inheritdoc />
    public partial class changed_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LogType",
                table: "AuditLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AuditLog",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogType",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AuditLog");
        }
    }
}
