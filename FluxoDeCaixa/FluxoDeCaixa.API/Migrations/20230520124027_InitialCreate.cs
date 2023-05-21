using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FluxoDeCaixa.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "livrocaixa",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataMovimento = table.Column<DateTime>(type: "Date", nullable: false),
                    SaldoAnterior = table.Column<decimal>(type: "decimal(14,2)", precision: 14, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_livrocaixa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    HashSenha = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false),
                    UltimoAcessoEm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BloqueioExpiraEm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NumeroFalhasAoAcessar = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "lancamento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataLancamento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValorLancamento = table.Column<decimal>(type: "decimal(14,2)", precision: 14, scale: 2, nullable: false),
                    TipoLancamento = table.Column<int>(type: "int", nullable: false),
                    LivroCaixaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lancamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_lancamento_livrocaixa_LivroCaixaId",
                        column: x => x.LivroCaixaId,
                        principalTable: "livrocaixa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "token",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Acesso = table.Column<string>(type: "varchar(2048)", unicode: false, maxLength: 2048, nullable: false, comment: "AcessToken"),
                    Atualizacao = table.Column<string>(type: "varchar(2048)", unicode: false, maxLength: 2048, nullable: false, comment: "RefreshToken"),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiraEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevogadoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_token", x => x.Id);
                    table.ForeignKey(
                        name: "FK_token_usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_lancamento_LivroCaixaId",
                table: "lancamento",
                column: "LivroCaixaId");

            migrationBuilder.CreateIndex(
                name: "IX_token_UsuarioId",
                table: "token",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_Email",
                table: "usuario",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lancamento");

            migrationBuilder.DropTable(
                name: "token");

            migrationBuilder.DropTable(
                name: "livrocaixa");

            migrationBuilder.DropTable(
                name: "usuario");
        }
    }
}
