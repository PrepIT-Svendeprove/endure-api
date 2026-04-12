using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Endure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Removed_DietaryRestrictions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DietaryRestriction");

            migrationBuilder.DropTable(
                name: "DietaryRestrictionType");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBatch_Warehouse_WarehouseId",
                table: "ProductBatch",
                column: "WarehouseId",
                principalTable: "Warehouse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBatch_Warehouse_WarehouseId",
                table: "ProductBatch");

            migrationBuilder.CreateTable(
                name: "DietaryRestrictionType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    NormalizedName = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true),
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
                    DietaryRestrictionTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    HashedCpr = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true),
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
    }
}
