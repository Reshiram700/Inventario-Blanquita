using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace Blanquita_Inventarios.Site.Helpers
{
    public class Config
    {
        /// <summary>
        /// Ruta completa del directorio físico donde reside la aplicación web.
        /// </summary>
        public static readonly string DirectorioAplicacion = ConfigurationManager.AppSettings["DirectorioAplicacion"];

        /// <summary>
        /// Ruta completa del directorio fisico donde se almacenaran los archivos
        /// </summary>
        public static readonly string DirectorioDocumentos = ConfigurationManager.AppSettings["DirectorioDocumentos"];

        /// <summary>
        /// Ruta completa del directorio fisico donde se almacenaran los archivos de plantillas
        /// </summary>
        public static readonly string DirectorioPlantillas = DirectorioAplicacion + "\\Plantillas";

        /// <summary>
        /// Ruta completa del directorio fisico donde se almacenaran los archivos de LOGS
        /// </summary>
        /// <summary>
        /// Ruta completa del directorio fisico donde se almacenaran los archivos de LOGS
        /// </summary>
        public static string DirectorioLog
        {
            get
            {
                // Usar App_Data/Logs - siempre tiene permisos de escritura
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "Logs");
                
                try
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }
                catch
                {
                    // Si falla, usar Temp
                    path = Path.GetTempPath();
                }
                
                return path;
            }
        }

        /// <summary>
        /// Url del Sitio web
        /// </summary>
        public static readonly string UrlSitio = ConfigurationManager.AppSettings["UrlSitio"];

        /// Configuracion para el envio de correos
        public static readonly string ServidorSMTP = ConfigurationManager.AppSettings["ServidorSMTP"];
        public static readonly int PuertoSMTP = Convert.ToInt32(ConfigurationManager.AppSettings["PuertoSMTP"]);
        public static readonly bool UsarSSL = ConfigurationManager.AppSettings["UsaSSL"].ToUpper() == "TRUE";
        public static readonly int SmtpTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpTimeout"]);
        public static readonly string EmailNombre = ConfigurationManager.AppSettings["InformadorNombre"];
        public static readonly string Email = ConfigurationManager.AppSettings["InformadorEmail"];
        public static readonly string EmailPassword = ConfigurationManager.AppSettings["InformadorPassword"];

        public static readonly int SqlTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["SqlTimeout"]);


        /// Configuracion de conexion del SAP
        public static readonly string SAPHostname = ConfigurationManager.AppSettings["SAPHostname"];
        public static readonly string SAPPuerto = ConfigurationManager.AppSettings["SAPPuerto"];
        public static readonly string SAPUser = ConfigurationManager.AppSettings["SAPUser"];
        public static readonly string SAPPwd = ConfigurationManager.AppSettings["SAPPwd"];
        public static readonly string SAPSchema = ConfigurationManager.AppSettings["SAPSchema"];

        /// Configuracion de conexion a DI API SAP
        public static readonly string DIAPI_Server = ConfigurationManager.AppSettings["DIAPI_Server"];
        public static readonly string DIAPI_CompanyDB = ConfigurationManager.AppSettings["DIAPI_CompanyDB"];
        public static readonly string DIAPI_UserName = ConfigurationManager.AppSettings["DIAPI_UserName"];
        public static readonly string DIAPI_Password = ConfigurationManager.AppSettings["DIAPI_Password"];
        public static readonly string DIAPI_DbUserName = ConfigurationManager.AppSettings["DIAPI_DbUserName"];
        public static readonly string DIAPI_DbPassword = ConfigurationManager.AppSettings["DIAPI_DbPassword"];
        public static readonly bool DIAPI_UseTrusted = ConfigurationManager.AppSettings["DIAPI_UseTrusted"].ToUpper() == "TRUE";

    }
}