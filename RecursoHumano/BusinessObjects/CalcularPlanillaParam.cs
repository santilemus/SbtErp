using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    /// <summary>
    /// Non Persistent BO, para los parametros necesarios para ejecutar el calculo de la planilla
    /// </summary>
    /// <remarks>
    /// ver informacion porque se hace esta implementacion en:
    /// https://docs.devexpress.com/eXpressAppFramework/116106/business-model-design-orm/non-persistent-objects/how-to-show-persistent-objects-in-a-non-persistent-objects-view
    /// https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.CompositeObjectSpace.AdditionalObjectSpaces
    /// </remarks>
    [DomainComponent, XafDisplayName("Calcular Planilla")]
    public class CalcularPlanillaParam : NonPersistentObjectImpl
    {
        private IObjectSpace objectSpace;
        private IList<TipoPlanilla> tipoPlanillas;
        private TipoPlanilla tipoPlanilla;

        [Browsable(false)]
        public IList<TipoPlanilla> TipoPlanillas
        {
            get
            {
                if (tipoPlanillas == null)
                    tipoPlanillas = ObjectSpace.GetObjects<TipoPlanilla>(new BinaryOperator("Activo", true));
                return tipoPlanillas;
            }
            internal set { tipoPlanillas = value; }
        }

        [XafDisplayName("Tipo Planilla"), DataSourceProperty("TipoPlanillas")]
        public TipoPlanilla TipoPlanilla
        {
            get
            {
                if (tipoPlanilla == null)
                    tipoPlanilla = ObjectSpace.GetObjects<TipoPlanilla>(new BinaryOperator("Activo", true)).FirstOrDefault();
                return tipoPlanilla;
            }
            set { TipoPlanilla = tipoPlanilla; }
        }

        [XafDisplayName("Fecha Inicio"), ToolTip("Fecha de Inicio del período de cálculo de la planilla")]
        public DateTime FechaInicio { get; set; }
        [XafDisplayName("Fecha Fin"), ToolTip("Fecha de Finalización del período de cálculo de la planilla")]
        public DateTime FechaFin { get; set; }
        [XafDisplayName("Fecha Pago"), ToolTip("Fecha de Pago de la planilla")]
        public DateTime FechaPago { get; set; }
        [Size(2000), ModelDefault("RowCount", "8"), XafDisplayName("Bitácora")]
        public string Bitacora { get; set; }

        public CalcularPlanillaParam()
        {

        }

        public CalcularPlanillaParam(TipoPlanilla ATipo, DateTime AFechaInicio, DateTime AFechaFin, DateTime AFechaPago)
        {
            TipoPlanilla = ATipo;
            FechaInicio = AFechaInicio;
            FechaFin = AFechaFin;
            FechaPago = AFechaPago;
        }

    }
}
