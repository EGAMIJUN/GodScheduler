using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GodScheduler.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignedWorkerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedWorkerId",
                table: "Cargos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedWorkerId",
                table: "Cargos");
        }
    }
}
