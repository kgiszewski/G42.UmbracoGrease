using System;
using System.Collections.Generic;
using System.Web;
using G42.UmbracoGrease.Core;
using G42.UmbracoGrease.G42Slack.Helpers;
using G42.UmbracoGrease.G42Slack.Models;
using G42.UmbracoGrease.Helpers;
using G42.UmbracoGrease.Interfaces;
using Umbraco.Core.Logging;

namespace G42.UmbracoGrease.G42ErrorReporting
{
    /// <summary>
    /// The default implementation of the Grease error handler.
    /// </summary>
    public class DefaultErrorHandler : IG42ErrorHandler
    {
        private static Dictionary<string, DateTime> _errorDictionary = new Dictionary<string, DateTime>();

        /// <summary>
        /// Handles the specific error handling logic.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <param name="ex">The ex.</param>
        public void Execute(object sender, EventArgs e, Exception ex)
        {   
            var context = HttpContext.Current;

            //computer hash of the url and error message and send email
            var hash = SecurityHelper.CalculateMd5Hash(string.Format("{0}{1}", context.Request.Url.AbsoluteUri, ex.Message));

            var interval = Grease.Services.G42AppSettingsService.GetValue<int>(Constants.ERROR_REPORTING_REPORTING_INTERVAL_KEY);

            if (_errorDictionary.ContainsKey(hash))
            {
                if (DateTime.UtcNow.AddMinutes(interval * -1) > _errorDictionary[hash])
                {
                    _errorDictionary[hash] = DateTime.UtcNow;

                    _saySomethingInSlack(context, ex, hash, interval);

                    _sendErrorToLog(context, ex, hash, interval);
                }
            }
            else
            {
                _errorDictionary.Add(hash, DateTime.UtcNow);

                _saySomethingInSlack(context, ex, hash, interval);

                _sendErrorToLog(context, ex, hash, interval);
            }
        }

        /// <summary>
        /// _sends the error to log.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="hash">The hash.</param>
        /// <param name="interval">The interval.</param>
        private void _sendErrorToLog(HttpContext context, Exception ex, string hash, int interval)
        {
            LogHelper.Info<DefaultErrorHandler>(string.Format("{0}\n{1}\n{2}\n{3}\n{4}\n{5}\n", context.Request.Url.AbsoluteUri, ex.Message, WebHelper.GetHeaders(false), IpHelper.GetIpAddress(), hash, interval));
        }

        /// <summary>
        /// Say something in Slack based on configured settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="hash">The hash.</param>
        /// <param name="interval">The interval.</param>
        /// <returns></returns>
        private string _saySomethingInSlack(HttpContext context, Exception ex, string hash, int interval)
        {
            var enabled = Grease.Services.G42AppSettingsService.GetValue<bool>(Constants.ERROR_REPORTING_SLACK_ENABLE_KEY);
            var hookUrl = Grease.Services.G42AppSettingsService.GetValue<string>(Constants.ERROR_REPORTING_SLACK_WEBHOOKURL_KEY);
            var channel = Grease.Services.G42AppSettingsService.GetValue<string>(Constants.ERROR_REPORTING_SLACK_CHANNEL_KEY);
            var botName = Grease.Services.G42AppSettingsService.GetValue<string>(Constants.ERROR_REPORTING_SLACK_BOTNAME_KEY);
            var emoji = Grease.Services.G42AppSettingsService.GetValue<string>(Constants.ERROR_REPORTING_SLACK_EMOJI_KEY);

            if (enabled && !string.IsNullOrEmpty(hookUrl) && !string.IsNullOrEmpty(channel))
            {
                return SlackHelper.SaySomething(ex.Message, botName, hookUrl, channel, emoji, new List<SlackAttachmentModel>
                {
                    new SlackAttachmentModel
                    {
                        Title = "Path",
                        Text = context.Request.Url.AbsoluteUri
                    },

                    new SlackAttachmentModel
                    {
                        Title = "Headers",
                        Text = WebHelper.GetHeaders(false)
                    },

                    new SlackAttachmentModel
                    {
                        Title = "IP",
                        Text = IpHelper.GetIpAddress()
                    },

                    new SlackAttachmentModel
                    {
                        Title = "Hash",
                        Text = hash
                    },

                    new SlackAttachmentModel
                    {
                        Title = "Interval Period in Minutes",
                        Text = interval.ToString()
                    }
                });
            }

            return "Slack not configured.";
        }
    }
}