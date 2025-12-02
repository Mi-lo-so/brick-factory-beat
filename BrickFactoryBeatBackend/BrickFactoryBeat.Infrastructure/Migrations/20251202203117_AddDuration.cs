using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrickFactoryBeat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Equipment_EquipmentId",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EquipmentId1",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_EquipmentId1",
                table: "Orders",
                column: "EquipmentId1",
                unique: true,
                filter: "[EquipmentId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Equipment_EquipmentId",
                table: "Orders",
                column: "EquipmentId",
                principalTable: "Equipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Equipment_EquipmentId1",
                table: "Orders",
                column: "EquipmentId1",
                principalTable: "Equipment",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Equipment_EquipmentId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Equipment_EquipmentId1",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_EquipmentId1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "EquipmentId1",
                table: "Orders");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Equipment_EquipmentId",
                table: "Orders",
                column: "EquipmentId",
                principalTable: "Equipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
