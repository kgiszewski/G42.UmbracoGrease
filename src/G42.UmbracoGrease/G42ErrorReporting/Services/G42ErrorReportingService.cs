using G42.UmbracoGrease.Core;
using G42.UmbracoGrease.G42AppSettings.Repositories;
using G42.UmbracoGrease.G42AppSettings.Services;
using G42.UmbracoGrease.G42ErrorReporting.Models;

namespace G42.UmbracoGrease.G42ErrorReporting.Services
{
    public class G42ErrorReportingService
    {
        public bool SaveErrorReportingConfig(G42ErrorReportingConfigModel model)
        {
            using (var uow = new PetaPocoUnitOfWork())
            {
                //general
                G42AppSettingsService.SaveSetting(uow, Constants.ERROR_REPORTING_ENABLE_KEY, (model.Enable) ? "1" : "0");
                G42AppSettingsService.SaveSetting(uow, Constants.ERROR_REPORTING_REPORTING_INTERVAL_KEY, model.ReportingInterval.ToString());

                //slack
                G42AppSettingsService.SaveSetting(uow, Constants.ERROR_REPORTING_SLACK_ENABLE_KEY, (model.SlackEnable) ? "1" : "0");
                G42AppSettingsService.SaveSetting(uow, Constants.ERROR_REPORTING_SLACK_WEBHOOKURL_KEY, model.SlackWebhookUrl);
                G42AppSettingsService.SaveSetting(uow, Constants.ERROR_REPORTING_SLACK_BOTNAME_KEY, model.SlackBotName, "GreaseErrorBot");
                G42AppSettingsService.SaveSetting(uow, Constants.ERROR_REPORTING_SLACK_CHANNEL_KEY, model.SlackChannel);
                G42AppSettingsService.SaveSetting(uow, Constants.ERROR_REPORTING_SLACK_EMOJI_KEY, model.SlackEmoji, ":rotating_light:");

                uow.Commit();
            }

            return true;
        }

        public G42ErrorReportingConfigModel GetErrorReportingConfig()
        {
            using (var uow = new PetaPocoUnitOfWork())
            {
                var rawSettings = G42AppSettingRepository.GetErrorReportingConfigs(uow);

                return new G42ErrorReportingConfigModel(rawSettings);
            }
        }
    }
}