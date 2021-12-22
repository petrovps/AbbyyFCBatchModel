using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportBatch
{
    public class DocumentModel
    {
        public class Responce
        {
            public string ModelVersion { get; set; } = "2.0";
            public string TaskId { get; set; } = Guid.NewGuid().ToString();
            public Status Status { get; set; }
            public Batch Batch { get; set; }
        }
        public class Status
        {
            public int Code { get; set; } = 200;
            public string Description { get; set; } = "Задача обработана успешно";

        }
        public class Batch
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public List<Document> Documents { get; set; }
            //  public List<Parameter> Parameters { get; set; }
        }

        public class Parameter
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }
        public class Document
        {
            public string Name { get; set; }
            public string Id { get; set; }
            public List<Field> Fields { get; set; }
            public List<Group> Groups { get; set; }
            public List<Page> Pages { get; set; }
            public List<RepeatableInstance> RepeatableInstances { get; set; }
        }
        public class Field
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public string SuspiciousSymbols { get; set; }

        }
        public class Group
        {
            public string Name { get; set; }
            public List<Field> Fields { get; set; }
            public List<Group> Groups { get; set; }
            public List<RepeatableInstance> RepeatableInstances { get; set; }
        }
        public class Page
        {
            public string ImageSource { get; set; }
            public int PageIndex { get; set; }
        }

        public class RepeatableInstance
        {
            public string Name { get; set; }
            public List<Instance> Instances { get; set; }
        }

        public class Instance
        {
            public List<Field> Fields { get; set; }
            public List<Group> Groups { get; set; }
            public List<RepeatableInstance> RepeatableInstances { get; set; }
        }
        public class Region
        {
            public int PageIndex { get; set; }
            public List<Rect> Rects { get; set; }
        }
        public class Rect
        {
            public int Left { get; set; }
            public int Right { get; set; }
            public int Top { get; set; }
            public int Bottom { get; set; }
        }
        class RepeatableField
        {
            public string Name { get; set; }
            public List<string> values { get; set; }
        }

    }
}
