using System;

namespace G42.UmbracoGrease.G42MigrationHelper.Migrations
{
    /// <summary>
    /// Class that represents a base migration.
    /// </summary>
    public abstract class MigrationBase
    {
        public abstract Version TargetVersion { get; }

        public abstract void Excecute();
    }
}