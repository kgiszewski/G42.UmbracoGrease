using System;
using G42.UmbracoGrease.Core;

namespace G42.UmbracoGrease.G42MigrationHelper.Migrations
{
    public class TargetVersionOneZeroThree : MigrationBase
    {
        public override Version TargetVersion
        {
            get { return new Version("1.0.3"); }
        }

        public override void Excecute()
        {
            using (var unitOfWork = new PetaPocoUnitOfWork())
            {
                var sql = @"
                    ALTER TABLE [G42Grease404TrackerDomainPaths]
                    ALTER COLUMN domain NVARCHAR(75)
                ";

                unitOfWork.Database.Execute(sql);

                sql = @"
                    ALTER TABLE [G42Grease404TrackerDomainPaths]
                    ALTER COLUMN path NVARCHAR(255)
                ";

                unitOfWork.Database.Execute(sql);

                sql = @"
                    ALTER TABLE [G42Grease404TrackerDomainPaths]
                    ADD CONSTRAINT [IX_G42Grease404TrackerDomainPaths] UNIQUE NONCLUSTERED 
                    (
	                    [domain] ASC,
	                    [path] ASC
                    )
                ";

                unitOfWork.Database.Execute(sql);

                unitOfWork.Commit();
            }
        }
    }
}