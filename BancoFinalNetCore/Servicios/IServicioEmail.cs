namespace BancoFinalNetCore.Servicios
{
    /// <summary>
    /// Interfaz que define los métodos para enviar correos electrónicos.
    /// </summary>
    public interface IServicioEmail
    {
        /// <summary>
        /// Envía un correo electrónico para recuperación de contraseña.
        /// </summary>
        /// <param name="emailDestino">Correo electrónico de destino.</param>
        /// <param name="nombreUsuario">Nombre del usuario.</param>
        /// <param name="token">Token necesario para recuperar la contraseña.</param>
        public void enviarEmailRecuperacion(string emailDestino, string nombreUsuario, string token);

        /// <summary>
        /// Envía un correo electrónico de confirmación.
        /// </summary>
        /// <param name="emailDestino">Correo electrónico de destino.</param>
        /// <param name="nombreUsuario">Nombre del usuario.</param>
        /// <param name="token">Token de confirmación.</param>
        void enviarEmailConfirmacion(string emailDestino, string nombreUsuario, string token);
    }
}