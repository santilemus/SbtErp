using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System.ComponentModel;


namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    /// <summary>
    /// Recurso Humano.
    /// BO con los resultados de la ejecucion de calculo de las planillas
    /// </summary>

    [ModelDefault("Caption", "Operaciones"), NavigationItem(false), DefaultProperty("Operacion"), CreatableItem(false)]
    [Persistent(nameof(PlanillaDetalleOperacion))]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class PlanillaDetalleOperacion : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public PlanillaDetalleOperacion(Session session)
            : base(session)
        {
        }

        public PlanillaDetalleOperacion(Session session, PlanillaDetalle planiDetalle, Operacion op, decimal AValor) : base(session)
        {
            PlanillaDetalle = planiDetalle;
            operacion = op;
            valor = AValor;
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
        [Persistent(nameof(Operacion)), Association("Operacion-PlanillaOperaciones")]
        Operacion operacion;
        [Persistent(nameof(Valor)), DbType("numeric(12,2)")]
        decimal valor;


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

        [PersistentAlias(nameof(operacion)), XafDisplayName("Operación")]
        public Operacion Operacion => operacion;

        [PersistentAlias(nameof(valor)), XafDisplayName("Valor")]
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