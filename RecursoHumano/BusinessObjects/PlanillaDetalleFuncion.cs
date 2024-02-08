using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    /// <summary>
    /// BO EmpleadoPlanilla. Contiene los datos que se obtienen al ejecutar select de la funcion dbo.fnPlaEmpleadoPlanilla
    /// y que devuelve datos de funciones que son necesarios para el calculo de la planilla.
    /// Se implementa el selectdata para, y debera ejecutarse cuando se procesa cada empleado para calcular la planilla
    /// </summary>
    /// <remarks>
    /// Ampliar y corregir la documentacion del BO cuando se complete la implementacion. Mas informacion de la implementacion 
    /// a realizr en https://docs.devexpress.com/XPO/8914/feature-center/querying-a-data-store/direct-sql-queries
    /// </remarks>
    [NonPersistent, NavigationItem(false)]
    //[ImageName("BO_Contact")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class PlanillaDetalleFuncion : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public PlanillaDetalleFuncion(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        Parametro parametro;
        //DateTime fechaPago;
        //DateTime fechaFin;
        //DateTime fechaInicio;
        decimal ingresoBrutoQuincena;
        decimal cotizaAcumuladaRenta;
        decimal cotizaAcumuladaAfp;
        decimal cotizaAcumuladaIsss;
        decimal totalHorasExtra;
        int diasMaternidad;
        int diasIncapacidad;
        int diasAmonestacion;
        int diasInasistencia;
        int diasLicenciaSinSueldo;
        SBT.Apps.Empleado.Module.BusinessObjects.Empleado empleado;


        [XafDisplayName("Empleado"), ImmediatePostData(true)]
        public SBT.Apps.Empleado.Module.BusinessObjects.Empleado Empleado
        {
            get => empleado;
            set => SetPropertyValue(nameof(Empleado), ref empleado, value);
        }

        [XafDisplayName("Licencia Sin Sueldo")]
        public int DiasLicenciaSinSueldo
        {
            get => diasLicenciaSinSueldo;
            set => SetPropertyValue(nameof(DiasLicenciaSinSueldo), ref diasLicenciaSinSueldo, value);
        }

        [XafDisplayName("Inasistencia")]
        public int DiasInasistencia
        {
            get => diasInasistencia;
            set => SetPropertyValue(nameof(DiasInasistencia), ref diasInasistencia, value);
        }

        [XafDisplayName("Amonestación")]
        public int DiasAmonestacion
        {
            get => diasAmonestacion;
            set => SetPropertyValue(nameof(DiasAmonestacion), ref diasAmonestacion, value);
        }

        [XafDisplayName("Incapacidad")]
        public int DiasIncapacidad
        {
            get => diasIncapacidad;
            set => SetPropertyValue(nameof(DiasIncapacidad), ref diasIncapacidad, value);
        }

        [XafDisplayName("Maternidad")]
        public int DiasMaternidad
        {
            get => diasMaternidad;
            set => SetPropertyValue(nameof(DiasMaternidad), ref diasMaternidad, value);
        }

        [XafDisplayName("Horas Extra")]
        public decimal TotalHorasExtra
        {
            get => totalHorasExtra;
            set => SetPropertyValue(nameof(TotalHorasExtra), ref totalHorasExtra, value);
        }

        [XafDisplayName("Acumulado ISSS")]
        public decimal CotizaAcumuladaIsss
        {
            get => cotizaAcumuladaIsss;
            set => SetPropertyValue(nameof(CotizaAcumuladaIsss), ref cotizaAcumuladaIsss, value);
        }

        [XafDisplayName("Acumulado AFP")]
        public decimal CotizaAcumuladaAfp
        {
            get => cotizaAcumuladaAfp;
            set => SetPropertyValue(nameof(CotizaAcumuladaAfp), ref cotizaAcumuladaAfp, value);
        }

        [XafDisplayName("Acumulado Renta")]
        public decimal CotizaAcumuladaRenta
        {
            get => cotizaAcumuladaRenta;
            set => SetPropertyValue(nameof(CotizaAcumuladaRenta), ref cotizaAcumuladaRenta, value);
        }

        [XafDisplayName("Ingreso Bruto")]
        public decimal IngresoBrutoQuincena
        {
            get => ingresoBrutoQuincena;
            set => SetPropertyValue(nameof(IngresoBrutoQuincena), ref ingresoBrutoQuincena, value);
        }

        //[XafDisplayName("Fecha Inicio")]
        //public DateTime FechaInicio
        //{
        //    get => fechaInicio;
        //    set => SetPropertyValue(nameof(FechaInicio), ref fechaInicio, value);
        //}

        //[XafDisplayName("Fecha Fin")]
        //public DateTime FechaFin
        //{
        //    get => fechaFin;
        //    set => SetPropertyValue(nameof(FechaFin), ref fechaFin, value);
        //}

        //[XafDisplayName("Fecha Pago")]
        //public DateTime FechaPago
        //{
        //    get => fechaPago;
        //    set => SetPropertyValue(nameof(FechaPago), ref fechaPago, value);
        //}

        [XafDisplayName("Parámetro")]
        public Parametro Parametro
        {
            get => parametro;
            set => SetPropertyValue(nameof(Parametro), ref parametro, value);
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