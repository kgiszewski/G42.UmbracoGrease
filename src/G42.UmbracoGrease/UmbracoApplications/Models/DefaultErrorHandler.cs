using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using G42.UmbracoGrease.G42AppSettings.PetaPocoModels;
using G42.UmbracoGrease.G42Slack.Helpers;
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

            //computer hash of the url and error message and send email
            var hash = SecurityHelper.CalculateMd5Hash(string.Format("{0}{1}", context.Request.Url.AbsoluteUri, ex.Message));

            if (_errorDictionary.ContainsKey(hash))
            {
                var sendInterval = G42GreaseAppSetting.Get("G42.UmbracoGrease:ErrorEmailInterval");

                var interval = 15;
                var tempInterval = 0;

                if (sendInterval != null)
                {
                    if (Int32.TryParse(sendInterval.Value, out interval))
                    {
                        interval = tempInterval;
                    }
                }

                if (DateTime.UtcNow.AddMinutes(interval * -1) > _errorDictionary[hash])
                {
                    _sendEmail(context, ex);

                    _saySomethingInSlack(context, ex);

                    _errorDictionary["hash"] = DateTime.UtcNow;

                    LogHelper.Info<DefaultErrorHandler>(string.Format("{0}\n{1}\n{2}\n{3}\n", context.Request.Url.AbsoluteUri, ex.Message, WebHelper.GetHeaders(false), IpHelper.GetIpAddress()));
                }
            }
            else
            {
                _errorDictionary.Add(hash, DateTime.UtcNow);

                 _sendEmail(context, ex);

                _saySomethingInSlack(context, ex);

                LogHelper.Info<DefaultErrorHandler>(string.Format("{0}\n{1}\n{2}\n{3}\n", context.Request.Url.AbsoluteUri, ex.Message, WebHelper.GetHeaders(false), IpHelper.GetIpAddress()));
            }
        }

        private string _saySomethingInSlack(HttpContext context, Exception ex)
        {
            var disabled = G42GreaseAppSetting.Get("G42.UmbracoGrease:SlackErrorDisabled");
            var hookUrl = G42GreaseAppSetting.Get("G42.UmbracoGrease:SlackErrorHookUrl");
            var channel = G42GreaseAppSetting.Get("G42.UmbracoGrease:SlackErrorChannel");
            var botName = G42GreaseAppSetting.Get("G42.UmbracoGrease:SlackErrorBotName");
            var emoji = G42GreaseAppSetting.Get("G42.UmbracoGrease:SlackErrorBotEmoji");

            if ((disabled == null || disabled.Value != "1") && hookUrl != null && channel != null)
            {
                var botNameValue = (botName != null) ? botName.Value : "GreaseBot";
                var emojiValue = (emoji != null) ? emoji.Value : ":rotating_light:";

                var message = string.Format("{0}\n{1}\n{2}\n{3}\n", context.Request.Url.AbsoluteUri, ex.Message, WebHelper.GetHeaders(false), IpHelper.GetIpAddress());

                return SlackHelper.SaySomething(message, botNameValue, hookUrl.Value, channel.Value, emojiValue);
            }

            return "Slack not configured.";
        }

        private void _sendEmail(HttpContext context, Exception ex)
        {
            var disabled = G42GreaseAppSetting.Get("G42.UmbracoGrease:EmailErrorDisabled");
            var sendTo = G42GreaseAppSetting.Get("G42.UmbracoGrease:ErrorEmailToCsv");
            var sendFrom = G42GreaseAppSetting.Get("G42.UmbracoGrease:ErrorEmailFrom");

            if ((disabled == null || disabled.Value != "1") && sendTo != null && sendFrom != null && !string.IsNullOrEmpty(sendTo.Value) && !string.IsNullOrEmpty(sendFrom.Value))
            {
                var sendToList = sendTo.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (sendToList.Any())
                {
                    LogHelper.Info<GreaseUmbracoApplication>("Sending email to the G42.UmbracoGreaseErrorEmailToCsv list: " + sendTo.Value);

                    var message = string.Format("<p>{0}</p>\n{1}\n<p>{2}</p>\n", ex.Message, WebHelper.GetHeaders(), IpHelper.GetIpAddress());

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