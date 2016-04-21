using System.Collections.Generic;
using System.Web;
using G42.UmbracoGrease.Core;
using G42.UmbracoGrease.G42404Helper.Models;
using G42.UmbracoGrease.G42404Helper.Repositories;

namespace G42.UmbracoGrease.G42404Helper.Services
{
    public class G42404Service
    {
        public void Add(HttpRequest request)
        {
            using (var uow = new PetaPocoUnitOfWork())
            {
                var domainPath = G42404Repository.GetDomainPath(uow, request);

                if (domainPath == null)
                {
                    domainPath = G42404Repository.AddDomainPath(uow, request);
                }
                else
                {
                    G42404Repository.TouchDomainPath(uow, domainPath);    
                }

                G42404Repository.AddTracker(uow, request, domainPath);
                
                uow.Commit();
            }
        }

        public IEnumerable<G42Grease404ResultsModel> GetResults(int countFilter)
        {
            using (var uow = new PetaPocoUnitOfWork())
            {
                return G42404Repository.GetResults(uow, countFilter);
            }
        }

        public void CreateTable()
        {
            using (var uow = new PetaPocoUnitOfWork())
            {
                G42404Repository.CreateTable(uow);
            }
        }
    }
}