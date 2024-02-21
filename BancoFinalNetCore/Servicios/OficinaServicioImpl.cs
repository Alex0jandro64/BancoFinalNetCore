using BancoFinalNetCore.DTO;
using BancoFinalNetCore.Util;
using DAL.Entidades; // Importación de espacio de nombres para las entidades del modelo de datos.

namespace BancoFinalNetCore.Servicios
{
    /// <summary>
    /// Clase que implementa la interfaz IOficinaServicio y proporciona la lógica para trabajar con las oficinas.
    /// </summary>
    public class OficinaServicioImpl : IOficinaServicio
    {
        private readonly MyDbContext _contexto; // Contexto de base de datos para interactuar con las entidades.
        private readonly IConvertirAdto _convertirAdto; // Servicio para convertir entre DTO y entidades.

        /// <summary>
        /// Constructor de la clase OficinaServicioImpl.
        /// </summary>
        /// <param name="contexto">Contexto de la base de datos.</param>
        /// <param name="convertirAdto">Servicio de conversión DTO.</param>
        public OficinaServicioImpl(
            MyDbContext contexto,
            IConvertirAdto convertirAdto
        )
        {
            _contexto = contexto; // Inicialización del contexto de la base de datos.
            _convertirAdto = convertirAdto; // Inicialización del servicio de conversión DTO.
        }

        /// <summary>
        /// Método para obtener todas las oficinas.
        /// </summary>
        /// <returns>Una lista de objetos OficinaDTO que representan todas las oficinas en la base de datos.</returns>
        public List<OficinaDTO> obtenerTodasLasOficinas()
        {
            try
            {
                // Se escribe un mensaje de registro al entrar al método.
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método obtenerTodasLasOficinas() de la clase OficinaServicioImpl");

                // Se obtienen todas las oficinas de la base de datos y se convierten a objetos OficinaDTO.
                return _convertirAdto.listaOficinaToDto(_contexto.Oficinas.ToList());
            }
            catch (Exception e)
            {
                // Se atrapa cualquier excepción que pueda ocurrir durante la ejecución del método.
                // Se escribe un mensaje de registro indicando el error.
                EscribirLog.escribirEnFicheroLog("[Error OficinaServicioImpl - obtenerTodasLasOficinas()] Error al obtener todas las oficinas: " + e.Message);

                // Se retorna una lista vacía en caso de error.
                return new List<OficinaDTO>();
            }
        }
    }
}