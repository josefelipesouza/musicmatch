using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicMatch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefatoracaoAgenda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agendas_Artistas_ArtistaId",
                table: "Agendas");

            migrationBuilder.DropForeignKey(
                name: "FK_Agendas_FormatosShow_FormatoShowId",
                table: "Agendas");

            migrationBuilder.DropTable(
                name: "FormatosShow");

            migrationBuilder.DropIndex(
                name: "IX_Agendas_FormatoShowId",
                table: "Agendas");

            migrationBuilder.DropColumn(
                name: "Cidade",
                table: "Artistas");

            migrationBuilder.DropColumn(
                name: "EquipamentoProprio",
                table: "Artistas");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Artistas");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Artistas");

            migrationBuilder.DropColumn(
                name: "FormatoShowId",
                table: "Agendas");

            migrationBuilder.AlterColumn<Guid>(
                name: "ArtistaId",
                table: "Agendas",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ArtistaId1",
                table: "Agendas",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                table: "Agendas",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "EquipamentoProprio",
                table: "Agendas",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FormatoShow",
                table: "Agendas",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Agendas",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Agendas",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_Agendas_ArtistaId1",
                table: "Agendas",
                column: "ArtistaId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Agendas_Artistas_ArtistaId",
                table: "Agendas",
                column: "ArtistaId",
                principalTable: "Artistas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Agendas_Artistas_ArtistaId1",
                table: "Agendas",
                column: "ArtistaId1",
                principalTable: "Artistas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agendas_Artistas_ArtistaId",
                table: "Agendas");

            migrationBuilder.DropForeignKey(
                name: "FK_Agendas_Artistas_ArtistaId1",
                table: "Agendas");

            migrationBuilder.DropIndex(
                name: "IX_Agendas_ArtistaId1",
                table: "Agendas");

            migrationBuilder.DropColumn(
                name: "ArtistaId1",
                table: "Agendas");

            migrationBuilder.DropColumn(
                name: "Cidade",
                table: "Agendas");

            migrationBuilder.DropColumn(
                name: "EquipamentoProprio",
                table: "Agendas");

            migrationBuilder.DropColumn(
                name: "FormatoShow",
                table: "Agendas");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Agendas");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Agendas");

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                table: "Artistas",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "EquipamentoProprio",
                table: "Artistas",
                type: "boolean",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.AlterColumn<Guid>(
                name: "ArtistaId",
                table: "Agendas",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "FormatoShowId",
                table: "Agendas",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "FormatosShow",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ArtistaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormatosShow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormatosShow_Artistas_ArtistaId",
                        column: x => x.ArtistaId,
                        principalTable: "Artistas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agendas_FormatoShowId",
                table: "Agendas",
                column: "FormatoShowId");

            migrationBuilder.CreateIndex(
                name: "IX_FormatosShow_ArtistaId",
                table: "FormatosShow",
                column: "ArtistaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agendas_Artistas_ArtistaId",
                table: "Agendas",
                column: "ArtistaId",
                principalTable: "Artistas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Agendas_FormatosShow_FormatoShowId",
                table: "Agendas",
                column: "FormatoShowId",
                principalTable: "FormatosShow",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
