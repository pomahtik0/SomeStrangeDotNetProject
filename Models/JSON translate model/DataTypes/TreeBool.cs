﻿using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace SomeStrangeDotNetProject.Models.JSON_translate_model.DataTypes
{
    public class TreeBool : TreeComponent
    {
        public bool Value { get; set; }

        public override void DbRead(DataTable dataTable)
        {
            var value = dataTable.AsEnumerable().Where(row => row.Field<int>("Id").Equals(Id)).Select(row => row.Field<string>("Value")).FirstOrDefault();
            this.Value = bool.Parse(value ?? "False");
        }

        public override int DbSave(SqlConnection connection, int tree_id)
        {
            using (var command = new SqlCommand(@"INSERT INTO Objects([Key], [Value], [Separator], [Parent_id], [Tree_id]) OUTPUT inserted.Id VALUES(@Key, @Value, @Separator, @Parent_id, @Tree_id)", connection)) // saving object
            {
                command.Parameters.AddWithValue("@Key", Key ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Value", Value.ToString() ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Separator", "Bool");
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
            Value = jsonElement.GetBoolean();
        }
    }
}
