using System.Net.Http.Formatting;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;

namespace G42.UmbracoGrease.Shared.Controllers
{
    [PluginController("G42UmbracoGrease")]
    [Umbraco.Web.Trees.Tree("G42UmbracoGrease", "G42UmbracoGreaseTree", "G42 Grease", iconClosed: "icon-folder")]
    public class GreaseTreeController : TreeController
    {
        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            var nodes = new TreeNodeCollection();

            if (id == "-1")
            {
                //404
                _addNode(nodes, queryStrings, "404Tracker", "", "404 Tracker", "icon-block", "/G42UmbracoGrease/G42UmbracoGreaseTree/_404-tracker-dashboard/404%20Tracker");

                //app_settings
                _addNode(nodes, queryStrings, "AppSettings", "", "App Settings", "icon-settings", "/G42UmbracoGrease/G42UmbracoGreaseTree/app-settings-dashboard/App%20Settings");

                //nodeHelper
                _addNode(nodes, queryStrings, "NodeHelper", "", "Node Helper", "icon-globe", "/G42UmbracoGrease/G42UmbracoGreaseTree/node-helper-dashboard/Node%20Helper");

                //search
                _addNode(nodes, queryStrings, "SearchTracker", "", "Search Tracker", "icon-search", "/G42UmbracoGrease/G42UmbracoGreaseTree/search-tracker-dashboard/Search%20Tracker");
            }

            return nodes;
        }

        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            var menu = new MenuItemCollection();

            return menu;
        }

        private TreeNode _addNode(TreeNodeCollection nodes, FormDataCollection queryStrings, string id, string parentId, string title, string icon, string routePath, bool hasChildren = false)
        {
            var node = CreateTreeNode(id, parentId, queryStrings, title, icon);
            node.RoutePath = routePath;
            node.HasChildren = hasChildren;

            nodes.Add(node);

            return node;
        }
    }
}