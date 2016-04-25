using G42.UmbracoGrease.G42404Helper.Services;
using G42.UmbracoGrease.G42AppSettings.Services;
using G42.UmbracoGrease.G42SearchHelper.Services;

namespace G42.UmbracoGrease.Core
{
    public class Grease
    {
        private static object _padlock = new object();

        private static Services _services;
        public static Services Services
        {
            get
            {
                if (_services == null)
                {
                    lock (_padlock)
                    {
                        if (_services == null)
                        {
                            _services = new Services(
                                new G42404Service(),
                                new G42SearchService(),
                                new G42AppSettingsService());
                        
                            return _services;
                        }

                        return _services;
                    }
                }

                return _services;
            }

            // the idea here is to allow the services to be swapped with a different set for testing
            set { _services = value; }
        }
    }
}