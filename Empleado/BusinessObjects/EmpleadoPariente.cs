using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;

namespace SBT.Apps.Empleado.Module.BusinessObjects
{
    [DefaultClassOptions, NavigationItem(false)]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Parientes")]
    [DevExpress.Persistent.Base.ImageNameAttribute("employee_woman-favorite2")]
    [DevExpress.ExpressApp.DC.XafDefaultPropertyAttribute("NombreCompleto")]
    [DevExpress.Persistent.Base.CreatableItemAttribute(false)]
    public class EmpleadoPariente : XPObject
    {
        [PersistentAlias("Nombre + ' ' + Apellido")]
        [DisplayName("Nombre Completo")]
        public string NombreCompleto
        {
            get { return Convert.ToString(EvaluateAlias("NombreCompleto")); }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Beneficiario = false;
        }

        private Empleado _empleado;
        private System.String _direccion;
        private System.DateTime _fechaNacimiento;
        private Listas tipo;
        private System.Boolean _beneficiario;
        private System.String _apellido;
        private System.String _nombre;
        public EmpleadoPariente(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Tipo Pariente")]
        [RuleRequiredField("EmpleadoPariente.CodTipo_requerido", DefaultContexts.Save, "Tipo Pariente es requerido")]
        [DataSourceCriteria("Categoria = 12")]
        [ExplicitLoading]
        public Listas Tipo
        {
            get
            {
                return tipo;
            }
            set
            {
                SetPropertyValue("Tipo", ref tipo, value);
            }
        }

        [DevExpress.Xpo.SizeAttribute(50), DbType("varchar(50)")]
        [RuleRequiredField("EmpleadoPariente.Nombre_requerido", "Save")]
        public System.String Nombre
        {
            get
            {
                return _nombre;
            }
            set
            {
                SetPropertyValue("Nombre", ref _nombre, value);
            }
        }

        [DevExpress.Xpo.SizeAttribute(50), DbType("varchar(50)")]
        [RuleRequiredField("EmpleadoPariente.Apellido_requerido", "Save")]
        public System.String Apellido
        {
            get
            {
                return _apellido;
            }
            set
            {
                SetPropertyValue("Apellido", ref _apellido, value);
            }
        }

        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        public System.Boolean Beneficiario
        {
            get
            {
                return _beneficiario;
            }
            set
            {
                SetPropertyValue("Beneficiario", ref _beneficiario, value);
            }
        }

        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Xpo.IndexedAttribute]
        [RuleRequiredField("EmpleadoPariente.FechaNacimiento_requerido", "Save")]
        public System.DateTime FechaNacimiento
        {
            get
            {
                return _fechaNacimiento;
            }
            set
            {
                SetPropertyValue("FechaNacimiento", ref _fechaNacimiento, value);
            }
        }

        [DevExpress.Xpo.SizeAttribute(150), DbType("varchar(150)")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        public System.String Direccion
        {
            get
            {
                return _direccion;
            }
            set
            {
                SetPropertyValue("Direccion", ref _direccion, value);
            }
        }

        [DevExpress.Xpo.AssociationAttribute("Parientes-Empleado")]
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
