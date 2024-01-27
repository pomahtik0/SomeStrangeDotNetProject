using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SomeStrangeDotNetProject.Models.JSON_translate_model;

namespace SomeStrangeDotNetProject.Controllers
{
    public class ShowTreesController : Controller
    {
        public IActionResult Index()
        {
            var list = TreeModel.GetAllDbTrees();

            var selectList = new SelectList(list, "Id", "Name");
            ViewBag.Items = selectList;
            return View("ShowTrees");
        }
    }
}
