using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Inventario.Module.BusinessObjects
{
    /// <summary>
    /// BO que corresponde al maestro de inventarios
    /// </summary>
    [ModelDefault("Caption", "Inventario"), NavigationItem("Inventario"), DefaultProperty(nameof(Producto)), CreatableItem(false)]
    [Persistent(nameof(Inventario))]
    //[ImageName("BO_Contact")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Inventario : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Inventario(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            cantidad = 0.0m;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades
        EmpresaUnidad bodega;
        Producto.Module.BusinessObjects.Producto producto;
        InventarioTipoMovimiento tipoMovimiento;
        decimal cantidad;


        public EmpresaUnidad Bodega
        {
            get => bodega;
            set => SetPropertyValue(nameof(Bodega), ref bodega, value);
        }

        /// <summary>
        /// Producto al cual corresponde el inventario
        /// </summary>
        [Association("Producto-Inventarios"), XafDisplayName("Producto")]
        public Producto.Module.BusinessObjects.Producto Producto
        {
            get => producto;
            set => SetPropertyValue(nameof(Producto), ref producto, value);
        }

        public InventarioTipoMovimiento TipoMovimiento
        {
            get => tipoMovimiento;
            set => SetPropertyValue(nameof(TipoMovimiento), ref tipoMovimiento, value);
        }

        [Persistent(nameof(Cantidad)), DbType("numeric(12,2)")]
        [ModelDefault("DisplayFormat", "{0:N2}"), Browsable(false)]
        public decimal Cantidad
        {
            get => cantidad;
            set => SetPropertyValue(nameof(Cantidad), ref cantidad, value);
        }

        [PersistentAlias("Iif([TipoMovimiento.Operacion] == 0, [Cantidad], 0)")]
        [ModelDefault("DisplayFormat", "{0:N2}")]
        public decimal Inicial => Convert.ToDecimal(EvaluateAlias(nameof(Inicial)));

        [PersistentAlias("Iif([TipoMovimiento.Operacion] == 1, [Cantidad], 0)")]
        [ModelDefault("DisplayFormat", "{0:N2}")]
        public decimal Entrada => Convert.ToDecimal(EvaluateAlias(nameof(Entrada)));

        [PersistentAlias("Iif([TipoMovimiento.OPeracion] == 2, [Cantidad], 0)")]
        [ModelDefault("DisplayFormat", "{0:N2}")]
        public decimal Salida => Convert.ToDecimal(Evaluate(nameof(Salida)));

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}