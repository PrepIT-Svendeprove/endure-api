using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Endure.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixed_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Warehouse");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBatch_StorageUnitId",
                table: "ProductBatch",
                column: "StorageUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBatch_StorageUnit_StorageUnitId",
                table: "ProductBatch",
                column: "StorageUnitId",
                principalTable: "StorageUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBatch_StorageUnit_StorageUnitId",
                table: "ProductBatch");

            migrationBuilder.DropIndex(
                name: "IX_ProductBatch_StorageUnitId",
                table: "ProductBatch");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Warehouse",
                type: "integer",
                nullable: true);
        }
    }
}
