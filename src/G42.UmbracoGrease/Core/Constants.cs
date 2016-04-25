namespace G42.UmbracoGrease.Core
{
    public class Constants
    {
        public const string ERROR_REPORTING_ENABLE_KEY = "errorReporting:Enable";
        public const string ERROR_REPORTING_REPORTING_INTERVAL_KEY = "errorReporting:ReportingInterval";

        public const string ERROR_REPORTING_SLACK_ENABLE_KEY = "errorReporting:SlackEnable";
        public const string ERROR_REPORTING_SLACK_WEBHOOKURL_KEY = "errorReporting:SlackWebhookUrl";
        public const string ERROR_REPORTING_SLACK_BOTNAME_KEY = "errorReporting:SlackBotname";
        public const string ERROR_REPORTING_SLACK_CHANNEL_KEY = "errorReporting:SlackChannel";
        public const string ERROR_REPORTING_SLACK_EMOJI_KEY = "errorReporting:SlackEmoji";

        public const int ERROR_REPORTING_DEFAULT_INTERVAL = 15;

        public const string _404_TRACKER_DEFAULT_DAYS_TO_RETAIN_KEY = "_404tracker:DaysToRetain";
        public const int _404_TRACKER_DEFAULT_DAYS_TO_RETAIN = 90;
    }
}