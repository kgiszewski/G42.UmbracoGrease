using System;
using System.Configuration;
using System.Xml;
using G42.UmbracoGrease.G42AppSettings.PetaPocoModels;
using G42.UmbracoGrease.G42MigrationHelper;
using G42.UmbracoGrease.Reports.PetaPocoModels;
using umbraco.cms.businesslogic.packager;
using Umbraco.Core;
using Umbraco.Core.Logging;

namespace G42.UmbracoGrease.Events
{
    public class PackageActions : ApplicationEventHandler
    {
        private string _dllVersion;

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

                G42Grease404Tracker.CreateTable();
                G42GreaseSearchTrackerKeyword.CreateTable();
                G42GreaseSearchTrackerSearch.CreateTable();
                G42GreaseAppSetting.CreateTable();

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

        private void _addLanguageKey()
        {
            var xd = new XmlDocument();

            xd.LoadXml(@"<Action runat='install' undo='true' alias='AddLanguageFileKey' language='en' position='beginning' area='sections' key='G42UmbracoGrease' value='G42 Grease' />");

            LogHelper.Info<PackageActions>("Running Grease language action.");
            PackageAction.RunPackageAction("G42.UmbracoGrease", "AddLanguageFileKey", xd.FirstChild);
        }
    }
}