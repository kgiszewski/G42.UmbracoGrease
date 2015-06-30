using System;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Xml;
using G42.UmbracoGrease.G42AppSettings.PetaPocoModels;
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

            _dllVersion = _version();

            LogHelper.Info<PackageActions>("Determining G42.UmbracoGrease:Version to be: " + _dllVersion);

            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings[versionAppsettingKey]))
            {
                LogHelper.Info<PackageActions>("Running initial setup block.");
                //_addDashboardTab();
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
                LogHelper.Info<PackageActions>("Running alternate setup block, found => " + ConfigurationManager.AppSettings[versionAppsettingKey]);

                if (ConfigurationManager.AppSettings[versionAppsettingKey] != _dllVersion)
                {
                    var config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                    config.AppSettings.Settings.Remove(versionAppsettingKey);
                    config.AppSettings.Settings.Add(versionAppsettingKey, _dllVersion);
                    config.Save();
                }
            }
        }

        private void _addDashboardTab()
        {
            var xd = new XmlDocument();

            xd.LoadXml(@"<Action runat='install' undo='false' alias='AddXmlFragment' file='~/config/Dashboard.config' xpath='//* [@alias=""StartupDeveloperDashboardSection""]' position='end'><tab caption='Umbraco Grease'><control>/app_plugins/G42.UmbracoGrease/developerdashboard/views/dashboard.html</control></tab></Action>");

            LogHelper.Info<PackageActions>("Running G42.UmbracoGrease dashboard action.");

            try
            {
                PackageAction.RunPackageAction("G42.UmbracoGrease", "AddXmlFragment", xd.FirstChild);
            }
            catch (Exception ex)
            {
                LogHelper.Error<PackageActions>(ex.Message, ex); 
            }
        }

        private void _addLanguageKey()
        {
            var xd = new XmlDocument();

            xd.LoadXml(@"<Action runat='install' undo='true' alias='AddLanguageFileKey' language='en' position='beginning' area='sections' key='G42UmbracoGrease' value='G42 Grease' />");

            LogHelper.Info<PackageActions>("Running Grease language action.");
            PackageAction.RunPackageAction("G42.UmbracoGrease", "AddLanguageFileKey", xd.FirstChild);
        }

        private string _version()
        {
            var asm = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(asm.Location);

            return fvi.FileVersion;
        }
    }
}