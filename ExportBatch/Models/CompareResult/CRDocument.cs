using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ExportBatch.Models.Export;
namespace ExportBatch.Models.CompareResult
{
    public class CRDocument
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sections")]
        public List<CRSection> Sections { get; set; }


        [JsonProperty("quality")]
        public double Quality { get; set; }
        //[JsonProperty("id")]
        //public string Id { get; set; }
        public CRDocument() { }
        public CRDocument(Document recognised, Document verified)
        {
            if (!verified.Name.Equals(recognised.Name))
            { Quality = 0; return; }

            //if (verified.Id != null)
            //    Id = verified.Id;
            Name = verified.Name;
            double rcqualyty = 0;

            var crSections = new List<CRSection>();
           // for (int rd = 0; rd < recognised.Sections.Count; rd++)
            {
                for (int vd = 0; vd < verified.Sections.Count; vd++)
                {
                    var crseq = new CRSection(recognised.Sections[vd], verified.Sections[vd]);
                    rcqualyty += crseq.Quality;
                    crSections.Add(crseq);
                }
            }

            Quality = rcqualyty / verified.Sections.Count;

            Sections = crSections;
        }
      
    }
}
