using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Endure.Data.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClimateDevice_StorageUnit_WarehouseId_StorageUnitId",
                table: "ClimateDevice");

            migrationBuilder.RenameColumn(
                name: "WarehouseId",
                table: "ClimateDevice",
                newName: "WareHouseId");

            migrationBuilder.RenameIndex(
                name: "IX_ClimateDevice_WarehouseId_StorageUnitId",
                table: "ClimateDevice",
                newName: "IX_ClimateDevice_WareHouseId_StorageUnitId");

            migrationBuilder.AlterColumn<double>(
                name: "Temperature",
                table: "ClimateTelemetry",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Humidity",
                table: "ClimateTelemetry",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SetHumidity",
                table: "ClimateDevice",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SetTemperature",
                table: "ClimateDevice",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddForeignKey(
                name: "FK_ClimateDevice_StorageUnit_WareHouseId_StorageUnitId",
                table: "ClimateDevice",
                columns: new[] { "WareHouseId", "StorageUnitId" },
                principalTable: "StorageUnit",
                principalColumns: new[] { "WarehouseId", "Id" },
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClimateDevice_StorageUnit_WareHouseId_StorageUnitId",
                table: "ClimateDevice");

            migrationBuilder.DropColumn(
                name: "SetHumidity",
                table: "ClimateDevice");

            migrationBuilder.DropColumn(
                name: "SetTemperature",
                table: "ClimateDevice");

            migrationBuilder.RenameColumn(
                name: "WareHouseId",
                table: "ClimateDevice",
                newName: "WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_ClimateDevice_WareHouseId_StorageUnitId",
                table: "ClimateDevice",
                newName: "IX_ClimateDevice_WarehouseId_StorageUnitId");

            migrationBuilder.AlterColumn<double>(
                name: "Temperature",
                table: "ClimateTelemetry",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<double>(
                name: "Humidity",
                table: "ClimateTelemetry",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AddForeignKey(
                name: "FK_ClimateDevice_StorageUnit_WarehouseId_StorageUnitId",
                table: "ClimateDevice",
                columns: new[] { "WarehouseId", "StorageUnitId" },
                principalTable: "StorageUnit",
                principalColumns: new[] { "WarehouseId", "Id" },
                onDelete: ReferentialAction.SetNull);
        }
    }
}
