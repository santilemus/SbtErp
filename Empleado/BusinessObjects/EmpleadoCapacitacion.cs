using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Linq;

namespace SBT.Apps.Empleado.Module.BusinessObjects
{
    [NavigationItem(false)]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Empleado Capacitación")]
    [DevExpress.Persistent.Base.ImageNameAttribute("employee_man-certificate")]
    [DevExpress.ExpressApp.DC.XafDefaultPropertyAttribute("Descripcion")]
    [DevExpress.Persistent.Base.CreatableItemAttribute(false)]
    public class EmpleadoCapacitacion : XPObjectBaseBO
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            FechaInicio = DateTime.Now;
            DiasDuracion = 1;
        }

        private Empleado _empleado;
        private System.Int16 _diasDuracion;
        private System.DateTime _fechaInicio;
        private System.String _descripcion;
        private Listas _codigo;
        public EmpleadoCapacitacion(DevExpress.Xpo.Session session)
          : base(session)
        {
        }
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Código")]
        [RuleRequiredField("EmpleadoCapacitacion.Codigo_Requerido", DefaultContexts.Save, "Capacitación es requerida")]
        [DataSourceCriteria("Categoria = 'Capacitacion'")]
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
        [DevExpress.Xpo.SizeAttribute(150), DbType("varchar(150)")]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Descripción")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [RuleRequiredField("EmpleadoCapacitacion.Descripcion_Requerido", "Save")]
        public System.String Descripcion
        {
            get
            {
                return _descripcion;
            }
            set
            {
                SetPropertyValue("Descripcion", ref _descripcion, value);
            }
        }

        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [RuleRequiredField("EmpleadoCapacitacion.FechaInicio_Requerido", "Save")]
        public System.DateTime FechaInicio
        {
            get
            {
                return _fechaInicio;
            }
            set
            {
                SetPropertyValue("FechaInicio", ref _fechaInicio, value);
            }
        }

        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Días de Duración")]
        [RuleRequiredField("EmpleadoCapacitacion.DiasDuracion_Requerido", "Save")]
        public System.Int16 DiasDuracion
        {
            get
            {
                return _diasDuracion;
            }
            set
            {
                SetPropertyValue("DiasDuracion", ref _diasDuracion, value);
            }
        }

        [DevExpress.Xpo.AssociationAttribute("Capacitaciones-Empleado")]
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
