using System;
using G42.UmbracoGrease.G42404Helper.Services;

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
                            _services = new Services(new G42404Service());
                        
                            return _services;
                        }

                        return _services;
                    }
                }

                return _services;
            }
        }
    }
}