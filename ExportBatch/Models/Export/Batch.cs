using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ABBYY.FlexiCapture;

namespace ExportBatch.Models.Export
{
    public class Batch
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("documents")]
        public List<Document> Documents { get; set; }
        [JsonProperty("properties")]
        public List<Property> Properties { get; set; }

        [JsonProperty("batchTypeName")]
        public string BatchTypeName { get; set; }

        [JsonProperty("creationDate")]
        public string CreationDate { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }

        public Batch() { }

        public Batch(IBatch Batch)
        {
            Name = Batch.Name;
            BatchTypeName = Batch.BatchTypeName;
            Id = Batch.Id;
            CreationDate = Batch.CreationDate.ToString();
            Properties = GetProps(Batch.Properties).Where(item => item != null).ToList(); 
            Description = Batch.Comment;
            Documents = GetDocuments(Batch.Documents).Where(item => item != null).ToList();

        }
        private List<Document> GetDocuments(IDocuments docs)
        {
            var rDocuments = new List<Document>();
            foreach (IDocument doc in docs)
            {
                if(doc != null && !string.IsNullOrEmpty(doc.DefinitionName))
                rDocuments.Add(new Document(doc));
            }
            return rDocuments;
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
