using System;
using G42.UmbracoGrease.Core;
using G42.UmbracoGrease.Extensions;
using G42.UmbracoGrease.G42MigrationHelper.Migrations;
using NUnit.Framework;

namespace G42.UmbracoGrease.Tests.MigrationTests
{
    [Category("Migrations")]
    [TestFixture]
    public class ZeroSixTwo
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

                //add v0.6.5 table
                uow.Database.Execute(@"
                    CREATE TABLE [dbo].[G42Grease404Tracker](
	                    [id] [bigint] IDENTITY(1,1) NOT NULL,
	                    [domain] [nvarchar](50) NOT NULL,
	                    [path] [nvarchar](255) NOT NULL,
	                    [referrer] [nvarchar](255) NOT NULL,
	                    [userAgent] [nvarchar](max) NULL,
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
        public void Can_Migrate_To_Zero_Six_Two()
        {
            //assert domain/path table does not exists
            using (var uow = new PetaPocoUnitOfWork())
            {
                Assert.That(uow.Database.DoesTableExist("G42Grease404Tracker"));
            }

            var migration = new TargetVersionZeroSixTwo();

            migration.Excecute();

            using (var uow = new PetaPocoUnitOfWork())
            {
                var sql = @"
                    SELECT object_id FROM sys.columns 
                    WHERE Name = N'ipAddress' AND Object_ID = Object_ID(N'G42Grease404Tracker')
                ";

                var result = uow.Database.ExecuteScalar<int>(sql);

                Assert.That(result != 0);
            }
        }
    }
}
