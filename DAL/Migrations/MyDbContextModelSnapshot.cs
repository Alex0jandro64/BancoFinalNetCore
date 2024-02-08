﻿// <auto-generated />
using System;
using DAL.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(MyDbContext))]
    partial class MyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DAL.Entidades.Cita", b =>
                {
                    b.Property<long>("IdCita")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id_cita");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("IdCita"));

                    b.Property<DateTime>("FechaCita")
                        .HasColumnType("timestamp")
                        .HasColumnName("fecha_cita");

                    b.Property<string>("MotivoCita")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("motivo_cita");

                    b.Property<long>("OficinaCitaId")
                        .HasColumnType("bigint")
                        .HasColumnName("id_oficina");

                    b.Property<long>("UsuarioCitaId")
                        .HasColumnType("bigint")
                        .HasColumnName("id_usuario");

                    b.HasKey("IdCita");

                    b.HasIndex("OficinaCitaId");

                    b.HasIndex("UsuarioCitaId");

                    b.ToTable("citas", "bf_operacional");
                });

            modelBuilder.Entity("DAL.Entidades.CuentaBancaria", b =>
                {
                    b.Property<long>("IdCuenta")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id_cuenta");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("IdCuenta"));

                    b.Property<string>("CodigoIban")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("codigo_iban_cuenta");

                    b.Property<decimal>("SaldoCuenta")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("saldo_cuenta");

                    b.Property<long>("UsuarioCuentaId")
                        .HasColumnType("bigint");

                    b.HasKey("IdCuenta");

                    b.HasIndex("UsuarioCuentaId");

                    b.ToTable("cuentasBancarias", "bf_operacional");
                });

            modelBuilder.Entity("DAL.Entidades.Oficina", b =>
                {
                    b.Property<long>("IdOficina")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id_oficina");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("IdOficina"));

                    b.Property<string>("DireccionOficina")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("direccion_oficina");

                    b.HasKey("IdOficina");

                    b.ToTable("oficinas", "bf_operacional");
                });

            modelBuilder.Entity("DAL.Entidades.Transaccion", b =>
                {
                    b.Property<long>("IdTransaccion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id_transaccion");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("IdTransaccion"));

                    b.Property<double>("CantidadTransaccion")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("cantidad_transaccion");

                    b.Property<DateTime>("FechaTransaccion")
                        .HasColumnType("timestamp")
                        .HasColumnName("fecha_transaccion");

                    b.Property<long>("UsuarioTransaccionDestinatarioId")
                        .HasColumnType("bigint")
                        .HasColumnName("usuarioTransaccionDestinatario");

                    b.Property<long>("UsuarioTransaccionRemitenteId")
                        .HasColumnType("bigint")
                        .HasColumnName("usuarioTransaccionRemitente");

                    b.HasKey("IdTransaccion");

                    b.HasIndex("UsuarioTransaccionDestinatarioId");

                    b.HasIndex("UsuarioTransaccionRemitenteId");

                    b.ToTable("transacciones", "bf_operacional");
                });

            modelBuilder.Entity("DAL.Entidades.Usuario", b =>
                {
                    b.Property<long>("IdUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id_usuario");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("IdUsuario"));

                    b.Property<string>("ApellidosUsuario")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("apellidos_usuario");

                    b.Property<string>("ClaveUsuario")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("clave_usuario");

                    b.Property<string>("DniUsuario")
                        .IsRequired()
                        .HasColumnType("varchar(9)")
                        .HasColumnName("dni_usuario");

                    b.Property<string>("EmailUsuario")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("email_usuario");

                    b.Property<DateTime?>("ExpiracionToken")
                        .HasColumnType("timestamp")
                        .HasColumnName("expiracionToken_usuario");

                    b.Property<DateTime?>("FchAltaUsuario")
                        .HasColumnType("timestamp")
                        .HasColumnName("fch_alta_usuario");

                    b.Property<DateTime?>("FchBajaUsuario")
                        .HasColumnType("timestamp")
                        .HasColumnName("fch_baja_usuario");

                    b.Property<byte[]>("FotoPerfil")
                        .HasColumnType("bytea")
                        .HasColumnName("foto_perfil_usuario");

                    b.Property<bool>("MailConfirmado")
                        .HasColumnType("boolean")
                        .HasColumnName("mail_confirmado_usuario");

                    b.Property<string>("NombreUsuario")
                        .IsRequired()
                        .HasColumnType("varchar(70)")
                        .HasColumnName("nombre_usuario");

                    b.Property<string>("Rol")
                        .HasColumnType("varchar(20)")
                        .HasColumnName("rol_usuario");

                    b.Property<string>("TlfUsuario")
                        .IsRequired()
                        .HasColumnType("varchar(9)")
                        .HasColumnName("tlf_usuario");

                    b.Property<string>("Token")
                        .HasColumnType("varchar(100)")
                        .HasColumnName("tokenRecuperacion_usuario");

                    b.HasKey("IdUsuario");

                    b.ToTable("usuarios", "bf_operacional_usu");
                });

            modelBuilder.Entity("DAL.Entidades.Cita", b =>
                {
                    b.HasOne("DAL.Entidades.Oficina", "OficinaCita")
                        .WithMany("CitasOficina")
                        .HasForeignKey("OficinaCitaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Entidades.Usuario", "UsuarioCita")
                        .WithMany("CitasUsuario")
                        .HasForeignKey("UsuarioCitaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OficinaCita");

                    b.Navigation("UsuarioCita");
                });

            modelBuilder.Entity("DAL.Entidades.CuentaBancaria", b =>
                {
                    b.HasOne("DAL.Entidades.Usuario", "UsuarioCuenta")
                        .WithMany("CuentasBancarias")
                        .HasForeignKey("UsuarioCuentaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UsuarioCuenta");
                });

            modelBuilder.Entity("DAL.Entidades.Transaccion", b =>
                {
                    b.HasOne("DAL.Entidades.CuentaBancaria", "UsuarioTransaccionDestinatario")
                        .WithMany("TrasaccionesDestinatarios")
                        .HasForeignKey("UsuarioTransaccionDestinatarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Entidades.CuentaBancaria", "UsuarioTransaccionRemitente")
                        .WithMany("TrasaccionesRemitentes")
                        .HasForeignKey("UsuarioTransaccionRemitenteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UsuarioTransaccionDestinatario");

                    b.Navigation("UsuarioTransaccionRemitente");
                });

            modelBuilder.Entity("DAL.Entidades.CuentaBancaria", b =>
                {
                    b.Navigation("TrasaccionesDestinatarios");

                    b.Navigation("TrasaccionesRemitentes");
                });

            modelBuilder.Entity("DAL.Entidades.Oficina", b =>
                {
                    b.Navigation("CitasOficina");
                });

            modelBuilder.Entity("DAL.Entidades.Usuario", b =>
                {
                    b.Navigation("CitasUsuario");

                    b.Navigation("CuentasBancarias");
                });
#pragma warning restore 612, 618
        }
    }
}
