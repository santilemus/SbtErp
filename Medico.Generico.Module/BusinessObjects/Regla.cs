﻿using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    /// <summary>
    /// Clase que corresponde a las BO con la definicion de las reglas, que se pueden aplicar a los recordatorios del paciente")
    /// </summary>
    [DefaultClassOptions, Persistent("Regla"), ModelDefault("Caption", "Reglas - Plan Medico"), NavigationItem("Salud"),
        XafDefaultProperty(nameof(Descripcion)), CreatableItem(false)]
    [ImageName("list-key")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Regla : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Regla(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            Tipo = ETipoRegla.Recordatorio;
            PracticaDefecto = EEstadoRegla.PorDefecto;
        }

        #region Propiedades

        ETipoRegla tipo = ETipoRegla.Recordatorio;
        EEstadoRegla practicaDefecto = EEstadoRegla.PorDefecto;
        string descripcion;

        [Size(100), DbType("varchar(100)"), Persistent("Descripcion"), XafDisplayName("Descripción"),
            RuleRequiredField("Regla.Descripcion_Requerido", "Save")]
        public string Descripcion
        {
            get => descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref descripcion, value);
        }

        [DbType("smallint"), Persistent("Tipo"), XafDisplayName("Tipo Regla")]
        public ETipoRegla Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        [DbType("smallint"), Persistent("PracticaDefecto"), XafDisplayName("Practica Defecto")]
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