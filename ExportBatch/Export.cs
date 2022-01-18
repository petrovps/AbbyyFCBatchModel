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
            var EPages = new List<DocumentModel.Page>();
            foreach (IPage page in Document.Pages)
                EPages.Add(MakePage(page));
            EDocument.Pages = EPages.Where(item => item != null).ToList();

            var ESections = new List<DocumentModel.Section>();
            foreach (IField Section in Document.Sections)
                ESections.Add(MakeSection(Section));
            EDocument.Sections = ESections.Where(item => item != null).ToList();
            return EDocument;
        }
        private static DocumentModel.Page MakePage(IPage Page)
        {
            DocumentModel.Page EPage = new DocumentModel.Page();
            EPage.ImageSource = Page.ImageSource;
            EPage.PageIndex = Page.ImageSourcePageNumber;
            return EPage;
        }
        private static DocumentModel.Section MakeSection(IField section)
        {
            DocumentModel.Section Section = new DocumentModel.Section();
            Section.Name = section.Name;
            #region Fields
            var FieldList = new List<DocumentModel.Field>();
            foreach (IField field in section.Children)
                FieldList.Add(MakeField(field));
            Section.Fields = FieldList.Where(item => item != null).ToList();
            #endregion Fields
            #region Groups
            var GroupList = new List<DocumentModel.Group>();
            foreach (IField field in section.Children)
                GroupList.Add(MakeGroup(field));
            Section.Groups = GroupList.Where(item => item != null).ToList();
            #endregion Groups
            #region Tables
            var TableList = new List<DocumentModel.Table>();
            foreach (IField field in section.Children)
                TableList.Add(MakeTable(field));
            Section.Tables = TableList.Where(item => item != null).ToList();
            #endregion Tables
            return Section;
        }
        private static DocumentModel.Field MakeField(IField Field)
        {
            DocumentModel.Field EField = new DocumentModel.Field();
            if (Field.Type == TExportFieldType.EFT_Checkmark || Field.Type == TExportFieldType.EFT_CurrencyField || Field.Type == TExportFieldType.EFT_DateTimeField || Field.Type == TExportFieldType.EFT_NumberField || Field.Type == TExportFieldType.EFT_TextField || Field.Type == TExportFieldType.EFT_TimeField)
            {
                EField.Name = Field.Name;
                if (!HasItems(Field)) // если поле не содержит повторяющихся элементов (обычное поле)
                {
                   
                    EField.Value = Field.Text;
                    EField.SuspiciousSymbols = Field.SuspiciousSymbols;
                    EField.Regions = MakeRegions(Field);

                }
                else
                {
                    var FieldList = new List<DocumentModel.Field>();
                    foreach (IField Items in Field.Items)
                    {
                        DocumentModel.Field field = new DocumentModel.Field();
                        field.Name = Items.Name;
                        field.Value = Items.Text;
                        field.SuspiciousSymbols = Items.SuspiciousSymbols;
                        field.Regions = MakeRegions(Items);
                        FieldList.Add(field);
                    }
                    EField.Instances = FieldList.Where(item => item != null).ToList();
                }
                return EField;
            }

            return null;
        }

        private static List<DocumentModel.Region> MakeRegions(IField Field)
        {
            List<DocumentModel.Region> ERegions = new List<DocumentModel.Region>();
            foreach (IFieldRegion _region in Field.Regions)
            {
                DocumentModel.Region region = new DocumentModel.Region();
                region.PageIndex = _region.PageIndex;
                List<DocumentModel.Rect> ERects = new List<DocumentModel.Rect>();
                foreach(IRect _rect in _region.Rects)
                {
                    DocumentModel.Rect rect = new DocumentModel.Rect();
                    rect.Bottom = _rect.Bottom;
                    rect.Left = _rect.Left;
                    rect.Right = _rect.Right;
                    rect.Top = _rect.Top;
                    ERects.Add(rect);
                }
                region.Rects=ERects.Where(item => item != null).ToList();

                ERegions.Add(region);
            }
          
            return ERegions;
        }





        private static DocumentModel.Group MakeGroup(IField Field)
        {
            DocumentModel.Group EGroup = new DocumentModel.Group();
            if (Field.Type == TExportFieldType.EFT_Group)
            {
                EGroup.Name = Field.Name;
                if (!HasItems(Field)) // Проверяем что группа не содержит повторяющиеся элементы (обычная группа)
                {
                    #region Fields
                    var FieldList = new List<DocumentModel.Field>();
                    foreach (IField field in Field.Children)
                        FieldList.Add(MakeField(field));
                    EGroup.Fields = FieldList.Where(item => item != null).ToList(); ;
                    #endregion Fields
                    #region Group
                    var GroupList = new List<DocumentModel.Group>();
                    foreach (IField field in Field.Children)
                        GroupList.Add(MakeGroup(field));
                    EGroup.Groups = GroupList.Where(item => item != null).ToList();
                    #endregion Group
                    #region Tables
                    var TableList = new List<DocumentModel.Table>();
                    foreach (IField field in Field.Children)
                        TableList.Add(MakeTable(field));
                    EGroup.Tables = TableList.Where(item => item != null).ToList();
                    #endregion Tables
                  
                }
                else
                {
                    var InstancesList = new List<DocumentModel.Group>();
                    foreach(IField Instance in Field.Items)
                    {
                        InstancesList.Add(MakeGroup(Instance));
                    }
                    EGroup.Instances = InstancesList.Where(item => item != null).ToList();
                }


                return EGroup;

            }
            return null;
        }
        private static DocumentModel.Table MakeTable(IField Field)
        {
            DocumentModel.Table ETable = new DocumentModel.Table();
            if (Field.Type == TExportFieldType.EFT_Table)
            {
                ETable.Name = Field.Name;
                var RowList = new List<DocumentModel.Row>();
                if (Field.Rows.Count > 0)
                {
                    int rowcount = 0;
                    foreach (IField Row in Field.Rows)
                    {
                        RowList.Add(MakeRow(Row, rowcount));
                        rowcount++;
                    }
                    ETable.Rows = RowList.Where(item => item != null).ToList(); ;
                }
                return ETable;
            }
            return null;
        }
        private static DocumentModel.Row MakeRow(IField Row, int index)
        {
            DocumentModel.Row ERow = new DocumentModel.Row();
            ERow.Index = index;
            var ColumnList = new List<DocumentModel.Column>();
            foreach (IField Column in Row.Children)
            {
                ColumnList.Add(MakeColumn(Column));
            }
            ERow.Columns = ColumnList;
            return ERow;
        }
        private static DocumentModel.Column MakeColumn(IField Column)
        {
            DocumentModel.Column EColumn = new DocumentModel.Column();
            EColumn.ColumnName = Column.Name;
            EColumn.Value = Column.Text;
            EColumn.SuspiciousSymbols = Column.SuspiciousSymbols;
             EColumn.Regions = MakeRegions(Column);

            return EColumn;
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
