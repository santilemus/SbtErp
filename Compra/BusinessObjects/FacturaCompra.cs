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

namespace SBT.Apps.Compra.Module.BusinessObjects
{
    /// <summary>
    /// BO que corresponde a los encabezados de las facturas de compra
    /// </summary>
    [DefaultClassOptions, ModelDefault("Caption", "Factura"), NavigationItem("Compra"), CreatableItem(false)]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class FacturaCompra : XPCustomBaseDoc
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public FacturaCompra(Session session)
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
        string numeroDocumento;
        int? diasCredito;
        Listas condicionPago;
        Listas tipoFactura;
        ETipoCompra tipo = ETipoCompra.Servicio;
        Tercero.Module.BusinessObjects.Tercero proveedor;
        EOrigenCompra origen = EOrigenCompra.Local;
        OrdenCompra ordenCompra;
        [Persistent(nameof(Oid)), Key(true), DbType("bigint")]
        long oid = -1;

        /// <summary>
        /// Oid del BO. Es la llave primaria del registro
        /// </summary>
        [PersistentAlias(nameof(oid)), XafDisplayName("Oid"), Index(0)]
        public long Oid => oid;


        [Association("OrdenCompra-Facturas"), XafDisplayName("Orden Compra"), Index(5)]
        public OrdenCompra OrdenCompra
        {
            get => ordenCompra;
            set => SetPropertyValue(nameof(OrdenCompra), ref ordenCompra, value);
        }

        /// <summary>
        /// Proveedor al cual se gira la orden de compra
        /// </summary>
        [XafDisplayName("Proveedor"), RuleRequiredField("FacturaCompra.Proveedor_Requerido", DefaultContexts.Save), Index(6)]
        public Tercero.Module.BusinessObjects.Tercero Proveedor
        {
            get => proveedor;
            set => SetPropertyValue(nameof(Proveedor), ref proveedor, value);
        }

        /// <summary>
        /// Tipo de Compra. Puede ser: Servicio, Producto, ActivoFijo
        /// </summary>
        [DbType("smallint"), XafDisplayName("Tipo Compra"), RuleRequiredField("FacturaCompra.Tipo_Requerido", "Save"), Index(7)]
        public ETipoCompra Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        [DbType("smallint"), XafDisplayName("Origen Compra"), Index(8)]
        public EOrigenCompra Origen
        {
            get => origen;
            set => SetPropertyValue(nameof(Origen), ref origen, value);
        }

        /// <summary>
        /// Tipo de Factura que el proveedor debe emitir para la orden de compra
        /// </summary>
        [XafDisplayName("Tipo Factura"), RuleRequiredField("FacturaCompra.TipoFactura_Requerido", DefaultContexts.Save)]
        [DataSourceCriteria("[Categoria] == 15 And [Activo] == True"), VisibleInLookupListView(true), Index(9)]
        public Listas TipoFactura
        {
            get => tipoFactura;
            set => SetPropertyValue(nameof(TipoFactura), ref tipoFactura, value);
        }

        [Size(20), DbType("varchar(20)"), XafDisplayName("No Documento"), Index(10)]
        public string NumeroDocumento
        {
            get => numeroDocumento;
            set => SetPropertyValue(nameof(NumeroDocumento), ref numeroDocumento, value);
        }


        /// <summary>
        /// Condicion de Pago. Es obligatorio cuando se trata de Credito Fiscal o Factura Consumidor Final
        /// </summary>
        [XafDisplayName("Condición Pago"), RuleRequiredField("FacturaCompra.CondicionPago_Requerido", "Save",
            TargetCriteria = "@This.Tipo.Codigo In ('COVE01', 'COVE02')")]
        [DataSourceCriteria("[Categoria] == 17 And [Activo] == True")]   // Categoria = 17 es condicion de pago
        [VisibleInListView(false), Index(11)]
        public Listas CondicionPago
        {
            get => condicionPago;
            set => SetPropertyValue(nameof(CondicionPago), ref condicionPago, value);
        }

        /// <summary>
        /// Dias de crédito, cuando la condicion de pago es al crédito
        /// </summary>
        [DbType("smallint"), XafDisplayName("Días Crédito"), Index(12), VisibleInLookupListView(false)]
        public int? DiasCredito
        {
            get => diasCredito;
            set => SetPropertyValue(nameof(DiasCredito), ref diasCredito, value);
        }

        /// <summary>
        /// Concepto de la compra
        /// </summary>
        [Size(200), DbType("varchar(200)"), XafDisplayName("Concepto"), Index(10)]
        public string Concepto
        {
            get => concepto;
            set => SetPropertyValue(nameof(Concepto), ref concepto, value);
        }

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