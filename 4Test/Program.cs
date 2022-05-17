// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportBatch.Models.RCSRequest;
using System.IO;
using Newtonsoft.Json;
using ExportBatch;


//var recognitionresult = new RCSRequest();
//var recognitionResultJson = JsonConvert.SerializeObject(recognitionresult);
//System.IO.File.WriteAllText("RequestModel.json", recognitionResultJson);

var batch = new QualityAnalysis() { };

//batch.Compare(@"C:\Users\pspet\Downloads\dta\RecognisedData1.json", @"C:\Users\pspet\Downloads\dta\VerifiedData1.json");

Console.ReadLine();
