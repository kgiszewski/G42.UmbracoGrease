using System;
using G42.UmbracoGrease.Core;
using G42.UmbracoGrease.Extensions;
using G42.UmbracoGrease.G42MigrationHelper.Migrations;
using NUnit.Framework;

namespace G42.UmbracoGrease.Tests.MigrationTests
{
    [Category("Migrations")]
    [TestFixture]
    public class OneZeroZero
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            PetaPocoUnitOfWork.ConnectionString = "testDb";

            using (var uow = new PetaPocoUnitOfWork())
            {
                //remove current tables
                if (uow.Database.DoesTableExist("G42Grease404Tracker"))
                {
                    Console.WriteLine("Removing Tracker...");
                    uow.Database.Execute(@"DROP TABLE [G42Grease404Tracker]");
                }

                if (uow.Database.DoesTableExist("G42Grease404TrackerDomainPaths"))
                {
                    Console.WriteLine("Removing Domain\\Paths...");
                    uow.Database.Execute(@"DROP TABLE [G42Grease404TrackerDomainPaths]");
                }

                //add v0.6.5 table
                uow.Database.Execute(@"
                    CREATE TABLE [dbo].[G42Grease404Tracker](
	                    [id] [bigint] IDENTITY(1,1) NOT NULL,
	                    [domain] [nvarchar](50) NOT NULL,
	                    [path] [nvarchar](255) NOT NULL,
	                    [referrer] [nvarchar](255) NOT NULL,
	                    [userAgent] [nvarchar](max) NULL,
                        [ipAddress] [nvarchar](50) DEFAULT NULL,
	                    [updatedOn] [datetime] NOT NULL,
                     CONSTRAINT [PK_G42Grease404Tracker] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
                    )
                ");

                uow.Commit();
            }
        }

        [Test]
        public void Can_Migrate_To_One_Zero_Zero()
        {
            //assert domain/path table does not exists
            using (var uow = new PetaPocoUnitOfWork())
            {
                Assert.That(uow.Database.DoesTableExist("G42Grease404Tracker"));
                Assert.That(!uow.Database.DoesTableExist("G42Grease404TrackerDomainPaths"));
            }

            var migration = new TargetVersionOneZeroZero();

            migration.Excecute();

            using (var uow = new PetaPocoUnitOfWork())
            {
                Assert.That(uow.Database.DoesTableExist("G42Grease404Tracker"));
                Assert.That(uow.Database.DoesTableExist("G42Grease404TrackerDomainPaths"));
            }
        }
    }
}
