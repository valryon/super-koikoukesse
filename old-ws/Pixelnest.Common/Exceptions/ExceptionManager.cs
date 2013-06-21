using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Pixelnest.Common.Exceptions
{
    /// <summary>
    /// Gravité de l'exception
    /// </summary>
    public enum ExceptionGravity
    {
        Warning,
        Error,
        Fatal
    }

    /// <summary>
    /// Gestionnaire simple d'exceptions avec envoi de mails
    /// </summary>
    public class ExceptionManager
    {
        public delegate Dictionary<string, string> GetExtraMailDataEventHandler(Exception e, string gravity, string errorMessage);

        public event GetExtraMailDataEventHandler SetExtraMailData;
        #region Singleton

        private static ExceptionManager instance;

        /// <summary>
        /// Initialize le gestionnaire d'exception
        /// </summary>
        /// <param name="appicationName"></param>
        /// <param name="smtpServer"></param>
        /// <param name="emailAdresses"></param>
        public static void Initialize(string appicationName, string smtpServer, string fromAdress, List<string> toAdresses, GetExtraMailDataEventHandler evt = null)
        {
            instance = new ExceptionManager(appicationName, smtpServer, fromAdress, toAdresses);
            instance.SetExtraMailData += evt;
        }

        /// <summary>
        /// Gère une exception et envoi un mail aux personnes enregistrées
        /// </summary>
        /// <param name="e">Exception levée</param>
        /// <param name="gravity">Sévérité de l'erreur</param>
        /// <param name="logEnabled">Ecrire une trace dans le log (doit être initialisé !)</param>
        /// <param name="mailEnabled">Envoyer un mail</param>
        public static void HandleException(ExceptionGravity gravity, System.Exception e)
        {
            if (instance != null)
            {
                instance.processHandleException(e, gravity);
            }
        }

        #endregion

        // Comportement interne, tout n'est pas en statique

        private string applicationName;
        private string smtpServer;
        private string fromAdress;
        private List<string> toAdresses;

        private ExceptionManager(string appName, string smtpServer, string fromAdress, List<string> toAdresses)
        {
            this.applicationName = appName;
            this.smtpServer = smtpServer;
            this.fromAdress = fromAdress;
            this.toAdresses = toAdresses;
        }

        private void processHandleException(System.Exception e, ExceptionGravity gravity)
        {
            // Convertit l'exception en e-mail
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromAdress);
            mail.Subject = applicationName + " - Rapport d'exception";

            toAdresses.ForEach(ad => mail.CC.Add(new MailAddress(ad)));

            string body = string.Empty;

            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format(
                    "<p>Une exception a été levée et n'a pas été attrapée pour l'application {0}.</p>"
                    + "<ul>"
                    + "<li><strong>Date :</strong> {1}</li>", applicationName, DateTime.Now));

            if (this.SetExtraMailData != null)
            {
                Dictionary<string, string> extraData = SetExtraMailData(e, gravity.ToString(), extractException(e));
                foreach (string s in extraData.Keys)
                {
                    sb.AppendFormat("<li><strong>{0} :</strong>{1}</li>", s, extraData[s]);
                }
            }

            sb.Append(string.Format(
                    "<li><strong>Aide :</strong> {2}</li>"
                    + "<li><strong>Gravité :</strong> {3}</li>"
                    + "<li><strong>Détails :</strong><br />{4}</li></ul>"
                    , applicationName, DateTime.Now, e.HelpLink, gravity.ToString(), extractException(e)));


            mail.Body = sb.ToString();
            mail.Priority = MailPriority.Normal;
            mail.IsBodyHtml = true;

            sendMail(mail);
        }

        private string extractException(System.Exception e)
        {
            string text = string.Format(
               "<ul>"
               + "<li><strong>Type :</strong> {0}</li>"
               + "<li><strong>Message :</strong> {1}</li>"
               + "<li><strong>TargetSite :</strong> {2}</li>"
               + "<li><strong>Stacktrace :</strong> {3}</li>"
               , e.GetType().FullName, e.Message, e.TargetSite, formatStacktrace(e.StackTrace));

            if (e.InnerException != null)
            {
                text += string.Format("<li><strong>InnerException :</strong>{0}</li>", extractException(e.InnerException));
            }

            text += "</ul>";

            return text;
        }

        private string formatStacktrace(string rawStackTrace)
        {
            if (!string.IsNullOrEmpty(rawStackTrace))
            {
                return "<br />" + rawStackTrace.Replace("\r", "").Replace("\n", "<br />");
            }
            else
            {
                return string.Empty;
            }
        }

        private void sendMail(MailMessage mail)
        {
            using (SmtpClient smtp = new SmtpClient(smtpServer))
            {
                try
                {
                    smtp.Send(mail);
                }
                catch (System.Exception ex)
                {
                    throw new ApplicationException("Impossible d'envoyer un e-mail technique : " + ex.Message, ex);
                }
            }
        }
    }
}
