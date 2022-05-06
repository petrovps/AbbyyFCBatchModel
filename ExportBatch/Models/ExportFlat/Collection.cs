using ABBYY.FlexiCapture;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExportBatch.Models.ExportFlat
{
    public class Collection
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("items")]
        public List<Item> Items { get; set; }

        public Collection() { }
        public Collection(IField FieldWithItems, string cName)
        {
           Name =  cName;
           if(HasItems(FieldWithItems))
           Items = GetItems(FieldWithItems).Where(item => item != null).ToList();
        }

        private static List<Item> GetItems(IField FieldWithItems)
        {
            var rItems = new List<Item>();

            foreach(IField item in FieldWithItems.Items)
            {
                 rItems.Add(new Item(item));
            }
            return rItems;
        }
        private static bool IsLeaf(IField field)
        {
            return
                field.Type != TExportFieldType.EFT_CheckmarkGroup &&
                field.Type != TExportFieldType.EFT_Document &&
                field.Type != TExportFieldType.EFT_Group &&
                field.Type != TExportFieldType.EFT_Section &&
                field.Type != TExportFieldType.EFT_Table &&
                field.Type != TExportFieldType.EFT_TableRow &&
                field.Items == null &&
                field.Children == null;
        }
        private static bool HasItems(IField Field)
        {
            bool IsRepeatable = false;
            try
            {
                if (Field.Items.Count > 0)
                    IsRepeatable = true;

            }
            catch (NullReferenceException e)
            {
                IsRepeatable = false;
            }

            return IsRepeatable;
        }

    }
}
