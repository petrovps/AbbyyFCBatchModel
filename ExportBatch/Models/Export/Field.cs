using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ABBYY.FlexiCapture;

namespace ExportBatch.Models.Export
{
    public class Field
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("items")]
        public List<Item> Items { get; set; }

        [JsonProperty("isMatched")]
        public bool IsMatched { get; set; } //наложено ли поле
        [JsonProperty("hasRuleError")]
        public bool HasRuleError { get; set; } //Содержит ли ошибки правил проверок
        [JsonProperty("isSuspicious")]
        public bool IsSuspicious { get; set; } // Уверено ли распознано
        [JsonProperty("isValid")]
        public bool IsValid { get; set; } //не содержит ошибок формата
        [JsonProperty("isVerified")]
        public bool IsVerified { get; set; } // верифицировано ли поле
        [JsonProperty("isExportable")]
        public bool IsExportable { get; set; } // Преднозначено ли для экспорта
        [JsonProperty("regions")]
        public List<Region> Regions { get; set; } // координаты поля на изображениях документа

        public Field() { }
        public Field(IField Field)
        {
            Name = Field.Name;
            //if (!IsLeaf(Field)) return;
            Type = Field.Type.ToString();
            if(IsLeaf(Field))
            {
                Value = Field.Text;
                IsMatched = Field.IsMatched;
                HasRuleError = Field.HasRuleError;
                IsSuspicious = Field.IsSuspicious;
                IsValid = Field.IsValid;

                if (IsMatched)
                    Regions=GetRegions(Field.Regions);
                //SuspiciousSymbols = Field.SuspiciousSymbols;
                //if (Field.SuspiciousSymbols.Length > 0)
                //    Accuracy = Field.SuspiciousSymbols.Replace("1", "").Length * 100 / Field.SuspiciousSymbols.Length;
            }
            else if (!IsLeaf(Field) && HasItems(Field))
            {
                var items = new List<Item>();
                foreach(IField item in Field.Items)
                {
                    items.Add(new Item(item));
                }
                Items = items.Where(item => item != null).ToList(); ;
            }

        }

      private List<Region> GetRegions(IFieldRegions Regions)
        {
            List<Region> regions = new List<Region>();
            foreach (IFieldRegion region in Regions)
                regions.Add(new Region(region));

            return regions;
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
