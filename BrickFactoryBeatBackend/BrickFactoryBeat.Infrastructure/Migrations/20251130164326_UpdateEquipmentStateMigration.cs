using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrickFactoryBeat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEquipmentStateMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NewState",
                table: "StateHistory",
                newName: "State");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "State",
                table: "StateHistory",
                newName: "NewState");
        }
    }
}
