using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace prjframe
{
    internal static class emailService
    {
        // Lecture des paramètres SMTP depuis App.config
        private static readonly string Host = ConfigurationManager.AppSettings["SmtpHost"];
        private static readonly int Port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
        private static readonly string User = ConfigurationManager.AppSettings["SmtpUser"];
        private static readonly string Pass = ConfigurationManager.AppSettings["SmtpPass"];
        private static readonly string From = ConfigurationManager.AppSettings["SmtpFrom"];
        private static readonly string FromName = ConfigurationManager.AppSettings["SmtpFromName"];

        /// <summary>
        /// Envoie un email HTML avec un logo inline.
        /// </summary>
        /// <param name="to">Adresse du destinataire</param>
        /// <param name="subject">Objet de l’email</param>
        /// <param name="bodyContent">Corps du message (HTML)</param>
        public static void SendEmail(string to, string subject, string bodyContent)
        {
            using (var msg = new MailMessage())
            {
                msg.From = new MailAddress(From, FromName);
                msg.To.Add(to);
                msg.Subject = subject;
                msg.IsBodyHtml = true;

                // Construction du HTML : HEADER sans logo, footer avec logo agrandi
                string html = $@"
                    <html>
                      <body style=""font-family:Segoe UI, sans-serif; color:#333;"">
                        <!-- Contenu principal -->
                        <div style=""padding:20px;"">
                           {bodyContent}
                        </div>

                        <!-- Séparateur -->
                        <hr style=""margin:30px 0; border:none; border-top:1px solid #ccc;"" />

                        <!-- FOOTER -->
                        <div style=""text-align:center; font-size:0.9em; color:#666;"">
                          <p>Centre Médical MonApp • 123 rue Exemple, Tunis • Tél : +216 12 345 678</p>
                          <!-- Logo plus gros en footer -->
                          <div style=""margin-top:15px;"">
                            <img src=""cid:logo"" alt=""Logo"" style=""width:200px; height:auto;"" />
                          </div>
                        </div>
                      </body>
                    </html>";

                // Création de la vue HTML et ajout du logo inline
                AlternateView view = AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html);
                string logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logo.png");
                var logo = new LinkedResource(logoPath, "image/png")
                {
                    ContentId = "logo",
                    TransferEncoding = TransferEncoding.Base64
                };
                view.LinkedResources.Add(logo);
                msg.AlternateViews.Add(view);

                // Envoi SMTP
                using (var smtp = new SmtpClient(Host, Port))
                {
                    smtp.Credentials = new NetworkCredential(User, Pass);
                    smtp.EnableSsl = true;
                    smtp.Send(msg);
                }
            }
        }
    }
}

