using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace SomeStrangeDotNetProject.Models.JSON_translate_model.DataTypes
{
    public class TreeNull : TreeComponent
    {
        public override void DbRead(DataTable table)
        {
            return;
        }

        public override int DbSave(SqlConnection connection, int tree_id)
        {
            using (var command = new SqlCommand(@"INSERT INTO Objects([Key], [Value], [Separator], [Parent_id], [Tree_id]) OUTPUT inserted.Id VALUES(@Key, @Value, @Separator, @Parent_id, @Tree_id)", connection)) // saving object
            {
                command.Parameters.AddWithValue("@Key", Key ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Value", DBNull.Value);
                command.Parameters.AddWithValue("@Separator", "Null");
                command.Parameters.AddWithValue("@Parent_id", Parent?.Id ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Tree_id", tree_id);
                connection.Open();
                this.Id = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
            return Id;
        }

        public override void ReadFromJson(JsonElement jsonElement)
        {
            return;
        }

        public override string Render()
        {
            return $"<li>{Key ?? " "}: <strong color='red'>Null</strong></li>";
        }
    }
}
