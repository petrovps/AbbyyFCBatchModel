using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DocumentModel
{
    class Program
    {
        static void Main(string[] args)
        {
            var Batch = new Batch();

            Batch.Id = 1;
            Batch.Name = "BatchName";

            var Document = new Document();

            Document.Name = "DocName";

            var RInstance = new List<Instance>();
            var _RInstance = new Instance();

            var field = new Field();

            field.Name = "FieldName";
            field.Value = "FieldValue";
            _RInstance.Fields = new Field[] { field };

            var group = new Group { Fields = new Field[] { new Field { Name = "123123", Value = "123123123" } } };
            _RInstance.Groups = new Group[] { group };

            RInstance.Add(_RInstance);
            Document.RepeatableInstances = RInstance.ToArray();
            Batch.Documents = new Document[] { Document };
            var set = new JsonSerializerSettings();

           
             // var json = JsonConvert.SerializeObject(Batch, Json);
          



            Console.WriteLine(json);
            Console.ReadKey();
        }
    }
}
