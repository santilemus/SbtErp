﻿using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System.ComponentModel;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    /// <summary>
    /// Recurso Humano
    /// BO que implementa la parametrizacion de las operaciones por tipo de planilla
    /// </summary>

    [ModelDefault("Caption", "Planilla Operación"), NavigationItem(false), DefaultProperty("Tipo"), CreatableItem(false)]
    [Persistent(nameof(OperacionTipoPlanilla))]
    //[ImageName("BO_Contact")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class OperacionTipoPlanilla : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public OperacionTipoPlanilla(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        Operacion operacion;
        TipoPlanilla tipo;

        [Association("Operacion-TipoPlanillas"), XafDisplayName("Operación")]
        public Operacion Operacion
        {
            get => operacion;
            set => SetPropertyValue(nameof(Operacion), ref operacion, value);
        }

        [Association("TipoPlanilla-Operaciones"), XafDisplayName("Tipo Planilla"), RuleRequiredField("PlanillaOperacion.Tipo_Requerido", "Save")]
        public TipoPlanilla Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }
        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}