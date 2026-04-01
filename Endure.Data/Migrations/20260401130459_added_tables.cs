using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Endure.Data.Migrations
{
    /// <inheritdoc />
    public partial class added_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<int>(type: "integer", nullable: false),
                    LogLevel = table.Column<int>(type: "integer", nullable: false),
                    ModuleType = table.Column<int>(type: "integer", nullable: false),
                    Log = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DietaryRestrictionType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DietaryRestrictionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DietaryRestriction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HashedCpr = table.Column<string>(type: "text", nullable: false),
                    DietaryRestrictionTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DietaryRestriction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DietaryRestriction_DietaryRestrictionType_DietaryRestrictio~",
                        column: x => x.DietaryRestrictionTypeId,
                        principalTable: "DietaryRestrictionType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DietaryRestriction_DietaryRestrictionTypeId",
                table: "DietaryRestriction",
                column: "DietaryRestrictionTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLog");

            migrationBuilder.DropTable(
                name: "DietaryRestriction");

            migrationBuilder.DropTable(
                name: "DietaryRestrictionType");
        }
    }
}
