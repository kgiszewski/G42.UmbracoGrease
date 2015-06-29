using System;
using System.Collections.Generic;
using System.Linq;
using G42.UmbracoGrease.Reports.PetaPocoModels;

namespace G42.UmbracoGrease.Reports.Models
{
    public class G42GreaseSearchTableModel
    {
        public IEnumerable<DomainSearch> Data { get; set; }

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
        public class DomainSearch
        {
            public string Domain { get; set; }
            public IEnumerable<DomainKeyword> Keywords { get; set; }
        }

        public class DomainKeyword
        {
            public string Keyword { get; set; }
            public int Count { get; set; }
            public DateTime LastUsedOn { get; set; }
        }
    }
}