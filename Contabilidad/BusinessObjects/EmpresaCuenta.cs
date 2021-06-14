using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Contabilidad.BusinessObjects;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    [DefaultClassOptions, ModelDefault("Caption", "Empresa Cuenta Liquidación"), NavigationItem("Contabilidad"), CreatableItem(false)]
    [DefaultProperty(nameof(TipoCuenta)), Persistent(nameof(EmpresaCuenta))]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class EmpresaCuenta : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public EmpresaCuenta(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        string concepto;
        Catalogo cuenta;
        ETipoEmpresaCuenta tipoCuenta;
        Empresa empresa;

        [XafDisplayName("Empresa")]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }

        [XafDisplayName("Tipo Cuenta"), RuleRequiredField("EmpresaCuenta.Tipo_Requerido", "Save")]
        public ETipoEmpresaCuenta TipoCuenta
        {
            get => tipoCuenta;
            set => SetPropertyValue(nameof(TipoCuenta), ref tipoCuenta, value);
        }

        [XafDisplayName("Cuenta"), RuleRequiredField("EmpresaCuenta.Cuenta_Requerido", "Save")]
        public Catalogo Cuenta
        {
            get => cuenta;
            set => SetPropertyValue(nameof(Cuenta), ref cuenta, value);
        }

        
        [Size(150), DbType("varchar(150)"), XafDisplayName("Concepto")]
        public string Concepto
        {
            get => concepto;
            set => SetPropertyValue(nameof(Concepto), ref concepto, value);
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }

    public enum ETipoEmpresaCuenta
    {
        [XafDisplayName("Capital Social")]
        CapitalSocial = 0,
        [XafDisplayName("Liquidación")]
        Liquidacion = 1,
        [XafDisplayName("Reserva Legal del Ejercicio")]
        ReservaLegalEjercicio = 2,
        [XafDisplayName("Reserva Legal Ejercicios Anteriores")]
        ReservaLegalAnterior = 3,
        [XafDisplayName("Renta a Pagar")]
        RentaPagar = 4,
        [XafDisplayName("Utilidad del Ejercicio")]
        UtilidadEjercicio = 5,
        [XafDisplayName("Pérdida del Ejercicio")]
        PerdidaEjercicio = 6
    }
}