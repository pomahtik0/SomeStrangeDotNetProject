using SomeStrangeDotNetProject.Models.JSON_translate_model.DataTypes;
using System.Data.SqlClient;
using System.Text.Json;

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

        }
    }
}
