using System.Net.Mail;
using System.Text.RegularExpressions;

namespace G42.UmbracoGrease.Helpers
{
    public static class FormHelper
    {
        public static void SendMail(string to, string from, string subject, string body, bool html)
        {
            var message = new MailMessage
            {
                From = new MailAddress(from),
                Subject = subject,
                IsBodyHtml = html
            };

            message.To.Add(to);
            message.Body = body;
            var smtp = new SmtpClient();
            smtp.Send(message);
        }

        public static void SendFormEmail(string sendToEmail, string sendFromEmail, string sendToSubject, string formattedForm)
        {
            if (IsValidEmail(sendToEmail) && IsValidEmail(sendFromEmail))
            {
                var messageBody = formattedForm;

                SendMail(sendToEmail, sendFromEmail, sendToSubject, messageBody, true);
            }
        }

        public static bool IsValidEmail(this string str)
        {
            if (str == null)
                return false;

            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
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