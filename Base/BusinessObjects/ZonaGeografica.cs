using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System.ComponentModel;
using DevExpress.Xpo;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que representa las Zonas Geográficas en un catálogo Jerarquico, donde nivel superior Pais --> segundo nivel Estado o Provincia --> tercer nivel Ciudad
    /// Modificaciones
    /// 18/Junio/2020 por SELM
    /// 1. Se elimino la implementacion de ITreeNode, porque el editor tipo Tree del mantenimiento, tiene limitaciones
    ///    y es mas lenta la carga comparado con un ListView. La mayor desventaja es que DataAccess debe ser siempre 
    ///    ClientMode, lo cual es una limitacion seria cuando son muchos registros porque deben pasar todos al cliente
    ///    para construir el tree.
    /// </summary>
    [DefaultClassOptions, CreatableItem(false)]
    [DefaultProperty("Nombre")]
    [NavigationItem("Catalogos")]
    [ImageName("place_blue")]
    [RuleIsReferenced("ZonaGeografica_Referencia", DefaultContexts.Delete, typeof(ZonaGeografica), nameof(Codigo),
        MessageTemplateMustBeReferenced = "Para borrar el objeto '{TargetObject}', debe estar seguro que no es utilizado (referenciado) en ningún lugar.",
        InvertResult = true, FoundObjectMessageFormat = "'{0}'", FoundObjectMessagesSeparator = ";")]
    public class ZonaGeografica : XPCustomObject
    {
        /// <summary>
        /// Metodo para la inicialización de propiedades y/o objetos del BO. Se ejecuta una sola vez después de la creación del objeto
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Activa = true;

        }

        string moneda;
        string codigoTelefonico;
        private System.Boolean _activa;
        private ZonaGeografica _zonaPadre;
        private System.String _gentilicio;
        private System.String _nombre;
        private System.String _codigo;
        private string codigoPaisMH;
        public ZonaGeografica(DevExpress.Xpo.Session session)
          : base(session)
        {
        }
        [DevExpress.Xpo.SizeAttribute(8), DbType("varchar(8)")]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Código")]
        [DevExpress.Persistent.Base.ToolTipAttribute("Código de la Zona Geográfica")]
        [DevExpress.Xpo.KeyAttribute, VisibleInLookupListView(true), VisibleInListView(true)]
        [RuleRequiredField("ZonaGeografica.Codigo_Requerido", "Save"), Index(0)]
        [RuleUniqueValue("ZonaGeografica.Codigo_Unico", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
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
        [DevExpress.Xpo.SizeAttribute(60), DbType("varchar(60)"), Index(0)]
        [DevExpress.Persistent.Base.ToolTipAttribute("Nombre de la zona geográfica")]
        [RuleRequiredField("ZonaGeografica.Nombre_Requerido", "Save")]
        [RuleUniqueValue("ZonaGeografica.Nombre_Unico", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
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

        [DevExpress.Xpo.SizeAttribute(25), DbType("varchar(25)")]
        [DevExpress.Persistent.Base.ToolTipAttribute("Gentilicio para las personas de la zona geográfica")]
        [VisibleInListView(false)]
        public System.String Gentilicio
        {
            get
            {
                return _gentilicio;
            }
            set
            {
                SetPropertyValue("Gentilicio", ref _gentilicio, value);
            }
        }
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute, DbType("varchar(8)")]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Zona Padre")]
        [DevExpress.Persistent.Base.ToolTipAttribute("Código de la zona padre")]
        [DevExpress.Xpo.IndexedAttribute]
        [Association("ZonaPadre-Zonas")]
        public ZonaGeografica ZonaPadre
        {
            get
            {
                return _zonaPadre;
            }
            set
            {
                SetPropertyValue("ZonaPadre", ref _zonaPadre, value);
            }
        }


        [Size(4), XafDisplayName("Código Teléfonico"), DbType("varchar(4)"), VisibleInListView(true)]
        public string CodigoTelefonico
        {
            get => codigoTelefonico;
            set => SetPropertyValue(nameof(CodigoTelefonico), ref codigoTelefonico, value);
        }

        /// <summary>
        /// Código del país de acuerdo al catálogo emitido por el MH de El Salvaor
        /// </summary>
        [Size(4), DbType("varchar(4)"), System.ComponentModel.DisplayName("Código País MH")]
        [RuleRequiredField("ZonaGeografica.CodigoPaisMH_requerido", DefaultContexts.Save, SkipNullOrEmptyValues = true, 
            TargetCriteria = "Len(Trim([Codigo])) == 3")]
        [RuleUniqueValue("ZonaGeografica.CodigoPaisMH_Unico", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction,
            IncludeCurrentObject = true)]
        [ToolTip(@"Código del país de acuerdo a la codificación emitida por el Ministerio de Haciena para la factuaración electrónica")]
        [Indexed(Name = "idxZonaGeografica_CodigoPaisMH")]
        public string CodigoPaisMH
        {
            get => codigoPaisMH;
            set => SetPropertyValue(nameof(CodigoPaisMH), ref codigoPaisMH, value);
        }

        /// <summary>
        /// Moneda usada en el pais, agregar validacion para que solo se ingrese cuando la zona geografica padre es nula
        /// </summary>
        [Size(3), DbType("varchar(3)"), XafDisplayName("Moneda"), VisibleInListView(false), VisibleInLookupListView(false)]
        [Appearance("ZonaGeografica.Moneda_Ocultar", Criteria = "Not([ZonaPadre] Is Null)",
            Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "DetailView")]
        [RuleRequiredField("ZonaGeografica.Moneda_Requerida", DefaultContexts.Save, TargetCriteria = "[ZonaPadre] Is Null", SkipNullOrEmptyValues = true)]
        public string Moneda
        {
            get => moneda;
            set => SetPropertyValue(nameof(Moneda), ref moneda, value);
        }


        [RuleRequiredField("ZonaGeografica.Activa_Requerido", "Save")]
        public System.Boolean Activa
        {
            get => _activa;
            set => SetPropertyValue(nameof(Activa), ref _activa, value);
        }

        #region Colleciones
        [Association("ZonaPadre-Zonas"), DevExpress.Xpo.Aggregated]
        public XPCollection<ZonaGeografica> Zonas
        {
            get
            {
                return GetCollection<ZonaGeografica>(nameof(Zonas));
            }
        }
        #endregion
    }
}
