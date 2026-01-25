using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GodScheduler.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddOnahamaSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cargos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    work_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    work_name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    cargo_name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    quantity = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    work_place = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    required_skill = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    s_time = table.Column<TimeSpan>(type: "time", nullable: true),
                    e_time = table.Column<TimeSpan>(type: "time", nullable: true),
                    conf_flg = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cargos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wokers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    s_name = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    competence_bhh = table.Column<int>(type: "int", nullable: false),
                    competence_wwm = table.Column<int>(type: "int", nullable: false),
                    branch_cd = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    created_uid = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wokers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cargos");

            migrationBuilder.DropTable(
                name: "Wokers");
        }
    }
}
