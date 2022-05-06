using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ABBYY.FlexiCapture;

namespace ExportBatch.Models.Export
{
    public class Section
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("fields")]
        public List<Field> Fields { get; set; }


        public Section() { }
        public Section(IField Section)
        {
            Name=Section.Name;
            var rFields = new List<Field>();
            foreach(IField item in Section.Children)
            {
                rFields.Add(new Field(item));
            }
            Fields = rFields.Where(item => item != null).ToList();
        }


    }
}
