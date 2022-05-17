using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ExportBatch.Models.Export;

namespace ExportBatch.Models.CompareResult
{
    public class CRSection
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("fields")]
        public List<CRField> Fields { get; set; }
        [JsonProperty("quality")]
        public double Quality { get; set; }

        public CRSection() { }
        public CRSection(Section verified, Section recognised) 
        {
            Name = verified.Name;

            double rcqualyty = 0;

            var crFields = new List<CRField>();
            int count = 0;
            //  for (int rd = 0; rd < recognised.Fields.Count; rd++)
            {
               
                for (int vd = 0; vd < verified.Fields.Count; vd++)
                {
                    if (string.IsNullOrEmpty(recognised.Fields[vd].Value) && string.IsNullOrEmpty(verified.Fields[vd].Value))
                        continue;
                    var crfield = new CRField(recognised.Fields[vd], verified.Fields[vd]);
                    rcqualyty += crfield.Quality;
                    crFields.Add(crfield);
                    count++;
                }
            }
            Fields = crFields;
            Quality = rcqualyty / count;

        }
    }
}
