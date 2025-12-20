using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sosa.Gym.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Dni",
                table: "Usuarios",
                column: "Dni",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Usuarios_Dni",
                table: "Usuarios");
        }
    }
}
