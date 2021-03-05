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

namespace SBT.Apps.Banco.Module.BusinessObjects
{
    /// <summary>
    /// Bancos
    /// BO que corresponde al detalle de las conciliaciones bancarias
    /// </summary>
    
    [ModelDefault("Caption", "Conciliación Detalle"), NavigationItem(false), DefaultProperty("Transaccion"), CreatableItem(false),
        Persistent("BanConciliacionDetalle")]
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
        decimal monto;
        string beneficiario;
        DateTime fecha;
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

        [PersistentAlias("Transaccion.NumeroPorTipo"), XafDisplayName("Número x Clasificación"), Index(2)]
        public int NumeroPorTipo
        {
            get { return Convert.ToInt32(EvaluateAlias(nameof(NumeroPorTipo))); }
        }

        [PersistentAlias("Transaccion.ChequeNo"), XafDisplayName("Cheque No"), Index(3)]
        public int ChequeNo
        {
            get { return Convert.ToInt32(EvaluateAlias(nameof(ChequeNo))); }
        }

        [DbType("datetime2"), XafDisplayName("Fecha"), Index(4), 
            RuleRequiredField("BancoConciliacionDetalle.Fecha_Requerido", DefaultContexts.Save)]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
        }

        [Size(150), DbType("varchar(150)"), XafDisplayName("Beneficiario"), Index(5)]
        public string Beneficiario
        {
            get => beneficiario;
            set => SetPropertyValue(nameof(Beneficiario), ref beneficiario, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Monto"), Index(6),
            RuleValueComparison("BancoConciliacionDetalle.Monto >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0,
            SkipNullOrEmptyValues = false)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Monto
        {
            get => monto;
            set => SetPropertyValue(nameof(Monto), ref monto, value);
        }

        [PersistentAlias("Transaccion.Clasificacion"), XafDisplayName("Clasificación"), Index(7)]
        public BancoClasificacionTransac Clasificacion
        {
            get { return (BancoClasificacionTransac)(EvaluateAlias(nameof(Clasificacion))); }
        }
        
        [DbType("smallint"), XafDisplayName("Estado"), Index(8)]
        public EConciliacionDetalleEstado Estado
        {
            get => estado;
            set => SetPropertyValue(nameof(Estado), ref estado, value);
        }


        #endregion

        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}