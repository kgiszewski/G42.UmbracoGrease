using System;
using System.Collections.Generic;
using System.Linq;
using G42.UmbracoGrease.Reports.PetaPocoModels;

namespace G42.UmbracoGrease.Reports.Models
{
    public class _404TableModel
    {
        public IEnumerable<DomainPaths> Data { get; set; } 

        public _404TableModel(IEnumerable<_404Tracker> data)
        {
            var domains = new List<DomainPaths>();

            foreach (var domain in data.GroupBy(x => x.Domain))
            {
                var urls = new List<PathLastTried>();

                foreach (var url in domain)
                {
                    urls.Add(new PathLastTried()
                    {
                        Path = url.Path,
                        LastTried = url.LastTried,
                        Count = url.Count
                    });
                }

                domains.Add(new DomainPaths()
                {
                    Domain = domain.Key,
                    Urls = urls.OrderByDescending(x => x.Count)
                });
            }

            Data = domains.OrderBy(x => x.Domain);
        }

        public class DomainPaths
        {
            public string Domain { get; set; }
            public IEnumerable<PathLastTried> Urls { get; set; } 
        }

        public class PathLastTried
        {
            public string Path { get; set; }
            public DateTime LastTried { get; set; }
            public int Count { get; set; }
        }
    }
}