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
using SBT.Apps.Medico.Generico.Module.BusinessObjects;

namespace SBT.Apps.Medico.Expediente.Module.BusinessObjects.Ginecologia
{
    /// <summary>
    /// BO que corresponde a la UltrasonografiaObstetrica. Ver información en los siguientes enlaces
    /// https://es.slideshare.net/wideleonlobo/ecografia-obstetrica-biometria-fetal
    /// El siguiente ilustra los datos que pueden ser calculados en el sistema
    /// https://www.msdmanuals.com/medical-calculators/GestationDate-es.htm 
    /// https://www.uv.es/jjsanton/Ecografia%20y%20Doppler/6.Biometriafetalbasica.pdf
    /// </summary>
    [DefaultClassOptions, ModelDefault("Caption", "Ultrasonografía Obstetrica"), DefaultProperty(nameof(Fecha))]
    [NavigationItem(false), Persistent(nameof(UltraSonografiaObstetrica)), CreatableItem(false)]
    [ImageName("UltraSonografiaObstetrica")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class UltraSonografiaObstetrica : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public UltraSonografiaObstetrica(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades


        Empleado.Module.BusinessObjects.Empleado tecnico;
        ECantidadFeto tipoEmbarazo;
        ETipoUltrasonografiaObstetrica tipoUltrasonografia;
        Consulta consulta;
        string datosPlan;
        string diagnostico;
        DateTime fecha;


        [Association("Consulta-UltraSonografiaObstetricas")]
        public Consulta Consulta
        {
            get => consulta;
            set => SetPropertyValue(nameof(Consulta), ref consulta, value);
        }

        
        [XafDisplayName("Técnico"), ToolTip("Técnico responsable de realizar la ultrasonografía")]
        public Empleado.Module.BusinessObjects.Empleado Tecnico
        {
            get => tecnico;
            set => SetPropertyValue(nameof(Tecnico), ref tecnico, value);
        }

        [DbType("datetime2"), XafDisplayName("Fecha")]
        [RuleRequiredField("UltrasonografiaObstetrica.Fecha_Requerido", DefaultContexts.Save)]
        public DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
        }

        [DbType("smallint"), XafDisplayName("Tipo")]
        public ETipoUltrasonografiaObstetrica TipoUltrasonografia
        {
            get => tipoUltrasonografia;
            set => SetPropertyValue(nameof(TipoUltrasonografia), ref tipoUltrasonografia, value);
        }

        [DbType("smallint"), XafDisplayName("Tipo Embarazo")]
        public ECantidadFeto TipoEmbarazo
        {
            get => tipoEmbarazo;
            set => SetPropertyValue(nameof(TipoEmbarazo), ref tipoEmbarazo, value);
        }

        [Size(200), DbType("varchar(200)"), XafDisplayName("Diagnóstico"), ModelDefault("RowCount", "4"), VisibleInListView(false)]
        public string Diagnostico
        {
            get => diagnostico;
            set => SetPropertyValue(nameof(Diagnostico), ref diagnostico, value);
        }


        [Size(200), DbType("varchar(200)"), XafDisplayName("Plan"), ModelDefault("RowCount", "4"), VisibleInListView(false)]
        [Persistent(nameof(DatosPlan))]
        public string DatosPlan
        {
            get => datosPlan;
            set => SetPropertyValue(nameof(DatosPlan), ref datosPlan, value);
        }

        #endregion

        #region Colecciones
        [Association("UltrasonografiaObstetrica-Detalles"), DevExpress.Xpo.Aggregated, XafDisplayName("Detalle")]
        public XPCollection<UltrasonografiaObstetricaDetalle> Detalles => GetCollection<UltrasonografiaObstetricaDetalle>(nameof(Detalles));
        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}