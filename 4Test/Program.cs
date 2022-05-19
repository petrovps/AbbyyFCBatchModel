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
using ExportBatch.Models.CompareResult;
using ExportBatch.Models.Export;

//var recognitionresult = new RCSRequest();
//var recognitionResultJson = JsonConvert.SerializeObject(recognitionresult);
//System.IO.File.WriteAllText("RequestModel.json", recognitionResultJson);

//var batch = new QualityAnalysis() { };
var recognysed = JsonConvert.DeserializeObject<Batch>(File.ReadAllText(@"D:\GIT\AbbyyFCBatchModel\4Test\bin\Debug\net6.0\RecognisedData.json"));
var verified = JsonConvert.DeserializeObject<Batch>(File.ReadAllText(@"D:\GIT\AbbyyFCBatchModel\4Test\bin\Debug\net6.0\VerifiedData.json"));


CRBatch p = new CRBatch(recognysed, verified);
File.WriteAllText("compareresult.json",JsonConvert.SerializeObject(p));
Console.ReadLine();
