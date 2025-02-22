﻿using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    [DefaultClassOptions, ModelDefault("Caption", "Plan Medico"), NavigationItem("Salud"), XafDefaultProperty("Nombre"),
        Persistent("PlanMedico"), CreatableItem(false)]
    [ImageName("PlanMedico")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class PlanMedico : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public PlanMedico(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            Activo = true;
        }

        #region Propiedades

        bool activo;
        string nombre;

        [Size(80), DbType("varchar(80)"), Persistent("Nombre"), XafDisplayName("Nombre"),
            RuleRequiredField("Plan.Nombre_Requerido", "Save")]
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        [DbType("bit"), Persistent("Activo"), XafDisplayName("Activo")]
        public bool Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }
        #endregion

        #region Colecciones
        [Association("Plan-Detalles"), DevExpress.Xpo.Aggregated, XafDisplayName("Reglas")]
        public XPCollection<PlanMedicoDetalle> Detalles
        {
            get
            {
                return GetCollection<PlanMedicoDetalle>(nameof(Detalles));
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