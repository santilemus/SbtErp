using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Activo.Module.BusinessObjects
{
    [DefaultClassOptions, ModelDefault("Caption", "Movimiento"), NavigationItem(false), CreatableItem(false), DefaultProperty(nameof(Fecha))]
    [Persistent(nameof(ActivoMovimiento))]
    [ImageName(nameof(ActivoMovimiento))]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ActivoMovimiento : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ActivoMovimiento(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        string comentario;
        string numeroResolucion;
        DateTime fechaResolucion;
        DateTime fechaDevolucion;
        string conceptoPrestamo;
        Empleado.Module.BusinessObjects.Empleado prestadoA;
        Tercero.Module.BusinessObjects.Tercero proveedor;
        Empleado.Module.BusinessObjects.Empleado empleadoDestino;
        EmpresaUnidad unidadDestino;
        EmpresaUnidad unidadOrigen;
        Empleado.Module.BusinessObjects.Empleado empleadoOrigen;
        EActivoTipoMovimiento tipo;
        DateTime fecha;
        ActivoCatalogo activo;

        [Association("ActivoCatalogo-Movimientos"), XafDisplayName("Activo")]
        public ActivoCatalogo Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }

        [DbType("datetime"), XafDisplayName("Fecha"), RuleRequiredField("ActivoMovimiento.Fecha_Requerido", "Save")]
        [Indexed(nameof(Activo), nameof(Tipo), Name = "idxActivoFechaTipo_ActivoMovimiento", Unique = true)]
        [ModelDefault("DisplayFormat", "{0:d}"), ModelDefault("EditMask", "d")]
        public DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
        }

        [XafDisplayName("Tipo"), RuleRequiredField("ActivoMovimiento.Tipo_Requerido", DefaultContexts.Save)]
        public EActivoTipoMovimiento Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        [XafDisplayName("Unidad Origen")]
        [ToolTip("Unidad de la cual sale el activo en un traslado")]
        public EmpresaUnidad UnidadOrigen
        {
            get => unidadOrigen;
            set => SetPropertyValue(nameof(UnidadOrigen), ref unidadOrigen, value);
        }

        [XafDisplayName("Empleado Origen")]
        [ToolTip("Empleado que entrega el activo en un traslado")]
        public Empleado.Module.BusinessObjects.Empleado EmpleadoOrigen
        {
            get => empleadoOrigen;
            set => SetPropertyValue(nameof(EmpleadoOrigen), ref empleadoOrigen, value);
        }

        [XafDisplayName("Unidad Destino")]
        [RuleRequiredField("ActivoMovimiento.UnidadDestino_Requerido", DefaultContexts.Save, TargetCriteria = "[Tipo] == 0")]
        [ToolTip("Unidad a la cual pertenece el empleado que recibe el activo en un traslado")]
        public EmpresaUnidad UnidadDestino
        {
            get => unidadDestino;
            set => SetPropertyValue(nameof(UnidadDestino), ref unidadDestino, value);
        }

        [XafDisplayName("Empleado Destino")]
        [ToolTip("Empleado que recibe el activo en un traslado")]
        [RuleRequiredField("ActivoMovimiento.EmpleadoDestino_Requerido", DefaultContexts.Save, TargetCriteria = "[Tipo] == 0")]
        public Empleado.Module.BusinessObjects.Empleado EmpleadoDestino
        {
            get => empleadoDestino;
            set => SetPropertyValue(nameof(EmpleadoDestino), ref empleadoDestino, value);
        }

        [XafDisplayName("Proveedor")]
        [ToolTip("Proveedor al que se entrego el activo para su reparacion")]
        [RuleRequiredField("ActivoMovimiento.Proveedor_Requerido", DefaultContexts.Save, TargetCriteria = "[Tipo] == 1")]
        public Tercero.Module.BusinessObjects.Tercero Proveedor
        {
            get => proveedor;
            set => SetPropertyValue(nameof(Proveedor), ref proveedor, value);
        }

        [XafDisplayName("Prestado a")]
        [ToolTip("Empleado que recibe el activo en calidad de prestamo o responsable del préstamo a un tercero")]
        public Empleado.Module.BusinessObjects.Empleado PrestadoA
        {
            get => prestadoA;
            set => SetPropertyValue(nameof(PrestadoA), ref prestadoA, value);
        }

        [Size(150), DbType("varchar(150)"), XafDisplayName("Concepto Préstamo")]
        [ToolTip("Concepto del préstamo del activo")]
        [RuleRequiredField("ActivoMovimiento.ConceptoPrestamo_Requerido", "Save", TargetCriteria = "[Tipo] == 2")]
        public string ConceptoPrestamo
        {
            get => conceptoPrestamo;
            set => SetPropertyValue(nameof(ConceptoPrestamo), ref conceptoPrestamo, value);
        }

        [DbType("datetime"), XafDisplayName("Fecha Devolución")]
        [RuleValueComparison("ActivoMovimiento.FechaDevolucion >= Fecha", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual,
            "[Fecha]", ParametersMode.Expression, SkipNullOrEmptyValues = true, TargetCriteria = "[Tipo] == 2")]
        public DateTime FechaDevolucion
        {
            get => fechaDevolucion;
            set => SetPropertyValue(nameof(FechaDevolucion), ref fechaDevolucion, value);
        }

        [DbType("datetime"), XafDisplayName("Fecha Resolución")]
        [RuleRequiredField("ActivoMovimiento.FechaResolucion_Requerido", "Save", TargetCriteria = "[Tipo] == 3")]
        [ToolTip("Fecha Resolución de autorización de descarga del activo")]
        public DateTime FechaResolucion
        {
            get => fechaResolucion;
            set => SetPropertyValue(nameof(FechaResolucion), ref fechaResolucion, value);
        }

        [Size(25), DbType("varchar(25)"), XafDisplayName("No Resolución")]
        [RuleRequiredField("ActivoMovimiento.NumeroResolucion_Requerido", "Save", TargetCriteria = "[Tipo] == 3")]
        [ToolTip("No Resolución de autorizacion de descarga del activo")]
        public string NumeroResolucion
        {
            get => numeroResolucion;
            set => SetPropertyValue(nameof(NumeroResolucion), ref numeroResolucion, value);
        }

        [Size(250), DbType("varchar(250)"), XafDisplayName("Comentario")]
        public string Comentario
        {
            get => comentario;
            set => SetPropertyValue(nameof(Comentario), ref comentario, value);
        }

        #endregion
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue(nameof(PersistentProperty), ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }

    public enum EActivoTipoMovimiento
    {
        Traslado = 0,
        Reparacion = 1,
        Prestamo = 2,
        Descarga = 3,
        Otro = 4
    }
}