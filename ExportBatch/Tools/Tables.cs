using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABBYY.FlexiCapture;

namespace ExportBatch.Tools
{
    public class Tables
    {
        public void RemoveEmptyRows(IBatch batch, IProcessingCallback processing)
        {
            foreach (IDocument doc in batch.Documents)
            {
                foreach (IField section in doc.Sections)
                {
                    foreach (IField field in section.Children)
                    {
                        // processing.ReportMessage($"field.Name = {field.Name}, field.Type = {field.Type}");
                        CheckTable(field, processing);
                    }
                }
            }
        }
        private static void CheckTable(IField field, IProcessingCallback processing)
        {
            if (field.Type == TExportFieldType.EFT_Table)
            {
                // foreach (IField row in field.Rows)
                var EmptyRows = new List<int>();
                for (int i = 0; i < field.Rows.Count; i++)
                {
                    bool RemoveRow = true;
                    foreach (IField f in field.Rows[i].Children)
                    {
                       // processing.ReportMessage($"field.Name = {f.Name}, field.Type = {f.Type}");
                        if (!string.IsNullOrWhiteSpace(f.Text))
                            RemoveRow = false;
                    }
                    if(RemoveRow)
                        EmptyRows.Add(i);
                }

                processing.ReportMessage($"Путсых строк: {EmptyRows.Count};");
                processing.ReportMessage($"Путсыt строки: {string.Join(",", EmptyRows)};");

                foreach (int i in EmptyRows)
                {
                    field.Rows.Delete(field.Rows[i]);
                    processing.ReportMessage($"Строка {i} Удалена;");
                }
                   
            }
        }
    }
}
