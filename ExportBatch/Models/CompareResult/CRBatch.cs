using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            //if(verified.Id!= null)
            //    Id = verified.Id;

            CreationDate = verified.CreationDate;
            double crBatchQuality = 0;
            var crDocuments = new List<CRDocument>();
            int DocCount = 0;
            for (int vd = 0; vd < verified.Documents.Count; vd++)
            {
                if (string.IsNullOrEmpty(verified.Documents[vd].Name))
                    continue;

                if (verified.Documents[vd].Name.Equals(recognised.Documents[vd]))
                {
                    var crdoc = new CRDocument(recognised.Documents[vd], verified.Documents[vd]);
                    crBatchQuality += crdoc.Quality;
                    crDocuments.Add(crdoc);
                    DocCount++;
                }
                else
                {
                    var doc = FindDoc(recognised.Documents, verified.Documents[vd].Name);
                    if(doc != null)
                    {
                        var crdoc = new CRDocument(doc, verified.Documents[vd]);
                        crBatchQuality += crdoc.Quality;
                        crDocuments.Add(crdoc);
                        DocCount++;
                    }
                    else
                    {
                        var crdoc = new CRDocument();
                        crdoc.Name=verified.Documents[vd].Name;
                        crdoc.Quality = 0;
                        crdoc.Sections = null;
                        crDocuments.Add(crdoc);
                        DocCount++;
                    }

                }
              
            }
            Quality = crBatchQuality / DocCount;
            Documents = crDocuments;
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
