using Microsoft.AspNetCore.Mvc;
using SomeStrangeDotNetProject.Models;
using SomeStrangeDotNetProject.Models.JSON_translate_model.CollectionDataTypes;
using System.Data.SqlClient;
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
        public async Task<IActionResult> UploadNewTree(IFormFile file)
        {
            if(!HttpContext.Session.Keys.Any(str => str == "connection_string"))
            {
                return BadRequest("Connect database first");
            }
            // Check if the file is null or empty
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file selected");
            }

            var fileName = Path.GetFileName(file.FileName);
            var fileExt = Path.GetExtension(file.FileName);

            // Validate the file extension
            if (fileExt != ".json" && fileExt != ".txt")
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
                using (SqlConnection conn = new SqlConnection(HttpContext.Session.GetString("connection_string")))
                {
                    root.DbSaveRoot(conn, fileName);
                };
                // Return a success message
                return View("Index");
            }

            else throw new NotImplementedException();
        }

        public IActionResult Index()
        {
            return View("ConnectDb");
        }

        public IActionResult UploadTree()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveConnectionString(string text)
        {
            SqlConnectionStringBuilder connectionString = new SqlConnectionStringBuilder();
            connectionString.AttachDBFilename = text;
            connectionString.DataSource = "(localDb)\\MSSQLLocalDB";
            // TODO: check for valid string
            HttpContext.Session.SetString("connection_string", connectionString.ConnectionString);
            return RedirectToAction("Index", "ShowTrees");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
