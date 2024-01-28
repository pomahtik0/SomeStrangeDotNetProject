using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SomeStrangeDotNetProject.Models.JSON_translate_model;
using SomeStrangeDotNetProject.Models.JSON_translate_model.CollectionDataTypes;
using System.Data.SqlClient;

namespace SomeStrangeDotNetProject.Controllers
{
    public class ShowTreesController : Controller
    {
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
                tree = list.Where(x => x.Name == request.Dequeue()).FirstOrDefault();
                if (tree != null)
                {
                    tree.TreeRoot = new TreeObject(conn, tree.Id);
                    if(!tree.FindAndReRoot(request))
                    {
                       // помилка при пошуку гілки
                    }
                }
                else
                {
                    // дерева з такою назвою не існує
                }
            }
        }
    }
}
