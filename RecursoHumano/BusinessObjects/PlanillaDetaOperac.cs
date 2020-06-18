﻿using System;
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
    /// Recurso Humano.
    /// BO con los resultados de la ejecucion de calculo de las planillas
    /// </summary>

    [DefaultClassOptions, ModelDefault("Caption", "Operaciones"), NavigationItem(false), DefaultProperty("Operacion")]
    [Persistent("PlaPlanillaDetaOperac")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class PlanillaDetaOperac : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public PlanillaDetaOperac(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades


        PlanillaDetalle planillaDetalle;
        //[Persistent(nameof(Empleado))]
        //SBT.Apps.Empleado.Module.BusinessObjects.Empleado empleado;
        [Persistent(nameof(Operacion))]
        Operacion operacion = null;
        [Persistent(nameof(Valor)), DbType("numeric(12,2)")]
        decimal valor = 0.0m;

        
        [Association("PlanillaDetalle-Operaciones"), XafDisplayName("Planilla Detalle")]
        public PlanillaDetalle PlanillaDetalle
        {
            get => planillaDetalle;
            set => SetPropertyValue(nameof(PlanillaDetalle), ref planillaDetalle, value);
        }

        ///// <summary>
        ///// Evaluar si se deja, para implementar de solo lectura el campo Empleado. En teoria esto es innecesario, 
        ///// pero esta aca para validar que solo se calcule una operacion por empleado y detalle de la planilla
        ///// </summary>
        //[XafDisplayName("Empleado"), PersistentAlias(nameof(empleado))]
        //public SBT.Apps.Empleado.Module.BusinessObjects.Empleado Empleado
        //{
        //    get => empleado;
        //}

        [Association("Operacion-DetalleOperaciones"), PersistentAlias(nameof(operacion)), XafDisplayName("Operación")]
        public Operacion Operacion
        {
            get => operacion;
        }

        [PersistentAlias(nameof(valor)), XafDisplayName("Valor")]
        public decimal Valor
        {
            get { return valor; }
        }
        
        #endregion

        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}