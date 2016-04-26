using System.Collections.Generic;
using G42.UmbracoGrease.Core;
using G42.UmbracoGrease.G42SearchHelper.Models;
using G42.UmbracoGrease.G42SearchHelper.Repositories;

namespace G42.UmbracoGrease.G42SearchHelper.Services
{
    public class G42SearchService
    {

        public IEnumerable<G42GreaseSearchTrackerKeyword> Get(int countFilter)
        {
            using (var uow = new PetaPocoUnitOfWork())
            {
                return G42SearchRepository.Get(uow, countFilter);
            }
        }

        public void AddSearch(string keywords)
        {
            using (var uow = new PetaPocoUnitOfWork())
            {
                G42SearchRepository.AddSearch(uow, keywords);

                uow.Commit();
            }
        }

        public void CreateSearchTrackerKeywordsTable()
        {
            using (var uow = new PetaPocoUnitOfWork())
            {
                G42SearchRepository.CreateSearchTrackerKeywordsTable(uow);
                
                uow.Commit();
            }
        }

        public void CreateSearchTrackerSearchesTable()
        {
            using (var uow = new PetaPocoUnitOfWork())
            {
                G42SearchRepository.CreateSearchTrackerSearchesTable(uow);

                uow.Commit();
            }
        }
    }
}