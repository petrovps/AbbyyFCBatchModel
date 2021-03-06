using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ABBYY.FlexiCapture;


namespace ExportBatch.Models.ExportFlat
{
    public class Document
    {
        [JsonProperty("type")]
        public string Name { get; set; }

        [JsonProperty("sections")]
        public List<Section> Sections { get; set; }

        [JsonProperty("properties")]
        public List<Property> Properties { get; set; }

        public Document() { }
        public Document(IDocument document) 
        {
            Name = document.DocumentDefinition.Name;
            Properties = GetProps(document.Properties).Where(item => item != null).ToList(); ;
            Sections = GetSections(document.Sections).Where(item => item != null).ToList(); ;
        }

        private static List<Section> GetSections(IFields Sections)
        {
            var rSections = new List<Section>();
            foreach (IField section in Sections)
            {
                rSections.Add(new Section(section));
            }
            return rSections;
        }


        private static List<Property> GetProps(IProperties Properties)
        {
            var props = new List<Property>();
            foreach (IProperty prop in Properties)
            {
                props.Add(new Property(prop) );
            }
            return props;
        }
    }
}
