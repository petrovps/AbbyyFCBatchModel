using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ABBYY.FlexiCapture;

namespace ExportBatch.Models.ExportFlat
{
    internal class Responce
    {
        [JsonProperty("modelVersion")]
        public string ModelVersion { get; set; }

        [JsonProperty("requestId")]
        public string RequestId { get; set; }

        [JsonProperty("modelType")]
        public string ModelType { get; set; }

        [JsonProperty("properties")]
        public List<Property> Properties { get; set; }

        [JsonProperty("documents")]
        public List<Document> Documents { get; set; }

        [JsonProperty("status")]
        public ResponceStatus Status { get; set; }

        public Responce() { }

        public Responce(IBatch Batch, StatusCodes statusCode = StatusCodes.OK) 
        {
            ModelVersion = "1.0";
            RequestId = Guid.NewGuid().ToString();
            ModelType = "standard";
            Properties = GetProps(Batch.Properties).Where(item => item != null).ToList(); ;
            Status = new ResponceStatus(statusCode);
            Documents = GetDocs(Batch.Documents).Where(item => item != null).ToList(); ;
        }
        
        private static List<Document> GetDocs(IDocuments Documents)
        {
            var rDocs = new List<Document>();
            foreach (IDocument doc in Documents)
            {
                rDocs.Add(new Document(doc));
            }
            return rDocs;
        }


        private static List<Property> GetProps(IProperties Properties)
        {
            var props = new List<Property>();
            foreach (IProperty prop in Properties)
            {
                props.Add(new Property(prop));
            }
            return props;
        }
    }
}
