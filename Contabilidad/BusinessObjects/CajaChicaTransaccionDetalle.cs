﻿using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;

namespace SBT.Apps.Banco.Module.BusinessObjects
{
    /// <summary>
    /// Bancos. BO para el detalle de las transacciones de caja chica 
    /// </summary>
    [ModelDefault("Caption", "Caja Chica - Transacción Detalle"), NavigationItem(false), CreatableItem(false),
        Persistent(nameof(CajaChicaTransaccionDetalle)), DefaultProperty("Proveedor")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CajaChicaTransaccionDetalle : XPObjectCustom
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public CajaChicaTransaccionDetalle(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        CajaChicaTransaccion cajaChicaTransaccion;
        SBT.Apps.Tercero.Module.BusinessObjects.Tercero proveedor;
        DateTime fechaCompra = DateTime.Now;
        string numeroFactura;
        Listas tipoFactura;
        decimal cantidad;
        string concepto;
        decimal valorGravado;
        decimal iva;
        decimal renta;
        SBT.Apps.Contabilidad.BusinessObjects.Catalogo cuentaContable;

        [Association("CajaChicaTransaccion-Detalles"), XafDisplayName("Transacción"),
            Persistent("CajaChicaTransaccion")]
        public CajaChicaTransaccion CajaChicaTransaccion
        {
            get => cajaChicaTransaccion;
            set => SetPropertyValue(nameof(CajaChicaTransaccion), ref cajaChicaTransaccion, value);
        }


        [Persistent("Proveedor"), XafDisplayName("Proveedor")]
        public SBT.Apps.Tercero.Module.BusinessObjects.Tercero Proveedor
        {
            get => proveedor;
            set => SetPropertyValue(nameof(Proveedor), ref proveedor, value);
        }

        [DbType("datetime"), Persistent("FechaCompra"), XafDisplayName("Fecha Compra"),
            RuleRequiredField("CajaChicaTransaccionDetalle.FechaCompra_Requerido", "Save")]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime FechaCompra
        {
            get => fechaCompra;
            set => SetPropertyValue(nameof(FechaCompra), ref fechaCompra, value);
        }

        [Persistent("TipoFactura"), XafDisplayName("Tipo Factura")]
        [DataSourceCriteria("[Categoria] == 15 && [Categoria.Codigo] In ('COVE01', 'COVE02', 'COVE05', 'COVE10', 'COVE11')")]
        public Listas TipoFactura
        {
            get => tipoFactura;
            set => SetPropertyValue(nameof(TipoFactura), ref tipoFactura, value);
        }

        [Size(12), DbType("varchar(12)"), Persistent("NumeroFactura"), XafDisplayName("No Factura")]
        public string NumeroFactura
        {
            get => numeroFactura;
            set => SetPropertyValue(nameof(NumeroFactura), ref numeroFactura, value);
        }

        [DbType("numeric(8,2)"), Persistent("Cantidad"), XafDisplayName("Cantidad")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [RuleValueComparison("CajaChicaTransaccionDetalle.Cantidad > 0", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, SkipNullOrEmptyValues = false)]
        public decimal Cantidad
        {
            get => cantidad;
            set => SetPropertyValue(nameof(Cantidad), ref cantidad, value);
        }


        [Size(150), DbType("varchar(150)"), Persistent("Concepto"), XafDisplayName("Concepto")]
        public string Concepto
        {
            get => concepto;
            set => SetPropertyValue(nameof(Concepto), ref concepto, value);
        }

        [DbType("money"), Persistent("ValorGravado"), XafDisplayName("Valor Gravado")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [RuleValueComparison("CajaChicaTransaccionDetalle.ValorGravado > 0", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, SkipNullOrEmptyValues = false)]
        public decimal ValorGravado
        {
            get => valorGravado;
            set => SetPropertyValue(nameof(ValorGravado), ref valorGravado, value);
        }
        [DbType("money"), Persistent("Iva"), XafDisplayName("Iva")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [RuleValueComparison("CajaChicaTransaccionDetalle.Iva >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = false)]
        public decimal Iva
        {
            get => iva;
            set => SetPropertyValue(nameof(Iva), ref iva, value);
        }

        [DbType("money"), Persistent("Renta"), XafDisplayName("Renta")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [RuleValueComparison("CajaChicaTransaccionDetalle.Renta >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = false)]
        [ToolTip("Cuando son pagos a proveedores de servicios vía el fondo de caja chica, en este campo se guarda la información de la renta calculada y" +
            "descontada al proveedor por los servicios prestados")]
        public decimal Renta
        {
            get => renta;
            set => SetPropertyValue(nameof(Renta), ref renta, value);
        }

        /// FALTAN AQUI LAS COLUMNAS DE PRESUPUESTO Y CUENTA PRESUPUESTARIA
        [Persistent("CuentaContable"), XafDisplayName("Cuenta Contable"),
            ToolTip("Si se dispone del modulo de presupuestos, esta es la cuenta presupuestaria también")]
        public SBT.Apps.Contabilidad.BusinessObjects.Catalogo CuentaContable
        {
            get => cuentaContable;
            set => SetPropertyValue(nameof(CuentaContable), ref cuentaContable, value);
        }


        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}