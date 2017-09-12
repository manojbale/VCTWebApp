using System;
using System.Net;
using System.Net.Mail;

namespace VCTWebApp.Shell
{
    public class EmailHelper : IDisposable
    {
        public string FromDisplayName { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsNotifyForSucess { get; set; }


        public EmailHelper()
        {
            FromDisplayName = string.Empty;
            From = string.Empty;
            To = string.Empty;
            Cc = string.Empty;
            Bcc = string.Empty;
            Subject = string.Empty;
            Message = string.Empty;
            IsNotifyForSucess = false;
        }

        internal bool SendMail(string mailServer, string mailPort, string mailUser, string mailPassword, string mailUseDefaultCredentials, string mailEnableSsl)
        {
            bool isEmailSent = false;
            var mailMessage = new System.Net.Mail.MailMessage();
            try
            {
                if (!string.IsNullOrEmpty(FromDisplayName.Trim()))
                    mailMessage.From = new System.Net.Mail.MailAddress(From, FromDisplayName);
                else
                    mailMessage.From = new System.Net.Mail.MailAddress(From);

                mailMessage.To.Add(To);

                if (Cc.Length > 0) mailMessage.CC.Add(Cc);
                if (Bcc.Length > 0) mailMessage.Bcc.Add(Bcc);

                mailMessage.Subject = Subject;
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = Message;
                var smtpClient = new System.Net.Mail.SmtpClient(mailServer);
                if (mailUser.Length > 0 && mailPassword.Length > 0)
                {
                    var mailCredential = new NetworkCredential(mailUser, mailPassword);
                    smtpClient.Credentials = mailCredential;
                }
                if (mailPort.Length > 0)
                    smtpClient.Port = Convert.ToInt16(mailPort);
                if (mailUseDefaultCredentials.Length > 0 && mailUseDefaultCredentials.Trim().ToUpper().Equals("TRUE"))
                    smtpClient.UseDefaultCredentials = true;
                if (mailEnableSsl.Length > 0 && mailEnableSsl.Trim().ToUpper().Equals("TRUE"))
                    smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);
                isEmailSent = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            mailMessage.Dispose();
            return isEmailSent;
        }

        public void Dispose()
        {
            try
            {
                From = string.Empty;
                To = string.Empty;
                Cc = string.Empty;
                Bcc = string.Empty;
                Subject = string.Empty;
                Message = string.Empty;
            }
            catch { }
        }

    }
}
