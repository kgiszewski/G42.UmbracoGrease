using System;
using System.Collections.Generic;
using System.Linq;
using G42.UmbracoGrease.G42AppSettings.Cache;
using G42.UmbracoGrease.Helpers;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;

namespace G42.UmbracoGrease.G42AppSettings.PetaPocoModels
{
    [PrimaryKey("id")]
    [TableName("G42GreaseAppSettings")]
    public class G42GreaseAppSetting
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime UpdatedOn { get; set; }

        public static G42GreaseAppSetting Get(string key)
        {
            return GetAll().FirstOrDefault(x => x.Key == key);
        }

        public static IEnumerable<G42GreaseAppSetting> GetAll()
        {
            return AppSettingsCache.Instance.Items;
        }

        internal static IEnumerable<G42GreaseAppSetting> GetAllFromDb()
        {
            return DbHelper.DbContext.Database.Fetch<G42GreaseAppSetting>("SELECT * FROM G42GreaseAppSettings ORDER BY [key]");
        }

        public static void Save(string key, string value)
        {
            var setting = Get(key);

            setting.UpdatedOn = DateTime.UtcNow;

            setting.Value = value;

            DbHelper.DbContext.Database.Save(setting);

            AppSettingsCache.Clear();
        }

        public static void Remove(string key)
        {
            DbHelper.DbContext.Database.Execute("DELETE FROM G42GreaseAppSettings WHERE [key] = @0", key);

            AppSettingsCache.Clear();
        }

        internal static void Remove(int id)
        {
            DbHelper.DbContext.Database.Execute("DELETE FROM G42GreaseAppSettings WHERE id = @0", id);

            AppSettingsCache.Clear();
        }

        public static void Add(string key, string value)
        {
            if (!string.IsNullOrEmpty(key))
            {
                var setting = Get(key);

                if (setting == null)
                {
                    LogHelper.Info<G42GreaseAppSetting>("Adding new key...");

                    if (value == null)
                    {
                        value = "";
                    }

                    DbHelper.DbContext.Database.Save(new G42GreaseAppSetting()
                    {
                        Key = key,
                        Value = value,
                        UpdatedOn = DateTime.UtcNow
                    });

                    AppSettingsCache.Clear();
                }
                else
                {
                    LogHelper.Info<G42GreaseAppSetting>("Key exists already=>" + key);
                }
            }
        }

        internal static void CreateTable()
        {
            if (!DbHelper.DbContext.Database.TableExist("G42GreaseAppSettings"))
            {
                LogHelper.Info<G42GreaseAppSetting>("Creating table.");

                DbHelper.DbContext.Database.Execute(@"
                    CREATE TABLE [dbo].[G42GreaseAppSettings](
	                    [id] [bigint] IDENTITY(1,1) NOT NULL,
	                    [key] [nvarchar](150) NOT NULL,
	                    [value] [nvarchar](150) NOT NULL,
	                    [updatedOn] [datetime] NOT NULL,
                     CONSTRAINT [PK_GreaseAppSettings] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
                    ) 
                ");
            }
            else
            {
                LogHelper.Info<G42GreaseAppSetting>("Table exists.");
            }
        }
    }
}