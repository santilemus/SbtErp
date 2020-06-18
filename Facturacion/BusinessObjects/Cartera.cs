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
using SBT.Apps.Empleado.Module.BusinessObjects;

namespace SBT.Apps.CxC.Module.BusinessObjects
{
    /// <summary>
    /// Cuenta por Cobrar. BO para la definicion de las carteras de cuentas por cobrar
    /// </summary>
    [DefaultClassOptions, ModelDefault("Caption", "Cartera CxC"), NavigationItem("Cuentas por Cobrar"), 
        DefaultProperty(nameof(Nombre)), Persistent("CxCCartera")]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Cartera : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Cartera(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            empresa = EmpresaDeSesion();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        bool activa = true;
        SBT.Apps.Empleado.Module.BusinessObjects.Empleado vendedor;
        string nombre;
        [Persistent(nameof(Empresa))]
        Empresa empresa;

        [XafDisplayName("Empresa"), Index(0), Browsable(false), PersistentAlias(nameof(empresa))]
        public Empresa Empresa => empresa;


        [Size(80), DbType("varchar(80)"), XafDisplayName("Nombre"), RuleRequiredField("Cartera.Nombre_Requerido", DefaultContexts.Save),
            Index(1)]
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        /// <summary>
        /// Vendedor responsable de la cartera. Pendiente de evaluar si va aca o en el detalle
        /// </summary>
        [XafDisplayName("Vendedor"), Index(2), RuleRequiredField("Cartera.Vendedor_Requerido", DefaultContexts.Save)]
        [ToolTip("Vendedor responsable de la cartera")]
        [VisibleInLookupListView(true)]
        public SBT.Apps.Empleado.Module.BusinessObjects.Empleado Vendedor
        {
            get => vendedor;
            set => SetPropertyValue(nameof(Vendedor), ref vendedor, value);
        }

        [DbType("bit"), XafDisplayName("Activa"), Index(3), RuleRequiredField("Cartera.Activa_Requerido", DefaultContexts.Save)]
        public bool Activa
        {
            get => activa;
            set => SetPropertyValue(nameof(Activa), ref activa, value);
        }

        #endregion

        #region Colecciones
        [Association("Cartera-Clientes"), DevExpress.Xpo.Aggregated, Index(0), XafDisplayName("Clientes")]
        public XPCollection<CarteraCliente> Clientes
        {
            get
            {
                return GetCollection<CarteraCliente>(nameof(Clientes));
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