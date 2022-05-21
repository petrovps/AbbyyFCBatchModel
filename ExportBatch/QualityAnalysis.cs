using System;
using System.Collections.Generic;
using Newtonsoft.Json;
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




                    for(int i = 0; i< cRBatch.Documents.Count; i++)
                    {
                        if (!batch.Properties.Has(cRBatch.Documents[i].Name))
                            batch.Properties.Set($"Quality_{cRBatch.Documents[i].Name}", cRBatch.Documents[i].Quality.ToString());
                        else
                            batch.Properties.Set($"Quality_{cRBatch.Documents[i].Name}_[{i}]", cRBatch.Documents[i].Quality.ToString());
                    }
                    string cRBatchJson = JsonConvert.SerializeObject(cRBatch);

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


        /// <summary>
        /// на случай, если нужно поменять имя поля
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="processing"></param>
        /// <param name="attachmentname"></param>
        /// <param name=""></param>
        public void ChangeFieldName(IBatch batch, IProcessingCallback processing, string attachmentname, string DocName, string OldFieldName, string NewFieldName)
        {
            if (!attachmentname.EndsWith(".json"))
                attachmentname += ".json";

            if (!batch.Attachments.Has(attachmentname))
            {
                processing.ReportWarning($"Нет файла вложения {attachmentname}.");
                return;
            }

            var RecognisedData = JsonConvert.DeserializeObject<Batch>(batch.Attachments.Get(attachmentname).AsString);
            foreach (Document document in RecognisedData.Documents)
            {
                if(document.Name == DocName)
                {
                    foreach(Section Section in document.Sections)
                    {
                        foreach(Field field in Section.Fields)
                        {
                            if(field.Name == OldFieldName)
                                field.Name = NewFieldName;
                            break;
                        }
                    }
                }
            }

            if (batch.Attachments.Has(attachmentname))
                batch.Attachments.Delete(attachmentname);

            string RecognisedDataJson = JsonConvert.SerializeObject(RecognisedData);
            IUserAttachment attachment = batch.Attachments.AddNew(attachmentname);
            attachment.AsString = RecognisedDataJson;
            attachment.UploadAttachment();

        }
    }
}
