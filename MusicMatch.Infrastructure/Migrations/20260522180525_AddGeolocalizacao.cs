using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicMatch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGeolocalizacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Localizacao",
                table: "Artistas",
                newName: "Cidade");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Eventos",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Eventos",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "RaioKm",
                table: "Eventos",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Artistas",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Artistas",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "RaioKm",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Artistas");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Artistas");

            migrationBuilder.RenameColumn(
                name: "Cidade",
                table: "Artistas",
                newName: "Localizacao");
        }
    }
}
