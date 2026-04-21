using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Endure.Data.Migrations
{
    /// <inheritdoc />
    public partial class @fixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClimateTelemetry_ClimateDevice_WarehouseId_Id",
                table: "ClimateTelemetry");

            migrationBuilder.CreateIndex(
                name: "IX_ClimateTelemetry_WarehouseId_ClimateDeviceId",
                table: "ClimateTelemetry",
                columns: new[] { "WarehouseId", "ClimateDeviceId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ClimateTelemetry_ClimateDevice_WarehouseId_ClimateDeviceId",
                table: "ClimateTelemetry",
                columns: new[] { "WarehouseId", "ClimateDeviceId" },
                principalTable: "ClimateDevice",
                principalColumns: new[] { "WareHouseId", "Id" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClimateTelemetry_ClimateDevice_WarehouseId_ClimateDeviceId",
                table: "ClimateTelemetry");

            migrationBuilder.DropIndex(
                name: "IX_ClimateTelemetry_WarehouseId_ClimateDeviceId",
                table: "ClimateTelemetry");

            migrationBuilder.AddForeignKey(
                name: "FK_ClimateTelemetry_ClimateDevice_WarehouseId_Id",
                table: "ClimateTelemetry",
                columns: new[] { "WarehouseId", "Id" },
                principalTable: "ClimateDevice",
                principalColumns: new[] { "WareHouseId", "Id" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
