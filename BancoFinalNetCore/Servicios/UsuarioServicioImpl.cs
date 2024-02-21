using BancoFinalNetCore.DTO;
using BancoFinalNetCore.Util;
using DAL.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace BancoFinalNetCore.Servicios
{
        /// <summary>
        /// Clase que implementa la interfaz ServicioConsultas y detalla la lógica de los métodos.
        /// </summary>
        public class UsuarioServicioImpl : IUsuarioServicio
    {
        private readonly MyDbContext _contexto;
        private readonly IServicioEncriptar _servicioEncriptar;
        private readonly IConvertirAdao _convertirAdao;
        private readonly IConvertirAdto _convertirAdto;
        private readonly IServicioEmail _servicioEmail;
        private readonly ICuentaServicio _servicioCuenta;

        public UsuarioServicioImpl(
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
        /// Método para registrar un nuevo usuario.
        /// </summary>
        /// <param name="userDTO">Objeto UsuarioDTO que contiene la información del usuario a registrar.</param>
        /// <returns>El objeto UsuarioDTO con la información del usuario registrado, o null si ocurre un error.</returns>
        public UsuarioDTO registrarUsuario(UsuarioDTO userDTO)
        {
            try
            {
                // Se escribe un mensaje de registro al entrar al método.
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método registrarUsuario() de la clase UsuarioServicioImpl");

                // Se busca un usuario existente con el mismo correo electrónico pero que no haya confirmado su correo.
                var usuarioExistente = _contexto.Usuarios.FirstOrDefault(u => u.EmailUsuario == userDTO.EmailUsuario && !u.MailConfirmado);

                // Si se encuentra un usuario con el mismo correo y sin confirmar su correo...
                if (usuarioExistente != null)
                {
                    // Se cambia el correo del usuarioDTO para indicar que el correo no está confirmado.
                    userDTO.EmailUsuario = "EmailNoConfirmado";

                    // Se escribe un mensaje de registro al salir del método.
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrarUsuario() de la clase UsuarioServicioImpl");

                    // Se retorna el usuarioDTO modificado.
                    return userDTO;
                }

                // Se busca un usuario existente con el mismo correo electrónico y que ya haya confirmado su correo.
                var emailExistente = _contexto.Usuarios.FirstOrDefault(u => u.EmailUsuario == userDTO.EmailUsuario && u.MailConfirmado);

                // Si se encuentra un usuario con el mismo correo y ya confirmado...
                if (emailExistente != null)
                {
                    // Se cambia el correo del usuarioDTO para indicar que el correo está repetido.
                    userDTO.EmailUsuario = "EmailRepetido";

                    // Se escribe un mensaje de registro al salir del método.
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrarUsuario() de la clase UsuarioServicioImpl");

                    // Se retorna el usuarioDTO modificado.
                    return userDTO;
                }

                // Se encripta la contraseña del usuarioDTO.
                userDTO.ClaveUsuario = _servicioEncriptar.Encriptar(userDTO.ClaveUsuario);

                // Se convierte el usuarioDTO a un objeto Usuario del modelo de datos.
                Usuario usuarioDao = _convertirAdao.usuarioToDao(userDTO);

                // Se establece la fecha de alta y el rol del usuario.
                usuarioDao.FchAltaUsuario = DateTime.Now;
                usuarioDao.Rol = "ROLE_USER";

                // Se genera un token para el usuario.
                string token = generarToken();
                usuarioDao.Token = token;

                // Se añade el usuario a la base de datos y se guardan los cambios.
                _contexto.Usuarios.Add(usuarioDao);
                _contexto.SaveChanges();

                // Se actualiza el IdUsuario del usuarioDTO con el Id generado en la base de datos.
                userDTO.IdUsuario = usuarioDao.IdUsuario;

                // Se genera una cuenta bancaria para el usuario.
                CuentaBancaria cuentaNueva = _servicioCuenta.GenerarCuentaBancaria(userDTO);

                // Se envía un correo electrónico de confirmación al usuario.
                string nombreUsuario = usuarioDao.NombreUsuario;
                _servicioEmail.enviarEmailConfirmacion(userDTO.EmailUsuario, nombreUsuario, token);

                // Se escribe un mensaje de registro al salir del método.
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método registrarUsuario() de la clase UsuarioServicioImpl");

                // Se retorna el usuarioDTO registrado.
                return userDTO;
            }
            catch (DbUpdateException dbe)
            {
                // Se atrapa la excepción específica para problemas de actualización en la base de datos.
                // Se escribe un mensaje de registro indicando el error.
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - registrarUsuario()] Error de persistencia al actualizar la bbdd: " + dbe.Message);

                // Se retorna null debido a que ocurrió un error y no se pudo completar el registro del usuario.
                return null;
            }
            catch (Exception e)
            {
                // Se atrapa cualquier otra excepción que pueda ocurrir.
                // Se escribe un mensaje de registro indicando el error.
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - registrarUsuario()] Error al registrar un usuario: " + e.Message);

                // Se retorna null debido a que ocurrió un error y no se pudo completar el registro del usuario.
                return null;
            }
        }

        /// <summary>
        /// Método para generar un token de usuario.
        /// </summary>
        /// <returns>Una cadena que representa el token generado.</returns>
        private string generarToken()
        {
            try
            {
                // Se escribe un mensaje de registro al entrar al método.
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método generarToken() de la clase UsuarioServicioImpl");

                // Se utiliza un generador seguro de números aleatorios para generar el token.
                using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
                {
                    byte[] tokenBytes = new byte[30];
                    rng.GetBytes(tokenBytes);

                    // Se escribe un mensaje de registro al salir del método.
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método generarToken() de la clase UsuarioServicioImpl");

                    // Se convierte el token generado a una cadena hexadecimal sin guiones y en minúsculas.
                    return BitConverter.ToString(tokenBytes).Replace("-", "").ToLower();
                }
            }
            catch (ArgumentException ae)
            {
                // Se atrapa la excepción de tipo ArgumentException, que podría ocurrir si hay un error en la generación del token.
                // Se escribe un mensaje de registro indicando el error.
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl -  generarToken()] Error al generar un token de usuario " + ae.Message);

                // Se retorna null debido a que ocurrió un error y no se pudo generar el token.
                return null;
            }
        }

        /// <summary>
        /// Método para confirmar la cuenta de usuario con un token.
        /// </summary>
        /// <param name="token">El token de confirmación de la cuenta.</param>
        /// <returns>True si la cuenta se confirmó correctamente, de lo contrario, false.</returns>
        public bool confirmarCuenta(string token)
        {
            try
            {
                // Se escribe un mensaje de registro al entrar al método.
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método confirmarCuenta() de la clase UsuarioServicioImpl");

                // Se busca un usuario con el token proporcionado.
                Usuario? usuarioExistente = _contexto.Usuarios.Where(u => u.Token == token).FirstOrDefault();

                // Si se encuentra un usuario con el token y su cuenta no está confirmada...
                if (usuarioExistente != null && !usuarioExistente.MailConfirmado)
                {
                    // Se marca la cuenta como confirmada y se elimina el token.
                    usuarioExistente.MailConfirmado = true;
                    usuarioExistente.Token = null;
                    _contexto.Usuarios.Update(usuarioExistente);
                    _contexto.SaveChanges();

                    // Se escribe un mensaje de registro al salir del método indicando que la cuenta se confirmó correctamente.
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método confirmarCuenta() de la clase UsuarioServicioImpl. Cuenta confirmada OK.");

                    // Se retorna true indicando que la cuenta se confirmó correctamente.
                    return true;
                }
                else
                {
                    // Se escribe un mensaje de registro al salir del método indicando que no se pudo confirmar la cuenta.
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método confirmarCuenta() de la clase UsuarioServicioImpl. La cuenta no existe o ya está confirmada");

                    // Se retorna false indicando que no se pudo confirmar la cuenta.
                    return false;
                }
            }
            catch (ArgumentException ae)
            {
                // Se atrapa la excepción de tipo ArgumentException, que podría ocurrir si hay un error al confirmar la cuenta.
                // Se escribe un mensaje de registro indicando el error.
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - confirmarCuenta()] Error al confirmar la cuenta " + ae.Message);

                // Se retorna false debido a que ocurrió un error al confirmar la cuenta.
                return false;
            }
            catch (Exception e)
            {
                // Se atrapa cualquier otra excepción que pueda ocurrir.
                // Se escribe un mensaje de registro indicando el error.
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - confirmarCuenta()] Error al confirmar la cuenta " + e.Message);

                // Se retorna false debido a que ocurrió un error al confirmar la cuenta.
                return false;
            }
        }

        /// <summary>
        /// Método para iniciar el proceso de recuperación de la cuenta de usuario.
        /// </summary>
        /// <param name="emailUsuario">El correo electrónico del usuario para iniciar el proceso de recuperación.</param>
        /// <returns>True si se inició el proceso de recuperación correctamente, de lo contrario, false.</returns>
        public bool iniciarProcesoRecuperacion(string emailUsuario)
        {
            try
            {
                // Se escribe un mensaje de registro al entrar al método.
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método iniciarProcesoRecuperacion() de la clase UsuarioServicioImpl");

                // Se busca un usuario existente con el correo electrónico proporcionado.
                Usuario? usuarioExistente = _contexto.Usuarios.FirstOrDefault(u => u.EmailUsuario == emailUsuario);

                // Si se encuentra un usuario con el correo electrónico proporcionado...
                if (usuarioExistente != null)
                {
                    // Se genera un token para el usuario y se establece una fecha de expiración.
                    string token = generarToken();
                    DateTime fechaExpiracion = DateTime.Now.AddMinutes(1);

                    // Se actualiza el token y la fecha de expiración del usuario en la base de datos.
                    usuarioExistente.Token = token;
                    usuarioExistente.ExpiracionToken = fechaExpiracion;
                    _contexto.Usuarios.Update(usuarioExistente);
                    _contexto.SaveChanges();

                    // Se envía un correo electrónico de recuperación al usuario.
                    string nombreUsuario = usuarioExistente.NombreUsuario;
                    _servicioEmail.enviarEmailRecuperacion(emailUsuario, nombreUsuario, token);

                    // Se escribe un mensaje de registro al salir del método.
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método iniciarProcesoRecuperacion() de la clase UsuarioServicioImpl.");

                    // Se retorna true indicando que se inició el proceso de recuperación correctamente.
                    return true;
                }
                else
                {
                    // Se escribe un mensaje de registro indicando que no se encontró un usuario con el correo electrónico proporcionado.
                    EscribirLog.escribirEnFicheroLog("[INFO] El usuario con email " + emailUsuario + " no existe");

                    // Se retorna false indicando que no se pudo iniciar el proceso de recuperación.
                    return false;
                }
            }
            catch (DbUpdateException dbe)
            {
                // Se atrapa la excepción específica para problemas de actualización en la base de datos.
                // Se escribe un mensaje de registro indicando el error.
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - iniciarProcesoRecuperacion()] Error de persistencia al actualizar la bbdd: " + dbe.Message);

                // Se retorna false debido a que ocurrió un error al iniciar el proceso de recuperación.
                return false;
            }
            catch (Exception ex)
            {
                // Se atrapa cualquier otra excepción que pueda ocurrir.
                // Se escribe un mensaje de registro indicando el error.
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - iniciarProcesoRecuperacion()] Error al iniciar el proceso de recuperación: " + ex.Message);

                // Se retorna false debido a que ocurrió un error al iniciar el proceso de recuperación.
                return false;
            }
        }

        /// <summary>
        /// Obtiene un usuario por su token.
        /// </summary>
        /// <param name="token">El token del usuario a buscar.</param>
        /// <returns>El objeto UsuarioDTO si se encuentra, de lo contrario, null.</returns>
        public UsuarioDTO obtenerUsuarioPorToken(string token)
        {
            try
            {
                // Se escribe un mensaje de registro al entrar al método.
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método obtenerUsuarioPorToken() de la clase UsuarioServicioImpl");

                // Se busca un usuario en la base de datos que tenga el token proporcionado.
                Usuario? usuarioExistente = _contexto.Usuarios.FirstOrDefault(u => u.Token == token);

                // Si se encuentra un usuario con el token proporcionado...
                if (usuarioExistente != null)
                {
                    // Se convierte el usuario a un objeto UsuarioDTO.
                    UsuarioDTO usuario = _convertirAdto.usuarioToDto(usuarioExistente);

                    // Se escribe un mensaje de registro al salir del método.
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método obtenerUsuarioPorToken() de la clase UsuarioServicioImpl.");

                    // Se retorna el usuario encontrado.
                    return usuario;
                }
                else
                {
                    // Se escribe un mensaje de registro al salir del método, indicando que no se encontró un usuario con el token proporcionado.
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método obtenerUsuarioPorToken() de la clase UsuarioServicioImpl. No existe el usuario con el token " + token);

                    // Se retorna null ya que no se encontró un usuario con el token proporcionado.
                    return null;
                }
            }
            catch (ArgumentNullException e)
            {
                // Se atrapa la excepción de tipo ArgumentNullException, que podría ocurrir si hay un error en la operación de búsqueda en la base de datos.
                // Se escribe un mensaje de registro indicando el error.
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - obtenerUsuarioPorToken()] Error al obtener usuario por token " + e.Message);

                // Se retorna null debido a que ocurrió un error y no se pudo obtener el usuario.
                return null;
            }
        }

        /// <summary>
        /// Método para modificar la contraseña de un usuario usando un token de confirmación.
        /// </summary>
        /// <param name="usuario">El objeto UsuarioDTO que contiene el token y la nueva contraseña.</param>
        /// <returns>True si se modificó la contraseña correctamente, de lo contrario, false.</returns>
        public bool modificarContraseñaConToken(UsuarioDTO usuario)
        {
            try
            {
                // Se escribe un mensaje de registro al entrar al método.
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método modificarContraseñaConToken() de la clase UsuarioServicioImpl");

                // Se busca un usuario existente con el token proporcionado.
                Usuario? usuarioExistente = _contexto.Usuarios.FirstOrDefault(u => u.Token == usuario.Token);

                // Si se encuentra un usuario con el token proporcionado...
                if (usuarioExistente != null)
                {
                    // Se encripta la nueva contraseña.
                    string nuevaContraseña = _servicioEncriptar.Encriptar(usuario.ClaveUsuario);

                    // Se actualiza la contraseña y se invalida el token ya usado.
                    usuarioExistente.ClaveUsuario = nuevaContraseña;
                    usuarioExistente.Token = null; // Se establece como null para invalidar el token ya consumido al cambiar la contraseña
                    _contexto.Usuarios.Update(usuarioExistente);
                    _contexto.SaveChanges();

                    // Se escribe un mensaje de registro al salir del método.
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método modificarContraseñaConToken() de la clase UsuarioServicioImpl.");

                    // Se retorna true indicando que se modificó la contraseña correctamente.
                    return true;
                }
            }
            catch (DbUpdateException dbe)
            {
                // Se atrapa la excepción específica para problemas de actualización en la base de datos.
                // Se escribe un mensaje de registro indicando el error.
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - modificarContraseñaConToken()] Error de persistencia al actualizar la bbdd: " + dbe.Message);
            }
            catch (ArgumentNullException e)
            {
                // Se atrapa la excepción de tipo ArgumentNullException, que podría ocurrir si hay un error al modificar la contraseña.
                // Se escribe un mensaje de registro indicando el error.
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - verificarCredenciales()] Error al modificar contraseña del usuario: " + e.Message);

                // Se retorna false debido a que ocurrió un error al modificar la contraseña.
                return false;
            }

            // Se retorna false si no se pudo modificar la contraseña por alguna razón.
            return false;
        }

        /// <summary>
        /// Método para verificar las credenciales de un usuario.
        /// </summary>
        /// <param name="emailUsuario">El correo electrónico del usuario.</param>
        /// <param name="claveUsuario">La contraseña del usuario.</param>
        /// <returns>True si las credenciales son válidas, de lo contrario, false.</returns>
        public bool verificarCredenciales(string emailUsuario, string claveUsuario)
        {
            try
            {
                // Se escribe un mensaje de registro al entrar al método.
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método verificarCredenciales() de la clase UsuarioServicioImpl");

                // Se encripta la contraseña proporcionada.
                string contraseñaEncriptada = _servicioEncriptar.Encriptar(claveUsuario);

                // Se busca un usuario con el correo electrónico y la contraseña encriptada.
                Usuario? usuarioExistente = _contexto.Usuarios.FirstOrDefault(u => u.EmailUsuario == emailUsuario && u.ClaveUsuario == contraseñaEncriptada);

                // Si no se encuentra un usuario con las credenciales proporcionadas...
                if (usuarioExistente == null)
                {
                    // Se escribe un mensaje de registro indicando que el usuario no se encontró.
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método verificarCredenciales() de la clase UsuarioServicioImpl. Username no encontrado");

                    // Se retorna false indicando que las credenciales no son válidas.
                    return false;
                }

                // Si el usuario no tiene la cuenta confirmada...
                if (!usuarioExistente.MailConfirmado)
                {
                    // Se escribe un mensaje de registro indicando que el usuario no tiene la cuenta confirmada.
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método verificarCredenciales() de la clase UsuarioServicioImpl. El usuario no tiene la cuenta confirmada");

                    // Se retorna false indicando que las credenciales no son válidas.
                    return false;
                }

                // Se escribe un mensaje de registro al salir del método.
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método verificarCredenciales() de la clase UsuarioServicioImpl");

                // Se retorna true indicando que las credenciales son válidas.
                return true;
            }
            catch (ArgumentNullException e)
            {
                // Se atrapa la excepción de tipo ArgumentNullException, que podría ocurrir si hay un error al verificar las credenciales.
                // Se escribe un mensaje de registro indicando el error.
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - verificarCredenciales()] Error al comprobar las credenciales del usuario: " + e.Message);

                // Se retorna false debido a que ocurrió un error al verificar las credenciales.
                return false;
            }
        }

        /// <summary>
        /// Método para obtener un usuario por su correo electrónico.
        /// </summary>
        /// <param name="email">El correo electrónico del usuario a buscar.</param>
        /// <returns>Un objeto UsuarioDTO que representa al usuario encontrado, o null si no se encontró ningún usuario.</returns>
        public UsuarioDTO obtenerUsuarioPorEmail(string email)
        {
            try
            {
                // Se escribe un mensaje de registro al entrar al método.
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método obtenerUsuarioPorEmail() de la clase UsuarioServicioImpl");

                // Se crea un objeto UsuarioDTO.
                UsuarioDTO usuarioDTO = new UsuarioDTO();

                // Se busca un usuario en la base de datos por su correo electrónico, incluyendo sus cuentas bancarias relacionadas.
                var usuario = _contexto.Usuarios
                        .Include(u => u.CuentasBancarias)
                        .Include(u => u.CitasUsuario)
                            .ThenInclude(c => c.OficinaCita)
                        .FirstOrDefault(u => u.EmailUsuario == email);

                // Si se encontró un usuario...
                if (usuario != null)
                {
                    // Se convierte el usuario encontrado a un objeto UsuarioDTO.
                    usuarioDTO = _convertirAdto.usuarioToDto(usuario);
                }

                // Se escribe un mensaje de registro al salir del método.
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método obtenerUsuarioPorEmail() de la clase UsuarioServicioImpl");

                // Se retorna el UsuarioDTO encontrado (o un objeto nulo si no se encontró ningún usuario).
                return usuarioDTO;
            }
            catch (ArgumentNullException e)
            {
                // Se atrapa la excepción de tipo ArgumentNullException, que podría ocurrir si hay un error al obtener el usuario por su correo electrónico.
                // Se imprime un mensaje de error en la consola.
                Console.WriteLine("[Error UsuarioServicioImpl - obtenerUsuarioPorEmail()] Error al obtener el usuario por email: " + e.Message);

                // Se retorna null debido a que ocurrió un error al obtener el usuario.
                return null;
            }
        }

        /// <summary>
        /// Método para obtener todos los usuarios.
        /// </summary>
        /// <returns>Una lista de objetos UsuarioDTO que representan a todos los usuarios.</returns>
        public List<UsuarioDTO> obtenerTodosLosUsuarios()
        {
            try
            {
                // Se escribe un mensaje de registro al entrar al método.
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método obtenerTodosLosUsuarios() de la clase UsuarioServicioImpl");

                // Se obtienen todos los usuarios de la base de datos y se convierten a objetos UsuarioDTO.
                return _convertirAdto.listaUsuarioToDto(_contexto.Usuarios.ToList());
            }
            catch (Exception e)
            {
                // Se atrapa cualquier excepción que pueda ocurrir durante la ejecución del método.
                // Se escribe un mensaje de registro indicando el error.
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - obtenerTodosLosUsuarios()] Error al obtener todos los usuarios: " + e.Message);

                // Se retorna una lista vacía en caso de error.
                return new List<UsuarioDTO>();
            }
        }

        /// <summary>
        /// Método para buscar un usuario por su ID.
        /// </summary>
        /// <param name="id">El ID del usuario a buscar.</param>
        /// <returns>Un objeto UsuarioDTO que representa al usuario encontrado, o null si no se encontró ningún usuario.</returns>
        public UsuarioDTO buscarPorId(long id)
        {
            try
            {
                // Se escribe un mensaje de registro al entrar al método.
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método buscarPorId() de la clase UsuarioServicioImpl");

                // Se busca un usuario en la base de datos por su ID.
                Usuario? usuario = _contexto.Usuarios.FirstOrDefault(u => u.IdUsuario == id);

                // Si se encuentra un usuario...
                if (usuario != null)
                {
                    // Se escribe un mensaje de registro al salir del método.
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método buscarPorId() de la clase UsuarioServicioImpl");

                    // Se convierte el usuario encontrado a un objeto UsuarioDTO y se retorna.
                    return _convertirAdto.usuarioToDto(usuario);
                }
            }
            catch (ArgumentException iae)
            {
                // Se atrapa la excepción de tipo ArgumentException, que podría ocurrir si hay un error al buscar el usuario por su ID.
                // Se escribe un mensaje de registro indicando el error.
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - buscarPorId()] Al buscar el usuario por su id " + iae.Message);
            }

            // Se retorna null si no se encontró ningún usuario o ocurrió un error.
            return null;
        }

        /// <summary>
        /// Método para eliminar un usuario por su ID.
        /// </summary>
        /// <param name="id">El ID del usuario a eliminar.</param>
        public void eliminar(long id)
        {
            try
            {
                // Se escribe un mensaje de registro al entrar al método.
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método eliminar() de la clase UsuarioServicioImpl");

                // Se busca un usuario en la base de datos por su ID.
                Usuario? usuario = _contexto.Usuarios.Find(id);

                // Si se encuentra un usuario...
                if (usuario != null)
                {
                    // Se elimina el usuario de la base de datos y se guardan los cambios.
                    _contexto.Usuarios.Remove(usuario);
                    _contexto.SaveChanges();

                    // Se escribe un mensaje de registro indicando que el usuario fue eliminado correctamente.
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método eliminar() de la clase UsuarioServicioImpl. Usuario eliminado OK");
                }
            }
            catch (DbUpdateException dbe)
            {
                // Se atrapa la excepción de tipo DbUpdateException, que podría ocurrir si hay un error al eliminar el usuario.
                // Se escribe un mensaje de registro indicando el error.
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - eliminar()] Error de persistencia al eliminar el usuario por su id " + dbe.Message);
            }
        }

        /// <summary>
        /// Actualiza un usuario en la base de datos con la información proporcionada en el objeto UsuarioDTO.
        /// </summary>
        /// <param name="usuarioModificado">El objeto UsuarioDTO con la información actualizada del usuario.</param>
        public void actualizarUsuario(UsuarioDTO usuarioModificado)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método actualizarUsuario() de la clase UsuarioServicioImpl");

                // Buscar el usuario en la base de datos
                Usuario? usuarioActual = _contexto.Usuarios.Find(usuarioModificado.IdUsuario);

                if (usuarioActual != null)
                {
                    // Actualizar la foto de perfil del usuario
                    usuarioActual.FotoPerfil = usuarioModificado.FotoPerfil;

                    // Guardar los cambios en la base de datos
                    _contexto.Usuarios.Update(usuarioActual);
                    _contexto.SaveChanges();
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método actualizarUsuario() de la clase UsuarioServicioImpl. Usuario actualizado OK");
                }
                else
                {
                    // Usuario no encontrado en la base de datos
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método actualizarUsuario() de la clase UsuarioServicioImpl. Usuario no encontrado");
                }
            }
            catch (DbUpdateException dbe)
            {
                // Error al actualizar el usuario en la base de datos
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - actualizarUsuario()] Error de persistencia al modificar el usuario " + dbe.Message);
            }
            catch (Exception ex)
            {
                // Error no controlado
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - actualizarUsuario()] Error no controlado: " + ex.Message);
            }
        }

        /// <summary>
        /// Cuenta el número de usuarios por rol.
        /// </summary>
        /// <param name="rol">El rol de los usuarios a contar.</param>
        /// <returns>El número de usuarios con el rol especificado.</returns>
        public int contarUsuariosPorRol(string rol)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método contarUsuariosPorRol() de la clase UsuarioServicioImpl");
                return _contexto.Usuarios.Count(u => u.Rol == rol);
            }
            catch (Exception ex)
            {
                // Error no controlado
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - contarUsuariosPorRol()] Error no controlado: " + ex.Message);
                throw; // Lanzar la excepción para que sea manejada en un nivel superior
            }
        }

        /// <summary>
        /// Busca usuarios por coincidencia en su email.
        /// </summary>
        /// <param name="palabra">La palabra a buscar en los emails de los usuarios.</param>
        /// <returns>Una lista de usuarios que contienen la palabra en su email, o null si no se encontraron coincidencias.</returns>
        public List<UsuarioDTO> buscarPorCoincidenciaEnEmail(string palabra)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método buscarPorCoincidenciaEnEmail() de la clase UsuarioServicioImpl");
                List<Usuario> usuarios = _contexto.Usuarios.Where(u => u.EmailUsuario.Contains(palabra)).ToList();
                if (usuarios != null)
                {
                    return _convertirAdto.listaUsuarioToDto(usuarios);
                }
            }
            catch (Exception ex)
            {
                // Error no controlado
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - buscarPorCoincidenciaEnEmail()] Al buscar el usuario por su email " + ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Asigna un nuevo rol a un usuario.
        /// </summary>
        /// <param name="usuarioDto">El DTO del usuario al que se le asignará el nuevo rol.</param>
        public void darRolUsuario(UsuarioDTO usuarioDto)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método darRolUsuario() de la clase UsuarioServicioImpl");
                var usuario = _contexto.Usuarios.Find(usuarioDto.IdUsuario);
                if (usuario != null && usuario.Rol.Equals("ROLE_USER"))
                {
                    usuario.Rol = "ROLE_ADMIN";
                    _contexto.SaveChanges();
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método darRolUsuario() de la clase UsuarioServicioImpl. Rol actualizado OK");
                }
                else
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método darRolUsuario() de la clase UsuarioServicioImpl. Usuario no encontrado o no tiene el rol correcto");
                }
            }
            catch (Exception ex)
            {
                // Error no controlado
                EscribirLog.escribirEnFicheroLog("[Error UsuarioServicioImpl - darRolUsuario()] Al buscar el usuario por su email " + ex.Message);
            }
        }
    }
}