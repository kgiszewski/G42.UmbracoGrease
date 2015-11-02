using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using G42.UmbracoGrease.G42Slack.Models;
using G42.UmbracoGrease.Helpers;
using Newtonsoft.Json;

namespace G42.UmbracoGrease.G42Slack.Helpers
{
    /// <summary>
    /// Wrapper class to handle posting to Slack.
    /// </summary>
    public static class SlackHelper
    {
        /// <summary>
        /// Says something in Slack.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="username">The username.</param>
        /// <param name="url">The URL.</param>
        /// <param name="channel">The channel.</param>
        /// <param name="emoji">The emoji.</param>
        /// <param name="attachements">The attachements.</param>
        /// <param name="iconUrl">The icon URL.</param>
        /// <returns></returns>
        public static string SaySomething(string text, string username, string url, string channel = "", string emoji = "", IEnumerable<SlackAttachmentModel> attachements = null, string iconUrl = "")
        {
            var slackPost = new SlackSaySomethingModel()
            {
                Username = username,
                Channel = channel,
                Emoji = emoji,
                IconUrl = iconUrl,
                Text = text,
                Attachments = attachements
            };

            var postBody = JsonConvert.SerializeObject(slackPost);

            var content = new StringContent(postBody, Encoding.UTF8, "application/json");

            return WebHelper.Post(url, content).Result;
        }
    }
}