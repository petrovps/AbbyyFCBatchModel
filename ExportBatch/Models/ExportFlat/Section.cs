using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABBYY.FlexiCapture;
using Newtonsoft.Json;

namespace ExportBatch.Models.ExportFlat
{
    public class Section
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("fields")]
        public List<Field> Fields { get; set; }

        [JsonProperty("collections")]
        public List<Collection> Collections { get; set; }

        public Section() { }
        public Section(IField Section)
        {
            Name = Section.Name;
            Fields = GetFields(Section.Children).Where(item => item != null).ToList(); 
            if(!Section.Name.Contains("Бухгалтерский баланс"))
            Collections = GetColldections(Section.Children).Where(item => item != null).ToList(); 
            else
            Collections = GetColldectionsBB(Section).Where(item => item != null).ToList();

        }

        private List<Field> GetFields(IFields Children)
        {

            var fields = new List<Field>();
            foreach(IField field in Children)
            {
                if (!IsLeaf(field)) continue;
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

        private List<Collection> GetColldectionsBB(IField Section)
        {
            var collection = new List<Collection>();


            collection.Add(new Collection(Section.Field("Active").Field("VneoborotActives"), "VneoborotActives"));
            collection.Add(new Collection(Section.Field("Active").Field("OborotActives"), "OborotActives"));
            collection.Add(new Collection(Section.Field("Active").Field("ShortTable"), "ShortTable"));

            collection.Add(new Collection(Section.Field("Passive").Field("CapitalAndReserves"), "CapitalAndReserves"));
            collection.Add(new Collection(Section.Field("Passive").Field("Celevoe"), "Celevoe"));
            collection.Add(new Collection(Section.Field("Passive").Field("Dolgosrochnye"), "Dolgosrochnye"));
            collection.Add(new Collection(Section.Field("Passive").Field("Kratkosrochnye"), "Kratkosrochnye"));
            collection.Add(new Collection(Section.Field("Passive").Field("ShortTable"), "ShortTable"));

            return collection;
        }


        private IField GetFieldByName(IFields fields, string FieldName)
        {
            foreach (IField field in fields)
            { 
                if (field.Name.Equals(FieldName)) 
                { 
                return field;
                } 
            }
            return null;
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
    }
}
