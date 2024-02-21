using BancoFinalNetCore.DTO;
using BancoFinalNetCore.Util;
using DAL.Entidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace BancoFinalNetCore.Servicios
{
    /// <summary>
    /// Implementación concreta de la interfaz ICuentaServicio que proporciona funcionalidades relacionadas con las cuentas bancarias.
    /// </summary>
    public class ServicioCuentaImpl : ICuentaServicio
    {
        private static Random random = new Random();

        private readonly MyDbContext _contexto;
        private readonly IConvertirAdto _convertirAdto;

        /// <summary>
        /// Constructor de la clase ServicioCuentaImpl.
        /// </summary>
        /// <param name="contexto">El contexto de la base de datos.</param>
        /// <param name="convertirAdto">El servicio de conversión de DTO a objeto.</param>
        public ServicioCuentaImpl(
            MyDbContext contexto,
            IConvertirAdto convertirAdto
        )
        {
            _contexto = contexto;
            _convertirAdto = convertirAdto;
        }

        /// <summary>
        /// Obtiene las cuentas bancarias asociadas a un usuario por su ID.
        /// </summary>
        /// <param name="id">El ID del usuario.</param>
        /// <returns>Una lista de cuentas bancarias en formato DTO.</returns>
        public List<CuentaBancariaDTO> obtenerCuentasPorUsuarioId(long id)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método obtenerCuentasPorUsuarioId() de la clase ServicioCuentaImpl");

                List<CuentaBancaria> listaCuentas = _contexto.CuentasBancarias.Where(m => m.UsuarioCuentaId == id).ToList();
                return _convertirAdto.listaCuentaToDto(listaCuentas);
            }
            catch (ArgumentNullException e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ServicioCuentaImpl - obtenerCuentasPorUsuarioId()] - Argumento id es NULL al obtener las cuentas bancarias de un usuario: {e}");
                return null;
            }
            catch (Exception ex)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ServicioCuentaImpl - obtenerCuentasPorUsuarioId()] - Error al obtener las cuentas bancarias de un usuario: {ex}");
                return null;
            }
        }

        /// <summary>
        /// Genera una nueva cuenta bancaria para un usuario.
        /// </summary>
        /// <param name="usuarioDto">El DTO del usuario para el cual se generará la cuenta.</param>
        /// <returns>La cuenta bancaria generada.</returns>
        public CuentaBancaria GenerarCuentaBancaria(UsuarioDTO usuarioDto)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método GenerarCuentaBancaria() de la clase ServicioCuentaImpl");

                var usuario = _contexto.Usuarios.Find(usuarioDto.IdUsuario);
                CuentaBancaria cuentaBancaria = new CuentaBancaria();
                do
                {
                    cuentaBancaria.CodigoIban = GenerateRandomIBAN();
                } while (_contexto.CuentasBancarias.Any(u => u.CodigoIban == cuentaBancaria.CodigoIban));
                cuentaBancaria.SaldoCuenta = 0;

                cuentaBancaria.UsuarioCuenta = usuario;
                _contexto.CuentasBancarias.Add(cuentaBancaria);
                _contexto.SaveChanges();
                return cuentaBancaria;
            }
            catch (Exception ex)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ServicioCuentaImpl - GenerarCuentaBancaria()] - Error al generar una cuenta bancaria: {ex}");
                return null;
            }
        }


        /// <summary>
        /// Genera un número IBAN aleatorio.
        /// </summary>
        /// <returns>Un número IBAN generado aleatoriamente.</returns>
        public static string GenerateRandomIBAN()
        {
            try
            {
                // Código de país y dígitos de control (España)
                string countryCode = "ES";
                string controlDigits = "00";

                // Generar el número de cuenta aleatorio
                string accountNumber = GenerateRandomNumericString(20);

                // Concatenar todo y calcular dígitos de control
                string ibanBase = countryCode + controlDigits + accountNumber;
                int checksum = CalculateChecksum(ibanBase);

                // Construir IBAN final
                string iban = countryCode + checksum.ToString().PadLeft(2, '0') + accountNumber;

                return iban;
            }
            catch (Exception ex)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ServicioCuentaImpl - GenerateRandomIBAN()] - Error al generar un IBAN aleatorio: {ex}");
                return null;
            }
        }

        /// <summary>
        /// Genera una cadena de dígitos numéricos aleatorios de la longitud especificada.
        /// </summary>
        /// <param name="length">Longitud de la cadena a generar.</param>
        /// <returns>Una cadena de dígitos numéricos aleatorios.</returns>
        private static string GenerateRandomNumericString(int length)
        {
            try
            {
                const string chars = "0123456789";
                return new string(Enumerable.Repeat(chars, length)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            catch (Exception ex)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ServicioCuentaImpl - GenerateRandomNumericString()] - Error al generar una cadena de dígitos numéricos aleatorios: {ex}");
                return null;
            }
        }

        /// <summary>
        /// Calcula el dígito de control de un número IBAN.
        /// </summary>
        /// <param name="ibanBase">El número IBAN base.</param>
        /// <returns>El dígito de control calculado.</returns>
        private static int CalculateChecksum(string ibanBase)
        {
            try
            {
                // Reemplazar las letras por números según la especificación
                string replacedIban = ibanBase.Substring(4) + ibanBase.Substring(0, 4);
                replacedIban = replacedIban.Replace("A", "10").Replace("B", "11").Replace("C", "12")
                                             .Replace("D", "13").Replace("E", "14").Replace("F", "15")
                                             .Replace("G", "16").Replace("H", "17").Replace("I", "18")
                                             .Replace("J", "19").Replace("K", "20").Replace("L", "21")
                                             .Replace("M", "22").Replace("N", "23").Replace("O", "24")
                                             .Replace("P", "25").Replace("Q", "26").Replace("R", "27")
                                             .Replace("S", "28").Replace("T", "29").Replace("U", "30")
                                             .Replace("V", "31").Replace("W", "32").Replace("X", "33")
                                             .Replace("Y", "34").Replace("Z", "35");

                // Convertir a BigInteger
                BigInteger replacedIbanInt = BigInteger.Parse(replacedIban);

                // Calcular el módulo 97 del número y restar del 98
                int checksum = (int)(98 - (replacedIbanInt % 97));

                return checksum;
            }
            catch (Exception ex)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR ServicioCuentaImpl - CalculateChecksum()] - Error al calcular el dígito de control de un IBAN: {ex}");
                return -1;
            }
        }
    }
}