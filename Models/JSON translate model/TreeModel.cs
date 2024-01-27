using SomeStrangeDotNetProject.Models.JSON_translate_model.CollectionDataTypes;
using System.Data.SqlClient;

namespace SomeStrangeDotNetProject.Models.JSON_translate_model
{
    public class TreeModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public TreeObject? TreeRoot { get; set; }
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
    }
}
