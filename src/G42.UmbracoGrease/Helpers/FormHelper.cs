using System.Net.Mail;
using System.Text.RegularExpressions;

namespace G42.UmbracoGrease.Helpers
{
    public static class FormHelper
    {
        public static void SendMail(string toCsv, string from, string subject, string body, bool html, string ccCsv = "", string bccCsv = "")
        {
            var message = new MailMessage()
            {
                From = new MailAddress(from),
                Subject = subject,
                IsBodyHtml = html
            };

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

        public static void SendFormEmail(string sendToEmailCsv, string sendFromEmail, string sendToSubject, string formattedForm)
        {
            if (IsValidEmail(sendToEmailCsv) && IsValidEmail(sendFromEmail))
            {
                var messageBody = formattedForm;

                SendMail(sendToEmailCsv, sendFromEmail, sendToSubject, messageBody, true);
            }
        }

        public static bool IsValidEmail(this string str)
        {
            if (str == null)
                return false;

            var regex = new Regex(@"^([\w\.\-\+]+)@([\w\-]+)((\.(\w){2,3})+)$");
            var match = regex.Match(str);
            return match.Success;
        }

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