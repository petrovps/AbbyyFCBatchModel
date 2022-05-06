using Newtonsoft.Json;
using System;
using System.ComponentModel;
using ABBYY.FlexiCapture;

namespace ExportBatch.Models.ExportFlat
{
    public class ResponceStatus
    {

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        public ResponceStatus(StatusCodes code)
        {
            Code = (int)code;
            Description = code.GetDescription();
        }

        public ResponceStatus(int code, string description)
        {
            Code = code;
            Description = description;
        }


    }
    static class EnumExtensions
    {
        public static string GetDescription(this Enum enumValue)
        {
            return ((DescriptionAttribute)Attribute
                .GetCustomAttribute(enumValue.GetType()
                .GetField(enumValue.ToString()), typeof(DescriptionAttribute))).Description;
        }
    }
}
