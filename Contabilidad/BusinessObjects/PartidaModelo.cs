using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp.Editors;

namespace SBT.Apps.Contabilidad.Module.BusinessObjects
{
    /// <summary>
    /// Partida Modelo Encabezado. Son los datos generales de la partida modelo
    /// </summary>
    /// <remarks>
    /// <b>Comentario</b> Agregar la propiedad del BO para el cual se hace la partida modelo al encabezado y agregar filtro
    /// para los casos donde se va a generar una sola partida con el detalle para todos los registros que cumplen el filtro aplicado
    /// al BO que se esta contabilizando (para evitar implementar mecanismo de evaluación de los parámetros). 
    /// Ejemplo: una sola partida para todos los documentos de venta de un día, con el detale
    /// cada documento contabilizado. Quitar comentario, cuando se haya implementado y probado
    /// </remarks>

    [DefaultClassOptions, NavigationItem("Contabilidad"), ModelDefault("Caption", "Partidas Modelo"), DefaultProperty(nameof(Concepto)),
        Persistent("ConPartidaModelo"), CreatableItem(false)]
    [ImageName(nameof(PartidaModelo))]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class PartidaModelo : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public PartidaModelo(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            Tipo = ETipoPartida.Diario;
        }

        #region Propiedades

        ETipoPartidaModelo tipoModelo = ETipoPartidaModelo.Repetitiva;
        Empresa empresa;
        ETipoPartida tipo = ETipoPartida.Diario;
        int? presupuesto;
        string concepto;
        SqlObject consulta;
        private Type tipoBO;
        private string nombre;
        private string comentario;

        [DbType("int"), Persistent("Empresa"), XafDisplayName("Empresa"), Browsable(false)]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }

        [DbType("smallint"), Persistent("Tipo"), XafDisplayName("Tipo")]
        public ETipoPartida Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        /// <summary>
        /// Presupuesto que sera afectado con la transaccion contable
        /// </summary>
        /// <remarks>
        ///  Esta columna es por compatibilidad con sistema contable creado en delphi. Cuando se incorpore el modulo de presupuestos
        ///  debera cambiar el clasificacion para corresponder al BO de Presupuestos
        /// </remarks>
        [DbType("int"), Persistent("Presupuesto"), XafDisplayName("Presupuesto"), Browsable(false)]
        public int? Presupuesto
        {
            get => presupuesto;
            set => SetPropertyValue(nameof(Presupuesto), ref presupuesto, value);
        }

        [Size(150), DbType("varchar(150)"), Persistent("Concepto"), XafDisplayName("Concepto")]
        public string Concepto
        {
            get => concepto;
            set => SetPropertyValue(nameof(Concepto), ref concepto, value);
        }

        /// <summary>
        /// Tipo de la partida Modelo Repetitiva = 0, Modelo = 1, Preliminar = 2
        /// </summary>
        public ETipoPartidaModelo TipoModelo
        {
            get => tipoModelo;
            set => SetPropertyValue(nameof(TipoModelo), ref tipoModelo, value);
        }

        [Size(60), DbType("varchar(60)"), System.ComponentModel.DisplayName("Nombre")]
        [RuleRequiredField("PartidaModelo.Nombre_requerido", DefaultContexts.Save)]
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        [Size(200), DbType("varchar(200)"), System.ComponentModel.DisplayName("Comentario")]
        public string Comentario
        {
            get => comentario;
            set => SetPropertyValue(nameof(Comentario), ref comentario, value);
        }

        [DbType("int"), Persistent("Consulta"), XafDisplayName("Consulta"),
            ToolTip("Consulta que se debe ejecutar para obtener los datos para generar una partida automatica")]
        [RuleRequiredField("PartidaModelo.Consulta_Requerido", DefaultContexts.Save, SkipNullOrEmptyValues = true, TargetCriteria = "[TipoModelo] = 1")]
        public SqlObject Consulta
        {
            get => consulta;
            set => SetPropertyValue(nameof(Consulta), ref consulta, value);
        }

        #endregion

        #region Metodos
        /// <summary>
        /// Metodo para generar la partida contable a partir de la partida clasificacion o modelo seleccionada
        /// </summary>
        /// <param name="ParamNames"></param>
        /// <param name="ParamValues"></param>
        public void GenerarPartida(string[] ParamNames, object[] ParamValues)
        {
            // si no hay consulta, para obtener los datos de la fuente se sale
            if (Consulta == null)
                return;
            DevExpress.Xpo.DB.SelectedData data = Session.ExecuteQueryWithMetadata(Consulta.Ssql, ParamNames, ParamValues);
            // columnNames tendra los nombres de las columnas
            Dictionary<string, int> columnNames = new();
            for (int columnIndex = 0; columnIndex < data.ResultSet[0].Rows.Length; columnIndex++)
            {
                string columnName = (string)data.ResultSet[0].Rows[columnIndex].Values[0];
                columnNames.Add(columnName, columnIndex);
            }
            // ahora se itera por la lista de columnas
            foreach (DevExpress.Xpo.DB.SelectStatementResultRow row in data.ResultSet[1].Rows)
            {
                object firstName = row.Values[columnNames["FirstName"]];
                object lastName = row.Values[columnNames["LastName"]];
            }
        }

        private static XPClassInfo CreateParameterBO(SqlObject ASql)
        {
            if (ASql == null)
                return null;
            XPClassInfo info = XpoDefault.Dictionary.CreateClass(XpoDefault.Dictionary.QueryClassInfo(typeof(ParametroBO)), "PartidaParametro");
            info.AddAttribute(new NonPersistentAttribute());
            XPMemberInfo key = info.CreateMember("Oid", Type.GetType("System.Int16"));
            key.AddAttribute(new KeyAttribute(true));
            key.AddAttribute(new BrowsableAttribute(false));
            foreach (ConsultaParametro pa in ASql.Parametros)
            {
                string sTipo = $"System.{Enum.GetName(typeof(ETipoDato), pa.Tipo).Substring(1)}";
                XPMemberInfo mi = info.CreateMember(pa.Nombre, Type.GetType(sTipo));
                mi.AddAttribute(new DevExpress.Xpo.DisplayNameAttribute(pa.Descripcion));
            }
            return info;
        }
        #endregion

        #region Colecciones
        [Association("PartidaModelo-Detalles"), DevExpress.Xpo.Aggregated, XafDisplayName("Detalles")]
        public XPCollection<PartidaModeloDetalle> Detalles => GetCollection<PartidaModeloDetalle>(nameof(Detalles));

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}