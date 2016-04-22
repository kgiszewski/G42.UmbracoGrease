using System;
using System.Collections.Generic;
using System.Linq;

namespace G42.UmbracoGrease.G42SearchHelper.Models
{
    /// <summary>
    /// Model that represents search terms used by domain in a report form.
    /// </summary>
    public class G42GreaseSearchTableModel
    {
        public IEnumerable<DomainSearch> Data { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="G42GreaseSearchTableModel"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public G42GreaseSearchTableModel(IEnumerable<G42GreaseSearchTrackerKeyword> data)
        {
            var domains = new List<DomainSearch>();

            foreach (var domain in data.GroupBy(x => x.Domain))
            {
                var keywords = new List<DomainKeyword>();

                foreach (var keyword in domain)
                {
                    keywords.Add(new DomainKeyword()
                    {
                       Keyword = keyword.Keyword,
                       Count = keyword.Count,
                       LastUsedOn = keyword.LastUsedOn
                    });
                }

                domains.Add(new DomainSearch()
                {
                    Domain = domain.Key,
                    Keywords = keywords.OrderByDescending(x => x.Count)
                });
            }

            Data = domains.OrderBy(x => x.Domain);
        }

        /// <summary>
        /// Model that represents keywords by domain.
        /// </summary>
        public class DomainSearch
        {
            public string Domain { get; set; }
            public IEnumerable<DomainKeyword> Keywords { get; set; }
        }

        /// <summary>
        /// Model that represents the count and last time a keyword was used.
        /// </summary>
        public class DomainKeyword
        {
            public string Keyword { get; set; }
            public int Count { get; set; }
            public DateTime LastUsedOn { get; set; }
        }
    }
}