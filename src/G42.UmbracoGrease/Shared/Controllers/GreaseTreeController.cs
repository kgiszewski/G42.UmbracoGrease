using System.Net.Http.Formatting;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;

namespace G42.UmbracoGrease.Shared.Controllers
{
    [PluginController("G42UmbracoGrease")]
    [Umbraco.Web.Trees.Tree("G42UmbracoGrease", "G42UmbracoGreaseTree", "Grease", iconClosed: "icon-folder")]
    public class GreaseTreeController : TreeController
    {
        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            var nodes = new TreeNodeCollection();

            if (id == "-1")
            {
                //404
                _addNode(nodes, queryStrings, "404Tracker", "", "404 Tracker", "icon-block", "/G42UmbracoGrease/G42UmbracoGreaseTree/_404-tracker-dashboard/404%20Tracker");

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