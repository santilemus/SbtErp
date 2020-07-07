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
using SBT.Apps.Empleado.Module.BusinessObjects;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    /// <summary>
    /// Recurso Humano. BO que corresponde a las Transacciones del usuario y que afectan el calculo de los ingresos y descuentos
    /// aplicables al empleado, los cuales impactan en el devengado del empleado
    /// </summary>
    [DefaultClassOptions, ImageName("TransaccionEmpleado")]
    [DefaultProperty("Descripcion"), DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None),
        Persistent("PlaTransaccion"), ModelDefault("Caption", "Transacciones de Planillas"), NavigationItem("Recurso Humano")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Transaccion : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Transaccion(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Cancelado = false;
            ValorMoneda = 0.0m;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        SBT.Apps.Empleado.Module.BusinessObjects.Empleado empleado;
        ETipoTransaccion tipoTransaccion = ETipoTransaccion.Descuento;
        int numero;
        Listas clasificacion;
        Moneda moneda;
        decimal valorMoneda;
        string descripcion;
        DateTime fechaInicio;
        EFormaAplicarTransaccion formaAplicar = EFormaAplicarTransaccion.Ambas;
        decimal montoCuota = 0.0m;
        int cantidadCuotas = 1;
        int diaPago;
        bool cancelado = false;

#if Firebird
        [DbType("DM_ENTERO"), Persistent("ID_EMPLE")]
#else
        [DbType("int"), Persistent("Empleado")]
#endif 
        [XafDisplayName("Empleado"), Index(1)]
        [DetailViewLayout(LayoutColumnPosition.Left, LayoutGroupType.SimpleEditorsGroup)]
        public SBT.Apps.Empleado.Module.BusinessObjects.Empleado Empleado
        {
            get => empleado;
            set => SetPropertyValue(nameof(Empleado), ref empleado, value);
        }

#if Firebird
        [DbType("DM_ENTERO"), Persistent("NUMERO")]
#else
        [DbType("int"), Persistent("Oid")]
#endif 
        [XafDisplayName("No Empresa"), Index(2), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [DetailViewLayout(LayoutColumnPosition.Left, LayoutGroupType.SimpleEditorsGroup)]
        [ToolTip("Correlativo o Número de transacción por empresa")]
        public int Numero
        {
            get => numero;
            set => SetPropertyValue(nameof(Numero), ref numero, value);
        }

#if Firebird
        [DbType("DM_ENTERO_CORTO"), Persistent("TIPO")]
#else
        [DbType("smallint"), Persistent("Tipo")]
#endif
        [XafDisplayName("Tipo Transacción"), RuleRequiredField("CxC Transaccion.Tipo_Requerido", "Save"), Index(3)]
        [DetailViewLayout(LayoutColumnPosition.Left, LayoutGroupType.SimpleEditorsGroup), ImmediatePostData(true)]
        public ETipoTransaccion TipoTransaccion
        {
            get => tipoTransaccion;
            set
            {
                SetPropertyValue(nameof(TipoTransaccion), ref tipoTransaccion, value);
                RefreshTransClasificaciones();
            }
        }

#if Firebird
        [DbType("DM_DESCRIPCION25"), Persistent("CLASIFICACION")]
#else
        [DbType("varchar(25)"), Persistent("clasificacion")]
#endif
        [XafDisplayName("Clasificación"), RuleRequiredField("Transaccion.Clasificacion_Requerido", DefaultContexts.Save)]
        //DataSourceProperty("TransClasificaciones"), ImmediatePostData, Index(4)]
        [DataSourceCriteria("('@This.TipoTransaccion' = 1 And Categoria = 18) Or ('@This.TipoTransaccion' = 2 And Categoria = 19) " +
            "Or ('@This.TipoTransaccion' = 3 And Categoria = 20) ")]
        [DetailViewLayout(LayoutColumnPosition.Left, LayoutGroupType.SimpleEditorsGroup)]
        public Listas Clasificacion
        {
            get => clasificacion;
            set => SetPropertyValue(nameof(Clasificacion), ref clasificacion, value);
        }

#if Firebird
        [DbType("DM_CODIGO03"), Persistent("COD_MONEDA")]
#else
        [DbType("varchar(3)"), Persistent("Moneda")]
#endif
        [XafDisplayName("Moneda"), RuleRequiredField("Transaccion.Moneda_Requerido", "Save"), Index(5), VisibleInLookupListView(false), VisibleInListView(false)]
        [DetailViewLayout(LayoutColumnPosition.Left, LayoutGroupType.SimpleEditorsGroup)]
        public Moneda Moneda
        {
            get => moneda;
            set => SetPropertyValue(nameof(Moneda), ref moneda, value);
        }

#if Firebird
        [DbType("DM_DINERO122"), Persistent("VAL_MONE")]
#else
        [DbType("money"), Persistent("ValorMoneda")]
#endif
        [XafDisplayName("Valor Moneda"), RuleRequiredField("Transaccion.ValorMoneda_Requerido", "Save"), Index(6),
            VisibleInLookupListView(false), VisibleInListView(false)]
        [DetailViewLayout(LayoutColumnPosition.Left, LayoutGroupType.SimpleEditorsGroup)]
        public decimal ValorMoneda
        {
            get => valorMoneda;
            set => SetPropertyValue(nameof(ValorMoneda), ref valorMoneda, value);
        }

        SBT.Apps.Tercero.Module.BusinessObjects.Tercero proveedor;
#if Firebird
        [DbType("DM_ENTERO"), Persistent("ID_PROVEEDOR")]
#else
        [DbType("int"), Persistent("Proveedor")]
#endif
        [XafDisplayName("Proveedor"), Index(7), VisibleInLookupListView(false)]
        [DetailViewLayout(LayoutColumnPosition.Left, LayoutGroupType.SimpleEditorsGroup)]
        public SBT.Apps.Tercero.Module.BusinessObjects.Tercero Proveedor
        {
            get => proveedor;
            set => SetPropertyValue(nameof(Proveedor), ref proveedor, value);
        }

#if Firebird
        [DbType("DM_DESCRIPCION60"), Persistent("DESCRIPCION")]
#else
        [DbType("varchar(60)"), Persistent("Descripcion")]
#endif
        [Size(60), XafDisplayName("Descripción"), Index(8), VisibleInLookupListView(false), VisibleInListView(false)]
        [DetailViewLayout(LayoutColumnPosition.Right, LayoutGroupType.SimpleEditorsGroup)]
        public string Descripcion
        {
            get => descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref descripcion, value);
        }

#if Firebird
        [DbType("DM_FECHA"), Persistent("FECHA_INICIO")]
#else
        [DbType("datetime"), Persistent("FechaInicio")]
#endif
        [XafDisplayName("Fecha Inicio"), Index(9), VisibleInLookupListView(false), VisibleInListView(false),
            RuleRequiredField("Transaccion.FechaInicio_Requerido", DefaultContexts.Save)]
        [DetailViewLayout(LayoutColumnPosition.Right, LayoutGroupType.SimpleEditorsGroup)]
        public DateTime FechaInicio
        {
            get => fechaInicio;
            set => SetPropertyValue(nameof(FechaInicio), ref fechaInicio, value);
        }

#if Firebird
        [DbType("DM_ENTERO_CORTO"), Persistent("FORMA_APLICAR")]
#else
        [DbType("smallint"), Persistent("FormaAplicar")]
#endif
        [XafDisplayName("Forma Aplicar"), Index(10), VisibleInListView(false), VisibleInLookupListView(false),
            RuleRequiredField("Transaccion.FormaAplicar_Requerido", "Save")]
        [DetailViewLayout(LayoutColumnPosition.Right, LayoutGroupType.SimpleEditorsGroup)]
        public EFormaAplicarTransaccion FormaAplicar
        {
            get => formaAplicar;
            set => SetPropertyValue(nameof(FormaAplicar), ref formaAplicar, value);
        }

#if Firebird
        [DbType("DM_DINERO122"), Persistent("MONTO_CUOTA")]
#else
        [DbType("money"), Persistent("MontoCuota")]
#endif
        [XafDisplayName("Monto Cuota"), Index(11), RuleValueComparison("Transaccion.MontoCuota >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0,
            SkipNullOrEmptyValues = false), ModelDefault("FormatDisplay", "N2"), ModelDefault("EditMask", "N2"), VisibleInLookupListView(false), VisibleInListView(false)]
        [DetailViewLayout(LayoutColumnPosition.Right, LayoutGroupType.SimpleEditorsGroup)]
        public decimal MontoCuota
        {
            get => montoCuota;
            set => SetPropertyValue(nameof(MontoCuota), ref montoCuota, value);
        }

        [XafDisplayName("Cantidad Cuotas"), RuleRequiredField("Transaccion.CantidadCuotas_Requerido", "Save")]
        public int CantidadCuotas
        {
            get => cantidadCuotas;
            set => SetPropertyValue(nameof(CantidadCuotas), ref cantidadCuotas, value);
        }

#if Firebird
        [DbType("DM_ENTERO_CORTO"), Persistent("DIA_PAGO")]
#else
        [DbType("smallint"), Persistent("DiaPago")]
#endif
        [XafDisplayName("Día Pago"), Index(12), VisibleInListView(false), VisibleInLookupListView(false),
            RuleRange("Transaccion.DiaPago entre 1 y 31", DefaultContexts.Save, 1, 31, SkipNullOrEmptyValues = true)]
        [DetailViewLayout(LayoutColumnPosition.Right, LayoutGroupType.SimpleEditorsGroup)]
        public int DiaPago
        {
            get => diaPago;
            set => SetPropertyValue(nameof(DiaPago), ref diaPago, value);
        }

#if Firebird
        [DbType("DM_LOGICO"), Persistent("CANCELADO")]
#else
        [DbType("bit"), Persistent("Cancelado")]
#endif
        [XafDisplayName("Cancelado"), Index(13), VisibleInLookupListView(false), RuleRequiredField("Transaccion.Cancelado_Requerido", "Save")]
        [DetailViewLayout(LayoutColumnPosition.Right, LayoutGroupType.SimpleEditorsGroup)]
        public bool Cancelado
        {
            get => cancelado;
            set => SetPropertyValue(nameof(Cancelado), ref cancelado, value);
        }

        private XPCollection<Listas> transClasificaciones;

        [Browsable(false)]
        [CollectionOperationSet(AllowAdd = false)]
        public XPCollection<Listas> TransClasificaciones
        {
            get
            {
                if (transClasificaciones == null)
                {
                    transClasificaciones = new XPCollection<Listas>(Session);
                    RefreshTransClasificaciones();
                }
                return transClasificaciones;
            }
        }

        private void RefreshTransClasificaciones()
        {
            if (transClasificaciones == null)
                return;
            if (TipoTransaccion == ETipoTransaccion.Descuento)
                transClasificaciones.Criteria = CriteriaOperator.Parse("TipoLista = ? And Activo = ?", 10, true);
            else
                transClasificaciones.Criteria = CriteriaOperator.Parse("TipoLista = ? And Activo = ?", 9, true);
            Clasificacion = null;
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}