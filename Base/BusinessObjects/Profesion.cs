using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System;
using System.Linq;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using System.ComponentModel;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    ///  Objeto Persistente que representa el catálogo de profesiones (oficios)
    /// </summary>
    /// 
    [DefaultClassOptions]
    [DevExpress.ExpressApp.DC.XafDisplayName("Profesión")]
    [DevExpress.Persistent.Base.ImageName("user_id-certificate")]
    [DevExpress.Persistent.Base.NavigationItem("Catalogos")]
    [RuleIsReferenced("Profesion_Referencia", DefaultContexts.Delete, typeof(Profesion), nameof(Oid),
        MessageTemplateMustBeReferenced = "Para borrar el objeto '{TargetObject}', debe estar seguro que no es utilizado (referenciado) en ningún lugar.",
        InvertResult = true, FoundObjectMessageFormat = "'{0}'", FoundObjectMessagesSeparator = ";")]
    [DefaultProperty(nameof(Descripcion))]
    public class Profesion : XPObjectBaseBO
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Clasificacion = ClasificacionProfesion.Profesion;
        }

        bool activo;
        private ClasificacionProfesion _clasificacion;
        private System.String _tituloCorto;
        private System.String _descripcion;
        private string _codigo;
        public Profesion(DevExpress.Xpo.Session session)
          : base(session)
        {
        }


        /// <summary>
        /// Codigo CIUO-08 (Codigo de la clasificacion internacional uniforme de ocupaciones), revision 08 (ultima al 2019)
        /// </summary>
        [XafDisplayName("Código Ciuo"), Size(10), DbType("varchar(10)"), Persistent("CodigoCiuo")]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [RuleRequiredField("Profesion.Codigo_Requerido", DefaultContexts.Save, "Código es requerido ")]
        //[RuleUniqueValue("Profesion.Codigo_Unico", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        public string Codigo
        {
            get => _codigo;
            set => SetPropertyValue(nameof(Codigo), ref _codigo, value);
        }

        /// <summary>
        /// Descripción de la profesion (nombre largo)
        /// </summary>
        [DevExpress.Xpo.SizeAttribute(150), Persistent("Descripcion")]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [RuleRequiredField("Profesion.Nombre_Requerido", "Save"), DbType("varchar(150)")]
        public System.String Descripcion
        {
            get => _descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref _descripcion, value);
        }

        /// <summary>
        /// Título corto con el cual se llama a los que ejercen la profesion
        /// </summary>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Título Corto"), VisibleInListView(true), VisibleInLookupListView(true)]
        [Size(25), DbType("varchar(25)")]
        public System.String TituloCorto
        {
            get
            {
                return _tituloCorto;
            }
            set
            {
                SetPropertyValue("TituloCorto", ref _tituloCorto, value);
            }
        }

        /// <summary>
        /// Clasificacion de la profesion
        /// </summary>
        [RuleRequiredField("Profesion.Clasificacion_Requerido", "Save"), XafDisplayName("Clasificación")]
        public ClasificacionProfesion Clasificacion
        {
            get
            {
                return _clasificacion;
            }
            set
            {
                SetPropertyValue("Clasificacion", ref _clasificacion, value);
            }
        }

        [DbType("bit"), XafDisplayName("Activo"), RuleRequiredField("Profesion.Activo_Requerido", DefaultContexts.Save)]
        public bool Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }

    }
}
