using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using SBT.Apps.Base.Module.BusinessObjects;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    /// <summary>
    /// Recurso Humano.
    /// BO para el resumen del reporte de horas extras
    /// </summary>
    [DefaultClassOptions, ModelDefault("Caption", "Resumen RHExtra"), NavigationItem(false), DefaultProperty("TipoJornada")]
    [Persistent(nameof(ReporteHoraExtraResumen))]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ReporteHoraExtraResumen : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ReporteHoraExtraResumen(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        [Persistent(nameof(ReporteHoraExtra)), Association("ReporteHoraExtra-Resumenes")]
        ReporteHoraExtra reporteHoraExtra;
        [Persistent(nameof(TipoJornada)), Association("ParamJornada-Resumenes")]
        ParametroJornada tipoJornada;
        [Persistent(nameof(Horas)), FetchOnly]
        decimal horas;
        [Persistent(nameof(Porcentaje)), DbType("numeric(10,2)"), FetchOnly]
        decimal porcentaje;
        [Persistent(nameof(Valor)), DbType("numeric(12,4)"), FetchOnly]
        decimal valor;
 
        [PersistentAlias(nameof(reporteHoraExtra))]
        public ReporteHoraExtra ReporteHoraExtra
        {
            get => reporteHoraExtra;
        }

        [XafDisplayName("Tipo Jornada"), PersistentAlias(nameof(tipoJornada))]
        public ParametroJornada TipoJornada
        {
            get => tipoJornada;
        }

        [PersistentAlias(nameof(horas)), XafDisplayName("Horas")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Horas
        {
            get { return horas; }
        }       

        [PersistentAlias(nameof(porcentaje)), XafDisplayName("Porcentaje")]
        [ModelDefault("DisplayFormat", "{0:N4}"), ModelDefault("EditMask", "n4")]
        public decimal Porcentaje
        {
            get { return porcentaje; }
        }
        
        [PersistentAlias(nameof(valor)), XafDisplayName("Valor")]
        [ModelDefault("DisplayFormat", "{0:N4}"), ModelDefault("EditMask", "n4")]
        public decimal Valor
        {
            get { return valor; }
        }
        
        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}