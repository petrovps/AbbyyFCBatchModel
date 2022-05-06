using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABBYY.FlexiCapture;
using Newtonsoft.Json;

namespace ExportBatch.Models.ExportFlat
{
    public class Property
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        public Property(IProperty property)
        {
            Name = property.Name;
            Value = property.Value;
        }
    }
}
