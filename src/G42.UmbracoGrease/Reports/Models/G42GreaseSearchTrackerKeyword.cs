﻿using System;
using System.Collections.Generic;
using G42.UmbracoGrease.Helpers;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;

namespace G42.UmbracoGrease.Reports.Models
{
    /// <summary>
    /// Model that represents a search keyword in the DB.
    /// </summary>
    [TableName("G42GreaseSearchTrackerKeywords")]
    [PrimaryKey("id")]
    public class G42GreaseSearchTrackerKeyword
    {
        public int Id { get; set; }
        public string Domain { get; set; }
        public string Keyword { get; set; }
        public int Count { get; set; }
        public DateTime LastUsedOn { get; set; }

        /// <summary>
        /// Adds they keyword to the DB for the given domain.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="keyword">The keyword.</param>
        internal static void Add(string domain, string keyword)
        {
            var currentKeyword =
                DbHelper.DbContext.Database.SingleOrDefault<G42GreaseSearchTrackerKeyword>("SELECT * FROM G42GreaseSearchTrackerKeywords WHERE domain = @0 AND keyword = @1", domain, keyword);

            if (currentKeyword == null)
            {
                _insert(domain, keyword);
            }
            else
            {
                currentKeyword.Count++;
                currentKeyword.LastUsedOn = DateTime.UtcNow;
                DbHelper.DbContext.Database.Save(currentKeyword);
            }
        }

        /// <summary>
        /// Gets keywords that have the minimum count specified.
        /// </summary>
        /// <param name="countFilter">The count filter.</param>
        /// <returns></returns>
        public static IEnumerable<G42GreaseSearchTrackerKeyword> Get(int countFilter = 1)
        {
            return
                DbHelper.DbContext.Database.Fetch<G42GreaseSearchTrackerKeyword>(
                    "SELECT * FROM G42GreaseSearchTrackerKeywords WHERE count >= @0", countFilter);
        }

        /// <summary>
        /// Inserts the specified keyword and domain.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="keyword">The keyword.</param>
        private static void _insert(string domain, string keyword)
        {
            DbHelper.DbContext.Database.Save(new G42GreaseSearchTrackerKeyword()
            {
                Domain = domain,
                Keyword = keyword,
                Count = 1,
                LastUsedOn = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Creates the table in the DB.
        /// </summary>
        internal static void CreateTable()
        {
            if (!DbHelper.DbContext.Database.TableExist("G42GreaseSearchTrackerKeywords"))
            {
                LogHelper.Info<G42GreaseSearchTrackerKeyword>("Creating table.");

                DbHelper.DbContext.Database.Execute(@"
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
    }
}