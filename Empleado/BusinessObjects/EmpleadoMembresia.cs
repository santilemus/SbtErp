﻿using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace SBT.Apps.Empleado.Module.BusinessObjects
{
    [NavigationItem(false)]
    [DevExpress.Persistent.Base.CreatableItemAttribute(false)]
    [DevExpress.Persistent.Base.ImageNameAttribute("employee_man-info")]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Empleado Membresías")]
    public class EmpleadoMembresia : XPObject
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Vigente = true;
        }

        private Empleado _empleado;
        private System.Boolean _vigente = true;
        private AsociacionProfesional asociacionProfesional;
        public EmpleadoMembresia(DevExpress.Xpo.Session session)
          : base(session)
        {
        }
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Código Membresía")]
        [RuleRequiredField("EmpleadoMembresia.Codigo_Requerido", DefaultContexts.Save, "Membresía es requerido")]
        [ExplicitLoading]
        public AsociacionProfesional AsociacionProfesional
        {
            get => asociacionProfesional;
            set => SetPropertyValue(nameof(AsociacionProfesional), ref asociacionProfesional, value);
        }

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
