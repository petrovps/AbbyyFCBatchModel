using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABBYY.FlexiCapture;
using Newtonsoft.Json;
using ExportBatch.Models.Export;
using System.IO;

namespace ExportBatch
{
    public class Export
    {
        public void Batch(IBatch batch, IProcessingCallback processing, string filename="RecognisedData", string flag= "DataSaved")
        {
            var recognitionresult = new Batch(batch);
            var recognitionResultJson = JsonConvert.SerializeObject(recognitionresult);
            if (!batch.Attachments.Has(filename + ".json") && !batch.Properties.Has(flag))
            {
                if (batch.Attachments.Has(filename + ".json"))
                    batch.Attachments.Delete(filename + ".json");

                IUserAttachment attachment = batch.Attachments.AddNew(filename + ".json");
                attachment.AsString = recognitionResultJson;
                attachment.UploadAttachment();
                batch.Properties.Set(flag, "");
                processing.ReportMessage("Файл с результатами распознвания добавлен во вложение к пакету."+ filename + ".json");
            }
            else
            {
                processing.ReportMessage($"Данные распознвания уже были сохранены ранее. Если требуется пересохранить, удалите параметр пакета: {flag} или файл-вложение");
            }

        }

       

    //    public void FileProcessed(IBatch batch, IProcessingCallback processing, string exportPath = @"C:/Temp", bool isError = false)
    //    {
    //        try
    //        {
    //            var id = batch.Properties.Get("ID");
    //            var processName = batch.Properties.Get("ProcessName");
    //            var taskId = batch.Properties.Get("TaskId");
              

    //            var status = GetStatusCode(batch);
    //            var recognitionResult = new Responce(batch, status);
    //            var recognitionResultJson = JsonConvert.SerializeObject(recognitionResult);

    //            System.IO.File.WriteAllText(Path.Combine(exportPath, batch.Name+".json"), recognitionResultJson);

    //        }
    //        catch (Exception ex)
    //        {
    //            processing.ReportError(ex.Message + "; "+ ex.StackTrace);
    //            if (ex.InnerException != null)
    //                processing.ReportError(ex.InnerException.Message); ;
    //        }
    //    }
    //    private StatusCodes GetStatusCode(IBatch batch)
    //    {
    //        if (batch.Documents.Count == 0)
    //            return StatusCodes.UnknownError;

    //        foreach (IDocument doc in batch.Documents)
    //            if (!string.IsNullOrEmpty(doc.DefinitionName) && !doc.DefinitionName.Equals("Unknown"))
    //                return StatusCodes.OK;

    //        return StatusCodes.UnknownDocumentType;
    //    }
    //
    }
}
