﻿using System.Text.Json;

namespace SomeStrangeDotNetProject.Models.JSON_translate_model.CollectionDataTypes
{
    public class TreeObject : TreeComponent
    {
        protected List<TreeComponent> children = [];
        public override void ReadFromJson(JsonElement jsonElement)
        {
            throw new NotImplementedException();
        }
    }
}
