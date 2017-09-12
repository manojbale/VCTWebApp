#region Header

/********************************************************************************
 * Class Name:      EmailSender.cs                                                      
 *
 * Module Name:     Alert Notification
 * 
 * Author:          Rajendra Negi
 * 
 * Description:     This class is used to send the alert notification to user.               
 * 
 * Creation Date:   16/03/2009  (dd/mm/yyyy)                                              
 *                                                                              
 * Date             Modified By             Change                                  
 * ------------------------------------------------------------------------------
 
  *******************************************************************************/

#endregion Header

using System;
using System.Net.Mail;
using System.Net;

namespace VCTWeb.Core.Domain
{
    public class EmailSender
    {
        #region Fields

        private SmtpClient _client;

        #endregion Fields

        #region Constructors

        public EmailSender(string host, int port, bool useSSL)
        {
            if (port > 0)
                _client = new SmtpClient(host, port);
            else
                _client = new SmtpClient(host);

            _client.Credentials = CredentialCache.DefaultNetworkCredentials;
            _client.EnableSsl = useSSL;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Sends the mail to specified person.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="messageText">The message text.</param>
        /// <returns>bool</returns>
        public bool Send(string from, string recipient, string subject, string messageText)
        {
            try
            {
                recipient = recipient.Replace(";", ",");
                _client.Send(from, recipient, subject, messageText);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Public Methods
    }
}
