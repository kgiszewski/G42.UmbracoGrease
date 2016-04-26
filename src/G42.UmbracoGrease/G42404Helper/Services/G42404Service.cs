using System.Collections.Generic;
using System.Web;
using G42.UmbracoGrease.Core;
using G42.UmbracoGrease.G42404Helper.Models;
using G42.UmbracoGrease.G42404Helper.Repositories;
using G42.UmbracoGrease.G42AppSettings.Repositories;
using G42.UmbracoGrease.G42AppSettings.Services;

namespace G42.UmbracoGrease.G42404Helper.Services
{
    public class G42404Service
    {
        public void Add()
        {
            var request = HttpContext.Current.Request;

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

        public void Create404TrackerTable()
        {
            using (var uow = new PetaPocoUnitOfWork())
            {
                G42404Repository.Create404TrackerTable(uow);

                uow.Commit();
            }
        }

        public void Create404DomainPathsTable()
        {
            using (var uow = new PetaPocoUnitOfWork())
            {
                G42404Repository.Create404DomainPathsTable(uow);

                uow.Commit();
            }
        }

        public G42Grease404ConfigModel Get404TrackerConfig()
        {
            using (var uow = new PetaPocoUnitOfWork())
            {
                var rawSettings = G42AppSettingRepository.Get404TrackerConfigs(uow);

                return new G42Grease404ConfigModel(rawSettings);
            }
        }

        public bool Save404TrackerConfig(G42Grease404ConfigModel model)
        {
            using (var uow = new PetaPocoUnitOfWork())
            {
                G42AppSettingsService.SaveSetting(uow, Constants._404_TRACKER_DEFAULT_DAYS_TO_RETAIN_KEY, model.DaysToRetain.ToString(), "90");

                uow.Commit();
            }

            return true;
        }
    }
}