using Microsoft.AspNetCore.Razor.TagHelpers;
using SomeStrangeDotNetProject.Models.JSON_translate_model.DataTypes;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;
using System.Xml.Linq;

namespace SomeStrangeDotNetProject.Models.JSON_translate_model.CollectionDataTypes
{
    public class TreeObject : TreeComponent
    {
        protected List<TreeComponent> children = [];
        internal TreeObject()
        {

        }
        public TreeObject(JsonDocument document)
        {
            ReadFromJson(document.RootElement);
        }
        public TreeObject(SqlConnection connection, int tree_id)
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand("SELECT [Root_id] FROM [Trees] WHERE [Id]=@Tree_id", connection)) // command to get root id
            {
                command.Parameters.AddWithValue("@Tree_id", tree_id);
                var reader = command.ExecuteReader();
                reader.Read();
                this.Id = (int)reader[0];
                reader.Close();
            }
            
            DataTable dataTable = new DataTable();
            using (SqlDataAdapter adapter = new SqlDataAdapter("Select * FROM [Objects] WHERE [Tree_id]=@Tree_id", connection)) // creating dataAdapter to fill datatable with values of current tree
            {
                adapter.SelectCommand.Parameters.AddWithValue("@Tree_id", tree_id);
                adapter.Fill(dataTable);
            }
            connection.Close();
            DbRead(dataTable);
        }
        public override void ReadFromJson(JsonElement jsonElement)
        {
            foreach(var child in jsonElement.EnumerateObject())
            {
                TreeComponent treeComponent;
                switch(child.Value.ValueKind)
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

                treeComponent.Key = child.Name;
                treeComponent.Parent = this;
                treeComponent.ReadFromJson(child.Value);
                children.Add(treeComponent);
            }
        }

        public override void DbRead(DataTable dataTable)
        {
            var childrenRows = dataTable.AsEnumerable()
                .Where(row => row.Field<int?>("Parent_id").Equals(Id))
                .Select(row => new {key = row.Field<string?>("Key"), type = row.Field<string>("Separator"), id = row.Field<int>("Id") });
            foreach (var child in childrenRows)
            {
                TreeComponent childComponent;
                switch(child.type)
                {
                    case "Object":
                        childComponent = new TreeObject();
                        break;
                    case "Array":
                        childComponent = new TreeArray();
                        break;
                    case "String":
                        childComponent = new TreeString();
                        break;
                    case "Bool":
                        childComponent = new TreeBool();
                        break;
                    case "Number":
                        childComponent = new TreeNumber();
                        break;
                    case "Null":
                        childComponent = new TreeNull();
                        break;
                    default: throw new InvalidOperationException("I didn't put that there!");
                }
                childComponent.Id = child.id;
                childComponent.Key = child.key;
                childComponent.Parent = this;
                children.Add(childComponent);
                childComponent.DbRead(dataTable);
            }
        }

        public override int DbSave(SqlConnection connection, int tree_id)
        {
            using (var command = new SqlCommand(@"INSERT INTO Objects([Key], [Separator], [Parent_id], [Tree_id]) OUTPUT inserted.Id VALUES(@Key, @Separator, @Parent_id, @Tree_id)", connection)) // saving object
            {
                command.Parameters.AddWithValue("@Key", Key ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Separator", "Object");
                command.Parameters.AddWithValue("@Parent_id", Parent?.Id ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Tree_id", tree_id);
                connection.Open();
                this.Id = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
            children.ForEach(child => child.DbSave(connection, tree_id)); // saving children of the object
            return Id;
        }

        public void DbSaveRoot(SqlConnection connection, string root_name)
        {
            int root_id;
            using (var command = new SqlCommand(@"INSERT INTO [Trees]([Name]) OUTPUT inserted.Id VALUES(@Name)", connection))
            {
                command.Parameters.AddWithValue("@Name", root_name);
                connection.Open();
                root_id = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
            try
            {
                DbSave(connection, root_id);
            }
            catch
            {
                connection.Close();
                using (var command = new SqlCommand(@"DELETE FROM [Trees] WHERE [Id]=@id", connection)) // remove invalid root
                {
                    command.Parameters.AddWithValue("@id", root_id);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                throw; // rethrow exception
            }
            finally
            {
                connection.Close(); // closing connection if it is open;
            }
            using (var command = new SqlCommand(@"Update [Trees] SET [Root_id]=@Root_id WHERE [Id]=@Id", connection)) // link to the root
            {
                command.Parameters.AddWithValue("@Id", root_id);
                command.Parameters.AddWithValue("@Root_id", Id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public override string Render()
        {
            throw new NotImplementedException();
        }
    }
}
