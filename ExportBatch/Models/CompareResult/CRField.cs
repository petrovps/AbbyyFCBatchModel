﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ExportBatch.Models.Export;
namespace ExportBatch.Models.CompareResult
{
    public class CRField
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("quality")]
        public double Quality { get; set; }

        [JsonProperty("recognisedValue")]
        public string RecognisedValue { get; set; }
        [JsonProperty("verifiedValue")]
        public string VerifiedValue { get; set; }
        [JsonProperty("items")]
        public List<CRItem> Items { get; set; }

        //[JsonProperty("isMatched")]
        //public bool IsMatched { get; set; } //наложено ли поле
        //[JsonProperty("hasRuleError")]
        //public bool HasRuleError { get; set; } //Содержит ли ошибки правил проверок
        //[JsonProperty("isSuspicious")]
        //public bool IsSuspicious { get; set; } // Уверено ли распознано
        //[JsonProperty("isValid")]
        //public bool IsValid { get; set; } //не содержит ошибок формата
        //[JsonProperty("isVerified")]
        //public bool IsVerified { get; set; } // верифицировано ли поле
        //[JsonProperty("isExportable")]
        //public bool IsExportable { get; set; } // Преднозначено ли для экспорта
        //[JsonProperty("regions")]
        //public List<Region> Regions { get; set; } // координаты поля на изображениях документа
        //[JsonProperty("type")]
        //public string Type { get; set; }

        public CRField() { }
        public CRField(Field recognised, Field verified)
        {
            if (!verified.Name.Equals(recognised.Name))
            { return; }

            Name = verified.Name;
            RecognisedValue = recognised.Value;
            VerifiedValue = verified.Value;

            if (!string.IsNullOrEmpty(recognised.Value) && !string.IsNullOrEmpty(verified.Value))
            { Quality = 100 - (GetDistanceCore(recognised.Value, verified.Value) * 100); }
            else
            { Quality = 0; }

            if (HasItems(verified))
            {
                double rcqualyty = 0;

                var crItems = new List<CRItem>();
               // for (int rd = 0; rd < recognised.Items.Count; rd++)
                {
                    for (int vd = 0; vd < verified.Items.Count; vd++)
                    {
                        var critem = new CRItem(recognised.Items[vd], verified.Items[vd]);
                        rcqualyty += critem.Quality;
                        crItems.Add(critem);
                    }
                }
                Items = crItems;
                Quality = rcqualyty / recognised.Items.Count;
            }

        }


        public static bool HasItems(Field field)
        {
            if(field.Items != null && field.Items.Count > 0)
                return true;
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="word1"></param>
        /// <param name="word2"></param>
        /// <returns></returns>
        private static double GetDistanceCore(string word1, string word2)
        {
            var score = new int[word1.Length + 2, word2.Length + 2];

            var infinityScore = word1.Length + word2.Length;
            score[0, 0] = infinityScore;
            for (var i = 0; i <= word1.Length; i++)
            {
                score[i + 1, 1] = i;
                score[i + 1, 0] = infinityScore;
            }

            for (var j = 0; j <= word2.Length; j++)
            {
                score[1, j + 1] = j;
                score[0, j + 1] = infinityScore;
            }

            var sd = new SortedDictionary<char, int>();
            foreach (var letter in (word1 + word2))
            {
                if (!sd.ContainsKey(letter))
                    sd.Add(letter, 0);
            }

            for (var i = 1; i <= word1.Length; i++)
            {
                var DB = 0;
                for (var j = 1; j <= word2.Length; j++)
                {
                    var i1 = sd[word2[j - 1]];
                    var j1 = DB;

                    if (word1[i - 1] == word2[j - 1])
                    {
                        score[i + 1, j + 1] = score[i, j];
                        DB = j;
                    }
                    else
                    {
                        score[i + 1, j + 1] = Math.Min(Math.Min(score[i, j], score[i + 1, j]), score[i, j + 1]) + 1;
                    }

                    score[i + 1, j + 1] = Math.Min(score[i + 1, j + 1], score[i1, j1] + (i - i1 - 1) + 1 + (j - j1 - 1));
                }

                sd[word1[i - 1]] = i;
            }

            double maxLength = Math.Max(word1.Length, word2.Length);

            return score[word1.Length + 1, word2.Length + 1] / maxLength;
        }


    }
}