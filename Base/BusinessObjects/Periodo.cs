using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Linq;


namespace SBT.Apps.Base.Module.BusinessObjects
{
    [DefaultClassOptions, CreatableItem(false)]
    //[ImageName("BO_Contact")]
    [Persistent("ConPeriodo"), DefaultProperty("Oid"), NavigationItem("Catalogos"), ModelDefault("Caption", "Períodos")]
    //[RuleCombinationOfPropertiesIsUnique("Periodo.Empresa_Numero_Unico", DefaultContexts.Save, "Empresa, Oid", 
    //    CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [RuleCriteria("Periodo.Oid = Year", DefaultContexts.Save, "[Oid] == GetYear([FechaInicio]) && GetYear([FechaInicio]) == GetYear([FechaFin])", 
        "Fecha Inicio y Fecha Fin deben corresponder al mismo año y Oid debe ser igual al año", SkipNullOrEmptyValues = false)]
    public class Periodo : XPCustomObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Periodo(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            Activo = true;
        }

        #region Propiedades
        int oid = DateTime.Now.Year;
#if Firebird
        [DbType("DM_ENTERO_CORTO"), Persistent("NUMERO")]
#else
        [DbType("int"), Persistent("Oid")]
#endif
        [Index(1), Key(false), RuleRequiredField("Periodo.Oid_Requerido", DefaultContexts.Save), XafDisplayName("Oid")]
        [ToolTip("Oid. Debe ser igual al año")]
        public int Oid
        {
            get => oid;
            set => SetPropertyValue(nameof(Oid), ref oid, value);
        }

        DateTime fechaInicio;
#if Firebird
        [DbType("DM_FECHA"), Persistent("FECHA_INICIO")]
#else
        [DbType("datetime"), Persistent("FechaInicio")]
#endif
        [XafDisplayName("Fecha Inicio"), Index(2), RuleRequiredField("Periodo.FechaInicio_Requerido", "Save")]
        public DateTime FechaInicio
        {
            get => fechaInicio;
            set
            {
                bool changed = SetPropertyValue(nameof(FechaInicio), ref fechaInicio, value);
                if (!IsLoading && !IsSaving && changed)
                {
                    FechaFin = new DateTime(FechaInicio.Year, 12, 31);
                }
            }
        }

        DateTime fechaFin;
#if Firebird
        [DbType("DM_FECHA"), Persistent("FECHA_FIN")]
#else
        [DbType("datetime"), Persistent("FechaFin")]
#endif
        [XafDisplayName("Fecha Fin"), Index(3), RuleValueComparison("Periodo.FechaFin >= FechaInicio", DefaultContexts.Save,
            ValueComparisonType.GreaterThan, "FechaInicio", ParametersMode.Expression, SkipNullOrEmptyValues = false)]
        public DateTime FechaFin
        {
            get => fechaFin;
            set => SetPropertyValue(nameof(FechaFin), ref fechaFin, value);
        }

        string comentario;
#if Firebird
        [DbType("DM_DESCRIPCION250"), Persistent("COMENTARIO")]
#else
        [DbType("varchar(250)"), Persistent("Comentario")]
#endif
        [Size(250), XafDisplayName("Comentario"), Index(4), VisibleInListView(false), VisibleInLookupListView(false)]
        public string Comentario
        {
            get => comentario;
            set => SetPropertyValue(nameof(Comentario), ref comentario, value);
        }

        bool activo;
#if Firebird
        [DbType("DM_BOOLEAN"), Persistent("ACTIVO")]
        [ValueConverter(typeof(ToBooleanDataType))]
#else
        [DbType("bit"), Persistent("Activo")]
#endif
        [XafDisplayName("Activo"), Index(5), RuleRequiredField("Periodo.Activo_Requerido", DefaultContexts.Save)]
        public bool Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }

        #endregion

        #region Colecciones

        #endregion

        #region Metodos
        public bool Abierto(int APeriodo)
        {
            var obj = Session.GetObjectByKey<Periodo>(APeriodo);
            bool resultado = (obj != null && obj.activo);
            return resultado;
        }
        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}