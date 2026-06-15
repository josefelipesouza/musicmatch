using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicMatch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarCelulares : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Celular1",
                table: "Usuario",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Celular2",
                table: "Usuario",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Celular1",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Celular2",
                table: "Usuario");
        }
    }
}
