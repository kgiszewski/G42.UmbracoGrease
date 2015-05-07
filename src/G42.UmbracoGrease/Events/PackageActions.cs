using System;
using System.Configuration;
using System.Reflection;
using System.Xml;
using umbraco.cms.businesslogic.packager;
using Umbraco.Core;
using Umbraco.Core.Logging;

namespace G42.UmbracoGrease.Events
{
    public class PackageActions : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationStarted(umbracoApplication, applicationContext);

            const string versionAppsettingKey = "G42.UmbracoGrease:Version";

            LogHelper.Info<PackageActions>("Determining G42.UmbracoGrease:Version");

            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings[versionAppsettingKey]))
            {
                _addDashboardTab();

                var config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                config.AppSettings.Settings.Add(versionAppsettingKey, Assembly.GetExecutingAssembly().GetName().Version.ToString());
                config.Save();
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
    }
}