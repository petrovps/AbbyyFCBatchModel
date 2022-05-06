using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ABBYY.FlexiCapture;

namespace ExportBatch.Models.Export
{
    public class Item
    {
        [JsonProperty("fields")]
        public List<Field> Fields { get; set; }

        public Item() { }

        public Item(IField fitem)
        {
            var rFields = new List<Field>();
            foreach (IField f in fitem.Children)
            {
                rFields.Add(new Field(f));
            }
            Fields = rFields.Where(item => item != null).ToList();
        }
    }
}
