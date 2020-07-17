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
    [DefaultClassOptions, XafDefaultProperty(nameof(Nombre))]
    [ModelDefault("Caption", "Categoría Productos"), NavigationItem("Catalogos")]
    [RuleIsReferenced("Categoria_Referencia", DefaultContexts.Delete, typeof(Categoria), nameof(Oid),
        MessageTemplateMustBeReferenced = "Para borrar el objeto '{TargetObject}', debe estar seguro que no es utilizado (referenciado) en ningún lugar.",
        InvertResult = true, FoundObjectMessageFormat = "'{0}'", FoundObjectMessagesSeparator = ";")]
    [ImageName("CategoriaProducto")]
    [Persistent("ProCategoria")]
    public class Categoria : XPObjectBaseBO
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            EsGrupo = false;
            Activa = true;
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


        private Categoria padre;
        private EClasificacion clasificacion = EClasificacion.ProductoTerminado;
        private System.Boolean esGrupo = false;
        private System.Boolean activa = true;
        private System.String nombre;
        private System.String codigo;
        [Persistent(nameof(Nivel)), DbType("smallint")]
        private int nivel = 1;

        public Categoria(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        [ImmediatePostData(true)]
        [RuleRequiredField("Categoria.Padre_Requerido", DefaultContexts.Save, "Padre es requerido", SkipNullOrEmptyValues = true), VisibleInLookupListView(false)]
        public Categoria Padre
        {
            get => padre;
            set
            {
                bool changed = SetPropertyValue("Padre", ref padre, value);
                if (!IsLoading && !IsSaving && changed)
                {
                    nivel = Padre.Nivel + 1;
                    Codigo = value.Codigo;
                    Clasificacion = value.Clasificacion;
                }
            }
        }

        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Código")]
        [DevExpress.Xpo.SizeAttribute(20), DbType("varchar(20)")]
        [RuleRequiredField("Categoria.Codigo_Requerido", "Save")]
        [RuleUniqueValue]
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

        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Clasificación"), VisibleInLookupListView(false)]
        [RuleRequiredField("Categoria.Clasificacion_Requerido", "Save")]
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

        [XafDisplayName("Nivel")]
        [PersistentAlias(nameof(nivel))]
        public int Nivel => nivel;

        [RuleRequiredField("Categoria.Activa_Requerido", "Save"), XafDisplayName("Activa")]
        public System.Boolean Activa
        {
            get => activa;
            set => SetPropertyValue(nameof(Activa), ref activa, value);
        }


    }
}
