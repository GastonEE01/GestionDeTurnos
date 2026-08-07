using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionDeTurnos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarIndicesUnicosTurnoYHorario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Turnos_LocalId",
                table: "Turnos");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_LocalId_Date",
                table: "Turnos",
                columns: new[] { "LocalId", "Date" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Turnos_LocalId_Date",
                table: "Turnos");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_LocalId",
                table: "Turnos",
                column: "LocalId");
        }
    }
}
