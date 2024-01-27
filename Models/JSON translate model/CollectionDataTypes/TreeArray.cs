using SomeStrangeDotNetProject.Models.JSON_translate_model.DataTypes;
using System.Data.SqlClient;
using System.Text;
using System.Text.Json;

namespace SomeStrangeDotNetProject.Models.JSON_translate_model.CollectionDataTypes
{
    public class TreeArray : TreeObject
    {
        public override int DbSave(SqlConnection connection, int tree_id)
        {
            using (var command = new SqlCommand(@"INSERT INTO Objects([Key], [Separator], [Parent_id], [Tree_id]) OUTPUT inserted.Id VALUES(@Key, @Separator, @Parent_id, @Tree_id)", connection)) // saving object
            {
                command.Parameters.AddWithValue("@Key", Key ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Separator", "Array");
                command.Parameters.AddWithValue("@Parent_id", Parent?.Id ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Tree_id", tree_id);
                connection.Open();
                this.Id = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
            children.ForEach(child => child.DbSave(connection, tree_id)); // saving children of the object
            return Id;
        }
        public override void ReadFromJson(JsonElement jsonElement)
        {
            foreach (var child in jsonElement.EnumerateArray())
            {
                TreeComponent treeComponent;
                switch (child.ValueKind) // fabric?
                {
                    case JsonValueKind.String:
                        treeComponent = new TreeString();
                        break;

                    case JsonValueKind.Object:
                        treeComponent = new TreeObject();
                        break;

                    case JsonValueKind.True:
                    case JsonValueKind.False:
                        treeComponent = new TreeBool();
                        break;

                    case JsonValueKind.Null:
                        treeComponent = new TreeNull();
                        break;

                    case JsonValueKind.Number:
                        treeComponent = new TreeNumber();
                        break;

                    case JsonValueKind.Array:
                        treeComponent = new TreeArray();
                        break;
                    default: throw new InvalidOperationException("Have no idea");
                }

                treeComponent.Parent = this;
                treeComponent.ReadFromJson(child);
                children.Add(treeComponent);
            }
        }
        public override string Render()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(
                $"""
                <li>
                    <span class='caret'>{Key}</span> [
                    <ul class='nested'>
                """);

            foreach (var child in children)
            {
                sb.AppendLine(child.Render());
            }

            sb.AppendLine("</ul>]</li>");
            return sb.ToString();
        }
    }
}
