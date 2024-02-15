using BancoFinalNetCore.DTO;
using BancoFinalNetCore.Util;
using DAL.Entidades;
using System;
using System.Numerics;

namespace BancoFinalNetCore.Servicios
{
    public class ServicioCuentaImpl:ICuentaServicio
    {
        private static Random random = new Random();

        private readonly MyDbContext _contexto;
        private readonly IConvertirAdto _convertirAdto;
        public ServicioCuentaImpl(
            MyDbContext contexto,
            IConvertirAdto convertirAdto

        )
        {
            _contexto = contexto;
            _convertirAdto = convertirAdto;
        }

        public List<CuentaBancariaDTO> obtenerCuentasPorUsuarioId(long id)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método obtenerMotosPorPropietarioId() de la clase MotoServicioImpl");

                List<CuentaBancaria> listaCuentas = _contexto.CuentasBancarias.Where(m => m.UsuarioCuentaId == id).ToList();
                return _convertirAdto.listaCuentaToDto(listaCuentas);
            }
            catch (ArgumentNullException e)
            {
                EscribirLog.escribirEnFicheroLog($"[ERROR MotoServicioImpl - obtenerMotosPorPropietarioId()] - Argumento id es NULL al obtener las motos de un usuario: {e}");
                return null;
            }
        }

        public CuentaBancaria GenerarCuentaBancaria(UsuarioDTO usuarioDto)
        {
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

        
        public static string GenerateRandomIBAN()
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

        private static string GenerateRandomNumericString(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static int CalculateChecksum(string ibanBase)
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
    }
}

