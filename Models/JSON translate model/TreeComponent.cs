using SomeStrangeDotNetProject.Models.JSON_translate_model.CollectionDataTypes;
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
        public abstract void DbRead(DataTable table);
    }

    public interface IRender
    {
        public abstract string Render();
    }
    public abstract class TreeComponent : IJsonReadable, IDbSaveAndRead, IRender
    {
        public int Id { get; set; }
        
        protected string? key;
        
        public string? Key
        {
            get => key ?? (Parent != null ? $"{Parent.Key}.{Parent.children.IndexOf(this)}" : null);
            set => key = value;
        }
        
        public TreeObject? Parent { get; set; }

        public abstract void DbRead(DataTable table);
        
        public abstract int DbSave(SqlConnection connection, int tree_id);
        
        public abstract void ReadFromJson(JsonElement jsonElement);

        public abstract string Render();

        public virtual TreeComponent Find(Queue<string> searchRequest)
        {
            if (searchRequest.Count == 0) // if search finished
            {
                return this;
            }
            throw new ArgumentException();
        }
    }
}
