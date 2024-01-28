using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SomeStrangeDotNetProject.Models.JSON_translate_model;
using SomeStrangeDotNetProject.Models.JSON_translate_model.CollectionDataTypes;
using System.Data.SqlClient;

namespace SomeStrangeDotNetProject.Controllers
{
    public class ShowTreesController : Controller
    {
        public IActionResult GetTree(TreeModel treeModel)
        {

        }

        [HttpGet]
        public IActionResult GetTree(string? value)
        {
            using SqlConnection conn = new SqlConnection(HttpContext.Session.GetString("connection_string"));
            var list = TreeModel.GetAllDbTrees(conn);
            ViewBag.Trees = new SelectList(list, "Id", "Name");
            TreeModel? tree = null;

            if (!string.IsNullOrEmpty(value))
            {
                Queue<string> request = new Queue<string>(value.Split('/'));
                string treeName = request.Dequeue();
                tree = list.Where(x => x.Name == treeName).FirstOrDefault();
                if (tree != null)
                {
                    tree.TreeRoot = new TreeObject(conn, tree.Id);
                    if(!tree.FindAndReRoot(request))
                    {
                        return BadRequest("Root path is wrong, try to check it");
                    }
                }
                else
                {
                    return BadRequest("No tree named like that");
                }
            }

            return View("ShowTrees", tree);
        }
    }
}
