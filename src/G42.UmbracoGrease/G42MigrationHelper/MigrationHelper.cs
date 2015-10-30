using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using G42.UmbracoGrease.G42MigrationHelper.Migrations;
using Umbraco.Core.Logging;

namespace G42.UmbracoGrease.G42MigrationHelper
{
    /// <summary>
    /// Class that is used to facilitate the migrations.
    /// </summary>
    public class MigrationHelper
    {
        /// <summary>
        /// Handles the migrations by using reflection to grab instances of MigrationBase.
        /// </summary>
        /// <param name="currentVersion">The current version.</param>
        public static void HandleMigrations(Version currentVersion)
        {
            LogHelper.Info<MigrationHelper>("Handling migrations based on current version =>" + currentVersion.ToString());

            var migrations = typeof (MigrationBase)
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof (MigrationBase)) && !t.IsAbstract)
                .Select(t => (MigrationBase) Activator.CreateInstance(t))
                .OrderBy(x => x.TargetVersion);

            LogHelper.Info<MigrationHelper>("Total Migrations=>" + migrations.Count());

            foreach (var migration in migrations)
            {
                LogHelper.Info<MigrationHelper>("Examining migration =>" + migration.TargetVersion);

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

        /// <summary>
        /// Helper that gets the DLL version.
        /// </summary>
        /// <returns></returns>
        public static string GetDllVersion()
        {
            var asm = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(asm.Location);

            return fvi.FileVersion;
        }
    }
}