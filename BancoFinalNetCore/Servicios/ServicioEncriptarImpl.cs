using BancoFinalNetCore.Util;
using System;
using System.Security.Cryptography;
using System.Text;

namespace BancoFinalNetCore.Servicios
{
    /// <summary>
    /// Implementación concreta de la interfaz IServicioEncriptar que proporciona funcionalidades de encriptación.
    /// </summary>
    public class ServicioEncriptarImpl : IServicioEncriptar
    {
        /// <summary>
        /// Encripta una contraseña utilizando el algoritmo SHA256.
        /// </summary>
        /// <param name="contraseña">La contraseña a encriptar.</param>
        /// <returns>La contraseña encriptada o null si ocurrió un error.</returns>
        public string Encriptar(string contraseña)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método Encriptar() de la clase ServicioEncriptarImpl");

                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(contraseña);
                    byte[] hashBytes = sha256.ComputeHash(bytes);
                    string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método Encriptar() de la clase ServicioEncriptarImpl");
                    return hash;
                }
            }
            catch (ArgumentException e)
            {
                EscribirLog.escribirEnFicheroLog("[Error ServicioEncriptarImpl - Encriptar()] Error al encriptar: " + e.Message);
                return null;
            }
            catch (Exception ex)
            {
                EscribirLog.escribirEnFicheroLog("[Error ServicioEncriptarImpl - Encriptar()] Error no controlado al encriptar: " + ex.Message);
                return null;
            }
        }
    }
}