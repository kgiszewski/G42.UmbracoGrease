using Newtonsoft.Json;

namespace G42.UmbracoGrease.G42Slack.Models
{
    public class SlackAttachmentFieldModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("short")]
        public bool Short { get; set; }
    }
}