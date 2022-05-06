using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ABBYY.FlexiCapture;

namespace ExportBatch.Models.Export
{
    public class Rect
    {
        [JsonProperty("left")]
        public int left { get; set; }
        [JsonProperty("top")]
        public int top { get; set; }
        [JsonProperty("right")]
        public int right { get; set; }
        [JsonProperty("bottom")]
        public int bottom { get; set; }

        public Rect() { }

        public Rect(IRect rect)
        {
            left = rect.Left;
            top = rect.Top;
            right = rect.Right;
            bottom = rect.Bottom;
        }

    }
}
