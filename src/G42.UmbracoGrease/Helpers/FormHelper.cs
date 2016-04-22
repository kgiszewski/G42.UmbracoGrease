using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace G42.UmbracoGrease.Helpers
{
    /// <summary>
    /// Helper that handles processing typical web forms.
    /// </summary>
    public static class FormHelper
    {
        /// <summary>
        /// Sends an email on behalf of the form.
        /// </summary>
        /// <param name="toCsv">To CSV.</param>
        /// <param name="from">From.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="html">if set to <c>true</c> [HTML].</param>
        /// <param name="ccCsv">The cc CSV.</param>
        /// <param name="bccCsv">The BCC CSV.</param>
        public static void SendMail(string toCsv, string from, string subject, string body, bool html, string ccCsv = "", string bccCsv = "")
        {
            using (var message = new MailMessage()
            {
                From = new MailAddress(from),
                Subject = subject,
                IsBodyHtml = html
            })
            {

                //to
                message.To.Add(toCsv);

                //cc
                if (!string.IsNullOrEmpty(ccCsv))
                {
                    message.CC.Add(ccCsv);
                }

                //bcc
                if (!string.IsNullOrEmpty(bccCsv))
                {
                    message.Bcc.Add(bccCsv);
                }

                message.Body = body;

                var smtp = new SmtpClient();
                smtp.Send(message);
            }
        }

        /// <summary>
        /// Basic helper to format a given key/value pair into something useable in an HTML email.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="addExtraBreak">if set to <c>true</c> [add extra break].</param>
        /// <returns></returns>
        public static string FormatField(string key, string value, bool addExtraBreak = false)
        {
            if (addExtraBreak)
            {
                return string.Format("<strong>{0}:<br/>\n</strong> {1}<br/>", key, value);
            }

            return string.Format("<strong>{0}:</strong> {1}<br/>", key, value);
        }
    }
}