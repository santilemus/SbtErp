using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.Persistent.Validation;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Medico.Generico.Module.BusinessObjects;

namespace SBT.Apps.Medico.Expediente.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que corresponde a los examenes físicos en las consultas. Es la clase para el objeto de negocios de ConsultaExamenFisico
    /// </summary>
    [NavigationItem(false)]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Examenes Físicos")]
    [DevExpress.Persistent.Base.CreatableItemAttribute(false)]
    [DevExpress.Persistent.Base.ImageNameAttribute("examenfis")]
    [RuleCombinationOfPropertiesIsUnique("ConsultaExamenFisico.FechaDescripcionConsulta_Unico", DefaultContexts.Save, "Fecha,Descripcion,Consulta", SkipNullOrEmptyValues = false)]
    public class ConsultaExamenFisico : XPObjectBaseBO
    {
        /// <summary>
        /// Metodo para la inicialización de propiedades y/o objetos del BO. Se ejecuta una sola vez después de la creación del objeto
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Fecha = DateTime.Now;
        }

        private Consulta _consulta;
        private PacienteFileData _documento;
        private System.String _resultado;
        private System.String _descripcion;
        private System.DateTime _fecha;
        public ConsultaExamenFisico(DevExpress.Xpo.Session session)
          : base(session)
        {
        }
        [RuleRequiredField("ConsultaExamenFisico.Fecha_Requerido", "Save")]
        public System.DateTime Fecha
        {
            get
            {
                return _fecha;
            }
            set
            {
                SetPropertyValue("Fecha", ref _fecha, value);
            }
        }
        [DevExpress.Xpo.SizeAttribute(250), DbType("varchar(250)")]
        [RuleRequiredField("ConsultaExamenFisico.Descripcion_Requerido", "Save")]
        public System.String Descripcion
        {
            get
            {
                return _descripcion;
            }
            set
            {
                SetPropertyValue("Descripcion", ref _descripcion, value);
            }
        }
        [DevExpress.Xpo.SizeAttribute(400), DbType("varchar(400)")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [RuleRequiredField("ConsultaExamenFisico.Resultado_Requerido", "Save")]
        public System.String Resultado
        {
            get
            {
                return _resultado;
            }
            set
            {
                SetPropertyValue("Resultado", ref _resultado, value);
            }
        }
        public PacienteFileData Documento
        {
            get => _documento;
            set => SetPropertyValue(nameof(Documento), ref _documento, value);
        }

        [DevExpress.Xpo.AssociationAttribute("ExamenesFisicos-Consulta")]
        public Consulta Consulta
        {
            get
            {
                return _consulta;
            }
            set
            {
                SetPropertyValue("Consulta", ref _consulta, value);
            }
        }

    }
}
