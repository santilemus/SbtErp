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
using SBT.Apps.Empleado.Module.BusinessObjects;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    /// <summary>
    /// Planilla 
    /// BO EmpleadoTipoPlanilla. Permite parametrizar las planillas que seran calculadas para cada empleado. En la version
    /// original del sistema (delphi), en cada implementacion (empresa) hay que definir las reglas para saber quienes se
    /// incluyen en cada planilla, lo cual implica hacer modificaciones a la aplicacion, durante la implementacion o cuando
    /// las reglas cambian. 
    /// En esta version, el objetivo es que sea el usuario el que indique por cada empleado los tipos de planilla en los
    /// cuales estara incluido y reducir la necesidad de hacer cambios.
    /// </summary>
    [DefaultClassOptions, ModelDefault("Caption", "Empleado-Tipo Planilla"), NavigationItem(false), CreatableItem(false),
        Persistent(nameof(EmpleadoTipoPlanilla)), XafDefaultProperty(nameof(TipoPlanilla))]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class EmpleadoTipoPlanilla : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public EmpleadoTipoPlanilla(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        TipoPlanilla tipoPlanilla;
        SBT.Apps.Empleado.Module.BusinessObjects.Empleado empleado;

        [XafDisplayName("Empleado"), Index(0)]
        public SBT.Apps.Empleado.Module.BusinessObjects.Empleado Empleado
        {
            get => empleado;
            set => SetPropertyValue(nameof(Empleado), ref empleado, value);
        }

        
        [XafDisplayName("Tipo Planilla"), Index(1), RuleRequiredField("EmpleadoTipoPlanilla.TipoPlanilla_Requerido", "Save")]
        public TipoPlanilla TipoPlanilla
        {
            get => tipoPlanilla;
            set => SetPropertyValue(nameof(TipoPlanilla), ref tipoPlanilla, value);
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}