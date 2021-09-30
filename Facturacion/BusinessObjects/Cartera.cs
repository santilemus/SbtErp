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
    [DefaultClassOptions, ModelDefault("Caption", "Cartera CxC"), NavigationItem("Cuenta por Cobrar"), 
        DefaultProperty(nameof(Nombre)), Persistent(nameof(Cartera))]
    [ImageName(nameof(Cartera))]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Cartera : XPObjectCustom
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Cartera(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        ETipoCartera tipo;
        bool activa = true;
        SBT.Apps.Empleado.Module.BusinessObjects.Empleado vendedor;
        string nombre;
        Empresa empresa;

        [XafDisplayName("Empresa"), Index(0), Browsable(false), Persistent(nameof(Empresa))]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }


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

        [XafDisplayName("Tipo Cartera"), DbType("smallint"), Index(3)]
        public ETipoCartera Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        [DbType("bit"), XafDisplayName("Activa"), Index(4), RuleRequiredField("Cartera.Activa_Requerido", DefaultContexts.Save)]
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

        [Association("Cartera-CxCTransacciones"), XafDisplayName("Transacciones"), Index(1)]
        public XPCollection<CxCTransaccion> CxCTransacciones => GetCollection<CxCTransaccion>(nameof(CxCTransacciones));

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }

    public enum ETipoCartera
    {
        Ventas = 0,
        Cobros = 1
    }
}