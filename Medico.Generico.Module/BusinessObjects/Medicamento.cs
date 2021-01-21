using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.Xpo;
using System.Linq;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using SBT.Apps.Producto.Module.BusinessObjects;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.ConditionalAppearance;

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    /// <summary>
    ///  Medicamentos hereda de Productos. Solo aplica cuando son medicamentos
    ///  Las pestañas Dosis y Vacunas solo se muestran cuando la categoria del producto => (medicamento) es Producto Terminado (Categoria.Clasificacion = 0)
    ///  y se ocultan en los otros casos
    /// </summary>
	[DefaultClassOptions]
    [DevExpress.Persistent.Base.ImageNameAttribute("medicamento")]
    [DevExpress.ExpressApp.DC.XafDefaultPropertyAttribute("Nombre")]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Medicamentos")]
    [DevExpress.Persistent.Base.NavigationItemAttribute("Inventario")]  
    //[MapInheritance(MapInheritanceType.ParentTable)]
    [RuleIsReferenced("Medicamente_Referencia", DefaultContexts.Delete, typeof(Medicamento), nameof(Oid),
        MessageTemplateMustBeReferenced = "Para borrar el objeto '{TargetObject}', debe estar seguro que no es utilizado (referenciado) en ningún lugar.",
        InvertResult = true, FoundObjectMessageFormat = "'{0}'", FoundObjectMessagesSeparator = ";")]
    [Appearance("MedicamentosDosisHide", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "MedicamentoDosis_ListView", 
        Criteria = "[Categoria.Clasificacion] != 0", TargetItems = "MedicDosis")]
    [Appearance("MedicamentosDosisShow", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show, Context = "MedicamentoDosis_ListView", 
        Criteria = "[Categoria.Clasificacion] = 0", TargetItems = "MedicDosis")]
    [Appearance("MedicamentoVacunaHide", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Medicamento_Vacunas_ListView",
        Criteria = "[Categoria.Clasificacion] != 0", TargetItems = "Vacunas")]
    [Appearance("MedicamentosVacunaShow", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show, Context = "Medicamento_Vacunas_ListView",
        Criteria = "[Categoria.Clasificacion] = 0", TargetItems = "Vacunas")]
    public class Medicamento: Producto.Module.BusinessObjects.Producto
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();

        }

        #region Propiedades 
        ENivelUsoMedicamento nivelUso = ENivelUsoMedicamento.P;
        EClasificacionVEN prioridad = EClasificacionVEN.NoEsenciales;
        private System.String _via;
        private System.String _concentracion;
        private System.String _contraIndicacion;
        public Medicamento(DevExpress.Xpo.Session session)
          : base(session)
        {
        }
        [DevExpress.Xpo.SizeAttribute(200), DbType("varchar(200)")]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("ContraIndicación")]
        [RuleRequiredField("Medicamente.ContraIndicacion_Requerido", "Save")]
        public System.String ContraIndicacion
        {
            get
            {
                return _contraIndicacion;
            }
            set
            {
                SetPropertyValue("ContraIndicacion", ref _contraIndicacion, value);
            }
        }
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Concentración"), DbType("varchar(100)")]
        public System.String Concentracion
        {
            get
            {
                return _concentracion;
            }
            set
            {
                SetPropertyValue("Concentracion", ref _concentracion, value);
            }
        }
        [DevExpress.Xpo.SizeAttribute(25), DbType("varchar(25)")]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Vía")]
        [RuleRequiredField("Diagnostico.Via_Requerido", "Save")]
        public System.String Via
        {
            get
            {
                return _via;
            }
            set
            {
                SetPropertyValue("Via", ref _via, value);
            }
        }

        [DbType("smallint"), Persistent("Prioridad"), XafDisplayName("Prioridad"), 
            RuleRequiredField("Medicamento.Prioridad_Requerido", DefaultContexts.Save)]
        public EClasificacionVEN Prioridad
        {
            get => prioridad;
            set => SetPropertyValue(nameof(Prioridad), ref prioridad, value);
        }

        [DbType("smallint"), Persistent("NivelUso"), XafDisplayName("Nivel Uso"), 
            RuleRequiredField("Medicamento.NivelUso_Requerido", "Save")]
        public ENivelUsoMedicamento NivelUso
        {
            get => nivelUso;
            set => SetPropertyValue(nameof(NivelUso), ref nivelUso, value);
        }

        [PersistentAlias("Iif([Prioridad] == 0, 'N', Iif([Prioridad] == 1, 'E', 'V'))")]
        [XafDisplayName("Código VEN")]
        public string CodigoVEN
        {
            get { return Convert.ToString(EvaluateAlias(nameof(CodigoVEN))); }
        }

        #endregion

        #region Colecciones

        [DevExpress.Xpo.AssociationAttribute("MedicamentoDosises-Medicamento"), DevExpress.Xpo.Aggregated]
		[DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Medicamento Dosis"), Index(0)]
		public XPCollection<MedicamentoDosis> MedicDosis
		{
		  get
		  {
			return GetCollection<MedicamentoDosis>("MedicDosis");
		  }
		}

        /// <summary>
        /// No puede ser Agregated la relacion porque no existe un producto para todas las vacunas y además porque nos
        /// toco relacionar a un mismo producto varias vacunas
        /// </summary>
        [Association("Medicamento-Vacunas"), XafDisplayName("Vacunas"), Index(1)]
        public XPCollection<Vacuna> Vacunas
        {
            get
            {
                return GetCollection<Vacuna>(nameof(Vacunas));
            }
        }

        /// <summary>
        /// Diferentes vias de administracion de un medicamento
        /// </summary>
        [Association("Medicamento-Vias"), DevExpress.Xpo.Aggregated, XafDisplayName("Vías de Administración"), Index(2)]
        public XPCollection<MedicamentoVia> Vias
        {
            get
            {
                return GetCollection<MedicamentoVia>(nameof(Vias));
            }
        }

        #endregion

    }
}
