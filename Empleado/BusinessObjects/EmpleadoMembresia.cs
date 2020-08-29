using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Linq;

namespace SBT.Apps.Empleado.Module.BusinessObjects
{
    [DefaultClassOptions, NavigationItem(false)]
    [DevExpress.Persistent.Base.CreatableItemAttribute(false)]
    [DevExpress.Persistent.Base.ImageNameAttribute("employee_man-info")]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Empleado Membresías")]
    public class EmpleadoMembresia : XPObjectBaseBO
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Vigente = true;
        }

        private Empleado _empleado;
        private System.Boolean _vigente;
        private Listas _codigo;
        public EmpleadoMembresia(DevExpress.Xpo.Session session)
          : base(session)
        {
        }
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Código Membresía")]
        [RuleRequiredField("EmpleadoMembresia.Codigo_Requerido", DefaultContexts.Save, "Membresía es requerido")]
        [DataSourceCriteria("[Categoria] == 3 && [Activo] == True")]
        public Listas Codigo
        {
            get
            {
                return _codigo;
            }
            set
            {
                SetPropertyValue("Codigo", ref _codigo, value);
            }
        }
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [RuleRequiredField("EmpleadoMembresia.Vigente_Requerido", "Save")]
        public System.Boolean Vigente
        {
            get
            {
                return _vigente;
            }
            set
            {
                SetPropertyValue("Vigente", ref _vigente, value);
            }
        }
        [DevExpress.Xpo.AssociationAttribute("Membresias-Empleado")]
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
