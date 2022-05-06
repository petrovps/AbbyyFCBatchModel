using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ABBYY.FlexiCapture;

namespace ExportBatch.Models.Export
{
    public class Region
    {
        [JsonProperty("documentPageIndex")]
        public int DocumentPageIndex { get; set; }
        [JsonProperty("rects")]
        public List<Rect> Rects { get; set; }

        public Region() { }

        public Region(IFieldRegion fieldRegion) 
        {
            List<Rect> rects = new List<Rect>();
            foreach(IRect rect in fieldRegion.Rects)
                rects.Add(new Rect(rect));
            Rects=rects.Where(item => item != null).ToList(); ;
        }


    }
}
