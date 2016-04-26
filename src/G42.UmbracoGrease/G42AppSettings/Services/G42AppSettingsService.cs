using System;
using G42.UmbracoGrease.Core;
using G42.UmbracoGrease.G42AppSettings.Models;
using G42.UmbracoGrease.G42AppSettings.Repositories;

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

        public void CreateAppSettingsTable()
        {
            using (var uow = new PetaPocoUnitOfWork())
            {
                G42AppSettingRepository.CreateTable(uow);
            }
        }

        internal static void SaveSetting(PetaPocoUnitOfWork uow, string key, string value, string defaultValue = "")
        {
            value = value ?? defaultValue;

            //some special cases
            if (key == Constants.ERROR_REPORTING_REPORTING_INTERVAL_KEY)
            {
                int reportingIntervalValue;

                if (!int.TryParse(value, out reportingIntervalValue) || reportingIntervalValue <= 0)
                {
                    value = Constants.ERROR_REPORTING_DEFAULT_INTERVAL.ToString();
                }
            }

            if (key == Constants._404_TRACKER_DEFAULT_DAYS_TO_RETAIN_KEY)
            {
                int reportingIntervalValue;

                if (!int.TryParse(value, out reportingIntervalValue) || reportingIntervalValue <= 0)
                {
                    value = Constants._404_TRACKER_DEFAULT_DAYS_TO_RETAIN.ToString();
                }
            }


            //get on with the saving bit
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