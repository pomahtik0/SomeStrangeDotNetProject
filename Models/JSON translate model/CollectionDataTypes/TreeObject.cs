using Microsoft.AspNetCore.Razor.TagHelpers;
using SomeStrangeDotNetProject.Models.JSON_translate_model.DataTypes;
using System.Data;
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
        public TreeObject(SqlConnection connection, string root_name)
        {
            int tree_id;
            connection.Open();
            using (SqlCommand command = new SqlCommand("SELECT [Root_id], [Tree_id] FROM [Roots] WHERE [Name]=@Name", connection)) // command to get root id
            {
                command.Parameters.AddWithValue("@Name", root_name);
                var reader = command.ExecuteReader();
                reader.Read();
                this.Id = (int)reader[0];
                tree_id = (int)reader[1];
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
                    default: throw new NotImplementedException();
                }
            }
        }

        public override void DbRead(DataTable dataTable)
        {
            var currentObjectKey = dataTable.AsEnumerable().Where(row => row.Field<int>("Id") == this.Id).Select(x => x.Field<string?>("Key")).FirstOrDefault(); 
            Key = currentObjectKey;

            var childrenRows = dataTable.AsEnumerable().Where(row => row.Field<int?>("Parent_id").Equals(Id)).Select(row => new { type = row.Field<string>("Separator"), id = row.Field<int>("Id") });
            foreach (var childRow in childrenRows)
            {

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
            using (var command = new SqlCommand(@"INSERT INTO [Roots]([Name]) OUTPUT inserted.Id VALUES(@Name)", connection))
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
                using (var command = new SqlCommand(@"DELETE FROM [Roots] WHERE [Id]=@id", connection)) // remove invalid root
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
        }
    }
}
