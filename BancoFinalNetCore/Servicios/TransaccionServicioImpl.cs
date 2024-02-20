using BancoFinalNetCore.DTO;
using BancoFinalNetCore.Util;
using DAL.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BancoFinalNetCore.Servicios
{
    public class TransaccionServicioImpl : ITransaccionServicio
    {

        private readonly MyDbContext _contexto;
        private readonly IServicioEncriptar _servicioEncriptar;
        private readonly IConvertirAdao _convertirAdao;
        private readonly IConvertirAdto _convertirAdto;
        private readonly IServicioEmail _servicioEmail;
        private readonly ICuentaServicio _servicioCuenta;

        public TransaccionServicioImpl(
            MyDbContext contexto,
            IServicioEncriptar servicioEncriptar,
            IConvertirAdao convertirAdao,
            IConvertirAdto convertirAdto,
            IServicioEmail servicioEmail,
            ICuentaServicio servicioCuenta
        )
        {
            _contexto = contexto;
            _servicioEncriptar = servicioEncriptar;
            _convertirAdao = convertirAdao;
            _convertirAdto = convertirAdto;
            _servicioEmail = servicioEmail;
            _servicioCuenta = servicioCuenta;
        }
        public TransaccionDTO registrarTransaccion(TransaccionDTO transaccionDto)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método registrarTransaccion() de la clase TransaccionServicioImpl");

                var cuentaRemitente = _contexto.CuentasBancarias.FirstOrDefault(u => u.IdCuenta == transaccionDto.CuentaRemitenteId);
                var cuentaDestino = _contexto.CuentasBancarias.FirstOrDefault(u => u.CodigoIban == transaccionDto.IbanDestino);
                decimal cantidad = transaccionDto.Cantidad;
                DateTime Fecha = DateTime.Now;

                if (cuentaDestino == null)
                {
                    transaccionDto.IbanDestino = "CuentaNoEncontrada";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrarTransaccion() de la clase TransaccionServicioImpl");
                    return transaccionDto;
                }

                if(cuentaDestino == cuentaRemitente)
                {
                    transaccionDto.IbanDestino = "MismaCuenta";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrarTransaccion() de la clase TransaccionServicioImpl");
                    return transaccionDto;
                }

                if (cuentaRemitente.SaldoCuenta - cantidad < 0)
                {
                    transaccionDto.IbanDestino = "NoSaldoSuficiente";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrarTransaccion() de la clase TransaccionServicioImpl");
                    return transaccionDto;
                }

                cuentaRemitente.SaldoCuenta = cuentaRemitente.SaldoCuenta-cantidad;
                cuentaDestino.SaldoCuenta = cuentaDestino.SaldoCuenta+cantidad;
                Transaccion transaccionDao = new Transaccion();
                transaccionDao.CantidadTransaccion = transaccionDto.Cantidad;
                transaccionDao.UsuarioTransaccionRemitente = cuentaRemitente;
                transaccionDao.UsuarioTransaccionDestinatario = cuentaDestino;
                transaccionDao.FechaTransaccion = Fecha;

                _contexto.Update(cuentaDestino);
                _contexto.Update(cuentaRemitente);
                _contexto.Add(transaccionDao);
                _contexto.SaveChanges();

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrarTransaccion() de la clase TransaccionServicioImpl");

                return transaccionDto;
            }
            catch (DbUpdateException dbe)
            {
                EscribirLog.escribirEnFicheroLog("[Error TransaccionServicioImpl - registrarTransaccion()] Error de persistencia al actualizar la bbdd: " + dbe.Message);
                return null;
            }
            catch (Exception e)
            {
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - registrarUsuario()] Error al registrar un usuario: " + e.Message);
                return null;
            }
        }

    }
}
