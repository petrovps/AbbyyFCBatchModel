using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using ExportBatch.Models.Export;
using ExportBatch.Models.CompareResult;
using ABBYY.FlexiCapture;

namespace ExportBatch
{
    public class QualityAnalysis
    {
        public void Compare(IBatch batch, IProcessingCallback processing, string RecognisedDatafile = "RecognisedData", string VerifiedDatafile = "VerifiedData", string flag = "CompareResultSaved")
        {
            if (!batch.Properties.Has(flag))
            {

                if (!RecognisedDatafile.EndsWith(".json"))
                    RecognisedDatafile += ".json";
                if (!VerifiedDatafile.EndsWith(".json"))
                    VerifiedDatafile += ".json";

                if (!batch.Attachments.Has(RecognisedDatafile) && !batch.Attachments.Has(VerifiedDatafile))
                    processing.ReportWarning("Нет результатов для анализа.");
                else
                {

                    var RecognisedData = JsonConvert.DeserializeObject<Batch>(batch.Attachments.Get(RecognisedDatafile).AsString);
                    var VerifiedData = JsonConvert.DeserializeObject<Batch>(batch.Attachments.Get(VerifiedDatafile).AsString);

                    CRBatch cRBatch = new CRBatch(RecognisedData, VerifiedData);

                    batch.Properties.Set("BatchQuality", cRBatch.Quality.ToString());

                    foreach(var cDoc in cRBatch.Documents)
                    {
                        foreach(IDocument doc in batch.Documents)
                        {
                            //if (cDoc.Id != null)
                            //{
                            //    if (cDoc.Id.Equals(doc.Id))
                            //    {
                            //        doc.Properties.Set("DocumentQuality", cDoc.Quality.ToString());
                            //        break;
                            //    }
                            //}
                            if (cDoc.Name.Equals(doc.DefinitionName))
                            {
                                doc.Properties.Set("DocumentQuality", cDoc.Quality.ToString());
                                break;
                            }
                        }
                    }


                    string cRBatchJson = JsonConvert.SerializeObject(cRBatch);
                    // File.WriteAllText("CompareResult.json", JsonConvert.SerializeObject(cRBatch));


                    if (batch.Attachments.Has("CompareResult.json"))
                        batch.Attachments.Delete("CompareResult.json");


                    IUserAttachment attachment = batch.Attachments.AddNew("CompareResult.json");
                    attachment.AsString = cRBatchJson;
                    attachment.UploadAttachment();
                    batch.Properties.Set(flag, "");
                    processing.ReportMessage("Файл с результатами распознвания добавлен во вложение к пакету. CompareResult.json");
                }

            }
            else
                processing.ReportMessage("Пакет имеет признак завершенного ранее сравнения. (рег. параметр пакета: CompareResultSaved) ");

        }


    }
}
