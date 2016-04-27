using System;
using G42.UmbracoGrease.Core;

namespace G42.UmbracoGrease.G42MigrationHelper.Migrations
{
    public class TargetVersionOneZeroZero : MigrationBase
    {

        public override Version TargetVersion
        {
            get { return new Version("1.0.0"); }
        }

        public override void Excecute()
        {
            using (var unitOfWork = new PetaPocoUnitOfWork())
            {
                var sql = @"
                    ALTER TABLE [G42Grease404Tracker]
                    ADD [domainPathId] [bigint] NOT NULL
                ";

                unitOfWork.Database.Execute(sql);

                sql = @"
                    ALTER TABLE [G42Grease404Tracker]
                    DROP COLUMN [domain]
                ";

                unitOfWork.Database.Execute(sql);

                sql = @"
                    ALTER TABLE [G42Grease404Tracker]
                    DROP COLUMN [path]
                ";

                unitOfWork.Database.Execute(sql);

                sql = @"
                    EXEC sp_RENAME 'G42Grease404Tracker.updatedOn', 'addedOn', 'COLUMN'
                ";

                unitOfWork.Database.Execute(sql);

                unitOfWork.Commit();
            }
        }
    }
}