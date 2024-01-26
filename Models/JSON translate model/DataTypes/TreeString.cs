using System.Text.Json;

namespace SomeStrangeDotNetProject.Models.JSON_translate_model.DataTypes
{
    public class TreeString : TreeComponent
    {
        public string Value { get; set; } = "";
        public override void ReadFromJson(JsonElement jsonElement)
        {
            Value = jsonElement.GetString() ?? "";
        }
    }
}
