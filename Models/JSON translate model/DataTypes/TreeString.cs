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
            throw new NotImplementedException();
        }

        public override void ReadFromJson(JsonElement jsonElement)
        {
            Value = jsonElement.GetString() ?? "";
        }
    }
}
