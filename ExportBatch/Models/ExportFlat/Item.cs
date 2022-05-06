using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABBYY.FlexiCapture;
using Newtonsoft.Json;

namespace ExportBatch.Models.ExportFlat
{
    public class Item
    {
        [JsonProperty("fields")]
        public List<Field> Fields { get; set; }

        public Item() { }

        public Item(IField field)
        {

            Fields = GetFields(field).Where(item => item != null).ToList(); 
           
        }

        private List<Field> GetFields(IField Field)
        {
            var fields = new List<Field>();
            if (!HasItems(Field) && IsLeaf(Field)) 
            { 
                fields.Add(new Field(Field));
                return fields;
            }



            if (IsLeaf(Field)) return null;



            foreach (IField field in Field.Children)
            {
                    fields.Add(new Field(field));
            }
            return fields;
        }

        private List<Collection> GetColldections(IFields Fields)
        {
            var collection = new List<Collection>();
            foreach (IField field in Fields)
            {
                if (IsLeaf(field)) continue;
                collection.Add(new Collection(field, field.Name));
            }
            return collection;
        }

        private bool IsLeaf(IField field)
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
