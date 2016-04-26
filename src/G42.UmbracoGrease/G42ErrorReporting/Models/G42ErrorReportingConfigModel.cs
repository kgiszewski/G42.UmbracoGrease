using System.Collections.Generic;
using System.Linq;
using G42.UmbracoGrease.Core;
using Newtonsoft.Json;
using G42.UmbracoGrease.G42AppSettings.Models;

namespace G42.UmbracoGrease.G42ErrorReporting.Models
{
    public class G42ErrorReportingConfigModel
    {
        [JsonProperty("enable")]
        public bool Enable { get; set; }
        [JsonProperty("reportingInterval")]
        public int ReportingInterval { get; set; }
        [JsonProperty("slackEnable")]
        public bool SlackEnable { get; set; }
        [JsonProperty("slackWebhookUrl")]
        public string SlackWebhookUrl { get; set; }
        [JsonProperty("slackBotName")]
        public string SlackBotName { get; set; }
        [JsonProperty("slackChannel")]
        public string SlackChannel { get; set; }
        [JsonProperty("slackEmoji")]
        public string SlackEmoji { get; set; }

        public G42ErrorReportingConfigModel()
        {
            
        }

        public G42ErrorReportingConfigModel(IEnumerable<G42AppSetting> settings)
        {
            var settingsList = settings.ToList();

            var enable = settingsList.FirstOrDefault(x => x.Key == Constants.ERROR_REPORTING_ENABLE_KEY);

            if(enable != null)
            {
                Enable = (enable.Value == "1");
            }

            var reportingInterval = settingsList.FirstOrDefault(x => x.Key == Constants.ERROR_REPORTING_REPORTING_INTERVAL_KEY);

            if (reportingInterval != null)
            {
                int reportingIntervalValue;

                int.TryParse(reportingInterval.Value, out reportingIntervalValue);

                ReportingInterval = reportingIntervalValue;
            }

            var slackEnable = settingsList.FirstOrDefault(x => x.Key == Constants.ERROR_REPORTING_SLACK_ENABLE_KEY);

            if (slackEnable != null)
            {
                SlackEnable = (slackEnable.Value == "1");
            }

            var slackWebhookUrl = settingsList.FirstOrDefault(x => x.Key == Constants.ERROR_REPORTING_SLACK_WEBHOOKURL_KEY);

            if (slackWebhookUrl != null)
            {
                SlackWebhookUrl = slackWebhookUrl.Value;
            }

            var slackBotname = settingsList.FirstOrDefault(x => x.Key == Constants.ERROR_REPORTING_SLACK_BOTNAME_KEY);

            if (slackBotname != null)
            {
                SlackBotName = slackBotname.Value;
            }

            var slackChannel = settingsList.FirstOrDefault(x => x.Key == Constants.ERROR_REPORTING_SLACK_CHANNEL_KEY);

            if (slackChannel != null)
            {
                SlackChannel = slackChannel.Value;
            }

            var slackEmoji = settingsList.FirstOrDefault(x => x.Key == Constants.ERROR_REPORTING_SLACK_EMOJI_KEY);

            if (slackEmoji != null)
            {
                SlackEmoji = slackEmoji.Value;
            }
        }
    }
}