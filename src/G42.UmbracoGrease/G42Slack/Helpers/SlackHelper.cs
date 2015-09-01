using System.Net.Http;
using System.Text;
using G42.UmbracoGrease.G42Slack.Models;
using G42.UmbracoGrease.Helpers;
using Newtonsoft.Json;

namespace G42.UmbracoGrease.G42Slack.Helpers
{
    public static class SlackHelper
    {
        public static string SaySomething(string text, string username, string url, string channel = "", string emoji = "", string iconUrl = "")
        {
            var slackPost = new SlackSaySomethingModel()
            {
                Username = username,
                Channel = channel,
                Emoji = emoji,
                IconUrl = iconUrl,
                Text = text
            };

            var postBody = JsonConvert.SerializeObject(slackPost);

            var content = new StringContent(postBody, Encoding.UTF8, "application/json");

            return WebHelper.Post(url, content).Result;
        }
    }
}