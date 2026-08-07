using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionDeTurnos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregoRelacionUsuarioALocal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Locales_LocalId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_LocalId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "LocalId",
                table: "Usuarios");

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioId",
                table: "Locales",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Locales_UsuarioId",
                table: "Locales",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locales_Usuarios_UsuarioId",
                table: "Locales",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locales_Usuarios_UsuarioId",
                table: "Locales");

            migrationBuilder.DropIndex(
                name: "IX_Locales_UsuarioId",
                table: "Locales");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Locales");

            migrationBuilder.AddColumn<Guid>(
                name: "LocalId",
                table: "Usuarios",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_LocalId",
                table: "Usuarios",
                column: "LocalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Locales_LocalId",
                table: "Usuarios",
                column: "LocalId",
                principalTable: "Locales",
                principalColumn: "Id");
        }
    }
}
