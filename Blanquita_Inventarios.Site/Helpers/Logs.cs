using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blanquita_Inventarios.Site.Helpers
{
    public static class Logs
    {
        /// <summary>
        /// Genera un archivo de texto en el servidor y graba el mensaje recibido
        /// </summary>
        /// <param name="mensaje">Mensaje a grabar</param>
        public static void General(DateTime fecha, string funcion, string mensaje)
        {
            // 1: Genera un identificador           
            DateTime today = DateTime.Now;
            string file = "LOG_" + fecha.ToString("yyyyMMdd") + ".log";
            string directory = Config.DirectorioLog;
            string directoryAndFile = directory + "\\" + file;

            System.IO.StreamWriter sw = new System.IO.StreamWriter(directoryAndFile, true);
            sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + ":::[" + funcion + "]:::" + mensaje);
            sw.Close();
        }
    }
}