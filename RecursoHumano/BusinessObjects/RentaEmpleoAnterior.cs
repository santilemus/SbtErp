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

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    [DefaultClassOptions, ModelDefault("Caption", "Renta Empleo Anterior"), NavigationItem("Recurso Humano"),
        CreatableItem(false), DefaultProperty(nameof(RazonSocial)), Persistent(nameof(RentaEmpleoAnterior))]
    [ImageName(nameof(RentaEmpleoAnterior))]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class RentaEmpleoAnterior : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public RentaEmpleoAnterior(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        [Persistent(nameof(ValorMoneda)), DbType("numeric(14,2)")]
        decimal valorMoneda;
        Moneda moneda;
        decimal rentaRetenida;
        decimal ingresoGravado;
        DateTime fechaHasta;
        string razonSocial;
        string nitPatrono;
        SBT.Apps.Empleado.Module.BusinessObjects.Empleado empleado;

        [XafDisplayName("Empleado"), RuleRequiredField("RentaEmpleadoAnterior.Empleado_Requerido", "Save"), Index(0)]
        public SBT.Apps.Empleado.Module.BusinessObjects.Empleado Empleado
        {
            get => empleado;
            set => SetPropertyValue(nameof(Empleado), ref empleado, value);
        }

        [Size(14), DbType("varchar(14)"), XafDisplayName("Nit Patrono"), Index(1),
            RuleRequiredField("RentaEmpleoAnterior.NitPatrono_Requerido", "Save")]
        public string NitPatrono
        {
            get => nitPatrono;
            set => SetPropertyValue(nameof(NitPatrono), ref nitPatrono, value);
        }


        [Size(100), DbType("varchar(100)"), XafDisplayName("Razón Social"), Index(2),
            RuleRequiredField("RentaEmpleadoAnterior.RazonSocial", "Save")]
        public string RazonSocial
        {
            get => razonSocial;
            set => SetPropertyValue(nameof(RazonSocial), ref razonSocial, value);
        }

        [DbType("datetime2"), XafDisplayName("Fecha Hasta"), Index(3),
            RuleRequiredField("RentaEmpleadoAnterior.FechaHasta_Requerido", "Save")]
        public DateTime FechaHasta
        {
            get => fechaHasta;
            set => SetPropertyValue(nameof(FechaHasta), ref fechaHasta, value);
        }


        [Size(3), DbType("varchar(3)"), XafDisplayName("Moneda"), Index(4), 
            RuleRequiredField("RentaEmpleoAnterior.Moneda_Requerido", "Save")]
        public Moneda Moneda
        {
            get => moneda;
            set
            {
                bool changed = SetPropertyValue(nameof(Moneda), ref moneda, value);
                if (!IsLoading && !IsSaving && changed)
                    valorMoneda = Moneda.FactorCambio;
            }
        }

        [PersistentAlias(nameof(valorMoneda)), XafDisplayName("Valor Moneda"), Index(5)]
        public decimal ValorMoneda => valorMoneda;

        [DbType("numeric(14,2)"), XafDisplayName("Ingreso Gravado"), Index(6),
            RuleRequiredField("RentaEmpleadoAnterior.IngresoGravado_Requrido", "Save")]
        public decimal IngresoGravado
        {
            get => ingresoGravado;
            set => SetPropertyValue(nameof(IngresoGravado), ref ingresoGravado, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Renta Retenida"), Index(7),
            RuleRequiredField("RentaEmpleoAnterior.RentaRetenida_Requerido", "Save")]
        public decimal RentaRetenida
        {
            get => rentaRetenida;
            set => SetPropertyValue(nameof(RentaRetenida), ref rentaRetenida, value);
        }
        #endregion


        #region Metodos

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