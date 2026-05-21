using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiendavirtualArepasSafaera.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCelularAUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "celular",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "celular",
                table: "Usuarios");
        }
    }
}
