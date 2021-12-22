using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABBYY.FlexiCapture;
using Newtonsoft.Json;
using System.IO;


namespace ExportBatch
{
    /// <summary>
    /// TO DO: 
    /// Нужно сделать проверку на пустую таблицу! иначе падает!
    /// </summary>
    public class Export
    {
        public static void MakeJson(IBatch Batch, IDocuments Documents, IProcessingCallback Processing)
        {
            try
            {
                File.WriteAllText("C:/Temp/" + Batch.Name + ".json", JsonConvert.SerializeObject(MakeResponce(Batch, Documents)));
            }
            catch (Exception e)
            { Processing.ReportError(e.Message + "; " + e.StackTrace); }

        }
        public static DocumentModel.Responce MakeResponce(IBatch Batch, IDocuments Documents)
        {
            DocumentModel.Responce responce = new DocumentModel.Responce();
            DocumentModel.Status status = new DocumentModel.Status();
            responce.Batch = MakeBatch(Batch, Documents);
            if (Batch.Equals(null))
            {
                status.Code = 500;
                status.Description = "Формирование резульата распознвания завершилось ошибкой";
            }
            responce.Status = status;
            return responce;
        }
        private static DocumentModel.Batch MakeBatch(IBatch Batch, IDocuments Documents)
        {
            DocumentModel.Batch EBatch = new DocumentModel.Batch();
            EBatch.Id = Batch.Id;
            EBatch.Name = Batch.Name;
            if (Documents.Count > 0)
            {
                List<DocumentModel.Document> Docs = new List<DocumentModel.Document>();
                foreach (IDocument Document in Documents)
                    Docs.Add(MakeDocument(Document));
                EBatch.Documents = Docs.Where(item => item != null).ToList();
            }
            return EBatch;
        }
        private static DocumentModel.Document MakeDocument(IDocument Document)
        {
            DocumentModel.Document EDocument = new DocumentModel.Document();
            EDocument.Name = Document.DefinitionName;
            EDocument.Id = Document.Id;

            var FieldList = new List<DocumentModel.Field>();
            foreach (IField field in Document.Sections[0].Children)
                FieldList.Add(MakeField(field));
            EDocument.Fields = FieldList.Where(item => item != null).ToList();
            var GroupList = new List<DocumentModel.Group>();
            foreach (IField field in Document.Sections[0].Children)
                GroupList.Add(MakeGroup(field));
            EDocument.Groups = GroupList.Where(item => item != null).ToList();
            var InstanceList = new List<DocumentModel.RepeatableInstance>();
            foreach (IField field in Document.Sections[0].Children)
                InstanceList.Add(MakeRepeatableInstance(field));
            EDocument.RepeatableInstances = InstanceList.Where(item => item != null).ToList();
            var EPages = new List<DocumentModel.Page>();
            foreach (IPage page in Document.Pages)
                EPages.Add(MakePage(page));
            EDocument.Pages = EPages.Where(item => item != null).ToList();
            return EDocument;
        }
        private static DocumentModel.Page MakePage(IPage Page)
        {
            DocumentModel.Page EPage = new DocumentModel.Page();
            EPage.ImageSource = Page.ImageSource;
            EPage.PageIndex = Page.ImageSourcePageNumber;
            return EPage;
        }
        private static DocumentModel.Field MakeField(IField Field)
        {

            DocumentModel.Field EField = new DocumentModel.Field();
            if (!HasItems(Field))
            {
                if (Field.Type != TExportFieldType.EFT_Table || Field.Type != TExportFieldType.EFT_Group)
                {
                    EField.Name = Field.Name;
                    EField.Value = Field.Text;
                    EField.SuspiciousSymbols = Field.SuspiciousSymbols;
                    return EField;
                }
            }

            return null;
        }
        private static DocumentModel.Group MakeGroup(IField Field)
        {
            DocumentModel.Group EGroup = new DocumentModel.Group();
            if (Field.Type == TExportFieldType.EFT_Group)
            {
                if (!HasItems(Field))
                {
                    var FieldList = new List<DocumentModel.Field>();
                    foreach (IField field in Field.Children)
                        FieldList.Add(MakeField(field));
                    EGroup.Fields = FieldList;
                    var GroupList = new List<DocumentModel.Group>();
                    foreach (IField field in Field.Children)
                        GroupList.Add(MakeGroup(field));
                    EGroup.Groups = GroupList.Where(item => item != null).ToList();
                    var InstanceList = new List<DocumentModel.RepeatableInstance>();
                    foreach (IField field in Field.Children)
                        InstanceList.Add(MakeRepeatableInstance(field));
                    EGroup.RepeatableInstances = InstanceList.Where(item => item != null).ToList();
                    return EGroup;
                }
            }
            return null;
        }
        private static DocumentModel.RepeatableInstance MakeRepeatableInstance(IField Field)
        {
            DocumentModel.RepeatableInstance ERepeatableInstance = new DocumentModel.RepeatableInstance();
            ERepeatableInstance.Name = Field.Name;
            if (Field.Type == TExportFieldType.EFT_Group || Field.Type == TExportFieldType.EFT_Table)
            {
                if (HasItems(Field))
                {
                    List<DocumentModel.Instance> EInstances = new List<DocumentModel.Instance>();
                    for (int i = 0; i < Field.Items.Count; i++)
                    {
                        EInstances.Add(MakeInstance(Field.Items[i].Children));
                    }

                    ERepeatableInstance.Instances = EInstances;
                    return ERepeatableInstance;
                }
            }
            return null;
        }
        private static DocumentModel.Instance MakeInstance(IFields Fields)
        {
            DocumentModel.Instance EInstance = new DocumentModel.Instance();
            var FieldList = new List<DocumentModel.Field>();
            foreach (IField field in Fields)
                FieldList.Add(MakeField(field));
            EInstance.Fields = FieldList;
            var GroupList = new List<DocumentModel.Group>();
            foreach (IField field in Fields)
                GroupList.Add(MakeGroup(field));
            EInstance.Groups = GroupList.Where(item => item != null).ToList();

            var RepeatableInstanceList = new List<DocumentModel.RepeatableInstance>();
            foreach (IField field in Fields)
                RepeatableInstanceList.Add(MakeRepeatableInstance(field));
            EInstance.RepeatableInstances = RepeatableInstanceList.Where(item => item != null).ToList();

            return EInstance;
        }
        private static bool HasItems(IField Field)
        {
            bool IsRepeatable = false;
            try
            {
                if (Field.Items.Count > 0)
                    IsRepeatable = true;

            }
            catch (NullReferenceException e)
            {
                IsRepeatable = false;
            }

            return IsRepeatable;
        }
    }
}
