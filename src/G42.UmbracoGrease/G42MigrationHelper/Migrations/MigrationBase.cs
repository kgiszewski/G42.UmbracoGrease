using System;

namespace G42.UmbracoGrease.G42MigrationHelper.Migrations
{
    public abstract class MigrationBase
    {
        public abstract Version TargetVersion { get; }

        public abstract void Excecute();
    }
}