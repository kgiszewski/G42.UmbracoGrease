using System;
using G42.UmbracoGrease.Core;
using G42.UmbracoGrease.Extensions;
using G42.UmbracoGrease.G42MigrationHelper.Migrations;
using NUnit.Framework;

namespace G42.UmbracoGrease.Tests.MigrationTests
{
    [TestFixture]
    [Category("Migrations")]
    public class OneZeroThree
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

                //add v1.0.0 tables
                uow.Database.Execute(@"
                    CREATE TABLE [dbo].[G42Grease404Tracker](
	                    [id] [bigint] IDENTITY(1,1) NOT NULL,
	                    [referrer] [nvarchar](max) NULL,
	                    [userAgent] [nvarchar](max) NULL,
	                    [addedOn] [datetime] NOT NULL,
	                    [ipAddress] [nvarchar](50) NULL,
	                    [domainPathId] [bigint] NOT NULL,
                     CONSTRAINT [PK_G42Grease404Tracker] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
                ");

                uow.Database.Execute(@"
                    CREATE TABLE [dbo].[G42Grease404TrackerDomainPaths](
	                    [id] [bigint] IDENTITY(1,1) NOT NULL,
	                    [domain] [nvarchar](max) NOT NULL,
	                    [path] [nvarchar](max) NOT NULL,
	                    [addedOn] [datetime] NOT NULL,
	                    [lastVisited] [datetime] NULL,
                     CONSTRAINT [PK_G42Grease404TrackerDomainPaths] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
                ");

                uow.Commit();
            }
        }

        [Test]
        public void Can_Migrate_To_One_Zero_Three()
        {
            //assert domain/path table does not exist
            using (var uow = new PetaPocoUnitOfWork())
            {
                Assert.That(uow.Database.DoesTableExist("G42Grease404Tracker"));
                Assert.That(uow.Database.DoesTableExist("G42Grease404TrackerDomainPaths"));
            }

            var migration = new TargetVersionOneZeroThree();

            migration.Excecute();

            //assert that the columns are altered

            using (var uow = new PetaPocoUnitOfWork())
            {
                var sql = @"
                    SELECT CHARACTER_MAXIMUM_LENGTH
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE 
                        TABLE_NAME = 'G42Grease404TrackerDomainPaths' AND 
                        COLUMN_NAME = 'domain'
                ";

                var result = uow.Database.ExecuteScalar<int>(sql);

                Assert.AreEqual(75, result);

                sql = @"
                    SELECT CHARACTER_MAXIMUM_LENGTH
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE 
                        TABLE_NAME = 'G42Grease404TrackerDomainPaths' AND 
                        COLUMN_NAME = 'path'
                ";

                result = uow.Database.ExecuteScalar<int>(sql);

                Assert.AreEqual(255, result);

                sql = @"
                    SELECT CONSTRAINT_NAME
                    FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
                    WHERE CONSTRAINT_NAME ='IX_G42Grease404TrackerDomainPaths'
                ";

                var constraintResult = uow.Database.ExecuteScalar<string>(sql);

                Assert.AreEqual("IX_G42Grease404TrackerDomainPaths", constraintResult);
            }

            //assert that the index is added
        }
    }
}
