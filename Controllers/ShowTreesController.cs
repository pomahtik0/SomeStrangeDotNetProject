using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SomeStrangeDotNetProject.Models.JSON_translate_model;
using SomeStrangeDotNetProject.Models.JSON_translate_model.CollectionDataTypes;
using System.Data.SqlClient;

namespace SomeStrangeDotNetProject.Controllers
{
    public class ShowTreesController : Controller
    {
        public IActionResult GetTree(TreeModel tree)
        {
            using SqlConnection conn = new SqlConnection(HttpContext.Session.GetString("connection_string"));
            var list = TreeModel.GetAllDbTrees(conn);
            var currentTree = list.Where(x => x.Id == tree.Id).First();
            var selectList = new SelectList(list, "Id", "Name");
            currentTree.TreeRoot = new TreeObject(conn, tree.Id);
            ViewBag.Trees = selectList;
            return View("ShowTrees", currentTree);
        }
    }
}
