using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Data;

namespace SBT.Apps.Xml
{
    /// <summary>
    /// Implementar un DataReader que proceso los datos de un archivo Xml y los guarda en una tabla de la base de datos
    /// Autor: Santiago Enrique Lemus Martínez
    /// Fecha: 04/02/2014
    /// Proyecto: Sistema de Pago de Garantías V2014
    /// </summary>
    public class XmlDataReader : IDataReader
    {
        private readonly string m_rowElementName;

        private readonly XmlReader m_xmlReader;
        private readonly int m_fieldCount = -1;
        private List<string> fColumns;   // lista de columnas (xml Element) que nos interesan procesar

        private bool m_disposed;

        protected IEnumerator<XElement> m_enumerator;
        private int _oridinalPosition = -1;


        public List<string> Columns
        {
            get { return fColumns; }
            set { fColumns = value; }
        }

        public object GetValue(int i)
        {
            IEnumerable<XElement> elements = from el in CurrentElement.Elements()
                                                 where el.Name.LocalName == fColumns[i]
                                              select el;
//            IList<XElement> lista = elements.ToList();
            if (!elements.First().IsEmpty)
                return elements.First().Value;
            else
                return DBNull.Value;
        }

        /// <summary>
        /// Inicializa el XmlDataStreamer. Después de la inicialización llamar Read() para desplazar el reader adelante.
        /// </summary>
        /// <param name="xmlReader">XmlReader uado para iterar en los datos. Será disposed cuando se complete.</param>
        /// <param name="fieldCount">IDataReader FiledCount.</param>
        /// <param name="rowElementName">Nombre del elemento XML que contiene la fila de datos</param>
        public XmlDataReader(XmlReader xmlReader, int fieldCount, string rowElementName)
        {
            m_rowElementName = rowElementName;
            m_fieldCount = fieldCount;
            m_xmlReader = xmlReader;
            m_enumerator = GetXmlStream().GetEnumerator();
        }

        public bool Read()
        {
            return m_enumerator.MoveNext();
        }

        public int FieldCount
        {
            get { return m_fieldCount; }
        }

        public XElement CurrentElement
        {
            get { return m_enumerator.Current; }
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/system.xml.linq.xstreamingelement.aspx
        /// </summary>
        /// <param name="m_xmlReader"></param>
        /// <returns></returns>
        private IEnumerable<XElement> GetXmlStream()
        {
            XElement rowElement;
            using (m_xmlReader)
            {
                m_xmlReader.MoveToContent();

                while (m_xmlReader.Read())
                {
                    if (IsRowElement())
                    {
                        rowElement = XElement.ReadFrom(m_xmlReader) as XElement;
                        if (rowElement != null)
                        {
                            yield return rowElement;
                        }
                    }
                }
            }
        }

        private bool IsRowElement()
        {
            if (m_xmlReader.NodeType != XmlNodeType.Element)
                return false;

            return m_xmlReader.Name == m_rowElementName;
        }

        public void Dispose()
        {
            if (m_disposed)
                return;

            m_enumerator.Dispose();
            m_disposed = true;

            Dispose();
            GC.SuppressFinalize(this);
        }


        #region Members not required by SqlBulkCopy


        public bool NextResult()
        {
            throw new NotImplementedException();
        }

        public int RecordsAffected
        {
            get { throw new NotImplementedException(); }
        }

        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public int Depth
        {
            get { throw new NotImplementedException(); }
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public bool IsClosed
        {
            get { throw new NotImplementedException(); }
        }

        public bool GetBoolean(int i)
        {
            return Convert.ToBoolean(GetValue(i));
        }

        public byte GetByte(int i)
        {
            return Convert.ToByte(GetValue(i));
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            return Convert.ToChar(GetValue(i));
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int i)
        {
            return Convert.ToDateTime(GetValue(i));
        }

        public decimal GetDecimal(int i)
        {
            return Convert.ToDecimal(GetValue(i));
        }

        public double GetDouble(int i)
        {
            return Convert.ToDouble(GetValue(i));
        }

        public Type GetFieldType(int i)
        {
            throw new NotImplementedException();
        }

        public float GetFloat(int i)
        {
            return Convert.ToSingle(GetValue(i));
        }

        public Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        public short GetInt16(int i)
        {
            return Convert.ToInt16(GetValue(i));
        }

        public int GetInt32(int i)
        {
            return Convert.ToInt32(GetValue(i));
        }

        public long GetInt64(int i)
        {
            return Convert.ToInt64(GetValue(i));
        }

        public string GetName(int i)
        {
            throw new NotImplementedException();
        }

        public int GetOrdinal(string name)
        {
            _oridinalPosition++;
            return _oridinalPosition;
        }

        public string GetString(int i)
        {
            return Convert.ToString(GetValue(i));
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull(int i)
        {
            return String.IsNullOrEmpty( Convert.ToString(GetValue(i)));
        }

        public object this[string name]
        {
            get { throw new NotImplementedException(); }
        }

        public object this[int i]
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

    }
}
