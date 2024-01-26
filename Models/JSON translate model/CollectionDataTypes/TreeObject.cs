using SomeStrangeDotNetProject.Models.JSON_translate_model.DataTypes;
using System.Data.SqlClient;
using System.Text.Json;
using System.Xml.Linq;

namespace SomeStrangeDotNetProject.Models.JSON_translate_model.CollectionDataTypes
{
    public class TreeObject : TreeComponent
    {
        protected List<TreeComponent> children = [];
        protected TreeObject()
        {

        }
        public TreeObject(JsonDocument document)
        {
            ReadFromJson(document.RootElement);
        }
        public override void ReadFromJson(JsonElement jsonElement)
        {
            foreach(var child in jsonElement.EnumerateObject())
            {
                switch(child.Value.ValueKind)
                {
                    case JsonValueKind.String:
                        var treeString = new TreeString() { Key = child.Name };
                        treeString.ReadFromJson(child.Value);
                        children.Add(treeString);
                        break;
                    case JsonValueKind.Object:
                        var treeObject = new TreeObject() { Key = child.Name };
                        treeObject.ReadFromJson(child.Value);
                        children.Add(treeObject);
                        break;
                    default: throw new InvalidOperationException();
                }
            }
        }

        public override int DbRead(SqlConnection connection, int root_id)
        {
            throw new NotImplementedException();
        }

        public override int DbSave(SqlConnection connection, int root_id)
        {
            throw new NotImplementedException();
        }

        public void DbSaveRoot(SqlConnection connection, string root_name)
        {
            int root_id;
            using (var command = new SqlCommand(@"INSERT INTO Roots(Root_name) OUTPUT inserted.Id VALUES(@Name)", connection))
            {
                command.Parameters.AddWithValue("@Name", root_name);
                connection.Open();
                root_id = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
            DbSave(connection, root_id);
        }
    }
}
