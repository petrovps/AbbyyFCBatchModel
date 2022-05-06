using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportBatch.Models.ExportFlat
{
    public enum StatusCodes
    {
        [Description("OK")]
        OK = 0,
        [Description("Download from SHK problem")]
        SHKProblem = 500,
        [Description("Unknown document type")]
        UnknownDocumentType = 600,
        [Description("Incorrect image format")]
        IncorrectImageFormat = 601,
        [Description("Unknown error")]
        UnknownError = 700
    }
}
