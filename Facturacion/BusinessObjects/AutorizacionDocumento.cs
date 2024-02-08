using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;

namespace SBT.Apps.Facturacion.Module.BusinessObjects
{
    /// <summary>
    /// Facturacion
    /// BO que corresponde a las resoluciones de autorizacion de correlativos para la emision de documentos legales
    /// </summary>

    [DefaultClassOptions, ModelDefault("Caption", "Autorización Documentos"), NavigationItem("Facturación"),
        Persistent("FacAutorizacionDoc"), DefaultProperty("Resolucion")]
    [ImageName(nameof(AutorizacionDocumento))]
    [CreatableItem(false)]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class AutorizacionDocumento : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public AutorizacionDocumento(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            Activo = true;
        }

        #region Propiedades


        ReportDataV2 reporte;
        EClaseDocumento clase;
        EmpresaUnidad agencia;
        Caja caja;
        string noSolicitud;
        string resolucion;
        DateTime fecha;
        Listas tipo;
        SBT.Apps.Tercero.Module.BusinessObjects.Tercero imprenta;
        string serie;
        int noDesde;
        int noHasta;
        bool activo;


        [XafDisplayName("Agencia"), RuleRequiredField("ResCorrelativo.Agencia_Requerido", "Save"), Index(0)]
        [DataSourceCriteria("[Empresa.Oid] == EmpresaActualOid() And [Activa] = True And [Role] == 2")]
        public EmpresaUnidad Agencia
        {
            get => agencia;
            set => SetPropertyValue(nameof(Agencia), ref agencia, value);
        }

        [Association("Caja-Correlativos"), XafDisplayName("Caja No"), Index(1),
            RuleRequiredField("ResCorrelativo.Caja_Requerido", DefaultContexts.Save, TargetCriteria = "[Tipo.Codigo] in ('COVE05', 'DACV03')")]
        [DataSourceCriteria("[Agencia] == '@This.Agencia'")]
        public Caja Caja
        {
            get => caja;
            set => SetPropertyValue(nameof(Caja), ref caja, value);
        }

        [Size(20), DbType("varchar(20)"), XafDisplayName("No Solicitud"), Index(2), VisibleInListView(false)]
        public string NoSolicitud
        {
            get => noSolicitud;
            set => SetPropertyValue(nameof(NoSolicitud), ref noSolicitud, value);
        }
        [Size(20), DbType("varchar(30)"), XafDisplayName("No Resolución"), Index(3),
            RuleRequiredField("ResCorrelativo.Resolucion_Requerido", "Save")]
        public string Resolucion
        {
            get => resolucion;
            set => SetPropertyValue(nameof(Resolucion), ref resolucion, value);
        }
        [DbType("datetime2"), Index(4), RuleRequiredField("ResCorrelativo.Fecha_Requerido", DefaultContexts.Save)]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
        }

        [XafDisplayName("Tipo Documento"), DbType("varchar(12)"), Index(5), VisibleInLookupListView(true)]
        [DataSourceCriteria("[Categoria] In (15, 16) && Activo == True && [Codigo] != 'COVE10' && [Codigo] != 'COVE11' && [Codigo] != 'DACV03'")]
        public Listas Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        /// <summary>
        /// Seleccionar la imprenta es obligatoria, excepto cuando se trata de Ticket de Venta y Devolucion
        /// </summary>
        [XafDisplayName("Imprenta"), Index(6), VisibleInListView(false),
            RuleRequiredField("ResCorrelativo.Imprenta_Requerido", DefaultContexts.Save,
            TargetCriteria = "!([Tipo.Codigo] in ('COVE05', 'DACV03'))")]
        public SBT.Apps.Tercero.Module.BusinessObjects.Tercero Imprenta
        {
            get => imprenta;
            set => SetPropertyValue(nameof(Imprenta), ref imprenta, value);
        }

        [Size(12), DbType("varchar(12)"), XafDisplayName("Serie"), Index(7)]
        public string Serie
        {
            get => serie;
            set => SetPropertyValue(nameof(Serie), ref serie, value);
        }

        [DbType("int"), XafDisplayName("Número Desde"), Index(8)]
        public int NoDesde
        {
            get => noDesde;
            set => SetPropertyValue(nameof(NoDesde), ref noDesde, value);
        }

        [DbType("int"), XafDisplayName("Número Hasta"), Index(9)]
        public int NoHasta
        {
            get => noHasta;
            set => SetPropertyValue(nameof(NoHasta), ref noHasta, value);
        }

        [XafDisplayName("Clase Documento"), DbType("smallint")]
        public EClaseDocumento Clase
        {
            get => clase;
            set => SetPropertyValue(nameof(Clase), ref clase, value);
        }

        [DbType("bit"), XafDisplayName("Activo"), Index(10)]
        public bool Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }


        [XafDisplayName("Reporte")]
        public ReportDataV2 Reporte
        {
            get => reporte;
            set => SetPropertyValue(nameof(Reporte), ref reporte, value);
        }

        #endregion


        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}