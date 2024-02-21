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

        /// <summary>
        /// Constructor de la clase TransaccionServicioImpl.
        /// </summary>
        /// <param name="contexto">El contexto de la base de datos.</param>
        public TransaccionServicioImpl(
            MyDbContext contexto
        )
        {
            _contexto = contexto;
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
        /// <summary>
        /// Método para obtener las transacciones asociadas a un usuario específico.
        /// </summary>
        /// <param name="userId">ID del usuario del que se desean obtener las transacciones.</param>
        /// <returns>Una lista de objetos Transaccion asociados al usuario especificado.</returns>
        public List<Transaccion> ObtenerTransaccionesDeUsuario(long userId)
        {
            try
            {
                // Se escribe un mensaje de registro al entrar al método.
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método ObtenerTransaccionesDeUsuario() de la clase UsuarioServicioImpl");

                // Se obtienen las transacciones donde el usuario es el destinatario o el remitente.
                var transacciones = _contexto.Transacciones
                    .Where(t => t.UsuarioTransaccionDestinatarioId == userId || t.UsuarioTransaccionRemitenteId == userId)
                    .ToList();

                // Se escribe un mensaje de registro al salir del método.
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ObtenerTransaccionesDeUsuario() de la clase UsuarioServicioImpl");

                return transacciones;
            }
            catch (Exception e)
            {
                // Se atrapa cualquier excepción que pueda ocurrir.
                // Se escribe un mensaje de registro indicando el error.
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - ObtenerTransaccionesDeUsuario()] Error al obtener transacciones del usuario: " + e.Message);

                // Se retorna una lista vacía debido a que ocurrió un error.
                return new List<Transaccion>();
            }
        }
    }
}