using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ExportBatch.Models.Export;
namespace ExportBatch.Models.CompareResult
{
    public class CRItem
    {
        [JsonProperty("fields")]
        public List<CRField> Fields { get; set; }

        [JsonProperty("quality")]
        public double Quality { get; set; }

        public CRItem() { }
        public CRItem(Item recognised, Item verified)
        {

            double rcqualyty = 0;

            var crFields = new List<CRField>();
            int count = 0;
            //for (int rd = 0; rd < recognised.Fields.Count; rd++)
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

            Quality = rcqualyty /count;
        }
    }
}
