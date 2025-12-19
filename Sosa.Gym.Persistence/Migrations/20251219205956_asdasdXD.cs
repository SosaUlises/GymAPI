using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sosa.Gym.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class asdasdXD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cuotas_ClienteId",
                table: "Cuotas");

            migrationBuilder.CreateIndex(
                name: "IX_Cuotas_ClienteId_Anio_Mes",
                table: "Cuotas",
                columns: new[] { "ClienteId", "Anio", "Mes" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cuotas_ClienteId_Anio_Mes",
                table: "Cuotas");

            migrationBuilder.CreateIndex(
                name: "IX_Cuotas_ClienteId",
                table: "Cuotas",
                column: "ClienteId");
        }
    }
}
