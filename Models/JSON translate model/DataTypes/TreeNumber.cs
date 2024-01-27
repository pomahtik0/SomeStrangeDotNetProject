using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace SomeStrangeDotNetProject.Models.JSON_translate_model.DataTypes
{
    public class TreeNumber : TreeComponent
    {
        public double Value { get; set; }

        public override void DbRead(DataTable dataTable)
        {
            var value = dataTable.AsEnumerable().Where(row => row.Field<int>("Id").Equals(Id)).Select(row => row.Field<string>("Value")).FirstOrDefault();
            this.Value = Convert.ToDouble(value);
        }

        public override int DbSave(SqlConnection connection, int tree_id)
        {
            throw new NotImplementedException();
        }

        public override void ReadFromJson(JsonElement jsonElement)
        {
            Value = jsonElement.GetDouble();
        }
    }
}
