using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Unidad")]
    [DevExpress.Persistent.Base.ImageNameAttribute("company-employee")]
    [DevExpress.ExpressApp.DC.XafDefaultPropertyAttribute("Nombre")]
    public class EmpresaUnidad : XPObject
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Activa = true;
            Role = ETipoRoleUnidad.Unidad;
        }
        ETipoRoleUnidad? role;
        private Empresa _empresa;
        private System.Boolean _activa;
        private System.String _codigo;
        private System.String _eMail;
        private System.String _telefono;
        private System.String _direccion;
        private System.String _nombre;
        private EmpresaUnidad _unidadPadre;
        public EmpresaUnidad(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Unidad Padre")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DataSourceCriteria("[Empresa] == '@This.Empresa'")]
        [DevExpress.Xpo.ExplicitLoading(1)]
        public EmpresaUnidad UnidadPadre
        {
            get
            {
                return _unidadPadre;
            }
            set
            {
                SetPropertyValue("UnidadPadre", ref _unidadPadre, value);
            }
        }


        [DevExpress.Xpo.AssociationAttribute("Unidades-Empresa")]
        public Empresa Empresa
        {
            get => _empresa;
            set => SetPropertyValue(nameof(Empresa), ref _empresa, value);
        }

        [Size(100), DbType("varchar(100)")]
        [RuleRequiredField("EmpresaUnidad.Nombre_Requerido", "Save")]
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
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Dirección")]
        [DevExpress.Xpo.Size(200), DbType("varchar(200)")]
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
        [DevExpress.Xpo.Size(20), DbType("varchar(20)")]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Teléfono")]
        public System.String Telefono
        {
            get
            {
                return _telefono;
            }
            set
            {
                SetPropertyValue("Telefono", ref _telefono, value);
            }
        }
        [RuleRequiredField("EmpresaUnidad.Activa_Requerido", "Save")]
        public System.Boolean Activa
        {
            get
            {
                return _activa;
            }
            set
            {
                SetPropertyValue("Activa", ref _activa, value);
            }
        }
        [DevExpress.Xpo.NonPersistentAttribute]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Xpo.Size(50), DbType("varchar(50)")]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Correo Electrónico")]
        public System.String EMail
        {
            get
            {
                return _eMail;
            }
            set
            {
                SetPropertyValue("EMail", ref _eMail, value);
            }
        }

        [DbType("smallint"), XafDisplayName("Role Unidad"), Persistent("IdRole")]
        public ETipoRoleUnidad? Role
        {
            get => role;
            set => SetPropertyValue(nameof(Role), ref role, value);
        }

        //[RuleUniqueValue("EmpresaUnidad.CodigoRole_UniqueValue", "Save", SkipNullOrEmptyValues = false)]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(true)]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Código Role")]
        [DevExpress.Xpo.Size(6), DbType("varchar(6)")]
        [RuleUniqueValue("EmpresaUnidad.Codigo_Unico", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        public System.String Codigo
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

    }
}
