using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI.WebControls;

namespace Blanquita_Inventarios.Site.Helpers
{
    public class Correo
    {
        public static string SendEmail_General(string plantilla, string asunto, string recipients, ListDictionary dictionary, string filePaths = "")
        {
            string result = string.Empty;

            try
            {
                string sgUsername = Config.EmailNombre;
                string sgPassword = Config.EmailPassword;
                string fromAddress = Config.Email;
                string emailHost = Config.ServidorSMTP;
                int emailPort = Config.PuertoSMTP;
                bool useSSL = Config.UsarSSL;
                int smtpTimeout = Config.SmtpTimeout;

                string addressCopyTO = "";
                if (recipients.Split(';').Length > 1)
                {
                    addressCopyTO = recipients.Substring((recipients.IndexOf(';') + 1));
                    recipients = recipients.Split(';')[0];
                }

                SmtpClient smtpClient = new SmtpClient()
                {
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(fromAddress, sgPassword),
                    Port = emailPort,
                    Host = emailHost,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = useSSL,
                    Timeout = smtpTimeout
                };

                string baseHtml = File.ReadAllText(Config.DirectorioPlantillas + "\\" + plantilla);

                MailDefinition md = new MailDefinition();
                md.From = fromAddress;
                md.IsBodyHtml = true;
                md.Subject = asunto;

                MailMessage message = md.CreateMailMessage(recipients, dictionary, baseHtml, new System.Web.UI.Control());
                message.From = new MailAddress(fromAddress, sgUsername);

                //Agregamos a los destinatarios en copia si es que hay más de 1
                if (!String.IsNullOrEmpty(addressCopyTO))
                {
                    foreach (string copyTo in addressCopyTO.Split(';'))
                    {
                        message.CC.Add(new MailAddress(copyTo));
                    }
                }

                //Agregamos el documento a adjuntar al email en caso de que se indique el archivo
                if (!String.IsNullOrEmpty(filePaths))
                {
                    foreach (string documentAttachment in filePaths.Split(';'))
                    {
                        //Revisamos si el archivo se encuentra en la ruta indicada
                        if (File.Exists(documentAttachment))
                        {
                            Attachment att = new Attachment(documentAttachment);
                            att.Name = System.IO.Path.GetFileName(documentAttachment);
                            message.Attachments.Add(att);
                        }
                    }
                }

                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                result = ex.Message;

                if (ex.InnerException != null)
                    result += "(**" + ex.InnerException.Message + "**)";

                Logs.General(DateTime.Now, "Send Email General", result);
            }

            return result;
        }
    }
}