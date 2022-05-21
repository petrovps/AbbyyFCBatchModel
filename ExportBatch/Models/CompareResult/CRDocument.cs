using System.Collections.Generic;
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


        public CRDocument() { }
        public CRDocument(Document recognised, Document verified)
        {
            Name = verified.Name;
            var crSections = new List<CRSection>();
            for (int vd = 0; vd < verified.Sections.Count; vd++)
            {
                var crseq = new CRSection(verified.Sections[vd], recognised.Sections[vd]);
                crSections.Add(crseq);
            }
            Quality = AverageQuality(crSections);
            Sections = crSections;
        }



        private static double AverageQuality(List<CRSection> List)
        {
            double rcqualyty = 0;
            int i = 0;
            foreach (CRSection item in List)
            {
                if (double.IsNaN(item.Quality))
                    continue;
                rcqualyty += item.Quality;
                i++;
            }
            return rcqualyty / i;
        }

    }
}
