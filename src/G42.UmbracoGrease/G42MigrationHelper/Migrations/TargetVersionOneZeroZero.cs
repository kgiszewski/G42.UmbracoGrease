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
                Console.WriteLine("Adding paths table...");
                unitOfWork.Database.Execute(@"
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

                unitOfWork.Database.Execute(@"
                    DROP TABLE [G42Grease404Tracker]
                ");

                Console.WriteLine("Adding tracker table...");

                unitOfWork.Database.Execute(@"
                    CREATE TABLE [dbo].[G42Grease404Tracker](
	                    [id] [bigint] IDENTITY(1,1) NOT NULL,
	                    [domainPathId] [bigint] NOT NULL,
	                    [referrer] [nvarchar](255) NOT NULL,
	                    [userAgent] [nvarchar](max) NULL,
	                    [addedOn] [datetime] NOT NULL,
	                    [ipAddress] [nvarchar](50) NULL,
                     CONSTRAINT [PK_G42Grease404Tracker] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
                ");

                unitOfWork.Commit();
            }
        }
    }
}