using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.CxC.Module.BusinessObjects;
using System;
using System.ComponentModel;

namespace SBT.Apps.Facturacion.Module.BusinessObjects
{
    [DefaultClassOptions, System.ComponentModel.DisplayName("Documentos Anulados"), CreatableItem(false), 
        NavigationItem("Contabilidad"), DefaultProperty(nameof(Venta))]
    [Persistent(@"vIvaDocumentosAnulados")]

    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class DocumentosAnulados : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public DocumentosAnulados(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            oid = -1;
            empresa = null;
            venta = null;
            transaccion = null;
            resolucion = string.Empty;
            clase = 1;
            preImpresoDesde = string.Empty;
            preImpresoHasta = string.Empty;
            tipoDocumento = string.Empty;
            tipoDetalle = string.Empty;
            serie = string.Empty;
            fecha = null;
            hasta = null;
            desde = null;

            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        [Key]
        [DbType("int"), Persistent(nameof(Oid))]
        int oid;
        [DbType("int"), Persistent(nameof(Empresa)), NoForeignKey, FetchOnly]
        Empresa empresa;
        [DbType("DateTime"), Persistent(nameof(Fecha)), FetchOnly]
        DateTime ? fecha;
        [DbType("bigint"), Persistent(nameof(Venta)), NoForeignKey, FetchOnly]
        Venta venta;
        [DbType("int"), Persistent(nameof(Transaccion)), FetchOnly]
        CxCTransaccion transaccion;
        [DbType("varchar(19)"), Persistent(nameof(Resolucion)), FetchOnly]
        string resolucion;
        [DbType("int"), Persistent(nameof(Clase)), FetchOnly]
        int clase;
        [DbType("varchar(9)"), Persistent(nameof(PreImpresoDesde)), FetchOnly]
        string preImpresoDesde;
        [DbType("varchar(9)"), Persistent(nameof(PreImpresoHasta)), FetchOnly]
        string preImpresoHasta;
        [DbType("varchar(2)"), Persistent(nameof(TipoDocumento)), FetchOnly]
        string tipoDocumento;
        [DbType("varchar(3)"), Persistent(nameof(TipoDetalle)), FetchOnly]
        string tipoDetalle;
        [DbType("varchar(13)"), Persistent(nameof(Serie)), FetchOnly]
        string serie;
        [DbType("int"), Persistent(nameof(Desde)), FetchOnly]
        int ? desde;
        [DbType("int"), Persistent(nameof(Hasta)), FetchOnly]
        int ? hasta;

        [PersistentAlias(nameof(oid)), System.ComponentModel.DisplayName("Oid")]
        [VisibleInListView(false)]
        public int Oid => oid;

        [PersistentAlias(nameof(empresa)), System.ComponentModel.DisplayName("Empresa")]
        [VisibleInListView(false)]
        public Empresa Empresa => empresa;

        [PersistentAlias(nameof(fecha)), System.ComponentModel.DisplayName("Fecha")]
        public DateTime? Fecha => fecha;

        [PersistentAlias(nameof(venta)), System.ComponentModel.DisplayName("Venta")]
        [VisibleInListView(false)]
        public Venta Venta => venta;

        [PersistentAlias(nameof(transaccion)), System.ComponentModel.DisplayName("Transacción")]
        [VisibleInListView(false)]
        public CxCTransaccion Transaccion => transaccion;
      
        [PersistentAlias(nameof(resolucion)), System.ComponentModel.DisplayName("Resolución")]
        public string Resolucion => resolucion;

        [PersistentAlias(nameof(clase)), System.ComponentModel.DisplayName("Clase")]
        public int Clase => clase;

        [PersistentAlias(nameof(preImpresoDesde)), System.ComponentModel.DisplayName("PreImpreso Desde")]
        public string PreImpresoDesde => preImpresoDesde;

        [PersistentAlias(nameof(preImpresoHasta)), System.ComponentModel.DisplayName("PreImpreso Hasta")]
        public string PreImpresoHasta => preImpresoHasta;

        [PersistentAlias(nameof(tipoDocumento)), System.ComponentModel.DisplayName("Tipo Documento")]
        public string TipoDocumento => tipoDocumento;

        [PersistentAlias(nameof(tipoDetalle)), System.ComponentModel.DisplayName("Tipo Detalle")]
        public string TipoDetalle => tipoDetalle;

        [PersistentAlias(nameof(serie)), System.ComponentModel.DisplayName("Serie")]
        public string Serie => serie;

        [PersistentAlias(nameof(desde)), System.ComponentModel.DisplayName("Desde")]
        public int? Desde => desde;

        [PersistentAlias(nameof(hasta)), System.ComponentModel.DisplayName("Hasta")]
        public int? Hasta => hasta;

        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue(nameof(PersistentProperty), ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}