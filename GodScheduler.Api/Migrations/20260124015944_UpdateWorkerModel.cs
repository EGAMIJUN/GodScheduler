using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GodScheduler.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWorkerModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FatigueLevel",
                table: "Wokers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Skills",
                table: "Wokers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FatigueLevel",
                table: "Wokers");

            migrationBuilder.DropColumn(
                name: "Skills",
                table: "Wokers");
        }
    }
}
