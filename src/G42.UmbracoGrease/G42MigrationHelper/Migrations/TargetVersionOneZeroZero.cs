using System;
using G42.UmbracoGrease.Core;
using G42.UmbracoGrease.G42404Helper.Repositories;

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
                G42404Repository.Create404DomainPathsTable(unitOfWork);

                var sql = @"
                    DROP TABLE [G42Grease404Tracker]
                ";

                unitOfWork.Database.Execute(sql);

                G42404Repository.Create404TrackerTable(unitOfWork);

                unitOfWork.Commit();
            }
        }
    }
}