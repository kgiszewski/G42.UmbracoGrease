using System;
using System.Web;
using G42.UmbracoGrease.G42RedirectHelper;
using G42.UmbracoGrease.Helpers;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;

namespace G42.UmbracoGrease.Reports.Models
{
    /// <summary>
    /// Model that represents a single search in the DB.
    /// </summary>
    [TableName("G42GreaseSearchTrackerSearches")]
    [PrimaryKey("id")]
    public class G42GreaseSearchTrackerSearch
    {
        public int Id { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public string Keywords { get; set; }
        public DateTime SearchedOn { get; set; }

        /// <summary>
        /// Adds the specified keywords to the DB.
        /// </summary>
        /// <param name="keywords">The keywords.</param>
        public static void Add(string keywords)
        {
            var context = HttpContext.Current;

            var domain = context.Request.Url.Host;

            foreach (var keyword in keywords.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries))
            {
                G42GreaseSearchTrackerKeyword.Add(domain, keyword);
            }

            DbHelper.DbContext.Database.Save(new G42GreaseSearchTrackerSearch()
            {
                Domain = domain,
                Path = RedirectHelper.GetCurrentPath(),
                Keywords = keywords,
                SearchedOn = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Creates the table in the DB.
        /// </summary>
        internal static void CreateTable()
        {
            if (!DbHelper.DbContext.Database.TableExist("G42GreaseSearchTrackerSearches"))
            {
                LogHelper.Info<G42GreaseSearchTrackerSearch>("Creating table.");

                DbHelper.DbContext.Database.Execute(@"
                    CREATE TABLE [dbo].[G42GreaseSearchTrackerSearches](
	                    [id] [int] IDENTITY(1,1) NOT NULL,
	                    [domain] [nvarchar](50) NOT NULL,
	                    [path] [nvarchar](255) NOT NULL,
	                    [keywords] [nvarchar](255) NOT NULL,
	                    [searchedOn] [datetime] NOT NULL,
                     CONSTRAINT [PK_G42GreaseSearchTrackerSearches] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) 
                    )
                ");
            }
            else
            {
                LogHelper.Info<G42GreaseSearchTrackerSearch>("Table exists.");
            }
        }
    }
}