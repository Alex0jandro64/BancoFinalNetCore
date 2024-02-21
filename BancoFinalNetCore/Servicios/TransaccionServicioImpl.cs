using BancoFinalNetCore.DTO;
using BancoFinalNetCore.Util;
using DAL.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BancoFinalNetCore.Servicios
{
    /// <summary>
    /// Implementación concreta de la interfaz ITransaccionServicio que proporciona funcionalidades relacionadas con las transacciones bancarias.
    /// </summary>
    public class TransaccionServicioImpl : ITransaccionServicio
    {
        private readonly MyDbContext _contexto;
        private readonly IServicioEncriptar _servicioEncriptar;
        private readonly IConvertirAdao _convertirAdao;
        private readonly IConvertirAdto _convertirAdto;
        private readonly IServicioEmail _servicioEmail;
        private readonly ICuentaServicio _servicioCuenta;

        /// <summary>
        /// Constructor de la clase TransaccionServicioImpl.
        /// </summary>
        /// <param name="contexto">El contexto de la base de datos.</param>
        /// <param name="servicioEncriptar">El servicio de encriptación.</param>
        /// <param name="convertirAdao">El servicio de conversión de DTO a DAO.</param>
        /// <param name="convertirAdto">El servicio de conversión de DAO a DTO.</param>
        /// <param name="servicioEmail">El servicio de envío de correo electrónico.</param>
        /// <param name="servicioCuenta">El servicio relacionado con las cuentas bancarias.</param>
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


        /// <summary>
        /// Registra una transacción en la base de datos.
        /// </summary>
        /// <param name="transaccionDto">El objeto TransaccionDTO con la información de la transacción.</param>
        /// <returns>El objeto TransaccionDTO registrado o null si ocurrió un error.</returns>
        public TransaccionDTO registrarTransaccion(TransaccionDTO transaccionDto)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método registrarTransaccion() de la clase TransaccionServicioImpl");

                // Obtener la cuenta remitente y la cuenta destino
                var cuentaRemitente = _contexto.CuentasBancarias.FirstOrDefault(u => u.IdCuenta == transaccionDto.CuentaRemitenteId);
                var cuentaDestino = _contexto.CuentasBancarias.FirstOrDefault(u => u.CodigoIban == transaccionDto.IbanDestino);
                decimal cantidad = transaccionDto.Cantidad;
                DateTime Fecha = DateTime.Now;

                // Comprobar si la cuenta destino no existe
                if (cuentaDestino == null)
                {
                    transaccionDto.IbanDestino = "CuentaNoEncontrada";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrarTransaccion() de la clase TransaccionServicioImpl");
                    return transaccionDto;
                }

                // Comprobar si la cuenta destino es la misma que la remitente
                if (cuentaDestino == cuentaRemitente)
                {
                    transaccionDto.IbanDestino = "MismaCuenta";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrarTransaccion() de la clase TransaccionServicioImpl");
                    return transaccionDto;
                }

                // Comprobar si la cuenta remitente tiene saldo suficiente
                if (cuentaRemitente.SaldoCuenta - cantidad < 0)
                {
                    transaccionDto.IbanDestino = "NoSaldoSuficiente";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrarTransaccion() de la clase TransaccionServicioImpl");
                    return transaccionDto;
                }

                // Actualizar saldos de las cuentas
                cuentaRemitente.SaldoCuenta = cuentaRemitente.SaldoCuenta - cantidad;
                cuentaDestino.SaldoCuenta = cuentaDestino.SaldoCuenta + cantidad;

                // Crear objeto Transaccion y guardarlo en la base de datos
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
                EscribirLog.escribirEnFicheroLog("[Error TransaccionServicioImpl - registrarTransaccion()] Error al registrar una transacción: " + e.Message);
                return null;
            }
        }
    }
}