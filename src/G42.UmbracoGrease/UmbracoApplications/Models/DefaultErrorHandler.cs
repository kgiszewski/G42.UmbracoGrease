using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using G42.UmbracoGrease.G42AppSettings.PetaPocoModels;
using G42.UmbracoGrease.Helpers;
using G42.UmbracoGrease.Interfaces;
using Umbraco.Core.Logging;

namespace G42.UmbracoGrease.UmbracoApplications.Models
{
    public class DefaultErrorHandler : IG42ErrorHandler
    {
        private static Dictionary<string, DateTime> _errorDictionary = new Dictionary<string, DateTime>();

        public void Execute(object sender, EventArgs e, Exception ex)
        {
            var context = HttpContext.Current;

            var sendTo = G42GreaseAppSetting.Get("G42.UmbracoGrease:ErrorEmailToCsv");

            var sendFrom = G42GreaseAppSetting.Get("G42.UmbracoGrease:ErrorEmailFrom");

            //computer hash of the url and error message and send email
            var hash = _calculateMD5Hash(string.Format("{0}{1}", context.Request.Url.AbsoluteUri, ex.Message));

            if (_errorDictionary.ContainsKey(hash))
            {
                if (DateTime.UtcNow.AddMinutes(-5) > _errorDictionary[hash])
                {
                    _sendEmail(sendTo, sendFrom, context, ex);
                    _errorDictionary["hash"] = DateTime.UtcNow;
                }
            }
            else
            {
                _errorDictionary.Add(hash, DateTime.UtcNow);
                _sendEmail(sendTo, sendFrom, context, ex);
            }
        }

        private string _getHeaders(HttpRequest request, bool useHtml = true)
        {
            var headers = String.Empty;

            foreach (var key in request.Headers.AllKeys)
            {
                var open = "";
                var close = "";

                if (useHtml)
                {
                    open = "<p>";
                    close = "</p>";
                }

                headers += string.Format("{2}{0}=>{1}{3}\n", key, request.Headers[key], open, close);
            }

            return headers;
        }

        private string _calculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            var sb = new StringBuilder();
            
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private void _sendEmail(G42GreaseAppSetting sendTo, G42GreaseAppSetting sendFrom, HttpContext context, Exception ex)
        {
            if (sendTo != null && sendFrom != null)
            {
                var sendToList = sendTo.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (sendToList.Any())
                {
                    LogHelper.Info<GreaseUmbracoApplication>("Sending email to the G42.UmbracoGreaseErrorEmailToCsv list: " + sendTo.Value);

                    LogHelper.Info<DefaultErrorHandler>(string.Format("{0}\n{1}\n{2}\n", context.Request.Url.AbsoluteUri, ex.Message, _getHeaders(context.Request, false)));

                    var message = string.Format("<p>{0}</p>\n{1}", ex.Message, _getHeaders(context.Request));

                    foreach (var email in sendToList)
                    {
                        FormHelper.SendMail(email.Trim(),
                            G42GreaseAppSetting.Get("G42.UmbracoGrease:ErrorEmailFrom").Value,
                            "Error reported by: " + context.Request.Url.AbsoluteUri,
                            message,
                            true);
                    }
                }

                context.Server.ClearError();

                context.Response.Redirect(G42GreaseAppSetting.Get("G42.UmbracoGrease:ErrorRedirectTo").Value);
            }
        }
    }
}