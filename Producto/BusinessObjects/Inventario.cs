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
    /// BO que corresponde al maestro de inventarios
    /// </summary>
    [DefaultClassOptions, ModelDefault("Caption", "Inventario"), NavigationItem("Inventario"), DefaultProperty(nameof(Producto)), CreatableItem(false)]
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
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades
        EmpresaUnidad bodega;
        Producto.Module.BusinessObjects.Producto producto;
        ETipoMovimientoInventario tipoMovimiento = ETipoMovimientoInventario.Inicial;
        [Persistent(nameof(Cantidad))]
        decimal cantidad = 0.0m;


        public EmpresaUnidad Bodega
        {
            get => bodega;
            set => SetPropertyValue(nameof(Bodega), ref bodega, value);
        }

        public Producto.Module.BusinessObjects.Producto Producto
        {
            get => producto;
            set => SetPropertyValue(nameof(Producto), ref producto, value);
        }

        public ETipoMovimientoInventario TipoMovimiento
        {
            get => tipoMovimiento;
            set => SetPropertyValue(nameof(TipoMovimiento), ref tipoMovimiento, value);
        }

        [PersistentAlias(nameof(cantidad))]
        [ModelDefault("DisplayFormat", "{0:N2}")]
        public decimal Cantidad => cantidad;

        [PersistentAlias("Iif([TipoMovimiento] == 0, [Cantidad], 0)")]
        [ModelDefault("DisplayFormat", "{0:N2}")]
        public decimal Inicial => Convert.ToDecimal(EvaluateAlias(nameof(Inicial)));

        [PersistentAlias("Iif([TipoMovimiento] In (1, 2, 3), [Cantidad], 0)")]
        [ModelDefault("DisplayFormat", "{0:N2}")]
        public decimal Entrada => Convert.ToDecimal(EvaluateAlias(nameof(Entrada)));

        [PersistentAlias("Iif([TipoMovimiento] In (4, 5), [Cantidad], 0)")]
        [ModelDefault("DisplayFormat", "{0:N2}")]
        public decimal Salida => Convert.ToDecimal(Evaluate(nameof(Salida)));

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

    public enum ETipoMovimientoInventario
    {
        Inicial = 0,
        Compra = 1,
        Ingreso = 2,
        Devolucion = 3,
        Facturado = 4,
        Egreso = 5
    }
}