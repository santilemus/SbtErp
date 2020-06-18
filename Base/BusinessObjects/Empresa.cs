using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// Objeto persistente que corresponde a las empresas del sistema
    /// </summary>
    [DefaultClassOptions]
    [DevExpress.Persistent.Base.NavigationItemAttribute("Catalogos")]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Empresas")]
    [DevExpress.ExpressApp.DC.XafDefaultPropertyAttribute("RazonSocial")]
    [DevExpress.Persistent.Base.ImageNameAttribute("company-info")]
    [RuleIsReferenced("Empresa_Referencia", DefaultContexts.Delete, typeof(Empresa), nameof(Oid), 
        MessageTemplateMustBeReferenced = "Para borrar el objeto '{TargetObject}', debe estar seguro que no es utilizado (referenciado) en ningún lugar.", 
        InvertResult = true, FoundObjectMessageFormat = "'{0}'", FoundObjectMessagesSeparator = ";")]
    public class Empresa : XPObjectBaseBO
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Activa = true;
        }

        private System.Boolean _activa;
        private System.Drawing.Image _logo;
        private System.String _nit;
        private System.String _sitioWeb;
        private System.String _eMail;
        private System.String _direccion;
        private ZonaGeografica _provincia;
        private ZonaGeografica _ciudad;
        private ZonaGeografica _pais;
        private System.String _razonSocial;
        public Empresa(DevExpress.Xpo.Session session)
          : base(session)
        {
        }
        [DevExpress.Xpo.SizeAttribute(200), DbType("varchar(200)"), Persistent("RazonSocial"), XafDisplayName("Razon Social")]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [RuleRequiredField("Empresa.RazonSocial_Requerido", "Save")]
        public System.String RazonSocial
        {
            get
            {
                return _razonSocial;
            }
            set
            {
                SetPropertyValue("RazonSocial", ref _razonSocial, value);
            }
        }
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("País")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false), VisibleInListView(false)]
        [RuleRequiredField("Empresa.Pais_Requerido", DefaultContexts.Save, "País es requerido")]
        [DataSourceCriteria("ZonaPadre is null and Activa = true")]
        public ZonaGeografica Pais
        {
            get
            {
                return _pais;
            }
            set
            {
                SetPropertyValue("Pais", ref _pais, value);
            }
        }
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Departamento")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false), VisibleInListView(false)]
        [RuleRequiredField("Empresa.Provincia_Requerido", DefaultContexts.Save, "Provincia es Requerido")]
        [DataSourceCriteria("ZonaPadre = '@This.Pais' and Activa = true")]
        public ZonaGeografica Provincia
        {
            get
            {
                return _provincia;
            }
            set
            {
                SetPropertyValue("Provincia", ref _provincia, value);
            }
        }
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Ciudad")]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [RuleRequiredField("Empresa.Ciudad_Requerido", DefaultContexts.Save, "Ciudad es Requerido")]
        [DataSourceCriteria("ZonaPadre = '@This.Provincia' and Activa = true")]
        public ZonaGeografica Ciudad
        {
            get
            {
                return _ciudad;
            }
            set
            {
                SetPropertyValue("Ciudad", ref _ciudad, value);
            }
        }
        [DevExpress.Xpo.SizeAttribute(200), DbType("varchar(200)"), Persistent("Direccion")]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute, ]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Dirección")]
        [RuleRequiredField("Empresa.Direccion_Requerido", "Save")]
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

        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Correo Electrónico")]
        [DevExpress.Xpo.SizeAttribute(60), DbType("varchar(60)")]
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
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Sitio Web")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Xpo.SizeAttribute(60), DbType("varchar(60)"), VisibleInListView(false)]
        public System.String SitioWeb
        {
            get
            {
                return _sitioWeb;
            }
            set
            {
                SetPropertyValue("SitioWeb", ref _sitioWeb, value);
            }
        }
        [DevExpress.Xpo.SizeAttribute(14), DbType("varchar(14)"), Persistent("Nit")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        public System.String Nit
        {
            get
            {
                return _nit;
            }
            set
            {
                SetPropertyValue("Nit", ref _nit, value);
            }
        }

        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [DevExpress.Persistent.Base.VisibleInListViewAttribute(false)]
        [RuleRequiredField("Empresa.Activa_Requerido", "Save")]
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
        [DevExpress.Xpo.ValueConverterAttribute(typeof(DevExpress.Xpo.Metadata.ImageValueConverter))]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Persistent.Base.VisibleInListViewAttribute(false)]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        public System.Drawing.Image Logo
        {
            get
            {
                return _logo;
            }
            set
            {
                SetPropertyValue("Logo", ref _logo, value);
            }
        }

        [DevExpress.Xpo.AssociationAttribute("Telefonos-Empresa"), VisibleInDetailView(true)]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Teléfonos"), DevExpress.Xpo.Aggregated]
        public XPCollection<EmpresaTelefono> Telefonos
        {
            get
            {
                return GetCollection<EmpresaTelefono>("Telefonos");
            }
        }

        [DevExpress.Xpo.AssociationAttribute("RegistroFiscal-Empresa"), DevExpress.Xpo.Aggregated, VisibleInDetailView(true)]
        public XPCollection<EmpresaNrf> RegistroFiscal
        {
            get
            {
                return GetCollection<EmpresaNrf>("RegistroFiscal");
            }
        }

        [DevExpress.Xpo.AssociationAttribute("Unidades-Empresa"), DevExpress.Xpo.Aggregated, VisibleInDetailView(true)]
        public XPCollection<EmpresaUnidad> Unidades
        {
            get
            {
                return GetCollection<EmpresaUnidad>("Unidades");
            }
        }

        [Association("Empresa-Usuarios"), DevExpress.Xpo.Aggregated, XafDisplayName("Usuarios"), VisibleInDetailView(true)]
        public XPCollection<Usuario> Usuarios
        {
            get
            {
                return GetCollection<Usuario>(nameof(Usuarios));
            }
        }

    }
}
