using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SomeStrangeDotNetProject.Models.JSON_translate_model;
using System.Data.SqlClient;

namespace SomeStrangeDotNetProject.Controllers
{
    public class ShowTreesController : Controller
    {
        public IActionResult Index()
        {
            using SqlConnection conn = new SqlConnection(HttpContext.Session.GetString("connection_string"));
            var list = TreeModel.GetAllDbTrees(conn);
            var selectList = new SelectList(list, "Id", "Name");
            ViewBag.Trees = selectList;
            return View("ShowTrees");
        }
    }
}
