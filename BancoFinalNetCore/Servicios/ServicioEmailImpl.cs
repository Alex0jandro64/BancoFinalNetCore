using BancoFinalNetCore.Util;
using System.IO;
using System.Net.Mail;

namespace BancoFinalNetCore.Servicios
{
    /// <summary>
    /// Implementación concreta de la interfaz IServicioEmail que proporciona funcionalidades para enviar correos electrónicos.
    /// </summary>
    public class ServicioEmailImpl : IServicioEmail
    {
        /// <summary>
        /// Envía un correo electrónico de confirmación.
        /// </summary>
        /// <param name="emailDestino">La dirección de correo electrónico de destino.</param>
        /// <param name="nombreUsuario">El nombre del usuario.</param>
        /// <param name="token">El token de confirmación.</param>
        void IServicioEmail.enviarEmailConfirmacion(string emailDestino, string nombreUsuario, string token)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método enviarEmailConfirmacion() de la clase ServicioEmailImpl");

                // URL del dominio
                string urlDominio = "https://localhost:7064";

                // Dirección de correo electrónico de origen
                string EmailOrigen = "alejandroalcerreca3@gmail.com";

                // URL de confirmación
                string urlDeRecuperacion = string.Format("{0}/auth/confirmar-cuenta/?token={1}", urlDominio, token);

                // Lectura del contenido HTML del correo desde un archivo
                string directorioProyecto = Directory.GetCurrentDirectory();
                string rutaArchivo = Path.Combine(directorioProyecto, "Plantilla/ConfirmarCorreo.html");
                string htmlContent = File.ReadAllText(rutaArchivo);

                // Sustitución de variables en el HTML
                htmlContent = string.Format(htmlContent, nombreUsuario, urlDeRecuperacion);

                // Creación y envío del correo electrónico
                MailMessage mensajeDelCorreo = new MailMessage(EmailOrigen, emailDestino, "CONFIRMAR EMAIL AlceBank", htmlContent);
                mensajeDelCorreo.IsBodyHtml = true;

                SmtpClient smtpCliente = new SmtpClient("smtp.gmail.com");
                smtpCliente.EnableSsl = true;
                smtpCliente.UseDefaultCredentials = false;
                smtpCliente.Port = 587;
                smtpCliente.Credentials = new System.Net.NetworkCredential(EmailOrigen, "qvrr gtfp wrsl tqol");

                smtpCliente.Send(mensajeDelCorreo);

                smtpCliente.Dispose();

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método enviarEmailConfirmacion() de la clase ServicioEmailImpl");
            }
            catch (IOException ioe)
            {
                EscribirLog.escribirEnFicheroLog("[Error ServicioEmailImpl - enviarEmailConfirmacion()] Error al leer el fichero html para enviar email de confirmacion: " + ioe.Message);
            }
            catch (SmtpException se)
            {
                EscribirLog.escribirEnFicheroLog("[Error ServicioEmailImpl - enviarEmailConfirmacion()] Error con el protocolo de envio de email: " + se.Message);
            }

        }

        /// <summary>
        /// Envía un correo electrónico de recuperación de contraseña.
        /// </summary>
        /// <param name="emailDestino">La dirección de correo electrónico de destino.</param>
        /// <param name="nombreUsuario">El nombre del usuario.</param>
        /// <param name="token">El token de recuperación.</param>
        void IServicioEmail.enviarEmailRecuperacion(string emailDestino, string nombreUsuario, string token)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método enviarEmailRecuperacion() de la clase ServicioEmailImpl");

                // URL del dominio
                string urlDominio = "https://localhost:7064";

                // Dirección de correo electrónico de origen
                string EmailOrigen = "alejandroalcerreca3@gmail.com";

                // URL de recuperación
                string urlDeRecuperacion = string.Format("{0}/auth/recuperar/?token={1}", urlDominio, token);

                // Lectura del contenido HTML del correo desde un archivo
                string directorioProyecto = Directory.GetCurrentDirectory();
                string rutaArchivo = Path.Combine(directorioProyecto, "Plantilla/RecuperacionContraseña.html");
                string htmlContent = File.ReadAllText(rutaArchivo);

                // Sustitución de variables en el HTML
                htmlContent = string.Format(htmlContent, nombreUsuario, urlDeRecuperacion);

                // Creación y envío del correo electrónico
                MailMessage mensajeDelCorreo = new MailMessage(EmailOrigen, emailDestino, "RECUPERAR CONTRASEÑA AlceBank", htmlContent);
                mensajeDelCorreo.IsBodyHtml = true;

                SmtpClient smtpCliente = new SmtpClient("smtp.gmail.com");
                smtpCliente.EnableSsl = true;
                smtpCliente.UseDefaultCredentials = false;
                smtpCliente.Port = 587;
                smtpCliente.Credentials = new System.Net.NetworkCredential(EmailOrigen, "qvrr gtfp wrsl tqol");

                smtpCliente.Send(mensajeDelCorreo);

                smtpCliente.Dispose();

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método enviarEmailRecuperacion() de la clase ServicioEmailImpl");
            }
            catch (IOException ioe)
            {
                EscribirLog.escribirEnFicheroLog("[Error ServicioEmailImpl - enviarEmailRecuperacion()] Error al leer el fichero html para enviar email de recuperacion: " + ioe.Message);
            }
            catch (SmtpException se)
            {
                EscribirLog.escribirEnFicheroLog("[Error ServicioEmailImpl - enviarEmailRecuperacion()] Error con el protocolo de envio de email: " + se.Message);
            }

        }
    }
}