using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Endure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Removed_Relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Warehouse_Warehouse_ParentId",
                table: "Warehouse");

            migrationBuilder.DropIndex(
                name: "IX_Warehouse_ParentId",
                table: "Warehouse");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Warehouse_ParentId",
                table: "Warehouse",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouse_Warehouse_ParentId",
                table: "Warehouse",
                column: "ParentId",
                principalTable: "Warehouse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
