using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    /// <summary>
    /// Recurso Humano. BO para la clasificacion de los tipos de planilla
    /// </summary>
    [DefaultClassOptions, ModelDefault("Caption", "Tipo Planilla"), NavigationItem("Recurso Humano"),
        DefaultProperty("Nombre"), DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None),
        Persistent(nameof(TipoPlanilla)), ImageName("TipoPlanilla")]
    [CreatableItem(false)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class TipoPlanilla : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public TipoPlanilla(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            Clase = EClasePlanilla.Salarios;
            FormaPago = EFormaPago.Quincenal;
            Activo = true;
        }

        #region Propiedades
        string nombre;
        EFormaPago formaPago;
        bool activo;


#if (Firebird)
        [DbType("DM_DESCRIPCION60"), Persistent("NOMBRE")]
#else
        [DbType("varchar(60)"), Persistent("Nombre")]
#endif
        [Size(60), XafDisplayName("Nombre"), Index(1), RuleUniqueValue("TipoPlanilla.Nombre_Unico", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction,
            SkipNullOrEmptyValues = false)]
        public string Nombre
        {
            get
            {
                return nombre;
            }
            set
            {
                SetPropertyValue("Nombre", ref nombre, value);
            }
        }

        EClasePlanilla clase = EClasePlanilla.Salarios;
#if (Firebird)
        [DbType("DM_ENTERO_CORTO"), Persistent("CLASE_PLANI")]
#else
        [DbType("smallint"), Persistent("Clase")]
#endif
        [XafDisplayName("Clase Planilla"), Index(2)]
        public EClasePlanilla Clase
        {
            get
            {
                return clase;
            }
            set
            {
                SetPropertyValue<EClasePlanilla>("Clase", ref clase, value);
            }
        }

#if (Firebird)
        [DbType("DM_ENTERO_CORTO"), Persistent("FORMA_PAGO")]
#else
        [DbType("smallint"), Persistent("FormaPago")]
#endif
        [XafDisplayName("Forma de Pago")]
        public EFormaPago FormaPago
        {
            get => formaPago;
            set => SetPropertyValue(nameof(FormaPago), ref formaPago, value);
        }

        DevExpress.Persistent.BaseImpl.ReportDataV2 reporte;

        [Size(50), XafDisplayName("Reporte"), Index(3), VisibleInLookupListView(false), Persistent("Reporte")]
        public DevExpress.Persistent.BaseImpl.ReportDataV2 Reporte
        {
            get => reporte;
            set => SetPropertyValue(nameof(Reporte), ref reporte, value);
        }

#if (Firebird)
        [DbType("DM_BOOLEAN"), Persistent("ACTIVO")]
#else
        [DbType("bit"), Persistent("Activo")]
#endif
        [XafDisplayName("Activo")]
        public bool Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }

        #endregion

        #region Coleciones
        [Association("TipoPlanilla-Operaciones"), XafDisplayName("Operaciones"), Index(0)]
        public XPCollection<OperacionTipoPlanilla> Operaciones
        {
            get
            {
                return GetCollection<OperacionTipoPlanilla>(nameof(Operaciones));
            }
        }

        //[Association("TipoPlanilla-Planillas"), XafDisplayName("Planillas")]
        //public XPCollection<Planilla> Planillas => GetCollection<Planilla>(nameof(Planillas));

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}