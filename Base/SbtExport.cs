using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SBT.Apps.Base.Module.BusinessObjects;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Xpo.Metadata.Helpers;
using System.Collections;
using System.IO;

namespace SBT.Apps.Base.Module
{
    public class SbtExport
    {
        private IDataLayer dal;
        private Type fBOClass;
        private List<ColumnDefinition> fProperties;

        public SbtExport(IDataLayer dataLayer)
        {
            dal = dataLayer;
            fProperties = new List<ColumnDefinition>();
            Separator = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            TextQualifier = "\"";
            ExportHeader = false;
        }

        public Type BOClass
        {
            get => fBOClass;
            set => fBOClass = value;
        }

        public List<ColumnDefinition> Properties
        {
            get => fProperties;
        }

        public EExportFormat Format
        {
            get;
            set;
        }

        public string FileName { get; set; }
        public string Separator { get; set; }
        public string TextQualifier { get; set; }
        public bool ExportHeader { get; set; }

        private string GetExportPropertiesNames()
        {
            string sResult = string.Empty;
            for (int idx = 0; idx < Properties.Count - 1; idx++)
                sResult += Properties[idx].PropertyName + ";";
            sResult += Properties[Properties.Count - 1].PropertyName;
            return sResult;
        }

        public MemoryStream Execute(CriteriaOperator criteria)
        {
            if (dal == null)
                throw new Exception("IDataLayer is null, can't create Session Object");
            if (string.IsNullOrEmpty(FileName))
                throw new Exception("The name of the output file is required in property FileName");
            using (Session ses = new Session(dal))
            {
                XPView viewData = new XPView(ses, BOClass.GetType(), GetExportPropertiesNames(), criteria);
                if (Format == EExportFormat.Csv || Format == EExportFormat.Text)
                    return DoExportCsv(viewData);
                else
                    return null;
            }
        }

        public MemoryStream Execute(CriteriaOperator criteria, string fileName = "")
        {
            if (!string.IsNullOrEmpty(fileName))
                FileName = fileName;
            return Execute(criteria);
        }

        private string CsvHeaderAdd()
        {
            if (!ExportHeader)
                return "";
            string sHeader = string.Empty;
            int x = Properties.Count;
            for (int c = 0; c < x - 1; c++)
            {
                sHeader += (string.IsNullOrEmpty(Properties[c].Caption)) ? Properties[c].PropertyName : Properties[c].Caption + Separator;
            }
            sHeader += (string.IsNullOrEmpty(Properties[x - 1].Caption)) ? Properties[x - 1].PropertyName : Properties[x - 1].Caption + Separator;
            return sHeader;
        }

        private string CsvRecordAdd(ViewRecord viewRecord)
        {
            string sRow = string.Empty;
            int x = Properties.Count;
            for (int idx = 0; idx <= x - 1; idx++)
            {
                object value;
                string sValue;
                ColumnDefinition colDef = Properties[idx];
                value = viewRecord[colDef.PropertyName];
                if (string.IsNullOrEmpty(colDef.Format))
                    sValue = colDef.Length == 0 ? Convert.ToString(value) : Convert.ToString(value).Substring(0, colDef.Length - 1);
                else
                    sValue = colDef.Length == 0 ? string.Format(colDef.Format, value) : string.Format(colDef.Format, value).Substring(0, colDef.Length - 1);
                sRow += string.Concat(sValue, idx < (x - 1) ? Separator : "");
            }
            return sRow;
        }


        private MemoryStream DoExportCsv(XPView view)
        {
            System.IO.StreamWriter output = new System.IO.StreamWriter(FileName, false, Encoding.UTF8);
            string sRow = string.Empty;
            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
            {
                streamWriter.WriteLine(CsvHeaderAdd());
                streamWriter.Flush();
                memoryStream.Position = 0;
                foreach (ViewRecord record in view)
                {
                    streamWriter.WriteLine(CsvRecordAdd(record));
                    streamWriter.Flush();
                    memoryStream.Position = 0;
                }
                return memoryStream;
            }
            //using (var csvWriter = new CsvWriter(streamWriter, csvConfig))
            //{
            //    return File(memoryStream, "text/csv", Filename);
            //}
        }
    }

    public enum EExportFormat
    {
        Csv = 0,
        Text = 1,
        Xml = 2
    }

    public class ColumnDefinition
    {
        public ColumnDefinition()
        {
            
        }

        public ColumnDefinition(SbtExport AOwner, string APropertyName)
        {
            Owner = AOwner;
            PropertyName = APropertyName;
        }

        public ColumnDefinition(SbtExport AOwner, string APropertyName, bool AExportEnumValue)
        {
            Owner = AOwner;
            PropertyName = APropertyName;
            fExportEnumValue = AExportEnumValue;
        }

        public ColumnDefinition(SbtExport AOwner, string APropertyName, string AFormat, int ALength = 0, string ACaption = "")
        {
            Owner = AOwner;
            PropertyName = APropertyName;
            Format = AFormat;
            Length = ALength;
            Caption = ACaption;
        }

        private SbtExport fOwner;
        private string fPropertyName;
        private bool fExportEnumValue;
        public SbtExport Owner 
        {   get => fOwner; 
            set
            {
                if (value != null)
                    fOwner = value;
                else
                {
                    throw new Exception("ExportXPObject not defined");
                }
            }
        }
        public int Length { get; set; }
        public string PropertyName 
        { 
            get => fPropertyName; 
            set
            {
                if (Owner.BOClass == null)
                    throw new Exception($"ExportObject.BObject is nulll. Can't Find a property {value}");
                if (Owner.BOClass.GetProperty(nameof(value)) != null)
                    fPropertyName = value;
                else
                {
                    throw new Exception($"Don't exists Property {value} in {Owner.BOClass.ToString()}");
                }
            }
        }

        public string Caption { get; set; }
        public int Index { get; set; }
        public string Format { get; set; }
        public bool ExportEnumValue 
        { 
            get => fExportEnumValue;
            set
            {
                if (value.GetType().IsEnum)
                    fExportEnumValue = value;
                else
                    fExportEnumValue = false;
            } 
        }


    }
}
