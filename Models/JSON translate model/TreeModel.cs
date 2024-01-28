using SomeStrangeDotNetProject.Models.JSON_translate_model.CollectionDataTypes;
using System;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace SomeStrangeDotNetProject.Models.JSON_translate_model
{
    public class TreeModel : IRender
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";
        
        public TreeComponent? TreeRoot { get; set; }

        public void ReadFromTxt(IFormFile file)
        {
            Name = Path.GetFileNameWithoutExtension(file.FileName);
            var readStream = file.OpenReadStream();
            using(StreamReader reader = new StreamReader(readStream))
            {
                string? str;
                TreeObject treeroot = new TreeObject();
                this.TreeRoot = treeroot;
                while((str = reader.ReadLine()) != null)
                {
                    var queue = new Queue<string>(str.Split(':'));
                    treeroot.FindOrCreate(queue);
                };
            }
        }

        public void ReadFromJson(IFormFile file) 
        {
            Name = Path.GetFileNameWithoutExtension(file.FileName);
            JsonDocument doc;
            doc = JsonDocument.Parse(file.OpenReadStream());
            TreeRoot = new TreeObject(doc);
        }

        public static IEnumerable<TreeModel> GetAllDbTrees(SqlConnection connection)
        {
            List<TreeModel> trees = new List<TreeModel>();
            using (SqlCommand dataAdapter = new SqlCommand("SELECT [Id], [Name] FROM [Trees]", connection))
            {
                connection.Open();
                var reader = dataAdapter.ExecuteReader();
                while (reader.Read())
                {
                    trees.Add(new TreeModel() { Id = (int)reader[0], Name = (string)reader[1] });
                }
                reader.Close();
                connection.Close();
            }
            return trees;
        }

        public void FindAndReRoot(Queue<string> searchRequest)
        {

        }

        public string Render()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine
                ("""
                <style>
                    /* Remove default bullets */
                    ul, #myUL {
                        list-style-type: none;
                    }

                    /* Remove margins and padding from the parent ul */
                    #myUL {
                        margin: 0;
                        padding: 0;
                    }

                    /* Style the caret/arrow */
                    .caret {
                        cursor: pointer;
                        user-select: none; /* Prevent text selection */
                    }

                        /* Create the caret/arrow with a unicode, and style it */
                        .caret::before {
                            content: "\25B6";
                            color: black;
                            display: inline-block;
                            margin-right: 6px;
                        }

                    /* Rotate the caret/arrow icon when clicked on (using JavaScript) */
                    .caret-down::before {
                        transform: rotate(90deg);
                    }

                    /* Hide the nested list */
                    .nested {
                        display: none;
                    }

                    /* Show the nested list when the user clicks on the caret/arrow (with JavaScript) */
                    .active {
                        display: block;
                    }
                </style>
                """); // adding styles

            sb.AppendLine("""<ul id="myUL">""");

            sb.AppendLine(TreeRoot?.Render());

            sb.AppendLine("</ul>");

            sb.AppendLine
                ("""              
                <script>
                    var toggler = document.getElementsByClassName("caret");
                    var i;

                    for (i = 0; i < toggler.length; i++) {
                        toggler[i].addEventListener("click", function () {
                            this.parentElement.querySelector(".nested").classList.toggle("active");
                            this.classList.toggle("caret-down");
                        });
                    }
                </script>
                """); // adding scripts

            return sb.ToString();
        }
    }
}
