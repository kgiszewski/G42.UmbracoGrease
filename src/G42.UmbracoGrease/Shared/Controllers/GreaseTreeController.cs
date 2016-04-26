using System.Net.Http.Formatting;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;

namespace G42.UmbracoGrease.Shared.Controllers
{
    /// <summary>
    /// Tree controller that represents the tree in the custom Umbraco section.
    /// </summary>
    [PluginController("G42UmbracoGrease")]
    [Umbraco.Web.Trees.Tree("G42UmbracoGrease", "G42UmbracoGreaseTree", "G42 Grease", iconClosed: "icon-folder")]
    public class GreaseTreeController : TreeController
    {
        /// <summary>
        /// The method called to render the contents of the tree structure
        /// </summary>
        /// <param name="id"></param>
        /// <param name="queryStrings">All of the query string parameters passed from jsTree</param>
        /// <returns></returns>
        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            var nodes = new TreeNodeCollection();

            if (id == "-1")
            {
                //general
                _addNode(nodes, queryStrings, "General", "", "General", "icon-settings", "/G42UmbracoGrease/G42UmbracoGreaseTree/general-dashboard/General");

                //404
                _addNode(nodes, queryStrings, "404Tracker", "", "404 Tracker", "icon-block", "/G42UmbracoGrease/G42UmbracoGreaseTree/_404-tracker-dashboard/404%20Tracker");

                //error reporting
                _addNode(nodes, queryStrings, "ErrorReporing", "", "Error Reporting", "icon-application-error", "/G42UmbracoGrease/G42UmbracoGreaseTree/error-reporting-dashboard/Error%20Reporting");

                //nodeHelper
                _addNode(nodes, queryStrings, "NodeHelper", "", "Node Helper", "icon-globe", "/G42UmbracoGrease/G42UmbracoGreaseTree/node-helper-dashboard/Node%20Helper");

                //search
                _addNode(nodes, queryStrings, "SearchTracker", "", "Search Tracker", "icon-search", "/G42UmbracoGrease/G42UmbracoGreaseTree/search-tracker-dashboard/Search%20Tracker");
            }

            return nodes;
        }

        /// <summary>
        /// Returns the menu structure for the node given.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="queryStrings"></param>
        /// <returns></returns>
        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            var menu = new MenuItemCollection();

            return menu;
        }

        /// <summary>
        /// Helper that adds the node to the tree.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <param name="queryStrings">The query strings.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <param name="title">The title.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="routePath">The route path.</param>
        /// <param name="hasChildren">if set to <c>true</c> [has children].</param>
        /// <returns></returns>
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