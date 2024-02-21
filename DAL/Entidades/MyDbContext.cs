using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entidades
{
    public partial class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }
        
        public MyDbContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseNpgsql("Server=localhost;Port=5566;Database=BancoFinalNet;User Id=postgres;Password=Alcerreca001139_; SearchPath=public");
            optionsBuilder.EnableSensitiveDataLogging();

        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<CuentaBancaria> CuentasBancarias { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<Oficina> Oficinas { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }

        // Agrega otras DbSet para tus otras entidades


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configura las relaciones y restricciones aquí si es necesario


        }
    }
}