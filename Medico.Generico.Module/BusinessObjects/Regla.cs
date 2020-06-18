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

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    /// <summary>
    /// Clase que corresponde a las BO con la definicion de las reglas, que se pueden aplicar a los recordatorios del paciente")
    /// </summary>
    [DefaultClassOptions, Persistent("Regla"), ModelDefault("Caption", "Reglas - Plan Medico"), NavigationItem("Salud"), 
        XafDefaultProperty(nameof(Descripcion))]
    [ImageName("list-key")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Regla : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Regla(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        ETipoRegla tipo = ETipoRegla.Recordatorio;
        EEstadoRegla practicaDefecto = EEstadoRegla.PorDefecto;
        string descripcion;

        [Size(100), DbType("varchar(100"), Persistent("Descripcion"), XafDisplayName("Descripción"),
            RuleRequiredField("Regla.Descripcion_Requerido", "Save")]
        public string Descripcion
        {
            get => descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref descripcion, value);
        }

        [DbType("smallint"), Persistent("Tipo"), XafDisplayName("Tipo Regla"), 
            RuleRequiredField("Regla.Tipo_Requerido", "Save")]
        public ETipoRegla Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        [DbType("smallint"), Persistent("PracticaDefecto"), XafDisplayName("Practica Defecto"), 
            RuleRequiredField("Regla.PracticaDefecto_Requerido", DefaultContexts.Save)]
        public EEstadoRegla PracticaDefecto
        {
            get => practicaDefecto;
            set => SetPropertyValue(nameof(PracticaDefecto), ref practicaDefecto, value);
        }

        #endregion


        #region Colecciones
        [Association("Regla-PlanDetalles"), XafDisplayName("Plan Detalles")]
        public XPCollection<PlanMedicoDetalle> PlanDetalles
        {
            get
            {
                return GetCollection<PlanMedicoDetalle>(nameof(PlanDetalles));
            }
        }
        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}