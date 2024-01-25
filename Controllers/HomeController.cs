using Microsoft.AspNetCore.Mvc;
using SomeStrangeDotNetProject.Models;
using System.Diagnostics;

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

            // Get the file name and extension
            var fileName = Path.GetFileName(file.FileName);
            var fileExt = Path.GetExtension(file.FileName);

            // Validate the file extension
            if (fileExt != ".json")
            {
                return BadRequest("Invalid file type");
            }

            //// Generate a unique file name to avoid name conflicts
            //var uniqueFileName = $"{Guid.NewGuid()}{fileExt}";

            //// Specify the file upload path
            //var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            //// Create the directory if it doesn't exist
            //if (!Directory.Exists(uploadPath))
            //{
            //    Directory.CreateDirectory(uploadPath);
            //}

            //// Combine the file name and the upload path
            //var filePath = Path.Combine(uploadPath, uniqueFileName);

            //// Copy the file to the server
            //using (var fileStream = new FileStream(filePath, FileMode.Create))
            //{
            //    await file.CopyToAsync(fileStream);
            //}

            // Return a success message
            return Ok($"File uploaded successfully: {fileName}");
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
