using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Compra.Module.BusinessObjects;
using System.ComponentModel;


namespace SBT.Apps.Inventario.Module.BusinessObjects
{
    [DefaultClassOptions, ModelDefault("Caption", "Inventario Movimiento"), NavigationItem("Inventario"),
        DefaultProperty(nameof(Numero)), Persistent(nameof(InventarioMovimiento))]
    [Appearance("InventarioMovimiento_IngresoDeCompra", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show,
        Criteria = "([TipoMovimiento.Codigo] In ('201', '202', '203'))", Context = "*", AppearanceItemType = "Any",
        TargetItems = "OrdenCompra;Factura")]
    [ImageName(nameof(InventarioMovimiento))]
    [CreatableItem(false)]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class InventarioMovimiento : XPCustomBaseDoc
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public InventarioMovimiento(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        EEstadoMovimientoInventario estado = EEstadoMovimientoInventario.Anulado;
        CompraFactura factura;
        OrdenCompra ordenCompra;
        InventarioTipoMovimiento tipoMovimiento;
        EmpresaUnidad bodega;
        [Persistent(nameof(Oid)), Key(true), DbType("bigint")]
        long oid = -1;

        [XafDisplayName("Oid"), PersistentAlias(nameof(oid))]
        public long Oid => oid;


        [XafDisplayName("Bodega")]
        public EmpresaUnidad Bodega
        {
            get => bodega;
            set => SetPropertyValue(nameof(Bodega), ref bodega, value);
        }


        [XafDisplayName("Tipo Movimiento")]
        public InventarioTipoMovimiento TipoMovimiento
        {
            get => tipoMovimiento;
            set => SetPropertyValue(nameof(TipoMovimiento), ref tipoMovimiento, value);
        }


        [Association("OrdenCompra-Ingresos"), XafDisplayName("Orden Compra")]
        [DataSourceCriteria("[Tipo] == 1 && [EstadoOrden] == 0 && [Aprobo] Is Not Null && [Detalles].Count() > 0 && [Facturas].Count() == 0")]
        public OrdenCompra OrdenCompra
        {
            get => ordenCompra;
            set => SetPropertyValue(nameof(OrdenCompra), ref ordenCompra, value);
        }

        [Association("CompraFactura-Ingresos"), XafDisplayName("Factura")]
        [DataSourceCriteria("[Tipo] == 1 && [Estado] In (0, 1) && [Detalles].Count() > 0 And [Ingresos].Count() == 0")]
        public CompraFactura Factura
        {
            get => factura;
            set => SetPropertyValue(nameof(Factura), ref factura, value);
        }

        [DbType("smallint"), XafDisplayName("Estado")]
        public EEstadoMovimientoInventario Estado
        {
            get => estado;
            set => SetPropertyValue(nameof(Estado), ref estado, value);
        }

        #endregion

        #region Colecciones
        [Association("InventarioMovimiento-Detalles"), DevExpress.Xpo.Aggregated, XafDisplayName("Detalles"), Index(0)]
        public XPCollection<InventarioMovimientoDetalle> Detalles => GetCollection<InventarioMovimientoDetalle>(nameof(Detalles));

        #endregion

        #region Metodos
        protected override void OnSaving()
        {
            if ((Session is not NestedUnitOfWork) && (Session.DataLayer != null) && Session.IsNewObject(this) &&
                (Session.ObjectLayer is SecuredSessionObjectLayer) && (Numero == null || Numero <= 0))
            {
                Numero = CorrelativoDoc();
            }
            base.OnSaving();            
        }

        protected override void OnSaved()
        {
            base.OnSaved();
            if (Oid <= 0)
                Session.Reload(this);
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }

    public enum EEstadoMovimientoInventario
    {
        Digitado = 0,
        Anulado = 2
    }
}