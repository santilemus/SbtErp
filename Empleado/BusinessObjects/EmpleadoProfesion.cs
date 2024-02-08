using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;

namespace SBT.Apps.Empleado.Module.BusinessObjects
{
    [DefaultClassOptions, NavigationItem(false)]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Formación Profesional")]
    [DevExpress.Persistent.Base.ImageNameAttribute("user_id-certificate")]
    [DevExpress.Persistent.Base.CreatableItemAttribute(false)]
    [RuleCombinationOfPropertiesIsUnique("EmpleadoProfesion_Unico", DefaultContexts.Save, "Profesion,NumeroProfesional", SkipNullOrEmptyValues = false)]
    public class EmpleadoProfesion : XPObject
    {

        private Profesion _profesion;
        private Empleado _empleado;
        private System.String _numeroProfesional;
        public EmpleadoProfesion(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Profesión"), VisibleInLookupListView(true), Index(1)]
        [RuleRequiredField("EmpleadoProfesion_Profesion", DefaultContexts.Save, "Profesión es requerida")]
        [ExplicitLoading]
        public Profesion Profesion
        {
            get
            {
                return _profesion;
            }
            set
            {
                SetPropertyValue("Profesion", ref _profesion, value);
            }
        }
        [DevExpress.Xpo.SizeAttribute(12), DbType("varchar(12)"), Index(2)]
        [DevExpress.Persistent.Base.ToolTipAttribute("Número de certificación emitido por el organismo de vigilancia de la profesión")]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Número Profesional")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(true)]
        public System.String NumeroProfesional
        {
            get
            {
                return _numeroProfesional;
            }
            set
            {
                SetPropertyValue("NumeroProfesional", ref _numeroProfesional, value);
            }
        }
        [DevExpress.Xpo.AssociationAttribute("Profesiones-Empleado"), Index(0)]
        [RuleRequiredField("EmpleadoProfesion_Empleado", DefaultContexts.Save, "El empleado es requerido")]
        public Empleado Empleado
        {
            get
            {
                return _empleado;
            }
            set
            {
                SetPropertyValue("Empleado", ref _empleado, value);
            }
        }

    }
}
