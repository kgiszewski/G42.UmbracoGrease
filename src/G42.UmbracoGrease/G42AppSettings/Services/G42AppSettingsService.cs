using System;
using G42.UmbracoGrease.Core;
using G42.UmbracoGrease.G42AppSettings.Models;
using G42.UmbracoGrease.G42AppSettings.Repositories;
using G42.UmbracoGrease.G42ErrorReporting.Models;

namespace G42.UmbracoGrease.G42AppSettings.Services
{
    public class G42AppSettingsService
    {
        public T GetValue<T>(string key)
        {
            using(var uow = new PetaPocoUnitOfWork())
            {
                var setting = G42AppSettingRepository.Get(uow, key);

                if (setting != null)
                {
                    var value = setting.Value;

                    var convertingToType = typeof (T);

                    if (convertingToType == typeof (bool))
                    {
                        if (value == "1")
                        {
                            return (T) (object) true;
                        }

                        if (value == "0")
                        {
                            return (T) (object) false;
                        }
                    }

                    return (T)Convert.ChangeType(value, convertingToType);
                }

                return default(T);
            }
        }

        public bool SaveErrorReportingConfig(G42ErrorReportingConfigModel model)
        {
            using (var uow = new PetaPocoUnitOfWork())
            {
                //general
                _saveSetting(uow, Constants.ERROR_REPORTING_ENABLE_KEY, (model.Enable) ? "1" : "0");
                _saveSetting(uow, Constants.ERROR_REPORTING_REPORTING_INTERVAL_KEY, model.ReportingInterval.ToString());

                //slack
                _saveSetting(uow, Constants.ERROR_REPORTING_SLACK_ENABLE_KEY, (model.SlackEnable) ? "1" : "0");
                _saveSetting(uow, Constants.ERROR_REPORTING_SLACK_WEBHOOKURL_KEY, model.SlackWebhookUrl);
                _saveSetting(uow, Constants.ERROR_REPORTING_SLACK_BOTNAME_KEY, model.SlackBotName, "GreaseErrorBot");
                _saveSetting(uow, Constants.ERROR_REPORTING_SLACK_CHANNEL_KEY, model.SlackChannel);
                _saveSetting(uow, Constants.ERROR_REPORTING_SLACK_EMOJI_KEY, model.SlackEmoji, ":rotating_light:");

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

        public void CreateAppSettingsTable()
        {
            using (var uow = new PetaPocoUnitOfWork())
            {
                G42AppSettingRepository.CreateTable(uow);
            }
        }

        private void _saveSetting(PetaPocoUnitOfWork uow, string key, string value, string defaultValue = "")
        {
            value = value ?? defaultValue;

            if (key == Constants.ERROR_REPORTING_REPORTING_INTERVAL_KEY)
            {
                int reportingIntervalValue;

                if (!int.TryParse(value, out reportingIntervalValue) || reportingIntervalValue <= 0)
                {
                    value = Constants.ERROR_REPORTING_DEFAULT_INTERVAL.ToString();
                }
            }

            var setting = G42AppSettingRepository.Get(uow, key);

            if (setting == null)
            {
                setting = new G42AppSetting
                {
                    Key = key,
                    Value = value
                };
            }
            else
            {
                setting.Value = value;
            }

            G42AppSettingRepository.Save(uow, setting);
        }
    }
}