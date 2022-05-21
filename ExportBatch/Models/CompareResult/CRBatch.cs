using System.Collections.Generic;
using Newtonsoft.Json;
using ExportBatch.Models.Export;

namespace ExportBatch.Models.CompareResult
{
    public class CRBatch
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("documents")]
        public List<CRDocument> Documents { get; set; }

        [JsonProperty("batchTypeName")]
        public string BatchTypeName { get; set; }

        [JsonProperty("creationDate")]
        public string CreationDate { get; set; }

        [JsonProperty("quality")]
        public double Quality { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }


        public CRBatch() { }

        public CRBatch(Batch recognised, Batch verified)
        {
            Name = verified.Name;
            BatchTypeName = verified.BatchTypeName;

            CreationDate = verified.CreationDate;
            var crDocuments = new List<CRDocument>();
            if (verified.Documents == null)
                return;

            for (int vd = 0; vd < verified.Documents.Count; vd++)
            {
                if (string.IsNullOrEmpty(verified.Documents[vd].Name))
                    continue;

                if (recognised.Documents!=null && verified.Documents[vd].Name.Equals(recognised.Documents[vd].Name))
                {
                    var crdoc = new CRDocument(recognised.Documents[vd], verified.Documents[vd]);
                    crDocuments.Add(crdoc);
                }
                else
                {
                    var doc = FindDoc(recognised.Documents, verified.Documents[vd].Name);
                    if(doc != null)
                    {
                        var crdoc = new CRDocument(doc, verified.Documents[vd]);
                        crDocuments.Add(crdoc);
                    }
                    else
                    {
                        var crdoc = new CRDocument();
                        crdoc.Name=verified.Documents[vd].Name;
                        crdoc.Quality = 0;
                        crdoc.Sections = null;
                        crDocuments.Add(crdoc);
                    }
                }
            }

            if (crDocuments.Count > 0)
            {
                Quality = AverageQuality(crDocuments);
                Documents = crDocuments;
            }
           
        }

        private static double AverageQuality(List<CRDocument> List)
        {
            double rcqualyty = 0;
            int i = 0;
            foreach (CRDocument item in List)
            {
                if (double.IsNaN(item.Quality))
                    continue;
                rcqualyty += item.Quality;
                i++;
            }
            return rcqualyty / i;
        }

        private static Document FindDoc(List<Document> WhereToSearch, string DocName)
        {
            foreach(Document doc in WhereToSearch)
            {
                if (doc.Name.Equals(DocName))
                    return doc;
            }
            return null;
        }

    }
}
