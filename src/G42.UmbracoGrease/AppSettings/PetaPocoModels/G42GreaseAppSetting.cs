using System;
using G42.UmbracoGrease.Helpers;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;

namespace G42.UmbracoGrease.AppSettings.PetaPocoModels
{
    [PrimaryKey("id")]
    [TableName("G42GreaseAppSettings")]
    public class G42GreaseAppSetting
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime UpdatedOn { get; set; }

        public static G42GreaseAppSetting GetAppSetting(string key)
        {
            var x = DbHelper.DbContext.Database.SingleOrDefault<G42GreaseAppSetting>("SELECT * FROM G42GreaseAppSettings WHERE [key] = @0", key);

            LogHelper.Info<G42GreaseAppSetting>((x == null).ToString);

            return x;
        }

        public static void SetAppSetting(string key, string value)
        {
            var setting = DbHelper.DbContext.Database.SingleOrDefault<G42GreaseAppSetting>("SELECT * FROM G42GreaseAppSettings WHERE [key] = @0", key);

            setting.UpdatedOn = DateTime.UtcNow;

            setting.Value = value;

            DbHelper.DbContext.Database.Save(setting);
        }
    }
}