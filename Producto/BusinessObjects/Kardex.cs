using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;

namespace SBT.Apps.Inventario.Module.BusinessObjects
{
    /// <summary>
    /// BO que corresponde al kardex o historial de movimientos del inventario
    /// </summary>
    /// <remarks>
    /// NOTA: Evaluar si la hacemos no persistente, solo para tener acceso al objeto porque hay datos
    /// que aun no existen en este Module, sino en otros (Ejemplo: No Venta, No Compra, etc)
    /// Implementar Interface para Ventas, Compras, para que por ese lado se pueda hacer referencia (evaluar)
    /// </remarks>
    [ModelDefault("Caption", "Kardex"), NavigationItem("Inventario"), CreatableItem(false)]
    [Persistent(nameof(Kardex))]
    [ImageName(nameof(Kardex))]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Kardex : XPCustomObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Kardex(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades
        object referencia;
        [Persistent(nameof(Oid)), DbType("bigint"), Key(true)]
        long oid = -1;
        EmpresaUnidad bodega;
        Producto.Module.BusinessObjects.Producto producto;
        DateTime fecha;
        InventarioTipoMovimiento tipoMovimiento;
        decimal cantidad;
        decimal costoUnidad;
        decimal precioUnidad;

        [PersistentAlias(nameof(oid)), XafDisplayName("Oid"), Browsable(false), Index(0)]
        public long Oid => oid;

        [XafDisplayName("Bodega"), Index(1)]
        public EmpresaUnidad Bodega
        {
            get => bodega;
            set => SetPropertyValue(nameof(Bodega), ref bodega, value);
        }

        [XafDisplayName("Producto"), Index(2)]
        public Producto.Module.BusinessObjects.Producto Producto
        {
            get => producto;
            set => SetPropertyValue(nameof(Producto), ref producto, value);
        }

        public DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
        }

        public InventarioTipoMovimiento TipoMovimiento
        {
            get => tipoMovimiento;
            set => SetPropertyValue(nameof(TipoMovimiento), ref tipoMovimiento, value);
        }

        /// <summary>
        /// Documento de referencia de la entrada o salida del producto
        /// Se tendra hacer casting al correspondiente tipo de documento para relacionarlo
        /// </summary>
        [DbType("bigint"), XafDisplayName("Referencia Ingreso")]
        [Indexed("TipoMovimiento", Name = "idxReferenciaTipoMovimiento_Kardex", Unique = true)]
        public object Referencia
        {
            get => referencia;
            set => SetPropertyValue(nameof(Referencia), ref referencia, value);
        }

        [DbType("numeric(12,2)")]
        public decimal Cantidad
        {
            get => cantidad;
            set => SetPropertyValue(nameof(Cantidad), ref cantidad, value);
        }

        [DbType("numeric(14,6)")]
        public decimal CostoUnidad
        {
            get => costoUnidad;
            set => SetPropertyValue(nameof(CostoUnidad), ref costoUnidad, value);
        }

        [DbType("numeric(14,4)")]
        public decimal PrecioUnidad
        {
            get => precioUnidad;
            set => SetPropertyValue(nameof(PrecioUnidad), ref precioUnidad, value);
        }

        #endregion


        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}