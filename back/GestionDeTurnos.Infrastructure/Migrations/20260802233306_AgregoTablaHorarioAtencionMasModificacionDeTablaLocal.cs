using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionDeTurnos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregoTablaHorarioAtencionMasModificacionDeTablaLocal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "HoraCierre",
                table: "HorariosAtencion",
                type: "time",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "interval");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "HoraApertura",
                table: "HorariosAtencion",
                type: "time",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "interval");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "HoraCierre",
                table: "HorariosAtencion",
                type: "interval",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "HoraApertura",
                table: "HorariosAtencion",
                type: "interval",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");
        }
    }
}
