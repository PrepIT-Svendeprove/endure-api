using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Endure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Changed_product_key : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBatch_Product_ProductId",
                table: "ProductBatch");

            migrationBuilder.DropIndex(
                name: "IX_ProductBatch_ProductId",
                table: "ProductBatch");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product",
                table: "Product");

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseId",
                table: "Product",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                table: "Product",
                columns: new[] { "WarehouseId", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductBatch_WarehouseId_ProductId",
                table: "ProductBatch",
                columns: new[] { "WarehouseId", "ProductId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBatch_Product_WarehouseId_ProductId",
                table: "ProductBatch",
                columns: new[] { "WarehouseId", "ProductId" },
                principalTable: "Product",
                principalColumns: new[] { "WarehouseId", "Id" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBatch_Product_WarehouseId_ProductId",
                table: "ProductBatch");

            migrationBuilder.DropIndex(
                name: "IX_ProductBatch_WarehouseId_ProductId",
                table: "ProductBatch");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "Product");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                table: "Product",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBatch_ProductId",
                table: "ProductBatch",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBatch_Product_ProductId",
                table: "ProductBatch",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
