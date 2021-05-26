using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Facturacion.Module.BusinessObjects
{
    /// <summary>
    /// Facturacion
    /// BO que corresponde a las cajas o estaciones de facturacion. Pueden ser las mismas cajas de cobro
    /// </summary>

    [DefaultClassOptions, ModelDefault("Caption", "Caja"), NavigationItem("Facturación"), DefaultProperty(nameof(NoCaja))]
    [Persistent("FacCaja")]
    [ImageName(nameof(Caja))]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Caja : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Caja(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades


        int noCaja;
        string descripcion;
        EmpresaUnidad agencia;
        string noSerie;
        string noAutorizacion;
        DateTime fechaAutorizacion;
        bool activa;
        string comentario;

        [DbType("int"), Persistent(nameof(NoCaja)), XafDisplayName("No Caja"), Index(0)]
        public int NoCaja
        {
            get => noCaja;
            set => SetPropertyValue(nameof(NoCaja), ref noCaja, value);
        }

        
        [Size(60), DbType("varchar(60)"), XafDisplayName("Descripción"), Index(1), VisibleInListView(false)]
        public string Descripcion
        {
            get => descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref descripcion, value);
        }

        [XafDisplayName("Sucursal"), Persistent(nameof(Agencia)), Index(2), VisibleInLookupListView(true)]
        [DataSourceCriteria("[Empresa.Oid] = EmpresaActualOid() And [Activa] == True And [Role] == 2")]
        public EmpresaUnidad Agencia
        {
            get => agencia;
            set => SetPropertyValue(nameof(Agencia), ref agencia, value);
        }
        [Size(25), DbType("varchar(25)"), Persistent(nameof(NoSerie)), XafDisplayName("No Serie"), Index(3), VisibleInListView(false)]
        public string NoSerie
        {
            get => noSerie;
            set => SetPropertyValue(nameof(NoSerie), ref noSerie, value);
        }
        [Size(25), DbType("varchar(25)"), Persistent(nameof(NoAutorizacion)), XafDisplayName("No Autorización"), Index(4), 
            VisibleInLookupListView(true)]
        public string NoAutorizacion
        {
            get => noAutorizacion;
            set => SetPropertyValue(nameof(NoAutorizacion), ref noAutorizacion, value);
        }
        [DbType("datetime2"), XafDisplayName("Fecha Autorización"), Persistent(nameof(FechaAutorizacion)), Index(5)]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime FechaAutorizacion
        {
            get => fechaAutorizacion;
            set => SetPropertyValue(nameof(FechaAutorizacion), ref fechaAutorizacion, value);
        }

        [DbType("bit"), XafDisplayName("Activa"), Persistent(nameof(Activa)), Index(6)]
        public bool Activa
        {
            get => activa;
            set => SetPropertyValue(nameof(Activa), ref activa, value);
        }

        [Size(100), DbType("varchar(100)"), XafDisplayName("Comentario"), Persistent(nameof(Comentario)), Index(7), 
            VisibleInListView(false)]
        public string Comentario
        {
            get => comentario;
            set => SetPropertyValue(nameof(Comentario), ref comentario, value);
        }
        #endregion

        #region Colecciones
        [Association("Caja-Correlativos"), XafDisplayName("Correlativos"), Index(0)]
        public XPCollection<AutorizacionDocumento> Autorizaciones
        {
            get
            {
                return GetCollection<AutorizacionDocumento>(nameof(Autorizaciones));
            }
        }

        [Association("Caja-Facturas"), DevExpress.Xpo.Aggregated, XafDisplayName("Facturas"), Index(1)]
        public XPCollection<Venta> Ventas
        {
            get
            {
                return GetCollection<Venta>(nameof(Ventas));
            }
        }
        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}