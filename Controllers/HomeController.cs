using Microsoft.AspNetCore.Mvc;
using SomeStrangeDotNetProject.Models;
using SomeStrangeDotNetProject.Models.JSON_translate_model;
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
        public IActionResult UploadNewTree(IFormFile file)
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

            var fileExt = Path.GetExtension(file.FileName);

            // Validate the file extension
            if (fileExt != ".json" && fileExt != ".txt")
            {
                return BadRequest("Invalid file type");
            }

            TreeModel tree = new TreeModel();

            if (fileExt == ".json")
            {
                tree.ReadFromJson(file);
            }
            else
            {
                tree.ReadFromTxt(file);
            }

            using (SqlConnection conn = new SqlConnection(HttpContext.Session.GetString("connection_string")))
            {
                (tree.TreeRoot as TreeObject)?.DbSaveRoot(conn, tree.Name);
            };
            return RedirectToAction("GetTree", "ShowTrees");
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
            var validateConn = new ValidatingDb(connectionString.ConnectionString);
            if (validateConn.Validate())
            {
                HttpContext.Session.SetString("connection_string", connectionString.ConnectionString);
                return RedirectToAction("GetTree", "ShowTrees");
            }
            else return BadRequest("Bad connection string");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
