using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ExportBatch.Models.Export;
namespace ExportBatch.Models.CompareResult
{
    public class CRItem
    {
        [JsonProperty("fields")]
        public List<CRField> Fields { get; set; }

        [JsonProperty("quality")]
        public double Quality { get; set; }

        public CRItem() { }
        public CRItem(Item recognised, Item verified)
        {
            double rcqualyty = 0;
            var crFields = new List<CRField>();
            int count = 0;

            for (int vd = 0; vd < verified.Fields.Count; vd++)
            {
                if (!verified.Fields[vd].Type.Equals("EFT_Table") && string.IsNullOrEmpty(recognised.Fields[vd].Value) && string.IsNullOrEmpty(verified.Fields[vd].Value))
                    continue;
                if (!verified.Fields[vd].IsExportable)//|| !verified.Fields[vd].IsMatched)
                    continue;

                if (verified.Fields[vd].Name.Equals(recognised.Fields[vd].Name))
                {
                    var crfield = new CRField(recognised.Fields[vd], verified.Fields[vd]);
                    rcqualyty += crfield.Quality;
                    crFields.Add(crfield);
                    count++;
                }
                else
                {
                    var field = FindField(recognised.Fields, verified.Fields[vd].Name);
                    if (field != null)
                    {
                        var crfield = new CRField(field, verified.Fields[vd]);
                        rcqualyty += crfield.Quality;
                        crFields.Add(crfield);
                        count++;
                    }
                    else
                    {
                        var crfield = new CRField();
                        crfield.VerifiedValue = verified.Fields[vd].Value;
                        crfield.Quality = 0;
                        crfield.Name = verified.Fields[vd].Name;
                        crFields.Add(crfield);
                        count++;
                    }
                }
            }

            Fields = crFields;
            Quality = rcqualyty / count;
        }


        private static Field FindField(List<Field> WhereToFind, string FieldName)
        {
            foreach (Field f in WhereToFind)
            {
                if (f.Name.Equals(FieldName))
                    return f;
            }
            return null;
        }


    }
}
