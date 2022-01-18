using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportBatch
{
    public class DocumentModel
    {
        public class Responce
        {
            public string ModelVersion { get; set; } = "2.0";
            public string TaskId { get; set; } = Guid.NewGuid().ToString();
            public Status Status { get; set; }
            public Batch Batch { get; set; }
        }
        public class Status
        {
            public int Code { get; set; } = 200;
            public string Description { get; set; } = "Задача обработана успешно";

        }
        public class Batch
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public List<Document> Documents { get; set; }
            //  public List<Parameter> Parameters { get; set; } // Регистрационные параметры
        }
        /// <summary>
        /// Регистрационный параметр
        /// </summary>
        public class Parameter
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }
        /// <summary>
        /// Документ
        /// </summary>
        public class Document
        {
            public string Name { get; set; }
            public string Id { get; set; }
            // public List<Field> Fields { get; set; }
            // public List<Group> Groups { get; set; }
            public List<Page> Pages { get; set; }
            public List<Section> Sections { get; set; }
        }
        /// <summary>
        /// Страница
        /// </summary>
        public class Page
        {
            public string ImageSource { get; set; }
            public int PageIndex { get; set; }
        }
        /// <summary>
        /// Раздел документа
        /// </summary>
        public class Section
        {
            public string Name { get; set; }
            public List<Field> Fields { get; set; }
            public List<Group> Groups { get; set; }
            public List<Table> Tables { get; set; }
        }
        /// <summary>
        /// Атрибут
        /// </summary>
        public class Field
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public string SuspiciousSymbols { get; set; }
            public List<Region> Regions { get; set; }
            public List<Field> Instances { get; set; } //Значения повторяющихся полей
        }
        /// <summary>
        /// Группа
        /// </summary>
        public class Group
        {
            public string Name { get; set; }
            public List<Field> Fields { get; set; }
            public List<Group> Groups { get; set; }
            public List<Table> Tables { get; set; }
            public List<Group> Instances { get; set; } //Значения повторяющихся групп
        }
        /// <summary>
        /// Таблица
        /// </summary>
        public class Table
        {
            public string Name { get; set; }
            public List<Row> Rows { get; set; }
        }
        /// <summary>
        /// Строка таблицы
        /// </summary>
        public class Row
        {
            public int Index { get; set; }
            public List<Column> Columns { get; set; }
        }
        /// <summary>
        /// Колонка таблицы
        /// </summary>
        public class Column
        {
            public string ColumnName { get; set; }
            public string Value { get; set; }
            public string SuspiciousSymbols { get; set; }
            public List<Region> Regions { get; set; }
        }
        /// <summary>
        /// Регион распознанного поля
        /// </summary>
        public class Region
        {
            public int PageIndex { get; set; }
            public List<Rect> Rects { get; set; }
        }
        /// <summary>
        /// Область распознанного региона
        /// </summary>
        public class Rect
        {
            public int Left { get; set; }
            public int Right { get; set; }
            public int Top { get; set; }
            public int Bottom { get; set; }
        }
    }
}
