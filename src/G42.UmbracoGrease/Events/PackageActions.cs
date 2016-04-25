using System;
using System.Configuration;
using System.Xml;
using G42.UmbracoGrease.Core;
using G42.UmbracoGrease.G42MigrationHelper;
using G42.UmbracoGrease.G42SearchHelper.Models;
using umbraco.cms.businesslogic.packager;
using Umbraco.Core;
using Umbraco.Core.Logging;

namespace G42.UmbracoGrease.Events
{
    /// <summary>
    /// A startup class that handles package installation tasks.
    /// </summary>
    public class PackageActions : ApplicationEventHandler
    {
        private string _dllVersion;

        /// <summary>
        /// Overridable method to execute when Bootup is completed, this allows you to perform any other bootup logic required for the application.
        /// Resolution is frozen so now they can be used to resolve instances.
        /// </summary>
        /// <param name="umbracoApplication"></param>
        /// <param name="applicationContext"></param>
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationStarted(umbracoApplication, applicationContext);

            const string versionAppsettingKey = "G42.UmbracoGrease:Version";

            _dllVersion = MigrationHelper.GetDllVersion();

            LogHelper.Info<PackageActions>("Determining G42.UmbracoGrease:Version to be: " + _dllVersion);

            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings[versionAppsettingKey]))
            {
                LogHelper.Info<PackageActions>("Running initial setup block, this assumes a fresh install and may cause issues if DB tables already exist.");
                _addLanguageKey();

                Grease.Services.G42404Service.Create404DomainPathsTable();
                Grease.Services.G42404Service.Create404TrackerTable();
                Grease.Services.G42SearchService.CreateSearchTrackerKeywordsTable();
                Grease.Services.G42SearchService.CreateSearchTrackerSearchesTable();
                Grease.Services.G42AppSettingsService.CreateAppSettingsTable();

                var config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                config.AppSettings.Settings.Add(versionAppsettingKey, _dllVersion);
                config.Save();
            }
            else
            {
                var currentVersion = ConfigurationManager.AppSettings[versionAppsettingKey];

                LogHelper.Info<PackageActions>("Grease already installed => " + currentVersion);

                if (currentVersion != _dllVersion)
                {
                    LogHelper.Info<PackageActions>(string.Format("Grease upgrading {0} to {1} ", currentVersion, _dllVersion));

                    var config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                    config.AppSettings.Settings.Remove(versionAppsettingKey);
                    config.AppSettings.Settings.Add(versionAppsettingKey, _dllVersion);
                    config.Save();

                    MigrationHelper.HandleMigrations(new Version(currentVersion));
                }
            }
        }

        /// <summary>
        /// Adds the language key to the core XML language files.
        /// </summary>
        private void _addLanguageKey()
        {
            var xd = new XmlDocument();

            xd.LoadXml(@"<Action runat='install' undo='true' alias='AddLanguageFileKey' language='en' position='beginning' area='sections' key='G42UmbracoGrease' value='G42 Grease' />");

            LogHelper.Info<PackageActions>("Running Grease language action.");
            PackageAction.RunPackageAction("G42.UmbracoGrease", "AddLanguageFileKey", xd.FirstChild);
        }
    }
}