using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Banco.Module.BusinessObjects
{
    /// <summary>
    /// Bancos
    /// BO que corresponde al detalle de las conciliaciones bancarias
    /// </summary>

    [ModelDefault("Caption", "Conciliación Detalle"), NavigationItem(false), DefaultProperty("Transaccion"), CreatableItem(false),
        Persistent(nameof(BancoConciliacionDetalle))]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class BancoConciliacionDetalle : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public BancoConciliacionDetalle(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        EConciliacionDetalleEstado estado = EConciliacionDetalleEstado.AplicadoYContabilizado;
        BancoTransaccion transaccion;
        BancoConciliacion conciliacion;

        [Association("BancoConciliacion-Detalles"), Index(0)]
        public BancoConciliacion Conciliacion
        {
            get => conciliacion;
            set => SetPropertyValue(nameof(Conciliacion), ref conciliacion, value);
        }

        [Association("BancoTransaccion-Conciliaciones"), XafDisplayName("Transacción Banco"), Index(1)]
        public BancoTransaccion Transaccion
        {
            get => transaccion;
            set => SetPropertyValue(nameof(Transaccion), ref transaccion, value);
        }          

        [DbType("smallint"), XafDisplayName("Estado"), Index(8)]
        public EConciliacionDetalleEstado Estado
        {
            get => estado;
            set => SetPropertyValue(nameof(Estado), ref estado, value);
        }


        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}