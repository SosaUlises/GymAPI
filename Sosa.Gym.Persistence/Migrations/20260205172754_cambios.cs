using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sosa.Gym.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class cambios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activa",
                table: "RutinasAsignadas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activa",
                table: "RutinasAsignadas",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
