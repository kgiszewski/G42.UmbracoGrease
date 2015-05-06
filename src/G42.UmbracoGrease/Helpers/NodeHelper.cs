using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Web;
using umbraco.cms.businesslogic.web;
using Umbraco.Core.Logging;
using G42.UmbracoGrease.Models;

namespace G42.UmbracoGrease.Helpers
{
    public sealed class NodeHelper
    {
        private static volatile NodeHelper _instance;
        private static object _padLock = new Object();
        private static Type _concreteType;

        private NodeHelper()
        {
            //find an implementation of SiteBase
            var type = typeof (Site);

            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => type.IsAssignableFrom(p));

            //if more than one, take extended one
            if (types.Count() == 1)
            {
                _concreteType = types.First();
            }
            else
            {
                _concreteType = types.FirstOrDefault(x => x.IsAssignableFrom(x) && x != type);
            }

            LogHelper.Info<Site>(_concreteType.AssemblyQualifiedName);
        }

        public List<Site> Sites = new List<Site>();

        public Site CurrentSite
        {
            get { return Sites.FirstOrDefault(x => x.Domain == HttpContext.Current.Request.Url.Host); }
        }

        public static NodeHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_padLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new NodeHelper();

                            var umbHelper = new UmbracoHelper(UmbracoContext.Current);

                            foreach (var domain in Domain.GetDomains())
                            {
                                var rootNode = umbHelper.TypedContent(domain.RootNodeId);
                                
                                if (rootNode != null)
                                {
                                    LogHelper.Info<NodeHelper>("Caching: " + domain.Name);
                                    var siteRootNode = umbHelper.TypedContent(domain.RootNodeId);

                                    if (siteRootNode != null)
                                    {
                                        var siteSettings = siteRootNode.Siblings().FirstOrDefault(x => x.DocumentTypeAlias.EndsWith("SiteSettings"));

                                        var site = ((Site)Activator.CreateInstance(_concreteType));

                                        site.Domain = domain.Name;
                                        site.RootId = rootNode.Id;
                                        site.HomeId = siteRootNode.Id;
                                        site.SiteSettingsId = (siteSettings != null) ? siteSettings.Id : siteRootNode.Id;
                                        
                                        site.MapSite(site, domain, rootNode, siteRootNode, siteSettings);

                                        _instance.Sites.Add(site);
                                    }
                                }
                            }
                        }
                    }
                }
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
    }
}