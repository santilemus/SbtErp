using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace SBT.Apps.Xml
{
    /// <summary>
    /// Implementa clase para exportar un datareader a xml
    /// Autor: Santiago Enrique Lemus Martínez
    /// Proyecto: Sistema de Pago de Garantías V2014
    /// Fecha: 26/mayo/2014
    /// </summary>
    public class XmlExport
    {
        private bool ValidacionPreviaOK(string Metodo)
        {
            if (DataReader == null)
                throw new NullReferenceException(this.ToString() + " Metodo: " + Metodo + Environment.NewLine + "Propiedad DataReader es nula");
            if (string.IsNullOrEmpty(FileName))
                throw new Exception(this.ToString() + " Metodo: " + Metodo + Environment.NewLine + "Propiedad FileName es nula o esta vacía");
            if (string.IsNullOrEmpty(RowElementName))
                throw new Exception(this.ToString() + " Metodo: " + Metodo + Environment.NewLine + "Propiedad RowElementName es nula o esta vacía");
            return true;
        }


        public string xmlns
        {
            get;
            set;
        }
        public SqlDataReader DataReader
        {
            get;
            set;
        }

        public string FileName
        {
            get;
            set;
        }

        public string RowElementName
        {
            get;
            set;
        }

        public string RootElement
        {
            get;
            set;
        }

        public string[] colNames
        {
            get;
            set;
        }

        /// <summary>
        /// Ejecuta el proceso de exportar el DataReader a formato xml
        /// </summary>
        /// <returns>La cantidad de RowElements (registros) exportados</returns>
        public int Execute()
        {
            if (!ValidacionPreviaOK("Execute"))
                return 0;
            XmlTextWriter oXml = new XmlTextWriter(FileName, Encoding.UTF8);
            
            oXml.Formatting = Formatting.Indented;
            oXml.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
            oXml.WriteStartElement(RootElement);
            oXml.WriteAttributeString("xmlns", xmlns);
            int iFilas = 0;
            while (DataReader.Read())
            {
                string sColName;
                string sValor;
                oXml.WriteStartElement(RowElementName);
                for (int idx = 0; idx < DataReader.FieldCount; idx ++)
                {
                    sColName = DataReader.GetName(idx);
                    if (colNames.Count() == 0 || colNames.Contains(sColName))
                    {
                        if (!DataReader.IsDBNull(idx))
                            sValor = (Type.GetTypeCode(DataReader.GetFieldType(idx)) != TypeCode.DateTime) ? Convert.ToString(DataReader.GetValue(idx)) :
                                string.Format("{0:dd/MM/yyyy}", Convert.ToString(DataReader.GetValue(idx)));
                        else
                            sValor = "";
                        //oXml.WriteElementString(sColName, !DataReader.IsDBNull(idx) ? Convert.ToString(DataReader.GetValue(idx)) : "");
                        oXml.WriteElementString(sColName, sValor);
                    }
                }
                oXml.WriteEndElement(); // cerrar rowElementName
                iFilas++;
            }
            oXml.WriteEndElement();     // cerrar RootElement
            oXml.Flush();
            oXml.Close();
            return iFilas;
        }

        /// <summary>
        /// Constructor de la clase para exportar a xml
        /// </summary>
        /// <param name="aXmlns">Corresponde al namespace que deberá agregarse al inicio del archivo xml</param>
        /// <param name="aRowElementName">El nombre del elemento o nodo que representa un registro</param>
        /// <param name="aFileName">El nombre del archivo xml a generar</param>
        public XmlExport(string aXmlns, string aRowElementName, string aFileName)
        {
            FileName = aFileName;
            xmlns = aXmlns;
            RowElementName = aRowElementName;
            colNames = new string[0] {};
        }
    }
}
