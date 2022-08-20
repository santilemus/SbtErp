using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Contabilidad.BusinessObjects;
using System;
using System.Linq;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// Objeto persistente que corresponde a las empresas del sistema
    /// </summary>
    /// <remarks>
    /// Ver infomacion de uso de @This en https://docs.devexpress.com/eXpressAppFramework/113204/concepts/filtering/current-object-parameter
    /// </remarks>
    [DefaultClassOptions, CreatableItem(false)]
    [DevExpress.Persistent.Base.NavigationItemAttribute("Catalogos")]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Empresa")]
    [DevExpress.ExpressApp.DC.XafDefaultPropertyAttribute("RazonSocial")]
    [DevExpress.Persistent.Base.ImageNameAttribute("company-info")]
    //[RuleIsReferenced("Empresa_Referencia", DefaultContexts.Delete, typeof(Empresa), nameof(Oid),
    //    MessageTemplateMustBeReferenced = "Para borrar el objeto '{TargetObject}', debe estar seguro que no es utilizado (referenciado) en ningún lugar.",
    //    InvertResult = true, FoundObjectMessageFormat = "'{0}'", FoundObjectMessagesSeparator = ";")]
    public class Empresa : XPObject
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Activa = true;
            Clasificacion = EClasificacionContribuyente.Otro;
            PorcentajeIva = 0.0m;
            PorcPercepcion = 0.0m;
            PorcRetencionIva = 0.0m;
        }

        decimal renta25;
        decimal renta20;
        decimal renta5;
        decimal renta10;
        decimal porcPercepcion;
        decimal porcRetencionIva;
        Moneda monedaDefecto;
        decimal porcentajeIva;
        ClaseSociedad claseSociedad;
        EClasificacionContribuyente clasificacion;
        string nrc;
        private System.Boolean _activa;
        private XPDelayedProperty _logo = new XPDelayedProperty();
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

        /// <summary>
        /// Pais de ubicacion de la empresa
        /// </summary>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("País")]
        [VisibleInListView(false), ImmediatePostData(true)]
        [RuleRequiredField("Empresa.Pais_Requerido", DefaultContexts.Save, "País es requerido")]
        [DataSourceCriteria("[ZonaPadre] Is Null && [Activa] == True")]
        [ExplicitLoading]
        public ZonaGeografica Pais
        {
            get => _pais;
            set => SetPropertyValue(nameof(Pais), ref _pais, value);
        }

        /// <summary>
        /// Provincia o departamento de ubicacion de la empresa
        /// </summary>
        /// <remarks>
        ///Informacion de @This en https://docs.devexpress.com/eXpressAppFramework/113204/concepts/filtering/current-object-parameter
        /// </remarks>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Departamento"), ImmediatePostData(true)]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false), VisibleInListView(false)]
        [RuleRequiredField("Empresa.Provincia_Requerido", DefaultContexts.Save, "Provincia es Requerido")]
        [DataSourceCriteria("[ZonaPadre] == '@This.Pais' and [Activa] == True")]
        [ExplicitLoading]
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
        //[DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [RuleRequiredField("Empresa.Ciudad_Requerido", DefaultContexts.Save, "Ciudad es Requerido")]
        [DataSourceCriteria("[ZonaPadre] == '@This.Provincia' and [Activa] == True")]
        [ExplicitLoading]
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

        [Size(14), DbType("varchar(14)"), XafDisplayName("NRC"), RuleRequiredField("Empresa.NRC_Requerido", "Save")]
        public string Nrc
        {
            get => nrc;
            set => SetPropertyValue(nameof(Nrc), ref nrc, value);
        }

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

        [DbType("smallint"), XafDisplayName("Clasificacion"), VisibleInListView(false), VisibleInLookupListView(false)]
        [ToolTip("Clasificación de la empresa como contribuyente")]
        public EClasificacionContribuyente Clasificacion
        {
            get => clasificacion;
            set => SetPropertyValue(nameof(Clasificacion), ref clasificacion, value);
        }


        [XafDisplayName("Clase Sociedad"), RuleRequiredField("Empresa.ClaseSociedad_Requerido", "Save")]
        public ClaseSociedad ClaseSociedad
        {
            get => claseSociedad;
            set => SetPropertyValue(nameof(ClaseSociedad), ref claseSociedad, value);
        }

        [XafDisplayName("Moneda Defecto")]
        [VisibleInListView(false)]
        [Delayed]
        public Moneda MonedaDefecto
        {
            get => GetDelayedPropertyValue<Moneda>(nameof(MonedaDefecto)); // monedaDefecto;
            set => SetDelayedPropertyValue(nameof(MonedaDefecto), value);
        }

        [DbType("numeric(10,4)"), XafDisplayName("Porcentaje Iva")]
        [ModelDefault("DisplayFormat", "{0:P4}"), ModelDefault("EditMask", "p4")]
        [RuleRange("Empresa.PorcentajeIva_RangoValido", DefaultContexts.Save, 0.0, 1.00)]
        [VisibleInListView(false)]
        public decimal PorcentajeIva
        {
            get => porcentajeIva;
            set => SetPropertyValue(nameof(PorcentajeIva), ref porcentajeIva, value);
        }

        [XafDisplayName("Factor Iva"), ModelDefault("DisplayFormat", "{0:N4}")]
        [VisibleInListView(false)]
        public decimal FactorIva => (1 + PorcentajeIva);

        [DbType("numeric(10,4)"), XafDisplayName("Porcentaje Retencion")]
        [ModelDefault("DisplayFormat", "{0:P4}"), ModelDefault("EditMask", "p4")]
        [RuleRange("Empresa.PorcRetencionIva_RangoValido", DefaultContexts.Save, 0.0, 1.00)]
        [VisibleInListView(false)]
        public decimal PorcRetencionIva
        {
            get => porcRetencionIva;
            set => SetPropertyValue(nameof(PorcRetencionIva), ref porcRetencionIva, value);
        }

        [DbType("numeric(10,4)"), XafDisplayName("Porcentaje Percepcion")]
        [ModelDefault("DisplayFormat", "{0:P4}"), ModelDefault("EditMask", "p4")]
        [RuleRange("Empresa.PorcPercepcion_RangoValido", DefaultContexts.Save, 0.0, 1.00)]
        [VisibleInListView(false)]
        public decimal PorcPercepcion
        {
            get => porcPercepcion;
            set => SetPropertyValue(nameof(PorcPercepcion), ref porcPercepcion, value);
        }

        [DbType("numeric(10,4)"), XafDisplayName("Renta 5")]
        [ModelDefault("DisplayFormat", "{0:P4}"), ModelDefault("EditMask", "p4")]
        [RuleRange("Empresa.Renta5_RangoValido", DefaultContexts.Save, 0.0, 1.00)]
        [ToolTip("Porcentaje de renta que se aplica entre otros a empresas por la compra de intangibles")]
        [VisibleInListView(false)]
        public decimal Renta5
        {
            get => renta5;
            set => SetPropertyValue(nameof(Renta5), ref renta5, value);
        }

        [DbType("numeric(10,4)"), XafDisplayName("Renta 10")]
        [ModelDefault("DisplayFormat", "{0:P4}"), ModelDefault("EditMask", "p4")]
        [RuleRange("Empresa.Renta10_RangoValido", DefaultContexts.Save, 0.0, 1.00)]
        [ToolTip("Porcentaje de renta que se aplica a la compra de servicios de personas naturales, intangibles, intereses entre otros")]
        [VisibleInListView(false)]
        public decimal Renta10
        {
            get => renta10;
            set => SetPropertyValue(nameof(Renta10), ref renta10, value);
        }

        [DbType("numeric(10,4)"), XafDisplayName("Renta 20")]
        [ModelDefault("DisplayFormat", "{0:P4}"), ModelDefault("EditMask", "p4")]
        [RuleRange("Empresa.Renta20_RangoValido", DefaultContexts.Save, 0.0, 1.00)]
        [ToolTip("Porcentaje de renta que se aplica a los no domiciliados por compra de cualquier servicio o renta")]
        [VisibleInListView(false)]
        public decimal Renta20
        {
            get => renta20;
            set => SetPropertyValue(nameof(Renta20), ref renta20, value);
        }

        [DbType("numeric(10,4)"), XafDisplayName("Renta 25")]
        [ModelDefault("DisplayFormat", "{0:P4}"), ModelDefault("EditMask", "p4")]
        [RuleRange("Empresa.Renta25_RangoValido", DefaultContexts.Save, 0.0, 1.00)]
        [ToolTip("Porcentaje de renta que se aplica a los sujetos constituidos, domiciliados o que residen en paraísos fiscales")]
        [VisibleInListView(false)]
        public decimal Renta25
        {
            get => renta25;
            set => SetPropertyValue(nameof(Renta25), ref renta25, value);
        }

        /// <summary>
        /// Imagen que corresponde al logo de la empresa
        /// </summary>
        /// <remarks>
        /// Se carga cuando se requiere ver info en https://docs.devexpress.com/XPO/2024/feature-center/data-exchange-and-manipulation/delayed-loading
        /// </remarks>
        //[DevExpress.Xpo.ValueConverterAttribute(typeof(DevExpress.Xpo.Metadata.ImageValueConverter))]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Persistent.Base.VisibleInListViewAttribute(false)]
        [ImageEditor(ListViewImageEditorMode = ImageEditorMode.PopupPictureEdit, DetailViewImageEditorMode = ImageEditorMode.PopupPictureEdit, ImageSizeMode = ImageSizeMode.StretchImage)]
        [Delayed(nameof(_logo), true)]
        public byte[] Logo
        {
            get => (byte[])_logo.Value; 
            set 
            {
                _logo.Value = value;
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

        [DevExpress.Xpo.AssociationAttribute("Empresa-Giros"), DevExpress.Xpo.Aggregated, XafDisplayName("Giros")]
        public XPCollection<EmpresaGiro> Giros
        {
            get
            {
                return GetCollection<EmpresaGiro>(nameof(Giros));
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

        [Association("Empresa-Usuarios"), /*DevExpress.Xpo.Aggregated,*/ XafDisplayName("Usuarios"), VisibleInDetailView(true)]
        public XPCollection<Usuario> Usuarios
        {
            get
            {
                return GetCollection<Usuario>(nameof(Usuarios));
            }
        }

        [Association("Empresa-Catalogos"), XafDisplayName("Cuentas")]
        [DataSourceCriteria("[CuentaEspecial] > 0 && [Empresa.Oid] == EmpresaActualOid()")]
        [ToolTip("Parametrización de las cuentas de liquidación y cierre del ejercicio contable")]
        public XPCollection<Catalogo> Cuentas
        {
            get
            {
                return GetCollection<Catalogo>(nameof(Cuentas));
            }
        }
    }

}
