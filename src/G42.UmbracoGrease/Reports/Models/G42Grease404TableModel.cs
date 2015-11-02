using System;
using System.Collections.Generic;
using System.Linq;
using G42.UmbracoGrease.Reports.PetaPocoModels;

namespace G42.UmbracoGrease.Reports.Models
{
    /// <summary>
    /// Model that represents the 404 data in a report format.
    /// </summary>
    public class G42Grease404TableModel
    {
        public IEnumerable<DomainPaths> Data { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="G42Grease404TableModel"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public G42Grease404TableModel(IEnumerable<G42Grease404Tracker> data)
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

        /// <summary>
        /// Model that represents domains and their paths.
        /// </summary>
        public class DomainPaths
        {
            public string Domain { get; set; }
            public IEnumerable<PathLastTried> Urls { get; set; } 
        }

        /// <summary>
        /// Model that represents the count and last time the path was tried.
        /// </summary>
        public class PathLastTried
        {
            public string Path { get; set; }
            public DateTime LastTried { get; set; }
            public int Count { get; set; }
        }
    }
}