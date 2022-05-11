using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ABBYY.FlexiCapture;

namespace ExportBatch.Models.Export
{
    public class Item
    {
        [JsonProperty("fields")]
        public List<Field> Fields { get; set; }

        public Item() { }

        public Item(IField fitem)
        {
            var rFields = new List<Field>();
            if (HasChildren(fitem))
            {
                foreach (IField f in fitem.Children)
                {
                    rFields.Add(new Field(f));
                }
            }
            else if (HasItems(fitem))
            {
                foreach (IField f in fitem.Items)
                {
                    rFields.Add(new Field(f));
                }
            }
            Fields = rFields;
        }

        private static bool HasItems(IField Field)
        {
            bool IsRepeatable = false;
            try
            {
                if (Field.Items.Count > 0)
                    IsRepeatable = true;

            }
            catch (NullReferenceException e)
            {
                IsRepeatable = false;
            }

            return IsRepeatable;
        }

        private static bool HasChildren(IField Field)
        {
            bool IsRepeatable = false;
            try
            {
                if (Field.Children.Count > 0)
                    IsRepeatable = true;

            }
            catch (NullReferenceException e)
            {
                IsRepeatable = false;
            }

            return IsRepeatable;
        }
    }
}
