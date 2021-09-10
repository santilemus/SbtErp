using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Compra.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;


namespace SBT.Apps.CxP.Module.BusinessObjects
{
    /// <summary>
    /// BO que corresponde al encabezado de las transacciones de cuentas por pagar. Como hay transacciones que tienen detalle
    /// y que corresponden a documentos como Notas de Credito, esas deberan heredar los BO de este; pero deben guardarse en
    /// la base de datos en la misma tabla para minimizar el complejo del modelo de datos
    /// También hay transacciones como los pagos a los proveedores que pueden tener atributos particulares o necesitan
    /// definirse en una clase aparte para facilitar su implementación. En esos casos siempre deben guardarse en la misma
    /// tabla de la base de datos.
    /// </summary>
    /// <remarks>
    /// Se debera crear un BO heredado en el ErpModule para incorporar la propiedad BancoTransaccion cuando se trata de pagos
    /// a los proveedores
    /// </remarks>

    [DefaultClassOptions, ModelDefault("Caption", "Transacción CxP"), CreatableItem(false), NavigationItem("Compras")]
    [DefaultProperty(nameof(Numero)), Persistent(nameof(CxPTransaccion))]
    //[ImageName("BO_Contact")]
    [RuleCriteria("CxPTransaccion No Anulado", "Save;Delete", "[Estado] != 2", "El estado de la Transacción no debe ser Anulado")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CxPTransaccion : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public CxPTransaccion(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            monto = 0.0m;
            Fecha = DateTime.Now;
            Estado = ECxPTransaccionEstado.Digitado;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        #region Propiedades

        int ? numero;
        Empresa empresa;
        decimal valorMoneda;
        Moneda moneda;
        CompraFactura factura;
        string usuarioAnulo;
        DateTime fechaAnula;
        string comentario;
        ECxPTransaccionEstado estado;
        decimal monto;
        CxCTipoTransaccion tipo;
        DateTime fecha;


        [XafDisplayName("Empresa"), Browsable(false), Index(0)]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }

        [DbType("datetime2"), XafDisplayName("Fecha"), Index(1)]
        [RuleRequiredField("CxPTransaccion.Fecha_Requerido", DefaultContexts.Save)]
        public DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
        }

        //[Association("CxCTipoTransaccion-CxPTransaccioness")]
        [XafDisplayName("Tipo"), Index(2)]
        [DataSourceCriteria("!IsNull([Padre]) && [Activo] == True")]
        public CxCTipoTransaccion Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        [DbType("int"), XafDisplayName("Número"), Index(3)]
        [ModelDefault("AllowEdit", "False")]
        [ToolTip("Numero Correlativo por tipo de documento y empresa")]
        public int ? Numero
        {
            get => numero;
            set => SetPropertyValue(nameof(Numero), ref numero, value);
        }

        [XafDisplayName("Moneda"), Index(4)]
        [DataSourceCriteria("[Activa] == True")]
        public Moneda Moneda
        {
            get => moneda;
            set
            {
                bool changed = SetPropertyValue(nameof(Moneda), ref moneda, value);
                if (!IsLoading && !IsSaving && changed)
                    ValorMoneda = moneda.FactorCambio;
            }
        }

        [DbType("numeric(12,2)"), XafDisplayName("Valor Moneda"), Index(5)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [ModelDefault("AllowEdit", "False")]
        public decimal ValorMoneda
        {
            get => valorMoneda;
            set => SetPropertyValue(nameof(ValorMoneda), ref valorMoneda, value);
        }

        [Association("CompraFactura-CxPTransacciones")]
        [XafDisplayName("Compra Factura"), Index(6)]
        public CompraFactura Factura
        {
            get => factura;
            set => SetPropertyValue(nameof(Factura), ref factura, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Monto"), Index(7)]
        [RuleValueComparison("CxPTransaccion.Monto > 0", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, SkipNullOrEmptyValues = false)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Monto
        {
            get => monto;
            set => SetPropertyValue(nameof(Monto), ref monto, value);
        }

        [DbType("smallint"), XafDisplayName("Estado"), Index(8)]
        public ECxPTransaccionEstado Estado
        {
            get => estado;
            set => SetPropertyValue(nameof(Estado), ref estado, value);
        }

        [Size(200), DbType("varchar(200)"), XafDisplayName("Comentario"), Index(97)]
        public string Comentario
        {
            get => comentario;
            set => SetPropertyValue(nameof(Comentario), ref comentario, value);
        }

        [XafDisplayName("Fecha Anulación")]
        [ModelDefault("AllowEdit", "False"), Index(98)]
        public DateTime FechaAnula
        {
            get => fechaAnula;
            set => SetPropertyValue(nameof(FechaAnula), ref fechaAnula, value);
        }
        [Size(25), XafDisplayName("Usuario Anulo")]
        [ModelDefault("AllowEdit", "False"), Index(99)]
        public string UsuarioAnulo
        {
            get => usuarioAnulo;
            set => SetPropertyValue(nameof(UsuarioAnulo), ref usuarioAnulo, value);
        }


        #endregion

        #region Metodos
        /// <summary>
        /// Metodo para anular un documento de cuenta por pagar
        /// </summary>
        [Action(Caption = "Anular", ConfirmationMessage = "Esta seguro de Anular el documento ?", ImageName = "Attention", AutoCommit = true)]
        protected virtual void Anular()
        {
            Monto = 0.0m;
            Estado = ECxPTransaccionEstado.Anulado;
            usuarioAnulo = DevExpress.ExpressApp.SecuritySystem.CurrentUserName;
            fechaAnula = DateTime.Now;
            Save();
        }

        /// <summary>
        /// Reescribir el metodo OnSaving para calcular el correlativo por tipo de documento de CxP
        /// </summary>
        protected override void OnSaving()
        {
            if (!(Session is NestedUnitOfWork) && (Session.DataLayer != null) && Session.IsNewObject(this) &&
               (Session.ObjectLayer is SimpleObjectLayer) && (Numero == null || Numero == 0))
            {
                object max;
                string sCriteria = "Empresa.Oid == ? && Tipo.Oid == ? && GetYear(Fecha) == ?";
                max = Session.Evaluate<CxPTransaccion>(CriteriaOperator.Parse("Max(Numero)"), CriteriaOperator.Parse(sCriteria, Empresa.Oid, Tipo.Oid, Fecha.Year));
                Numero = Convert.ToInt32(max ?? 0) + 1;
            }
            base.OnSaving();
        }
        #endregion


        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }

    public enum ECxPTransaccionEstado
    {
        Digitado = 0,
        Aplicado = 1,
        Anulado = 2
    }
}