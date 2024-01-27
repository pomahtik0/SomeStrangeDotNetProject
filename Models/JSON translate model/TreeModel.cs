using SomeStrangeDotNetProject.Models.JSON_translate_model.CollectionDataTypes;

namespace SomeStrangeDotNetProject.Models.JSON_translate_model
{
    public class TreeModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public TreeObject? TreeRoot { get; set; }
    }
}
