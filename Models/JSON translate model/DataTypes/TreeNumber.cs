using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace SomeStrangeDotNetProject.Models.JSON_translate_model.DataTypes
{
    public class TreeNumber : TreeComponent
    {
        public double Value { get; set; }

        public override void DbRead(DataTable table)
        {
            throw new NotImplementedException();
        }

        public override int DbSave(SqlConnection connection, int tree_id)
        {
            throw new NotImplementedException();
        }

        public override void ReadFromJson(JsonElement jsonElement)
        {
            throw new NotImplementedException();
        }
    }
}
