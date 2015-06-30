﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Web;
using umbraco.cms.businesslogic.web;
using Umbraco.Core.Logging;
using Umbraco.Core;
using G42.UmbracoGrease.G42NodeHelper.Models;

namespace G42.UmbracoGrease.G42NodeHelper
{
    public sealed class NodeHelper
    {
        private static volatile NodeHelper _instance;
        private static object _padLock = new Object();
        private static Type _siteType;

        private NodeHelper()
        {
            var baseType = typeof (Site);

            var types = PluginManager.Current.ResolveTypes<Site>().Where(x => x.IsSubclassOf(baseType));

            //if more than one, reject
            if (types.Count() > 1)
            {
                throw new Exception("You may only have one Site type.");
            }

            //find an implementation of Site
            _siteType = types.FirstOrDefault(x => x.IsSubclassOf(baseType));

            if (_siteType == null)
            {
                LogHelper.Info<NodeHelper>("Using default Site Type.");
                _siteType = baseType;
            }
            else
            {
                LogHelper.Info<NodeHelper>("Using " + _siteType.ToString());
            }
        }

        public List<Site> Sites = new List<Site>();

        public DateTime CreatedOn { get; private set; }

        public Site CurrentSite
        {
            get { return Sites.FirstOrDefault(x => x.Domain == HttpContext.Current.Request.Url.Host); }
        }

        public static bool IsInitialized()
        {
            return _instance != null;
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

                            _instance.CreatedOn = DateTime.UtcNow;

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

                                        var site = ((Site)Activator.CreateInstance(_siteType));

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
            private set
            {
                _instance = value;
            }
        }
        public static void Clear()
        {
            LogHelper.Info<NodeHelper>("Clearing...");
            _instance = null;
        }
    }
}