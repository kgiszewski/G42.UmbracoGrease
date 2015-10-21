using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using G42.UmbracoGrease.G42MigrationHelper.Migrations;
using Umbraco.Core.Logging;

namespace G42.UmbracoGrease.G42MigrationHelper
{
    public class MigrationHelper
    {
        public static void HandleMigrations(Version currentVersion)
        {
            LogHelper.Info<MigrationHelper>("Handling migrations based on current version =>" + currentVersion.ToString());

            var migrations = typeof (MigrationBase)
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof (MigrationBase)) && !t.IsAbstract)
                .Select(t => (MigrationBase) Activator.CreateInstance(t))
                .OrderBy(x => x.TargetVersion.Major)
                .ThenBy(x => x.TargetVersion.Minor)
                .ThenBy(x => x.TargetVersion.Build)
                .ThenBy(x => x.TargetVersion.Revision);

            foreach (var migration in migrations)
            {
                LogHelper.Info<MigrationHelper>("Examining migration =>" + migration.TargetVersion.ToString());

                if (migration.TargetVersion > currentVersion)
                {
                    LogHelper.Info<MigrationHelper>("Executing...");

                    try
                    {
                        migration.Excecute();
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error<MigrationHelper>("Migration failed: " + migration.GetType() + "\n" + ex.Message, ex);
                    }
                }
                else
                {
                    LogHelper.Info<MigrationHelper>("Skipped.");
                }
            }
        }

        public static string GetDllVersion()
        {
            var asm = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(asm.Location);

            return fvi.FileVersion;
        }
    }
}