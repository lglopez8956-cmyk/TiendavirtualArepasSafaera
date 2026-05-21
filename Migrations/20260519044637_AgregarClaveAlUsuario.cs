using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiendavirtualArepasSafaera.Migrations
{
    /// <inheritdoc />
    public partial class AgregarClaveAlUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "celular",
                table: "Usuarios",
                newName: "Celular");

            migrationBuilder.AddColumn<string>(
                name: "Clave",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Clave",
                table: "Usuarios");

            migrationBuilder.RenameColumn(
                name: "Celular",
                table: "Usuarios",
                newName: "celular");
        }
    }
}
