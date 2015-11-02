using System.Collections.Specialized;

namespace G42.UmbracoGrease.G42Slack.Models
{
    /// <summary>
    /// Model that represents a CLI message from Slack.
    /// </summary>
    public class SlackSlashCommandModel
    {
        public string Token { get; set; }
        public string TeamId { get; set; }
        public string TeamDomain { get; set; }
        public string ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Command { get; set; }
        public string Text { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlackSlashCommandModel"/> class.
        /// </summary>
        /// <param name="kvp">The KVP.</param>
        public SlackSlashCommandModel(NameValueCollection kvp)
        {
            Token = kvp.Get("token");
            TeamId = kvp.Get("team_id");
            TeamDomain = kvp.Get("team_domain");
            ChannelId = kvp.Get("channel_id");
            ChannelName = kvp.Get("channel_name");
            UserId = kvp.Get("user_id");
            UserName = kvp.Get("user_name");
            Command = kvp.Get("command");
            Text = kvp.Get("text");
        }
    }
}