using Microsoft.AspNetCore.Mvc;
using SomeStrangeDotNetProject.Models;
using SomeStrangeDotNetProject.Models.JSON_translate_model.CollectionDataTypes;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace SomeStrangeDotNetProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file, string text)
        {
            // Check if the file is null or empty
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file selected");
            }

            var fileExt = Path.GetExtension(file.FileName);

            // Validate the file extension
            if (fileExt != ".json" || fileExt != ".txt")
            {
                return BadRequest("Invalid file type");
            }

            if (fileExt == ".json")
            {
                JsonDocument doc;
                try
                {
                    doc = await JsonDocument.ParseAsync(file.OpenReadStream());
                }
                catch (JsonException)
                {
                    return BadRequest("Not a json content in file");
                }
                TreeObject root = new TreeObject(doc);
                // Return a success message
                throw new NotImplementedException();
            }

            else throw new NotImplementedException();
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
