using System.Text.Json;

namespace SomeStrangeDotNetProject.Models.JSON_translate_model
{
    public interface IJsonReadable
    {
        public abstract void ReadFromJson(JsonElement jsonElement);
    }

    public abstract class TreeComponent : IJsonReadable
    {
        public int Id { get; set; }
        public string? Key {  get; set; }
        public TreeComponent? Parent { get; set; }
        public abstract void ReadFromJson(JsonElement jsonElement);
    }
}
