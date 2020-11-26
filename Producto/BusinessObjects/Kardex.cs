using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Producto.Module.BusinessObjects;

namespace SBT.Apps.Inventario.Module.BusinessObjects
{
    /// <summary>
    /// BO que corresponde al kardex
    /// </summary>
    /// <remarks>
    /// NOTA: Evaluar si la hacemos no persistente, solo para tener acceso al objeto porque hay datos
    /// que aun no existen en este Module, sino en otros (Ejemplo: No Venta, No Compra, etc)
    /// Implementar Interface para Ventas, Compras, para que por ese lado se pueda hacer referencia (evaluar)
    /// </remarks>
    [ModelDefault("Caption", "Kardex"), NavigationItem("Inventario"), CreatableItem(false)]
    [Persistent(nameof(Kardex))]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Kardex : XPCustomBaseBO
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
        [Persistent(nameof(Oid)), DbType("bigint"), Key(true)]
        long oid;
        EmpresaUnidad bodega;
        Producto.Module.BusinessObjects.Producto producto;
        DateTime fecha;
        ETipoMovimientoInventario tipoMovimiento;
        decimal cantidad = 0.0m;
        decimal costoUnidad = 0.0m;
        decimal precioUnidad = 0.0m;

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

        public ETipoMovimientoInventario TipoMovimiento
        {
            get => tipoMovimiento;
            set => SetPropertyValue(nameof(TipoMovimiento), ref tipoMovimiento, value);
        }

        public decimal Cantidad
        {
            get => cantidad;
            set => SetPropertyValue(nameof(Cantidad), ref cantidad, value);
        }

        public decimal CostoUnidad
        {
            get => costoUnidad;
            set => SetPropertyValue(nameof(CostoUnidad), ref costoUnidad, value);
        }

        public decimal PrecioUnidad
        {
            get => precioUnidad;
            set => SetPropertyValue(nameof(PrecioUnidad), ref precioUnidad, value);
        }

        #endregion


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