using System.Text.Json;

namespace SomeStrangeDotNetProject.Models.JSON_translate_model
{
    public abstract class TreeComponent
    {
        public int Id { get; set; }
        public string? Key {  get; set; }
        public abstract void ReadFromJson(JsonElement jsonElement);
    }
}
