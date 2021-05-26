using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Producto.Module.BusinessObjects
{
    /// <summary>
    /// BO. Categoría de los Productos.
    /// El propósito es crear un agrupamiento jerarquico, donde los productos estarán vinculados al último nivel (detalle)
    /// Lo anterior es para no hacer jerarquico el catálogo de productos, sino el atributo de agrupamiento
    /// </summary>
    [DefaultClassOptions, XafDefaultProperty(nameof(Nombre)), CreatableItem(false)]
    [ModelDefault("Caption", "Categoría Productos"), NavigationItem("Catalogos")]
    [RuleIsReferenced("Categoria_Referencia", DefaultContexts.Delete, typeof(Categoria), nameof(Oid),
        MessageTemplateMustBeReferenced = "Para borrar el objeto '{TargetObject}', debe estar seguro que no es utilizado (referenciado) en ningún lugar.",
        InvertResult = true, FoundObjectMessageFormat = "'{0}'", FoundObjectMessagesSeparator = ";")]
    [ImageName("CategoriaProducto")]
    [Persistent("ProCategoria")]
    public class Categoria : XPObject
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            EsGrupo = false;
            Activa = true;
            Constante constante = Session.GetObjectByKey<Constante>("PORCENTAJE_IVA");
            if (constante != null)
                porcentajeIva = Convert.ToDecimal(constante.Valor);
        }

        //protected override void OnChanged(string propertyName, object oldValue, object newValue)
        //{
        //    base.OnChanged(propertyName, oldValue, newValue);
        //}

        /// <summary>
        /// Validación que debe dispararse cuando la categoría no es grupo (es el último nivel o detalle) y además tiene subcategorías.
        /// Aplica para la propiedad EsGrupo
        /// </summary>
        [NonPersistent, Browsable(false)]
        [RuleFromBoolProperty("Categoria.Grupo_NoModificar", DefaultContexts.Save, "No puede modificar la Propiedad es Grupo porque la categoría tiene uno o más hijos"
            , UsedProperties = "EsGrupo", SkipNullOrEmptyValues = false)]
        protected bool IsGrupAndHaveChild
        {
            get { return (!EsGrupo && Hijos(this) == 0) || (EsGrupo && (Hijos(this) >= 0)); }
        }


        private int Hijos(Categoria aCategoria)
        {
            return Convert.ToInt32(Session.Evaluate<Categoria>(CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Padre.Oid] == ?", aCategoria.Oid)));
        }

        public Categoria(DevExpress.Xpo.Session session) : base(session)
        {
        }

        #region Propiedades

        EMetodoCosteoInventario metodoCosteo;
        decimal porcentajeIva;
        EClasificacionIVA clasificacionIva = EClasificacionIVA.Gravado;
        private Categoria padre;
        private EClasificacion clasificacion = EClasificacion.ProductoTerminado;
        private System.Boolean esGrupo;
        private System.Boolean activa = true;
        private System.String nombre;
        private System.String codigo;

        [ImmediatePostData(true)]
        [RuleRequiredField("Categoria.Padre_Requerido", DefaultContexts.Save, "Padre es requerido", SkipNullOrEmptyValues = true), VisibleInLookupListView(false)]
        [ExplicitLoading]
        public Categoria Padre
        {
            get => padre;
            set
            {
                bool changed = SetPropertyValue("Padre", ref padre, value);
                if (!IsLoading && !IsSaving && changed && Session.IsNewObject(this))
                {
                    Codigo = value.Codigo;
                    Clasificacion = value.Clasificacion;
                }
            }
        }

        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Código")]
        [DevExpress.Xpo.SizeAttribute(20), DbType("varchar(20)")]
        [RuleRequiredField("Categoria.Codigo_Requerido", "Save")]
        [RuleUniqueValue]
        [VisibleInLookupListView(true)]
        public System.String Codigo
        {
            get => codigo;
            set => SetPropertyValue(nameof(Codigo), ref codigo, value);
        }

        [DevExpress.Xpo.SizeAttribute(150), DbType("varchar(150)")]
        [RuleRequiredField("Categoria.Nombre_Requerido", "Save")]
        public System.String Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        /// <summary>
        /// Clasificacion de la categoria de productos. Ejemplos: Producto Terminado, Materia Prima, Servicios, etc.
        /// </summary>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Clasificación"), VisibleInLookupListView(false)]
        [RuleRequiredField("Categoria.Clasificacion_Requerido", "Save"), Persistent(nameof(Clasificacion))]
        public EClasificacion Clasificacion
        {
            get => clasificacion;
            set => SetPropertyValue(nameof(Clasificacion), ref clasificacion, value);
        }

        [DevExpress.Persistent.Base.ImmediatePostDataAttribute(true)]
        [DevExpress.Persistent.Base.VisibleInListViewAttribute(false)]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Es Grupo")]
        public System.Boolean EsGrupo
        {
            get => esGrupo;
            set => SetPropertyValue(nameof(EsGrupo), ref esGrupo, value);
        }

        /// <summary>
        /// Clasificacion de los bienes y servicios de acuerdo a la base imponible para el calculo del IVA
        /// Pueden Ser: Gravados, Exentos, Excluidos. Ver la enumeracion EClasificacionIVA para mas detalle
        /// </summary>
        [DbType("smallint"), Persistent(nameof(ClasificacionIva)), XafDisplayName("Clasificación Iva")]
        [RuleRequiredField("Producto.ClasificacionIva_Requerido", "Save"), VisibleInListView(false)]
        public EClasificacionIVA ClasificacionIva
        {
            get => clasificacionIva;
            set => SetPropertyValue(nameof(Clasificacion), ref clasificacionIva, value);
        }

        /// <summary>
        /// Porcentaje de IVA aplicado a la categoria de Productos
        /// </summary>
        [DbType("numeric(14,4)"), XafDisplayName("Porcentaje IVA")]
        [ModelDefault("DisplayFormat", "{0:P2}"), ModelDefault("EditMask", "p2")]
        [VisibleInListView(false), VisibleInLookupListView(false)]
        public decimal PorcentajeIVA
        {
            get => porcentajeIva;
            set => SetPropertyValue(nameof(PorcentajeIVA), ref porcentajeIva, value);
        }

        /// <summary>
        /// Metodo de costeo del inventario para los productos de esta categoria
        /// </summary>
        [DbType("smallint"), XafDisplayName("Método Costeo Inventario"), Persistent(nameof(MetodoCosteo))]
        public EMetodoCosteoInventario MetodoCosteo
        {
            get => metodoCosteo;
            set => SetPropertyValue(nameof(MetodoCosteo), ref metodoCosteo, value);
        }

        [XafDisplayName("Nivel")]
        [PersistentAlias("Iif(!IsNull([Padre]), [Padre.Nivel] + 1, 1)")]
        public int Nivel => Convert.ToInt16(nameof(Nivel));

        [RuleRequiredField("Categoria.Activa_Requerido", "Save"), XafDisplayName("Activa")]
        public System.Boolean Activa
        {
            get => activa;
            set => SetPropertyValue(nameof(Activa), ref activa, value);
        }

        #endregion

        #region Colecciones
        //[Association("Categoria-Productos"), DevExpress.Xpo.Aggregated, XafDisplayName("Productos"), Index(0)]
        //public XPCollection<Producto> Productos => GetCollection<Producto>(nameof(Productos));

        [Association("Categoria-TributosCategoria"), DevExpress.Xpo.Aggregated, XafDisplayName("Tributos"), Index(1)]
        public XPCollection<TributoCategoria> TributosCategoria => GetCollection<TributoCategoria>(nameof(TributosCategoria));
        #endregion
    }
}
