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
using DevExpress.ExpressApp.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Empleado.Module.BusinessObjects;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    /// <summary>
    /// Recurso Humano
    /// BO para el detalle de los empleados incluidos en una planilla calculada por el sistema
    /// </summary>

    [DefaultClassOptions, ModelDefault("Caption", "Planilla Detalle"), NavigationItem(false), DefaultProperty("Empleado")]
    [Persistent(nameof(PlanillaDetalle))]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class PlanillaDetalle : XPCustomBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public PlanillaDetalle(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        [DbType("bigint"), Persistent(nameof(Oid)), Key(true)]
        int oid = -1;
        [Persistent(nameof(Planilla)), Association("Planilla-Detalles")]
        Planilla planilla = null;
        [Persistent(nameof(Empleado))]
        SBT.Apps.Empleado.Module.BusinessObjects.Empleado empleado = null;
        [Persistent(nameof(Unidad))]
        EmpresaUnidad unidad = null;
        [Persistent(nameof(Cargo))]
        Cargo cargo = null;

        [PersistentAlias(nameof(oid)), XafDisplayName("Oid")]
        public Int64 Oid
        {
            get => oid;
        }
        [PersistentAlias(nameof(planilla)), XafDisplayName("Planilla")]
        public Planilla Planilla
        {
            get => planilla;
        }
        [PersistentAlias(nameof(empleado)), XafDisplayName("Empleado")]
        public SBT.Apps.Empleado.Module.BusinessObjects.Empleado Empleado
        {
            get => empleado;
        }
        [PersistentAlias(nameof(unidad)), XafDisplayName("Unidad")]
        [ToolTip("Unidad a la cual pertenece el empleado, cuando se calculo la planilla")]
        public EmpresaUnidad Unidad
        {
            get => unidad;
        }
        [PersistentAlias(nameof(cargo)), XafDisplayName("Cargo")]
        [ToolTip("Cargo del empleado cuando se calculo la planilla (por fines históricos)")]
        public Cargo Cargo
        {
            get => cargo;
        }
        #endregion

        #region Colecciones
        [Association("PlanillaDetalle-Operaciones"), DevExpress.Xpo.Aggregated, XafDisplayName("Operaciones"), Index(0)]
        public XPCollection<PlanillaDetalleOperacion> Operaciones
        {
            get
            {
                return GetCollection<PlanillaDetalleOperacion>(nameof(Operaciones));
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