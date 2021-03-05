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
using SBT.Apps.Tercero.Module.BusinessObjects;

namespace SBT.Apps.CxC.Module.BusinessObjects
{
    /// <summary>
    /// Cuenta por Cobrar. BO que corresponde a los clientes de una cartera
    /// </summary>
    [ModelDefault("Caption", "Cartera Clientes"), NavigationItem(false), CreatableItem(false), 
        DefaultProperty(nameof(Cliente)), Persistent("CxCCarteraCliente")]
    [ImageName(nameof(CarteraCliente))]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CarteraCliente : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public CarteraCliente(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        SBT.Apps.Tercero.Module.BusinessObjects.Tercero cliente;
        Cartera cartera;

        [Association("Cartera-Clientes"), XafDisplayName("Cartera"), Index(0)]
        public Cartera Cartera
        {
            get => cartera;
            set => SetPropertyValue(nameof(Cartera), ref cartera, value);
        }
      
        [XafDisplayName("Cliente"), Index(1), RuleRequiredField("CarteraCliente.Cliente_Requerido", DefaultContexts.Save)]
        public SBT.Apps.Tercero.Module.BusinessObjects.Tercero Cliente
        {
            get => cliente;
            set => SetPropertyValue(nameof(Cliente), ref cliente, value);
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}