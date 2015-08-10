using System;
using System.Collections.Generic;
using System.Web;
using G42.UmbracoGrease.G42RedirectHelper;
using G42.UmbracoGrease.Helpers;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;

namespace G42.UmbracoGrease.Reports.PetaPocoModels
{
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

        [ResultColumn]
        public int Count { get; set; }

        [ResultColumn]
        public DateTime LastTried { get; set; }

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
                    UpdatedOn = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error<Exception>(ex.Message, ex);
            }
        }

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