using System;
using G42.UmbracoGrease.Helpers;

namespace G42.UmbracoGrease.G42MigrationHelper.Migrations
{
    public class TargetVersionZeroSixTwo : MigrationBase
    {

        public override Version TargetVersion
        {
            get { return new Version("0.6.2"); }
        }

        public override void Excecute()
        {      
            var sql = @"
                ALTER TABLE [G42Grease404Tracker]
                ADD [ipAddress] NVARCHAR(50) NULL
            ";

            DbHelper.DbContext.Database.Execute(sql);
        }
    }
}