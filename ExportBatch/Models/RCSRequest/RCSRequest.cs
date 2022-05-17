using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ExportBatch.Models.RCSRequest
{
    public class RCSRequest
    {
        [JsonProperty(PropertyName = "customerId")]
        public string CustomerId { get; set; }
        [JsonProperty(PropertyName = "requestId")]
        public string RequestId { get; set; }
        [JsonProperty(PropertyName = "processName")]
        public string ProcessName { get; set; }
        [JsonProperty(PropertyName = "processParams")]
        public List<Param> ProcessParams { get; set; }
        [JsonProperty(PropertyName = "files")]
        public List<File> Files { get; set; }
        [JsonProperty(PropertyName = "serviceParams")]
        public List<Param> ServiceParams { get; set; }
        [JsonProperty(PropertyName = "processVersion")]
        public string ProcessVersion { get; set; } = "2.0";

        public RCSRequest() {
            CustomerId = "Customer1";
            RequestId = Guid.NewGuid().ToString();
            ProcessName = "Process1";
            var Param = new Param();


            ProcessParams = new List<Param>() { Param };
            ServiceParams = new List<Param>() { Param };
            ProcessVersion = "2.0";

            var file = new File();
            Files = new List<File>() { file };

        }



    }

    public class Param
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        public Param() {
            Name = "ParamName";
            Value = "ParamValue";
        }

    }


    public class File
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "ref")]
        public string Ref { get; set; }
        [JsonProperty(PropertyName = "fileParams")]
        public List<Param> FileParams { get; set; }


        public File() {

            Name = "Filename.jpg";
            Type = "image";
            Ref =Guid.NewGuid().ToString();
            FileParams = new List<Param>();
        }

    }


}
