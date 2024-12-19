using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Tercero.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que corresponde a los terceros. Es la clase base para clientes, proveedores, bancos, etc
    /// </summary>
    [DefaultClassOptions, XafDisplayName("Terceros"), ImageName(nameof(Tercero)), NavigationItem("Catalogos"), XafDefaultProperty(nameof(Nombre))]
    [FriendlyKeyProperty(nameof(Nombre))]
    // reglas nuevas agregadas el 10/07/2024 para evitar que ingresen terceros sin NIT o DUI
    [RuleCriteria("Terceros.TerceroDocumento si Nit or DUI", DefaultContexts.Save, "!([Documentos][[Tipo.Codigo] In ('NIT, DUI')])")]
    // reglas nuevas agregadas el 10/07/2024 para evitar que borren terceros con objetos relacionados en otros BO
    [RuleIsReferenced("Tercero con direccion", DefaultContexts.Delete, typeof(TerceroDireccion), "Tercero", InvertResult = true,
        CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction,
        MessageTemplateMustBeReferenced = @"No puede borrar el objeto '{TargetObject}' porque se utiliza en la aplicación")]
    public class Tercero : XPObjectBaseBO
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            TipoPersona = TipoPersona.Juridica;
            TipoContribuyente = ETipoContribuyente.Gravado;
            Origen = ETerceroOrigen.Nacional;
            Activo = true;
        }

        ETerceroOrigen origen;
        EClasificacionContribuyente clasificacion;
        private System.Boolean activo;
        private ETipoContribuyente tipoContribuyente;
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
        [DataSourceCriteria("[Tercero] == '@This.Oid'")]
        [ExplicitLoading(1)]
        public TerceroDireccion DireccionPrincipal
        {
            get => direccionPrincipal;
            set
            {
                bool changed = SetPropertyValue(nameof(DireccionPrincipal), ref direccionPrincipal, value);
                if (!IsLoading && !IsSaving && changed &&
                    (Session.IsNewObject(this) || Direcciones.Count<TerceroDireccion>(y => y.Oid == DireccionPrincipal.Oid) == 0))
                {
                    Direcciones.Add(DireccionPrincipal);
                }
            }
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
        public TipoPersona TipoPersona
        {
            get => tipoPersona;
            set => SetPropertyValue(nameof(TipoPersona), ref tipoPersona, value);
        }

        [VisibleInListView(false), Index(5)]
        [XafDisplayName("Tipo Contribuyente")]
        public ETipoContribuyente TipoContribuyente
        {
            get => tipoContribuyente;
            set => SetPropertyValue(nameof(TipoContribuyente), ref tipoContribuyente, value);
        }

        [DbType("smallint"), XafDisplayName("Clasificacion"), Index(6)]
        [ToolTip("Clasificación del tercero como contribuyente")]
        [VisibleInListView(false), VisibleInLookupListView(false)]
        public EClasificacionContribuyente Clasificacion
        {
            get => clasificacion;
            set => SetPropertyValue(nameof(Clasificacion), ref clasificacion, value);
        }

        [DbType("smallint"), XafDisplayName("Origen")]
        public ETerceroOrigen Origen
        {
            get => origen;
            set => SetPropertyValue(nameof(Origen), ref origen, value);
        }

        [Index(8), VisibleInLookupListView(true)]
        public System.Boolean Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }

        [Browsable(false), VisibleInReports(true)]
        public string Nit
        {
            get
            {
                var nit = Documentos.FirstOrDefault<TerceroDocumento>(x => x.Tipo.Codigo == "NIT");
                return (nit != null) ? nit.Numero: null;
            }
        }

        [Browsable(false), VisibleInReports(true)]
        public string Dui
        {
            get
            {
                if (TipoPersona == TipoPersona.Juridica)
                    return null;
                else
                {
                    var dui = Documentos.FirstOrDefault<TerceroDocumento>(x => x.Tipo.Codigo == "DUI");
                    return (dui != null) ? dui.Numero : string.Empty;
                }
            }
        }

        [Association("Tercero-Telefonos"), XafDisplayName("Teléfonos")]
        [RuleRequiredField("Tercero.Telefonos_requerido", DefaultContexts.Save, CustomMessageTemplate = @"{TargetPropertyName} no debe estar vacío")]
        public XPCollection<TerceroTelefono> Telefonos => GetCollection<TerceroTelefono>(nameof(Telefonos));

        [Association("Tercero-Giros"), DevExpress.Xpo.Aggregated, XafDisplayName("Giros")]
        [RuleRequiredField("Tercero.Giros_requerido", DefaultContexts.Save, CustomMessageTemplate = @"{TargetPropertyName} no debe estar vacío")]
        public XPCollection<TerceroGiro> Giros => GetCollection<TerceroGiro>(nameof(Giros));

        [DevExpress.Xpo.Association("Tercero-Contactos"), XafDisplayName("Contactos")]
        public XPCollection<TerceroContacto> Contactos => GetCollection<TerceroContacto>(nameof(Contactos));

        [DevExpress.Xpo.Association("Tercero-Roles"), DevExpress.Xpo.Aggregated]
        [XafDisplayName("Roles")]
        [RuleRequiredField("Tercero.Roles_requerido", DefaultContexts.Save, CustomMessageTemplate = @"{TargetPropertyName} no debe estar vacío", 
            ResultType = ValidationResultType.Warning)]
        public XPCollection<TerceroRole> Roles => GetCollection<TerceroRole>(nameof(Roles));

        [Association("Tercero-Notas"), DevExpress.Xpo.Aggregated, XafDisplayName("Notas")]
        public XPCollection<TerceroNota> Notas => GetCollection<TerceroNota>(nameof(Notas));

        [Association("Tercero-Sucursales"), DevExpress.Xpo.Aggregated, XafDisplayName("Sucursales")]
        public XPCollection<TerceroSucursal> Sucursales => GetCollection<TerceroSucursal>(nameof(Sucursales));

        [Association("Tercero-Direcciones"), XafDisplayName("Direcciones")]
        [RuleRequiredField("Tercero.Dirección_requerido", DefaultContexts.Save, CustomMessageTemplate = @"{TargetPropertyName} no debe estar vacío")]
        public XPCollection<TerceroDireccion> Direcciones => GetCollection<TerceroDireccion>(nameof(Direcciones));

        [Association("Tercero-Documentos"), DevExpress.Xpo.Aggregated, XafDisplayName("Documentos")]
        [RuleRequiredField("Tercero.Documentos_requerido", DefaultContexts.Save, CustomMessageTemplate = @"{TargetPropertyName} no debe estar vacío")]
        public XPCollection<TerceroDocumento> Documentos => GetCollection<TerceroDocumento>(nameof(Documentos));

        [Association("Tercero-Creditos"), XafDisplayName("Créditos"), DevExpress.Xpo.Aggregated]
        public XPCollection<TerceroCredito> Creditos => GetCollection<TerceroCredito>(nameof(Creditos));

        [Association("Tercero-Garantias"), DevExpress.Xpo.Aggregated, XafDisplayName("Garantías")]
        public XPCollection<TerceroGarantia> Garantias => GetCollection<TerceroGarantia>(nameof(Garantias));

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
