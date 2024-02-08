using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class migracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "bf_operacional");

            migrationBuilder.EnsureSchema(
                name: "bf_operacional_usu");

            migrationBuilder.CreateTable(
                name: "oficinas",
                schema: "bf_operacional",
                columns: table => new
                {
                    id_oficina = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    direccion_oficina = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_oficinas", x => x.id_oficina);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                schema: "bf_operacional_usu",
                columns: table => new
                {
                    id_usuario = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    dni_usuario = table.Column<string>(type: "varchar(9)", nullable: false),
                    nombre_usuario = table.Column<string>(type: "varchar(70)", nullable: false),
                    apellidos_usuario = table.Column<string>(type: "varchar(100)", nullable: false),
                    tlf_usuario = table.Column<string>(type: "varchar(9)", nullable: false),
                    email_usuario = table.Column<string>(type: "varchar(100)", nullable: false),
                    clave_usuario = table.Column<string>(type: "varchar(100)", nullable: false),
                    fch_alta_usuario = table.Column<DateTime>(type: "timestamp", nullable: true),
                    fch_baja_usuario = table.Column<DateTime>(type: "timestamp", nullable: true),
                    tokenRecuperacion_usuario = table.Column<string>(type: "varchar(100)", nullable: true),
                    expiracionToken_usuario = table.Column<DateTime>(type: "timestamp", nullable: true),
                    rol_usuario = table.Column<string>(type: "varchar(20)", nullable: true),
                    mail_confirmado_usuario = table.Column<bool>(type: "boolean", nullable: false),
                    foto_perfil_usuario = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.id_usuario);
                });

            migrationBuilder.CreateTable(
                name: "citas",
                schema: "bf_operacional",
                columns: table => new
                {
                    id_cita = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_usuario = table.Column<long>(type: "bigint", nullable: false),
                    id_oficina = table.Column<long>(type: "bigint", nullable: false),
                    fecha_cita = table.Column<DateTime>(type: "timestamp", nullable: false),
                    motivo_cita = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_citas", x => x.id_cita);
                    table.ForeignKey(
                        name: "FK_citas_oficinas_id_oficina",
                        column: x => x.id_oficina,
                        principalSchema: "bf_operacional",
                        principalTable: "oficinas",
                        principalColumn: "id_oficina",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_citas_usuarios_id_usuario",
                        column: x => x.id_usuario,
                        principalSchema: "bf_operacional_usu",
                        principalTable: "usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cuentasBancarias",
                schema: "bf_operacional",
                columns: table => new
                {
                    id_cuenta = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioCuentaId = table.Column<long>(type: "bigint", nullable: false),
                    saldo_cuenta = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    codigo_iban_cuenta = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cuentasBancarias", x => x.id_cuenta);
                    table.ForeignKey(
                        name: "FK_cuentasBancarias_usuarios_UsuarioCuentaId",
                        column: x => x.UsuarioCuentaId,
                        principalSchema: "bf_operacional_usu",
                        principalTable: "usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transacciones",
                schema: "bf_operacional",
                columns: table => new
                {
                    id_transaccion = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    usuarioTransaccionRemitente = table.Column<long>(type: "bigint", nullable: false),
                    usuarioTransaccionDestinatario = table.Column<long>(type: "bigint", nullable: false),
                    cantidad_transaccion = table.Column<double>(type: "numeric(18,2)", nullable: false),
                    fecha_transaccion = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transacciones", x => x.id_transaccion);
                    table.ForeignKey(
                        name: "FK_transacciones_cuentasBancarias_usuarioTransaccionDestinatar~",
                        column: x => x.usuarioTransaccionDestinatario,
                        principalSchema: "bf_operacional",
                        principalTable: "cuentasBancarias",
                        principalColumn: "id_cuenta",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transacciones_cuentasBancarias_usuarioTransaccionRemitente",
                        column: x => x.usuarioTransaccionRemitente,
                        principalSchema: "bf_operacional",
                        principalTable: "cuentasBancarias",
                        principalColumn: "id_cuenta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_citas_id_oficina",
                schema: "bf_operacional",
                table: "citas",
                column: "id_oficina");

            migrationBuilder.CreateIndex(
                name: "IX_citas_id_usuario",
                schema: "bf_operacional",
                table: "citas",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_cuentasBancarias_UsuarioCuentaId",
                schema: "bf_operacional",
                table: "cuentasBancarias",
                column: "UsuarioCuentaId");

            migrationBuilder.CreateIndex(
                name: "IX_transacciones_usuarioTransaccionDestinatario",
                schema: "bf_operacional",
                table: "transacciones",
                column: "usuarioTransaccionDestinatario");

            migrationBuilder.CreateIndex(
                name: "IX_transacciones_usuarioTransaccionRemitente",
                schema: "bf_operacional",
                table: "transacciones",
                column: "usuarioTransaccionRemitente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "citas",
                schema: "bf_operacional");

            migrationBuilder.DropTable(
                name: "transacciones",
                schema: "bf_operacional");

            migrationBuilder.DropTable(
                name: "oficinas",
                schema: "bf_operacional");

            migrationBuilder.DropTable(
                name: "cuentasBancarias",
                schema: "bf_operacional");

            migrationBuilder.DropTable(
                name: "usuarios",
                schema: "bf_operacional_usu");
        }
    }
}
