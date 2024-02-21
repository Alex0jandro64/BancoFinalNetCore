namespace BancoFinalNetCore.Servicios
{
    /// <summary>
    /// Interfaz que define el método encargado de encriptar texto.
    /// </summary>
    public interface IServicioEncriptar
    {
        /// <summary>
        /// Encripta un texto dado.
        /// </summary>
        /// <param name="texto">Texto que se desea encriptar.</param>
        /// <returns>Texto encriptado.</returns>
        public string Encriptar(string texto);
    }
}