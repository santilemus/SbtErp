using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    [DefaultClassOptions]
    [ModelDefault("Caption", "Datos Generales"), XafDefaultProperty("NombreCompleto"), CreatableItem(false), NavigationItem(false)]
    [RuleCriteria("Persona.FechaNacimiento_FechaIngreso", DefaultContexts.Save, @"FechaIngreso >= FechaNacimiento", "Fecha Ingreso >= a Fecha de Nacimiento"),
     RuleCriteria("Persona.FechaIngreso_FechaRetiro", DefaultContexts.Save, @"FechaRetiro >= FechaIngreso", "Fecha Retiro >= a Fecha de Ingreso")]
    public class Persona : XPObjectBaseBO
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Genero = TipoGenero.NoAplica;
            EstadoCivil = EstadoCivil.Ninguno;

        }

        string causaMuerte;
        DateTime fechaMuerte;
        string idioma;
        private Listas _tipoSangre;
        private byte[] _fotografia;
        private System.DateTime _fechaRetiro;
        private System.DateTime _fechaIngreso;
        private EstadoCivil _estadoCivil;
        private TipoGenero _genero;
        private System.String _eMail;
        private System.DateTime _fechaNacimiento;
        private System.String _direccion;
        private ZonaGeografica _pais;
        private ZonaGeografica _provincia;
        private ZonaGeografica _ciudad;
        private System.String _apellido;
        private System.String _nombre;
        public Persona(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        #region Propiedades
        [DevExpress.Xpo.SizeAttribute(50), DbType("varchar(50)")]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Nombres")]
        [DevExpress.Persistent.Base.ToolTipAttribute("Nombres de la persona")]
        [RuleRequiredField("Persona.Nombre_Requerido", "Save")]
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
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Apellidos")]
        [DevExpress.Persistent.Base.ToolTipAttribute("Apellidos  de la persona")]
        [DevExpress.Xpo.SizeAttribute(50), DbType("varchar(50)")]
        [RuleRequiredField("Persona.Apellido_Requerido", "Save")]
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
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("País")]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Persistent.Base.VisibleInListViewAttribute(false)]
        [DevExpress.Persistent.Base.ToolTipAttribute("País de residencia")]
        [RuleRequiredField("Persona.Pais_Requerido", DefaultContexts.Save, "País es requerido")]
        [DataSourceCriteria("ZonaPadre is null and Activa = true")]
        [ExplicitLoading]
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
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [DevExpress.Persistent.Base.ToolTipAttribute("Provincia o departamento de residencia")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [RuleRequiredField("Persona.Provincia_Requerido", DefaultContexts.Save, "Provincia es requerido")]
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
        [DevExpress.Persistent.Base.ToolTipAttribute("Ciudad o zona geográfica de residencia")]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Ciudad")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [RuleRequiredField("Persona.Ciudad_Requerido", DefaultContexts.Save, "Ciudad es requerido")]
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
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Dirección"), DbType("varchar(100)")]
        [DevExpress.Persistent.Base.ToolTipAttribute("Dirección de residencia")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [RuleRequiredField("Persona.Direccion_Requerido", "Save", ResultType = ValidationResultType.Warning)]
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
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Fecha Nacimiento")]
        [DevExpress.Persistent.Base.ToolTipAttribute("Fecha de nacimiento")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [RuleRequiredField("Persona.FechaNacimiento_Requerido", "Save")]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
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

        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Xpo.SizeAttribute(60), DbType("varchar(60)")]
        [DevExpress.Persistent.Base.ToolTipAttribute("Direcciones de correo electrónico")]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Correo Electrónico")]
        [VisibleInListView(false)]
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
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Genero")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [RuleRequiredField("Persona.Genero_Requerido", "Save")]
        public TipoGenero Genero
        {
            get
            {
                return _genero;
            }
            set
            {
                SetPropertyValue("Genero", ref _genero, value);
            }
        }
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [RuleRequiredField("Persona.EstadoCivil_Requerido", "Save")]
        public EstadoCivil EstadoCivil
        {
            get
            {
                return _estadoCivil;
            }
            set
            {
                SetPropertyValue("EstadoCivil", ref _estadoCivil, value);
            }
        }
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [RuleRequiredField("Persona.FechaIngreso_Requerido", "Save")]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G"), VisibleInListView(false)]
        public System.DateTime FechaIngreso
        {
            get
            {
                return _fechaIngreso;
            }
            set
            {
                SetPropertyValue("FechaIngreso", ref _fechaIngreso, value);
            }
        }
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G"), VisibleInListView(false)]
        public System.DateTime FechaRetiro
        {
            get
            {
                return _fechaRetiro;
            }
            set
            {
                SetPropertyValue("FechaRetiro", ref _fechaRetiro, value);
            }
        }
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DataSourceCriteria("[Categoria] == 2 && [Activo] == True")]
        [ExplicitLoading]
        public Listas TipoSangre
        {
            get
            {
                return _tipoSangre;
            }
            set
            {
                SetPropertyValue("TipoSangre", ref _tipoSangre, value);
            }
        }


        [Size(25), XafDisplayName("Idiomas"), DbType("varchar(25)"), Persistent("Idioma"), VisibleInListView(false)]
        public string Idioma
        {
            get => idioma;
            set => SetPropertyValue(nameof(Idioma), ref idioma, value);
        }

        [DbType("datetime"), Persistent("FechaMuerte"), XafDisplayName("Fecha Muerte")]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G"), VisibleInListView(false)]
        public DateTime FechaMuerte
        {
            get => fechaMuerte;
            set => SetPropertyValue(nameof(FechaMuerte), ref fechaMuerte, value);
        }


        [Size(50), DbType("varchar(50)"), Persistent("CausaMuerte"), XafDisplayName("Causa Muerte"), VisibleInListView(false)]
        public string CausaMuerte
        {
            get => causaMuerte;
            set => SetPropertyValue(nameof(CausaMuerte), ref causaMuerte, value);
        }

        [PersistentAlias("concat(Nombre, Apellido)")]
        [DisplayName("Nombre Completo")]
        public string NombreCompleto
        {
            //get { return Convert.ToString(EvaluateAlias("NombreCompleto")); }
            get => ObjectFormatter.Format("{Nombre} {Apellido}", this, EmptyEntriesMode.RemoveDelimiterWhenEntryIsEmpty);
            }


        [PersistentAlias("Round(DateDiffMonth([FechaNacimiento], Now())/12.00, 2)")]
        [XafDisplayName("Edad"), VisibleInListView(false)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Edad
        {
            get { return Convert.ToDecimal(EvaluateAlias(nameof(Edad))); }
        }

        [Size(SizeAttribute.Unlimited), ImageEditor(ListViewImageEditorMode = ImageEditorMode.DropDownPictureEdit,
    DetailViewImageEditorMode = ImageEditorMode.PictureEdit, ListViewImageEditorCustomHeight = 34)]
        [Index(5), DetailViewLayout("Resumen del Error", LayoutGroupType.SimpleEditorsGroup, 1)]
        //[Delayed(true)]
        public byte[] Fotografia
        {
            get
            {
                return _fotografia;
            }
            set
            {
                SetPropertyValue("Fotografia", ref _fotografia, value);
            }
        }

        #endregion

        #region Colecciones
        [DevExpress.Xpo.AssociationAttribute("Telefonos-Persona"), DevExpress.Xpo.Aggregated]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Teléfonos")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [Index(0)]
        [ToolTip("Números de teléfono de contacto")]
        public XPCollection<PersonaTelefono> Telefonos
        {
            get
            {
                return GetCollection<PersonaTelefono>("Telefonos");
            }
        }

        [DevExpress.Xpo.AssociationAttribute("Documentos-Persona"), Index(1)]
        [DevExpress.Xpo.Aggregated]
        [ToolTip("Documentos personales y de identificación")]
        public XPCollection<PersonaDocumento> Documentos
        {
            get
            {
                return GetCollection<PersonaDocumento>("Documentos");
            }
        }

        [Association("Persona-Contactos"), XafDisplayName("Contactos"), Index(2), ToolTip("Personas a contactar en caso de emergencia"),
            DevExpress.Xpo.Aggregated]
        public XPCollection<PersonaContacto> Contactos
        {
            get
            {
                return GetCollection<PersonaContacto>(nameof(Contactos));
            }
        }

        #endregion

    }
}
