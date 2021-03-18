using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Tercero.Module.BusinessObjects
{
    /// <summary>
    /// Información Crediticia del Tercero. El diseño permite que se ingrese más de un registro por tercero, lo cual 
    /// facilita reflejar la historia crediticia del cliente, o diferentes montos de credito
    /// </summary>

    [ModelDefault("Caption", "Tercero Crédito"), NavigationItem(false), CreatableItem(false),
       DefaultProperty(nameof(FechaOtorgamiento)), Persistent(nameof(TerceroCredito))]
    [ImageName("credito")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class TerceroCredito : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public TerceroCredito(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades
        Empresa empresa;
        DateTime? fechaCancelacion;
        DateTime fechaOtorgamiento;
        bool vigente = true;
        string comentario;
        TerceroDireccion direccionCobro;
        int toleranciaPago;
        decimal limite;
        int diasCredito;
        string clasificacion;
        Tercero cliente;


        [XafDisplayName("Empresa"), Persistent(nameof(Empresa)), Browsable(false), Index(0)]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }

        [Association("Tercero-Creditos"), XafDisplayName("Cliente"), Index(0)]
        public Tercero Cliente
        {
            get => cliente;
            set => SetPropertyValue(nameof(Cliente), ref cliente, value);
        }

        /// <summary>
        /// Clasificacion del cliente cuando es necesario establecer una calificación para el otorgamiento de crédito
        /// </summary>
        [VisibleInListView(false), DbType("varchar(4)"), Index(1)]
        public System.String Clasificacion
        {
            get => clasificacion;
            set => SetPropertyValue(nameof(Clasificacion), ref clasificacion, value);
        }

        /// <summary>
        /// Fecha de otorgamiento del credito
        /// </summary>
        [DbType("datetime2"), XafDisplayName("Fecha Otorgamiento"), Index(2), VisibleInLookupListView(true)]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime FechaOtorgamiento
        {
            get => fechaOtorgamiento;
            set => SetPropertyValue(nameof(FechaOtorgamiento), ref fechaOtorgamiento, value);
        }

        /// <summary>
        /// Dias de credito otorgados por defecto al cliente
        /// </summary>
        [DbType("smallint"), XafDisplayName("Días Crédito"), Index(3), VisibleInLookupListView(true),
            RuleValueComparison("TerceroCredito.DiasCredito >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0,
            SkipNullOrEmptyValues = false)]
        public int DiasCredito
        {
            get => diasCredito;
            set => SetPropertyValue(nameof(DiasCredito), ref diasCredito, value);
        }

        /// <summary>
        /// Límite del credito otorgado al cliente
        /// </summary>
        [DbType("numeric(14,2)"), XafDisplayName("Límite Crédito"), Index(4), VisibleInLookupListView(true),
            RuleValueComparison("TerceroCredito.Limite >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0,
            SkipNullOrEmptyValues = false)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Limite
        {
            get => limite;
            set => SetPropertyValue(nameof(Limite), ref limite, value);
        }

        /// <summary>
        /// Dias Adicionales para esperar el pago del cliente
        /// </summary>
        [DbType("numeric(14,2)"), XafDisplayName("Tolerancia Pago"), Index(5), VisibleInListView(false),
            RuleValueComparison("TerceroCredito.ToleranciaPago >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0,
            SkipNullOrEmptyValues = true)]
        public int ToleranciaPago
        {
            get => toleranciaPago;
            set => SetPropertyValue(nameof(ToleranciaPago), ref toleranciaPago, value);
        }

        /// <summary>
        /// Direccion de cobro del cliente
        /// </summary>
        [XafDisplayName("Dirección Cobro"), Index(6), VisibleInListView(false)]
        [ExplicitLoading]
        public TerceroDireccion DireccionCobro
        {
            get => direccionCobro;
            set => SetPropertyValue(nameof(DireccionCobro), ref direccionCobro, value);
        }

        /// <summary>
        /// Fecha en la cual se cierra el credito otorgado al cliente
        /// </summary>
        [DbType("datetime2"), XafDisplayName("Fecha Cancelación"), Index(7)]
        [ToolTip("Fecha de cancelación del crédito al cliente")]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime? FechaCancelacion
        {
            get => fechaCancelacion;
            set => SetPropertyValue(nameof(FechaCancelacion), ref fechaCancelacion, value);
        }

        [Size(200), DbType("varchar(200)"), XafDisplayName("Comentario"), VisibleInListView(false), Index(8)]
        public string Comentario
        {
            get => comentario;
            set => SetPropertyValue(nameof(Comentario), ref comentario, value);
        }

        [DbType("bit"), XafDisplayName("Vigente"), RuleRequiredField("TerceroCredito.Vigente_Requerido", "Save"), Index(9)]
        public bool Vigente
        {
            get => vigente;
            set => SetPropertyValue(nameof(Vigente), ref vigente, value);
        }

        #endregion


        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}