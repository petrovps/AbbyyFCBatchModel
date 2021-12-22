using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentModel
{
    public class Batch
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Document[] Documents { get; set; }
    }
    public class Document
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public Field[] Fields { get; set; }
        public Group[] Groups { get; set; }
        public Page[] Pages { get; set; }
        public Instance[] RepeatableInstances { get; set; }
    }
    public class Field
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string SuspiciousSymbols { get; set; }

    }
    public class Group
    {
        public Field[] Fields { get; set; }
        public Group[] Groups { get; set; }
        public Page[] Pages { get; set; }
        public Instance[] RepeatableInstances { get; set; }
    }
    public class Page
    {
        public string ImageSource { get; set; }
        public int PageIndex { get; set; }
    }
    public class Instance
    {
        public Field[] Fields { get; set; }
        public Group[] Groups { get; set; }
        public Page[] Pages { get; set; }
        public Instance[] RepeatableInstances { get; set; }
    }
    public class Region
    {
        public int PageIndex { get; set; }
        public Rect[] Rects { get; set; }
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
        public string[] values { get; set; }
    }

}
