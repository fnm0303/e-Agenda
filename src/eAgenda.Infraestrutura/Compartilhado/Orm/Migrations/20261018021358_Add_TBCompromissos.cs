using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eAgenda.Infraestrutura.Compartilhado.Orm.Migrations
{
    /// <inheritdoc />
    public partial class Add_TBCompromissos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TBCompromissos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Assunto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DataOcorrencia = table.Column<DateOnly>(type: "date", nullable: false),
                    HoraInicio = table.Column<TimeOnly>(type: "time", nullable: false),
                    HoraTermino = table.Column<TimeOnly>(type: "time", nullable: false),
                    TipoCompromisso = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkOuLocal = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBCompromissos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TBCompromissos_Contato",
                columns: table => new
                {
                    CompromissoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContatosId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBCompromissos_Contato", x => new { x.CompromissoId, x.ContatosId });
                    table.ForeignKey(
                        name: "FK_TBCompromissos_Contato_TBCompromissos_CompromissoId",
                        column: x => x.CompromissoId,
                        principalTable: "TBCompromissos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TBCompromissos_Contato_TBContatos_ContatosId",
                        column: x => x.ContatosId,
                        principalTable: "TBContatos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TBCompromissos_Contato_ContatosId",
                table: "TBCompromissos_Contato",
                column: "ContatosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBCompromissos_Contato");

            migrationBuilder.DropTable(
                name: "TBCompromissos");
        }
    }
}
