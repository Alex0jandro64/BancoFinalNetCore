namespace BancoFinalNetCore.Util
{
    public static class EscribirLog
    {
        /// <summary>
        /// Método para escribir un mensaje en un archivo de registro.
        /// </summary>
        /// <param name="mensajeLog">Mensaje a escribir en el registro.</param>
        public static void escribirEnFicheroLog(string mensajeLog)
        {
            try
            {
                // Se intenta abrir o crear el archivo de registro
                using (FileStream fs = new FileStream(@AppDomain.CurrentDomain.BaseDirectory + "banco.log", FileMode.OpenOrCreate, FileAccess.Write))
                {
                    // Se utiliza un StreamWriter para escribir en el archivo de registro
                    using (StreamWriter m_streamWriter = new StreamWriter(fs))
                    {
                        // Se coloca el cursor al final del archivo
                        m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);

                        // Se formatea el mensaje de registro para incluir la fecha y hora actuales
                        mensajeLog = mensajeLog.Replace(Environment.NewLine, " | ");
                        mensajeLog = mensajeLog.Replace("\r\n", " | ").Replace("\n", " | ").Replace("\r", " | ");
                        m_streamWriter.WriteLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " " + mensajeLog);
                        m_streamWriter.Flush();
                    }
                }
            }
            catch (Exception e)
            {
                // Si ocurre un error al escribir en el archivo de registro, se imprime en la consola
                Console.WriteLine("[Error EscribirLog - escribirEnFicheroLog()] Error al escribir en el fichero log:" + e.Message);
            }
        }
    }
}