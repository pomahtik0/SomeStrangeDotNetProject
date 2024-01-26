using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace SomeStrangeDotNetProject.Models.JSON_translate_model
{
    public interface IJsonReadable
    {
        public abstract void ReadFromJson(JsonElement jsonElement);
    }

    public interface IDbSaveAndRead
    {
        public abstract int DbSave(SqlConnection connection, int tree_id);
        public abstract int DbRead(DataTable table);
    }

    public abstract class TreeComponent : IJsonReadable, IDbSaveAndRead
    {
        public int Id { get; set; }
        public string? Key {  get; set; }
        public TreeComponent? Parent { get; set; }

        public abstract int DbRead(DataTable table);
        public abstract int DbSave(SqlConnection connection, int tree_id);
        public abstract void ReadFromJson(JsonElement jsonElement);
    }
}
