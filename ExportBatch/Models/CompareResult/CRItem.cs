using System.Collections.Generic;
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
        public CRItem(Item verified, Item recognised)
        {

            var crFields = new List<CRField>();


            for (int vd = 0; vd < verified.Fields.Count; vd++)
            {

                if (!verified.Fields[vd].Type.Equals("EFT_Table") && string.IsNullOrEmpty(recognised.Fields[vd].Value) && string.IsNullOrEmpty(verified.Fields[vd].Value))
                    continue;
                if (!verified.Fields[vd].Type.Equals("EFT_Table") && !verified.Fields[vd].IsExportable)//|| !verified.Fields[vd].IsMatched)
                    continue;
                if (verified.Fields[vd].Type.Equals("EFT_Table") && verified.Fields[vd].Items == null) // пропустим пустые таблицы, т.к. иначе качетсво = 0
                    continue;

                if (verified.Fields[vd].Name.Equals(recognised.Fields[vd].Name))
                {
                    var crfield = new CRField(recognised.Fields[vd], verified.Fields[vd]);
                    crFields.Add(crfield);
                }
                else
                {
                    var field = FindField(recognised.Fields, verified.Fields[vd].Name);
                    if (field != null)
                    {
                        var crfield = new CRField(field, verified.Fields[vd]);
                        crFields.Add(crfield);
                    }
                    else
                    {
                        var crfield = new CRField();
                        crfield.VerifiedValue = verified.Fields[vd].Value;
                        crfield.Quality = 0;
                        crfield.Name = verified.Fields[vd].Name;
                        crFields.Add(crfield);
                    }
                }
            }

            if (crFields != null && crFields.Count!=0)
            {
                Fields = crFields;
                Quality = AverageQuality(crFields);
            }
            else
                return;

        }

        private static double AverageQuality(List<CRField> List)
        {
            double rcqualyty = 0;
            int i = 0;
            foreach (CRField item in List)
            {
                if (double.IsNaN(item.Quality))
                    continue;
                rcqualyty += item.Quality;
                i++;
            }
            return rcqualyty / i;
        }


        public CRItem(Item verified) 
        {
            var crFields = new List<CRField>();
            for (int vd = 0; vd < verified.Fields.Count; vd++)
            {
                if (!verified.Fields[vd].Type.Equals("EFT_Table") && !verified.Fields[vd].IsExportable)//|| !verified.Fields[vd].IsMatched)
                    continue;
                var crfield = new CRField();
                crfield.VerifiedValue = verified.Fields[vd].Value;
                crfield.Quality = 0;
                crfield.Name = verified.Fields[vd].Name;
                crFields.Add(crfield);

            }
            Fields = crFields;
            Quality = AverageQuality(crFields);

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
