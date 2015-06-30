using System;
using System.Collections.Generic;
using G42.UmbracoGrease.G42AppSettings.PetaPocoModels;
using G42.UmbracoGrease.Helpers;
using Umbraco.Core.Logging;

namespace G42.UmbracoGrease.G42AppSettings.Cache
{
    internal sealed class AppSettingsCache
    {
        private static volatile AppSettingsCache _instance;
        private static object _padLock = new Object();

        private AppSettingsCache()
        {
           
        }

        public DateTime CreatedOn { get; private set; }

        public static bool IsInitialized()
        {
            return _instance != null;
        }

        public IEnumerable<G42GreaseAppSetting> Items; 

        public static AppSettingsCache Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_padLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new AppSettingsCache();

                            _instance.CreatedOn = DateTime.UtcNow;

                            _instance.Items = DbHelper.DbContext.Database.Fetch<G42GreaseAppSetting>("SELECT * FROM G42GreaseAppSettings ORDER BY [key]");

                            LogHelper.Info<AppSettingsCache>("Loaded from DB");
                        }
                    }
                }
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }

        public static void Clear()
        {
            LogHelper.Info<AppSettingsCache>("Clearing...");
            _instance = null;
        }
    }
}