using ABBYY.FlexiCapture;
using Newtonsoft.Json;

namespace ExportBatch.Models.ExportFlat
{
    public class Field
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("provider")]
        public string Provider { get; set; } = "Abbyy";

        [JsonProperty("suspiciousSymbols")]
        public string SuspiciousSymbols { get; set; }

        [JsonProperty("accuracy")]
        public int Accuracy { get; set; }

        public Field() { }
        public Field(IField Field)
        {
            Name = Field.Name;
            //if (!IsLeaf(Field)) return;
            Value = Field.Text;
            SuspiciousSymbols = Field.SuspiciousSymbols;
            if(Field.SuspiciousSymbols.Length>0)
            Accuracy = Field.SuspiciousSymbols.Replace("1", "").Length * 100 / Field.SuspiciousSymbols.Length;

        }




        //private bool IsLeaf(IField field)
        //{
        //    return
        //        field.Type != TExportFieldType.EFT_CheckmarkGroup &&
        //        field.Type != TExportFieldType.EFT_Document &&
        //        field.Type != TExportFieldType.EFT_Group &&
        //        field.Type != TExportFieldType.EFT_Section &&
        //        field.Type != TExportFieldType.EFT_Table &&
        //        field.Type != TExportFieldType.EFT_TableRow &&
        //        field.Items == null &&
        //        field.Children == null;
        //}
    }
}
