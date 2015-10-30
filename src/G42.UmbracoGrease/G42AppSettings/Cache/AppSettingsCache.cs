using System;
using System.Collections.Generic;
using G42.UmbracoGrease.G42AppSettings.PetaPocoModels;
using Umbraco.Core.Logging;

namespace G42.UmbracoGrease.G42AppSettings.Cache
{
    /// <summary>
    /// Singleton class that acts as a key/value pair cache.
    /// </summary>
    internal sealed class AppSettingsCache
    {
        private static volatile AppSettingsCache _instance;
        private static object _padLock = new Object();

        /// <summary>
        /// Prevents a default instance of the <see cref="AppSettingsCache"/> class from being created.
        /// </summary>
        private AppSettingsCache()
        {
           
        }

        public DateTime CreatedOn { get; private set; }

        /// <summary>
        /// Determines whether this instance is initialized.
        /// </summary>
        /// <returns></returns>
        public static bool IsInitialized()
        {
            return _instance != null;
        }

        public IEnumerable<G42GreaseAppSetting> Items;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
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

                            _instance.Items = G42GreaseAppSetting.GetAllFromDb();

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

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public static void Clear()
        {
            LogHelper.Info<AppSettingsCache>("Clearing...");
            _instance = null;
        }
    }
}