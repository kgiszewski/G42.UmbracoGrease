using System;
using System.Collections.Generic;
using System.Web;
using G42.UmbracoGrease.Core;
using G42.UmbracoGrease.G42RedirectHelper;
using G42.UmbracoGrease.G42SearchHelper.Models;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;

namespace G42.UmbracoGrease.G42SearchHelper.Repositories
{
    internal class G42SearchRepository
    {
        /// <summary>
        /// Adds they keyword to the DB for the given domain.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="domain">The domain.</param>
        /// <param name="keyword">The keyword.</param>
        internal static void AddKeywords(PetaPocoUnitOfWork unitOfWork, string domain, string keyword)
        {
            var currentKeyword =
                unitOfWork
                .Database
                .SingleOrDefault<G42GreaseSearchTrackerKeyword>(@"
                    SELECT * 
                    FROM G42GreaseSearchTrackerKeywords 
                    WHERE domain = @0 AND keyword = @1", 
                domain, 
                keyword);

            if (currentKeyword == null)
            {
                InsertKeywordsAndDomain(unitOfWork, domain, keyword);
            }
            else
            {
                currentKeyword.Count++;
                currentKeyword.LastUsedOn = DateTime.UtcNow;
                unitOfWork.Database.Save(currentKeyword);
            }
        }

        /// <summary>
        /// Gets keywords that have the minimum count specified.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="countFilter">The count filter.</param>
        /// <returns></returns>
        internal static IEnumerable<G42GreaseSearchTrackerKeyword> Get(PetaPocoUnitOfWork unitOfWork, int countFilter = 1)
        {
            return
                unitOfWork
                .Database
                .Fetch<G42GreaseSearchTrackerKeyword>(@"
                    SELECT * 
                    FROM G42GreaseSearchTrackerKeywords 
                    WHERE count >= @0", 
                countFilter);
        }

        /// <summary>
        /// Inserts the specified keyword and domain.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="domain">The domain.</param>
        /// <param name="keyword">The keyword.</param>
        internal static void InsertKeywordsAndDomain(PetaPocoUnitOfWork unitOfWork, string domain, string keyword)
        {
            unitOfWork
            .Database
            .Save(new G42GreaseSearchTrackerKeyword
            {
                Domain = domain,
                Keyword = keyword,
                Count = 1,
                LastUsedOn = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Adds the specified keywords to the DB.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="keywords">The keywords.</param>
        internal static void AddSearch(PetaPocoUnitOfWork unitOfWork, string keywords)
        {
            //TODO: factor this out
            var context = HttpContext.Current;

            var domain = context.Request.Url.Host;

            foreach (var keyword in keywords.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                AddKeywords(unitOfWork, domain, keyword);
            }

            unitOfWork.Database.Save(new G42GreaseSearchTrackerSearch()
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
        internal static void CreateSearchTrackerKeywordsTable(PetaPocoUnitOfWork unitOfWork)
        {
            if (!unitOfWork.Database.TableExist("G42GreaseSearchTrackerKeywords"))
            {
                LogHelper.Info<G42GreaseSearchTrackerKeyword>("Creating table.");

                unitOfWork.Database.Execute(@"
                    CREATE TABLE [dbo].[G42GreaseSearchTrackerKeywords](
	                    [id] [int] IDENTITY(1,1) NOT NULL,
	                    [domain] [nvarchar](50) NOT NULL,
	                    [keyword] [nvarchar](50) NOT NULL,
	                    [count] [int] NOT NULL,
	                    [lastUsedOn] [datetime] NOT NULL,
                     CONSTRAINT [PK_G42GreaseSearchTrackerKeywords] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
                    )
                ");
            }
            else
            {
                LogHelper.Info<G42GreaseSearchTrackerKeyword>("Table exists.");
            }
        }

        /// <summary>
        /// Creates the table in the DB.
        /// </summary>
        internal static void CreateSearchTrackerSearchesTable(PetaPocoUnitOfWork unitOfWork)
        {
            if (!unitOfWork.Database.TableExist("G42GreaseSearchTrackerSearches"))
            {
                LogHelper.Info<G42GreaseSearchTrackerSearch>("Creating table.");

                unitOfWork.Database.Execute(@"
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