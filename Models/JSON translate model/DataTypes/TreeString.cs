using System.Data.SqlClient;
using System.Text.Json;

namespace SomeStrangeDotNetProject.Models.JSON_translate_model.DataTypes
{
    public class TreeString : TreeComponent
    {
        public string Value { get; set; } = "";

        public override int DbRead(SqlConnection connection, int root_id)
        {
            throw new NotImplementedException();
        }

        public override int DbSave(SqlConnection connection, int root_id)
        {
            using (var command = new SqlCommand(@"INSERT INTO Objects([Key], [Value], [Separator], [Parent_id], [Root_id]) OUTPUT inserted.Id VALUES(@Key, @Value, @Separator, @Parent_id, @Root_id)", connection))
            {
                command.Parameters.AddWithValue("@Key", Key ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Value", Value ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Separator", "String");
                command.Parameters.AddWithValue("@Parent_id", Parent?.Id ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Root_id", root_id);
                connection.Open();
                this.Id = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
            return Id;
        }

        public override void ReadFromJson(JsonElement jsonElement)
        {
            Value = jsonElement.GetString() ?? "";
        }
    }
}
