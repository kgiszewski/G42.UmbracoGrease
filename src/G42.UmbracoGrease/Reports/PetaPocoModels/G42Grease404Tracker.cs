using System;
using System.Collections.Generic;
using System.Web;
using G42.UmbracoGrease.G42AppSettings.PetaPocoModels;
using G42.UmbracoGrease.G42RedirectHelper;
using G42.UmbracoGrease.Helpers;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;

namespace G42.UmbracoGrease.Reports.PetaPocoModels
{
    /// <summary>
    /// Model that represents the 404 data stored in the DB.
    /// </summary>
    [PrimaryKey("id")]
    [TableName("G42Grease404Tracker")]
    public class G42Grease404Tracker
    {
        public long Id { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public string Referrer { get; set; }
        public string UserAgent { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string IpAddress { get; set; }

        [ResultColumn]
        public int Count { get; set; }

        [ResultColumn]
        public DateTime LastTried { get; set; }

        [Ignore]
        private static DateTime LastPurged { get; set; }

        /// <summary>
        /// Gets the 404s that have the minimum count specified.
        /// </summary>
        /// <param name="countFilter">The count filter.</param>
        /// <returns></returns>
        public static IEnumerable<G42Grease404Tracker> Get(int countFilter = 1)
        {
            return DbHelper.DbContext.Database.Fetch<G42Grease404Tracker>(@"
                SELECT *, (SELECT MAX(updatedOn) FROM G42Grease404Tracker WHERE domain = t1.domain AND path = t1.path) AS lastTried
                FROM (
	                SELECT domain, path, COUNT(id) AS count
	                FROM G42Grease404Tracker
	                GROUP BY domain, path
                ) AS t1
                WHERE t1.count >= @0
            ", countFilter);
        }

        /// <summary>
        /// Adds a 404 to the DB.
        /// </summary>
        public static void Add()
        {
            var context = HttpContext.Current;
            var referrer = "";

            if (context.Request.UrlReferrer != null)
            {
                referrer = context.Request.UrlReferrer.AbsoluteUri;
            }

            try
            {
                DbHelper.DbContext.Database.Save(new G42Grease404Tracker()
                {
                    Domain = context.Request.Url.Host,
                    Path = RedirectHelper.GetCurrentPath(),
                    Referrer = referrer,
                    UserAgent = context.Request.UserAgent,
                    IpAddress = IpHelper.GetIpAddress(),
                    UpdatedOn = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error<Exception>(ex.Message, ex);
            }

            //would like this to not run for every 404, but for now it'll do
            PurgeTable();
        }

        /// <summary>
        /// Purges the table of old items.
        /// </summary>
        internal static void PurgeTable()
        {
            if (LastPurged == DateTime.MinValue)
            {
                LastPurged = DateTime.UtcNow;
            }

            if (LastPurged > DateTime.UtcNow.AddDays(-1))
            {
                return;
            }

            var customDaysSetting = G42GreaseAppSetting.Get("G42.UmbracoGrease:404retainLogDays");
            var customDays = 90;
            var tempCustomDays = 0;

            if (customDaysSetting != null)
            {
                if (Int32.TryParse(customDaysSetting.Value, out tempCustomDays))
                {
                    customDays = tempCustomDays;
                }
            }

            var date = DateTime.UtcNow.AddDays(customDays * -1);

            LogHelper.Info<G42Grease404Tracker>("Purging 404's " + customDays + " days prior beginning =>" + date.ToString("R"));

            DbHelper.DbContext.Database.Execute(@"
                DELETE
                FROM G42Grease404Tracker
                WHERE updatedOn < @0
            ", date);

            LastPurged = DateTime.UtcNow;
        }

        /// <summary>
        /// Creates the 404 table.
        /// </summary>
        internal static void CreateTable()
        {
            if (!DbHelper.DbContext.Database.TableExist("G42Grease404Tracker"))
            {
                LogHelper.Info<G42Grease404Tracker>("Creating table.");

                DbHelper.DbContext.Database.Execute(@"
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
            }
            else
            {
                LogHelper.Info<G42Grease404Tracker>("Table exists.");
            }
        }
    }
}