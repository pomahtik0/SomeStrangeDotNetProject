using Microsoft.AspNetCore.Mvc;

namespace SomeStrangeDotNetProject.Controllers
{
    public class ShowTreesController : Controller
    {
        public IActionResult Index()
        {
            return View("ShowTrees");
        }
    }
}
