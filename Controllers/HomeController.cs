using Microsoft.AspNetCore.Mvc;
using SomeStrangeDotNetProject.Models;
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
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            // Check if the file is null or empty
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file selected");
            }

            var fileExt = Path.GetExtension(file.FileName);

            // Validate the file extension
            if (fileExt != ".json")
            {
                return BadRequest("Invalid file type");
            }
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
            return Ok($"File uploaded successfully: {fileText}");
        }
        public async Task<string> ReadAllText(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    return await reader.ReadToEndAsync();
                }
            }
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
