using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Endure.Data.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warehouse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsRoot = table.Column<bool>(type: "boolean", nullable: false),
                    ParentWarehouseId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Warehouse_Warehouse_ParentWarehouseId",
                        column: x => x.ParentWarehouseId,
                        principalTable: "Warehouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StorageUnit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ShortName = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ParentStorageUnitId = table.Column<Guid>(type: "uuid", nullable: true),
                    StorageType = table.Column<int>(type: "integer", nullable: false),
                    IsSlot = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageUnit", x => new { x.WarehouseId, x.Id });
                    table.ForeignKey(
                        name: "FK_StorageUnit_StorageUnit_WarehouseId_ParentStorageUnitId",
                        columns: x => new { x.WarehouseId, x.ParentStorageUnitId },
                        principalTable: "StorageUnit",
                        principalColumns: new[] { "WarehouseId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StorageUnit_Warehouse_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClimateDevice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LastReceived = table.Column<long>(type: "bigint", nullable: false),
                    IsConnected = table.Column<bool>(type: "boolean", nullable: false),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    StorageUnitId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClimateDevice", x => new { x.WarehouseId, x.Id });
                    table.ForeignKey(
                        name: "FK_ClimateDevice_StorageUnit_WarehouseId_StorageUnitId",
                        columns: x => new { x.WarehouseId, x.StorageUnitId },
                        principalTable: "StorageUnit",
                        principalColumns: new[] { "WarehouseId", "Id" },
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ProductBatch",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<int>(type: "integer", nullable: false),
                    BestBefore = table.Column<long>(type: "bigint", nullable: false),
                    LastUpdatedAt = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<string>(type: "character varying(20)", nullable: false),
                    StorageUnitId = table.Column<Guid>(type: "uuid", nullable: false),
                    Count = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBatch", x => new { x.WarehouseId, x.Id });
                    table.ForeignKey(
                        name: "FK_ProductBatch_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductBatch_StorageUnit_WarehouseId_StorageUnitId",
                        columns: x => new { x.WarehouseId, x.StorageUnitId },
                        principalTable: "StorageUnit",
                        principalColumns: new[] { "WarehouseId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClimateTelemetry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<int>(type: "integer", nullable: false),
                    Temperature = table.Column<double>(type: "double precision", nullable: true),
                    Humidity = table.Column<double>(type: "double precision", nullable: true),
                    ClimateDeviceId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClimateTelemetry", x => new { x.WarehouseId, x.Id });
                    table.ForeignKey(
                        name: "FK_ClimateTelemetry_ClimateDevice_WarehouseId_Id",
                        columns: x => new { x.WarehouseId, x.Id },
                        principalTable: "ClimateDevice",
                        principalColumns: new[] { "WarehouseId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClimateDevice_WarehouseId_StorageUnitId",
                table: "ClimateDevice",
                columns: new[] { "WarehouseId", "StorageUnitId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductBatch_ProductId",
                table: "ProductBatch",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBatch_WarehouseId_StorageUnitId",
                table: "ProductBatch",
                columns: new[] { "WarehouseId", "StorageUnitId" });

            migrationBuilder.CreateIndex(
                name: "IX_StorageUnit_WarehouseId_ParentStorageUnitId",
                table: "StorageUnit",
                columns: new[] { "WarehouseId", "ParentStorageUnitId" });

            migrationBuilder.CreateIndex(
                name: "IX_Warehouse_ParentWarehouseId",
                table: "Warehouse",
                column: "ParentWarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClimateTelemetry");

            migrationBuilder.DropTable(
                name: "ProductBatch");

            migrationBuilder.DropTable(
                name: "ClimateDevice");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "StorageUnit");

            migrationBuilder.DropTable(
                name: "Warehouse");
        }
    }
}
