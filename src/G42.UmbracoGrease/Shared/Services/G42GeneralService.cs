using G42.UmbracoGrease.Core;
using G42.UmbracoGrease.G42AppSettings.Repositories;
using G42.UmbracoGrease.G42AppSettings.Services;
using G42.UmbracoGrease.G42ErrorReporting.Models;
using G42.UmbracoGrease.Shared.Models;

namespace G42.UmbracoGrease.Shared.Services
{
    public class G42GeneralService
    {
        public G42GeneralConfigModel GetGeneralConfig()
        {
            using (var uow = new PetaPocoUnitOfWork())
            {
                var rawSettings = G42AppSettingRepository.GetGeneralConfigs(uow);

                return new G42GeneralConfigModel(rawSettings);
            }
        }

        public bool SaveGeneralConfig(G42GeneralConfigModel model)
        {
            using (var uow = new PetaPocoUnitOfWork())
            {
                G42AppSettingsService.SaveSetting(uow, Constants.VIEW_ENGINE_ENABLE_KEY, (model.ViewEngineEnable) ? "1" : "0");
            
                uow.Commit();
            }

            return true;
        }
    }
}