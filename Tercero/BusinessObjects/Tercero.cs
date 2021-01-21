using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Linq;

namespace SBT.Apps.Tercero.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que corresponde a los terceros. Es la clase base para clientes, proveedores, bancos, etc
    /// </summary>
    [DefaultClassOptions, XafDisplayName("Terceros"), ImageName(nameof(Tercero)), NavigationItem("Catalogos"), XafDefaultProperty(nameof(Nombre))]
    [RuleIsReferenced("Tercero_Referencia", DefaultContexts.Delete, typeof(SBT.Apps.Tercero.Module.BusinessObjects.Tercero), nameof(Oid),
        MessageTemplateMustBeReferenced = "Para borrar el objeto '{TargetObject}', debe estar seguro que no es utilizado (referenciado) en ningún lugar.",
        InvertResult = true, FoundObjectMessageFormat = "'{0}'", FoundObjectMessagesSeparator = ";")]
    public class Tercero : XPObjectBaseBO
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            TipoPersona = TipoPersona.Natural;
            TipoContribuyente = TipoContribuyente.Gravado;
            Activo = true;
        }

        private System.Boolean activo;
        private TipoContribuyente tipoContribuyente;
        private TipoPersona tipoPersona;
        private System.String sitioWeb;
        private System.String eMail;
        TerceroDireccion direccionPrincipal;
        private System.String nombre;

        public Tercero(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        [DevExpress.Xpo.SizeAttribute(200), DbType("varchar(200)"), Index(0)]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Nombre")]
        [RuleRequiredField("Tercero.Nombre_Requerido", "Save")]
        public System.String Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }


        [XafDisplayName("Dirección Principal"), Index(1)]
        public TerceroDireccion DireccionPrincipal
        {
            get => direccionPrincipal;
            set => SetPropertyValue(nameof(DireccionPrincipal), ref direccionPrincipal, value);
        }

        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false), Index(2)]
        [DevExpress.Xpo.SizeAttribute(60), DbType("varchar(60)")]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Correo Electrónico")]
        public System.String EMail
        {
            get => eMail;
            set => SetPropertyValue(nameof(EMail), ref eMail, value);
        }

        [DevExpress.Xpo.SizeAttribute(60), DbType("varchar(60)"), Index(3)]
        [DevExpress.Persistent.Base.VisibleInListView(false)]
        public System.String SitioWeb
        {
            get => sitioWeb;
            set => SetPropertyValue(nameof(SitioWeb), ref sitioWeb, value);
        }

        [DevExpress.Persistent.Base.ImmediatePostDataAttribute, Index(4)]
        [RuleRequiredField("Tercero.TipoPersona_Requerido", "Save")]
        public TipoPersona TipoPersona
        {
            get => tipoPersona;
            set => SetPropertyValue(nameof(TipoPersona), ref tipoPersona, value);
        }

        [RuleRequiredField("Tercero.TipoContribuyente_Requerido", "Save"), VisibleInListView(false), Index(5)]
        public TipoContribuyente TipoContribuyente
        {
            get => tipoContribuyente;
            set => SetPropertyValue(nameof(TipoContribuyente), ref tipoContribuyente, value);
        }

        [Index(6), VisibleInLookupListView(true)]
        [RuleRequiredField("Tercero.Activo_Requerido", "Save")]
        public System.Boolean Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }

        [DevExpress.Xpo.AssociationAttribute("Tercero-Telefonos"), DevExpress.Xpo.Aggregated]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Teléfonos")]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        public XPCollection<TerceroTelefono> Telefonos
        {
            get
            {
                return GetCollection<TerceroTelefono>(nameof(Telefonos));
            }
        }

        [DevExpress.Xpo.Association("Tercero-Giros"), DevExpress.Xpo.Aggregated]
        [DevExpress.Persistent.Base.VisibleInLookupListView(false), DevExpress.Persistent.Base.ImmediatePostData]
        [DevExpress.ExpressApp.DC.XafDisplayName("Giros")]
        public XPCollection<TerceroGiro> Giros
        {
            get
            {
                return GetCollection<TerceroGiro>(nameof(Giros));
            }
        }

        [DevExpress.Xpo.Association("Tercero-Contactos"), XafDisplayName("Contactos")]
        public XPCollection<TerceroContacto> Contactos
        {
            get
            {
                return GetCollection<TerceroContacto>(nameof(Contactos));
            }
        }

        [DevExpress.Xpo.Association("Tercero-Roles"),
        DevExpress.Xpo.Aggregated]
        [DevExpress.Persistent.Base.ImmediatePostData]
        [DevExpress.Persistent.Base.VisibleInLookupListView(false)]
        [DevExpress.ExpressApp.DC.XafDisplayName("Roles")]
        public XPCollection<TerceroRole> Roles
        {
            get
            {
                return GetCollection<TerceroRole>(nameof(Roles));
            }
        }

        [DevExpress.Xpo.Association("Tercero-Notas"), DevExpress.Xpo.Aggregated]
        [DevExpress.ExpressApp.DC.XafDisplayName("Notas")]
        public XPCollection<TerceroNota> Notas
        {
            get
            {
                return GetCollection<TerceroNota>(nameof(Notas));
            }
        }

        [DevExpress.Xpo.Association("Tercero-Sucursales"), DevExpress.Xpo.Aggregated]
        [DevExpress.ExpressApp.DC.XafDisplayName("Sucursales")]
        public XPCollection<TerceroSucursal> Sucursales
        {
            get
            {
                return GetCollection<TerceroSucursal>(nameof(Sucursales));
            }
        }

        [Association("Tercero-Direcciones"), DevExpress.Xpo.Aggregated, XafDisplayName("Direcciones")]
        public XPCollection<TerceroDireccion> Direcciones
        {
            get
            {
                return GetCollection<TerceroDireccion>(nameof(Direcciones));
            }
        }

        [Association("Tercero-Documentos"), DevExpress.Xpo.Aggregated, XafDisplayName("Documentos")]
        public XPCollection<TerceroDocumento> Documentos
        {
            get
            {
                return GetCollection<TerceroDocumento>(nameof(Documentos));
            }
        }

        [Association("Tercero-Creditos"), XafDisplayName("Créditos"), DevExpress.Xpo.Aggregated]
        public XPCollection<TerceroCredito> Creditos
        {
            get
            {
                return GetCollection<TerceroCredito>(nameof(Creditos));
            }
        }

        [Association("Tercero-Garantias"), DevExpress.Xpo.Aggregated, XafDisplayName("Garantías")]
        public XPCollection<TerceroGarantia> Garantias
        {
            get
            {
                return GetCollection<TerceroGarantia>(nameof(Garantias));
            }
        }

        //[Association("Tercero-Bancos"), DevExpress.Xpo.Aggregated, XafDisplayName("Bancos")]
        //public XPCollection<Banco> Bancos
        //{
        //    get
        //    {
        //        return GetCollection<Banco>(nameof(Bancos));
        //    }
        //}

    }
}
