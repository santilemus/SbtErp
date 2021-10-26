﻿using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Inventario.Module.BusinessObjects;
using System;
using System.ComponentModel;
using DevExpress.ExpressApp.Model;
using System.Linq;

namespace SBT.Apps.Producto.Module.BusinessObjects
{

    /// <summary>
    /// Clase Persistente que corresponde al catálogo de Productos
    /// </summary> 
    [DefaultClassOptions, NavigationItem("Inventario"), DefaultProperty(nameof(Nombre))]
    [DevExpress.Persistent.Base.ImageNameAttribute("account_book")]
    [RuleIsReferenced("Producto_Referencia", DefaultContexts.Delete, typeof(Producto), nameof(Oid),
        MessageTemplateMustBeReferenced = "Para borrar el objeto '{TargetObject}', debe estar seguro que no es utilizado (referenciado) en ningún lugar.",
        InvertResult = true, FoundObjectMessageFormat = "'{0}'", FoundObjectMessagesSeparator = ";")]
    [ListViewFilter("Todos", "")]
    [ListViewFilter("Producto Terminado", "[Categoria.Clasificacion] = 0")]
    [ListViewFilter("Servicios", "[Categoria.Clasificacion] = 4")]

    [Appearance("Productos - Servicios - Intangibles y Otros", AppearanceItemType = "ViewItem", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide,
            Context = "Any", Criteria = "[Categoria.Clasificacion] >= 4",
            TargetItems = "CodigoBarra;NombreCorto;UnidadMedida;Presentacion;CantMinima;CantMaxima;CostoPromedio;Atributos;ItemsEnsamble;Equivalentes;Lotes;CodigosBarra;Inventarios")]
    public class Producto : XPObjectBaseBO
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Activo = true;
            CantMinima = 0.0m;
            CantMaxima = 0.0m;
            costoPromedio = 0.0m;
        }

        decimal costoPromedio;
        Presentacion presentacion;
        private Categoria categoria;
        private System.String codigo;
        private System.Decimal cantMaxima;
        private System.Decimal cantMinima;
        private Empresa empresa;
        private System.String comentario;
        private System.Boolean activo;
        private System.String nombre;
        private System.String nombreCorto;
        private string unidadMedida;
        private ProductoCodigoBarra codigoBarra;
        public Producto(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        #region Propiedades

        [DevExpress.Persistent.Base.ToolTipAttribute("Línea del producto")]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Categoría")]
        [DevExpress.Xpo.IndexedAttribute]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [RuleRequiredField("Producto.Categoria_Requerido", DefaultContexts.Save, "La Categoría es requerida")]
        [DataSourceCriteria("[EsGrupo] == false && [Activa] == true && [Categorias][].Count() == 0")]
        //[Association("Categoria-Productos")]
        [ExplicitLoading]
        public Categoria Categoria
        {
            get => categoria;
            set
            {
                bool changed = SetPropertyValue(nameof(Categoria), ref categoria, value);
                if (!IsLoading && !IsSaving && changed & Session.IsNewObject(this))
                    Codigo = Categoria.Codigo;
            }
        }

        [DevExpress.Xpo.SizeAttribute(20), VisibleInLookupListView(true), DbType("varchar(20)")]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Código")]
        [RuleRequiredField("Producto.Codigo_Requerido", "Save")]
        public System.String Codigo
        {
            get => codigo;
            set => SetPropertyValue(nameof(Codigo), ref codigo, value);
        }

        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Código Barra")]
        [DevExpress.Persistent.Base.ToolTipAttribute("Código de barra principal o por defecto del producto")]
        //[NoForeignKey]
        [ExplicitLoading]
        public ProductoCodigoBarra CodigoBarra
        {
            get => codigoBarra;
            set => SetPropertyValue(nameof(CodigoBarra), ref codigoBarra, value);
        }

        [DbType("varchar(100)")]
        [RuleRequiredField("Producto.Nombre_Requerido", "Save")]
        public System.String Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }


        [DevExpress.Xpo.SizeAttribute(25), DbType("varchar(25)")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Persistent.Base.VisibleInListViewAttribute(false)]
        //[RuleRequiredField("Producto.NombreCorto_Requerido", "Save")]
        public System.String NombreCorto
        {
            get => nombreCorto;
            set => SetPropertyValue(nameof(NombreCorto), ref nombreCorto, value);
        }

        [DbType("varchar(8)"), Persistent("UnidadMedida")]
        [Size(8), XafDisplayName("Unidad Medida")]
        [RuleRequiredField("Producto.UnidadMedida_Requerido", "Save", TargetCriteria = "[Categoria.Clasificacion] < 4"), VisibleInLookupListView(false)]
        public string UnidadMedida
        {
            get => unidadMedida;
            set => SetPropertyValue(nameof(UnidadMedida), ref unidadMedida, value);
        }

        [Association("Presentacion-Productos"), XafDisplayName("Presentación"), Persistent("Presentacion"), VisibleInListView(false)]
        [ExplicitLoading]
        public Presentacion Presentacion
        {
            get => presentacion;
            set => SetPropertyValue(nameof(Presentacion), ref presentacion, value);
        }

        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Cantidad Mínima")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Persistent.Base.VisibleInListViewAttribute(false)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [RuleValueComparison("Producto.CantidadMinima_Mayor_o_Igual_0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0)]
        public System.Decimal CantMinima
        {
            get => cantMinima;
            set => SetPropertyValue(nameof(CantMinima), ref cantMinima, value);
        }

        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Cantidad Máxima")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [DevExpress.Persistent.Base.VisibleInListViewAttribute(false)]
        public System.Decimal CantMaxima
        {
            get => cantMaxima;
            set => SetPropertyValue(nameof(CantMaxima), ref cantMaxima, value);
        }

        [DbType("numeric(14, 6)"), XafDisplayName("Costo Promedio")]
        [RuleValueComparison("Producto.CostoPromedio >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0.0,
            TargetCriteria = "[Categoria.Clasificacion] < 4 && [Activo] == true", CustomMessageTemplate = "El costo promedio debe ser mayor o igual a cero")]
        public decimal CostoPromedio
        {
            get => costoPromedio;
            set => SetPropertyValue(nameof(CostoPromedio), ref costoPromedio, value);
        }

        [DevExpress.Xpo.SizeAttribute(200), DbType("varchar(200)"), VisibleInListView(false)]
        public System.String Comentario
        {
            get => comentario;
            set => SetPropertyValue(nameof(Comentario), ref comentario, value);
        }



        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        public System.Boolean Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }

        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Empresa")]
        [Browsable(false)]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }

        #endregion

        #region Colecciones
        [DevExpress.Xpo.AssociationAttribute("Precios-Producto"), XafDisplayName("Precios"),
        DevExpress.Xpo.Aggregated]
        public XPCollection<ProductoPrecio> Precios
        {
            get
            {
                return GetCollection<ProductoPrecio>("Precios");
            }
        }

        [AssociationAttribute("ItemsEnsamble-Producto"), DevExpress.Xpo.Aggregated, XafDisplayName("Ensamble Items")]
        public XPCollection<ProductoEnsamble> ItemsEnsamble
        {
            get
            {
                return GetCollection<ProductoEnsamble>("ItemsEnsamble");
            }
        }
        [DevExpress.Xpo.AssociationAttribute("Atributos-Producto"), DevExpress.Xpo.Aggregated, XafDisplayName("Atributos")]
        public XPCollection<ProductoAtributo> Atributos
        {
            get
            {
                return GetCollection<ProductoAtributo>("Atributos");
            }
        }
        [DevExpress.Xpo.AssociationAttribute("Equivalentes-Producto"), DevExpress.Xpo.Aggregated, XafDisplayName("Equivalentes")]
        public XPCollection<ProductoEquivalente> Equivalentes
        {
            get
            {
                return GetCollection<ProductoEquivalente>("Equivalentes");
            }
        }
        [DevExpress.Xpo.AssociationAttribute("Proveedores-Producto"),
        DevExpress.Xpo.Aggregated]
        public XPCollection<ProductoProveedor> Proveedores
        {
            get
            {
                return GetCollection<ProductoProveedor>("Proveedores");
            }
        }
        [DevExpress.Xpo.AssociationAttribute("Lotes-Producto"), DevExpress.Xpo.Aggregated, XafDisplayName("Lotes")]
        public XPCollection<InventarioLote> Lotes
        {
            get
            {
                return GetCollection<InventarioLote>("Lotes");
            }
        }
        [DevExpress.Xpo.AssociationAttribute("CodigosBarra-Producto"), DevExpress.Xpo.AggregatedAttribute, XafDisplayName("Códigos de Barra")]
        public XPCollection<ProductoCodigoBarra> CodigosBarra
        {
            get
            {
                return GetCollection<ProductoCodigoBarra>("CodigosBarra");
            }
        }

        [Association("Producto-Inventarios"), DevExpress.Xpo.Aggregated, XafDisplayName("Inventario")]
        public XPCollection<Inventario.Module.BusinessObjects.Inventario> Inventarios
        {
            get
            {
                return GetCollection<Inventario.Module.BusinessObjects.Inventario>(nameof(Inventarios));
            }
        }

        #endregion

        #region Metodos
        //protected override void OnSaving()
        //{
        //    base.OnSaving();


        //}

        #endregion

    }
}
