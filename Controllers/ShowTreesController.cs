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
        }
    }
}
