using System;
using System.Collections.Generic;
using System.Web;
using G42.UmbracoGrease.Core;
using G42.UmbracoGrease.Extensions;
using G42.UmbracoGrease.G42404Helper.Models;
using G42.UmbracoGrease.G42RedirectHelper;
using G42.UmbracoGrease.Helpers;
using Umbraco.Core.Logging;

namespace G42.UmbracoGrease.G42404Helper.Repositories
{
    internal class G42404Repository
    {
        internal static DateTime LastPurged;

        /// <summary>
        /// Gets the 404s that have the minimum count specified.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="countFilter">The count filter.</param>
        /// <returns></returns>
        internal static IEnumerable<G42Grease404ResultsModel> GetResults(PetaPocoUnitOfWork unitOfWork, int countFilter = 1)
        {
            return unitOfWork.Database.Fetch<G42Grease404ResultsModel>(@"
                SELECT count, domain, path, lastVisited
                FROM (
	                SELECT COUNT(id) AS count, domainPathId
	                FROM G42Grease404Tracker
	                GROUP BY domainPathId
	                HAVING COUNT(id) >= @0
                ) AS t1
                INNER JOIN G42Grease404TrackerDomainPaths dp on dp.id = t1.domainPathId
            ", countFilter);
        }

        internal static G42Grease404DomainPath GetDomainPath(PetaPocoUnitOfWork unitOfWork, string domain, string path)
        {
            return unitOfWork.Database.SingleOrDefault<G42Grease404DomainPath>(@"
                WHERE domain = @0 AND path = @1
            ", domain, path);
        }

        internal static void TouchDomainPath(PetaPocoUnitOfWork unitOfWork, G42Grease404DomainPath domainPath)
        {
            domainPath.LastVisited = DateTime.UtcNow;

            unitOfWork.Database.Save(domainPath);
        }

        internal static G42Grease404DomainPath AddDomainPath(PetaPocoUnitOfWork unitOfWork, string domain, string path)
        {
            var domainPath = new G42Grease404DomainPath
            {
                Domain = domain,
                Path = path,
                AddedOn = DateTime.UtcNow,
                LastVisited = DateTime.UtcNow
            };

            unitOfWork.Database.Insert(domainPath);

            return domainPath;
        }

        /// <summary>
        /// Adds a 404 to the DB.
        /// </summary>
        internal static void AddTracker(PetaPocoUnitOfWork unitOfWork, string referrer, string userAgent, G42Grease404DomainPath domainPath)
        {
            try
            {
                unitOfWork.Database.Save(new G42Grease404Tracker()
                {
                    DomainPathId = domainPath.Id,
                    Referrer = referrer,
                    UserAgent = userAgent,
                    IpAddress = IpHelper.GetIpAddress(),
                    AddedOn = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error<Exception>(ex.Message, ex);
            }

            //would like this to not run for every 404, but for now it'll do
            PurgeTable(unitOfWork);
        }

        /// <summary>
        /// Purges the table of old items.
        /// </summary>
        internal static void PurgeTable(PetaPocoUnitOfWork unitOfWork)
        {
            if (LastPurged == DateTime.MinValue)
            {
                LastPurged = DateTime.UtcNow;
            }

            if (LastPurged > DateTime.UtcNow.AddDays(-1))
            {
                return;
            }

            var customDays = Grease.Services.G42AppSettingsService.GetValue<int>(Constants._404_TRACKER_DEFAULT_DAYS_TO_RETAIN_KEY);

            var date = DateTime.UtcNow.AddDays(customDays * -1);

            LogHelper.Info<G42Grease404Tracker>("Purging 404's " + customDays + " days prior beginning =>" + date.ToString("R"));

            unitOfWork.Database.Execute(@"
                DELETE
                FROM G42Grease404Tracker
                WHERE addedOn < @0
            ", date);

            LastPurged = DateTime.UtcNow;
        }

        /// <summary>
        /// Creates the 404 table.
        /// </summary>
        internal static void Create404TrackerTable(PetaPocoUnitOfWork unitOfWork)
        {
            if (!unitOfWork.Database.DoesTableExist("G42Grease404Tracker"))
            {
                LogHelper.Info<G42Grease404Tracker>("Creating table...");

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
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
                    )
                ");
            }
            else
            {
                LogHelper.Info<G42Grease404Tracker>("Table exists.");
            }
        }

        /// <summary>
        /// Creates the 404 table.
        /// </summary>
        internal static void Create404DomainPathsTable(PetaPocoUnitOfWork unitOfWork)
        {
            if (!unitOfWork.Database.DoesTableExist("G42Grease404TrackerDomainPaths"))
            {
                LogHelper.Info<G42Grease404Tracker>("Creating table...");

                unitOfWork.Database.Execute(@"
                    CREATE TABLE [dbo].[G42Grease404TrackerDomainPaths](
	                    [id] [bigint] IDENTITY(1,1) NOT NULL,
	                    [domain] [nvarchar](75) NOT NULL,
	                    [path] [nvarchar](255) NOT NULL,
	                    [addedOn] [datetime] NOT NULL,
	                    [lastVisited] [datetime] NULL,
                     CONSTRAINT [PK_G42Grease404TrackerDomainPaths] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON),
                     CONSTRAINT [IX_G42Grease404TrackerDomainPaths] UNIQUE NONCLUSTERED 
                    (
	                    [domain] ASC,
	                    [path] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
                    )
                ");
            }
            else
            {
                LogHelper.Info<G42Grease404Tracker>("Table exists.");
            }
        }
    }
}