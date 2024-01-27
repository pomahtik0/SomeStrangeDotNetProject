using SomeStrangeDotNetProject.Models.JSON_translate_model.CollectionDataTypes;
using System.Data.SqlClient;

namespace SomeStrangeDotNetProject.Models.JSON_translate_model
{
    public class TreeModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public TreeObject? TreeRoot { get; set; }
        public IEnumerable<TreeModel> GetAllDbTrees(SqlConnection connection)
        {
            List<TreeModel> trees = new List<TreeModel>();
            using (SqlCommand dataAdapter = new SqlCommand("SELECT ([Id], [Name]) FROM [Trees]", connection))
            {
                connection.Open();
                var reader = dataAdapter.ExecuteReader();
                while (false) ;
                reader.Close();
                connection.Close();
            }

        }
    }
}
