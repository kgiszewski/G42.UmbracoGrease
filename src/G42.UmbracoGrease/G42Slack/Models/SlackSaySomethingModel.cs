using System.Collections.Generic;
using Newtonsoft.Json;

namespace G42.UmbracoGrease.G42Slack.Models
{
    /// <summary>
    /// Model that represents a Slack message.
    /// </summary>
    public class SlackSaySomethingModel
    {
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "icon_emoji")]
        public string Emoji { get; set; }

        [JsonProperty(PropertyName = "icon_url")]
        public string IconUrl { get; set; }

        [JsonProperty(PropertyName = "channel")]
        public string Channel { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "attachments")]
        public IEnumerable<SlackAttachmentModel> Attachments { get; set; } 
    }
}