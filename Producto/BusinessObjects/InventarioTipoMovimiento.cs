using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Inventario.Module.BusinessObjects
{
    /// <summary>
    /// Tipos de Movimientos de Inventario. Contiene la tipificacion de los movimentos del inventario
    /// </summary>
    /// 
    [DefaultClassOptions, ModelDefault("Caption", "Tipo Movimiento Inventario"), NavigationItem("Inventario"), CreatableItem(false)]
    [DefaultProperty(nameof(Nombre))]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class InventarioTipoMovimiento : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public InventarioTipoMovimiento(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        bool activo = true;
        string codigo;
        ETipoOperacionInventario operacion = ETipoOperacionInventario.Entrada;
        InventarioTipoMovimiento padre;
        string nombre;

        [Size(60), DbType("varchar(60)"), Persistent(nameof(Nombre))]
        [RuleRequiredField("InventarioTipoMovimiento.Nombre_Requerido", "Save")]
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }


        [Association("InventarioTipoMovimiento-TipoMovimientos"), XafDisplayName("Tipo Movimiento Padre"),
            Persistent(nameof(Padre))]
        public InventarioTipoMovimiento Padre
        {
            get => padre;
            set => SetPropertyValue(nameof(Padre), ref padre, value);
        }

        [XafDisplayName("Tipo Operación"), DbType("smallint"), RuleRequiredField("InventarioTipoMovimiento.Operacion_Requerido", DefaultContexts.Save)]
        public ETipoOperacionInventario Operacion
        {
            get => operacion;
            set => SetPropertyValue(nameof(Operacion), ref operacion, value);
        }


        [Size(8), DbType("varchar(8)"), Persistent(nameof(Codigo)), XafDisplayName("Código")]
        [RuleRequiredField("InventarioTipoMovimiento.Codigo_Requerido", "Save")]
        public string Codigo
        {
            get => codigo;
            set => SetPropertyValue(nameof(Codigo), ref codigo, value);
        }

        [DbType("bit"), XafDisplayName("Activo"), Persistent(nameof(Activo))]
        [RuleRequiredField("InventarioTipoMovimiento.Activo_Requrido", DefaultContexts.Save)]
        public bool Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }

        #endregion

        #region Colecciones
        [Association("InventarioTipoMovimiento-TipoMovimientos"), XafDisplayName("Tipo Movimientos")]
        public XPCollection<InventarioTipoMovimiento> TipoMovimientos => GetCollection<InventarioTipoMovimiento>(nameof(TipoMovimientos));
        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }



    public enum ETipoOperacionInventario
    {
        Inicial = 0,
        Entrada = 1,
        Salida = 2
    }
}