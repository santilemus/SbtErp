using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;

namespace SBT.Apps.Activo.Module.BusinessObjects
{
    [DefaultClassOptions]
    [ModelDefault("Caption", "Catálogo Activos"), NavigationItem("Activo Fijo"), CreatableItem(false), DefaultProperty(nameof(Nombre))]
    [Persistent(nameof(ActivoCatalogo))]
    [ImageName("book")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ActivoCatalogo : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ActivoCatalogo(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            VidaUtil = 0;
            ValorCompra = 0.0m;
            totalMejora = 0.0m;
            totalAjuste = 0.0m;
            totalDepreciacion = 0.0m;
            valorResidual = 0.0m;
            EstadoDepreciacion = EEstadoDepreciacion.Depreciando;
            MesesGarantia = 0;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        readonly private XPDelayedProperty foto = new();
        string observaciones;
        long ordenCompra;
        Listas estadoUso;
        DateTime fechaInicioGarantia;
        int mesesGarantia;
        DateTime fechaDescarga;
        EEstadoDepreciacion estadoDepreciacion;
        decimal valorResidual;
        [DbType("numeric(14,2)"), Persistent(nameof(TotalDepreciacion))]
        decimal totalDepreciacion;
        [DbType("numeric(14,2)"), Persistent(nameof(TotalMejora))]
        decimal totalMejora;
        [DbType("numeric(14,2)"), Persistent(nameof(TotalAjuste))]
        decimal totalAjuste;
        decimal valorCompra;
        decimal valorMoneda;
        Moneda moneda;
        int vidaUtil;
        DateTime inicioDepreciacion;
        string noSerie;
        string modelo;
        string marca;
        DateTime fechaCompra;
        Tercero.Module.BusinessObjects.Tercero proveedor;
        Empleado.Module.BusinessObjects.Empleado empleado;
        EmpresaUnidad unidad;
        ActivoCategoria categoria;
        string nombre;
        string codigo;
        Empresa empresa;

        [XafDisplayName("Empresa"), Browsable(false), Index(0)]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }

        [Size(20), DbType("varchar(20)"), XafDisplayName("Código"), RuleRequiredField("ActivoCatalogo.Codigo_Requerido", "Save")]
        [Indexed(nameof(Empresa), Name = "idxEmpresaActivo_ActivoCatalogo", Unique = true)]
        [VisibleInLookupListView(true), Index(1)]
        public string Codigo
        {
            get => codigo;
            set => SetPropertyValue(nameof(Codigo), ref codigo, value);
        }

        [Size(120), DbType("varchar(120)"), XafDisplayName("Nombre"), RuleRequiredField("ActivoCatalogo.Nombre_Requerido", "Save")]
        [VisibleInListView(true), Index(2)]
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        [Association("Categoria-Activos"), XafDisplayName("Categoría"), RuleRequiredField("ActivoCatalogo.Categoria_Requerido", "Save")]
        [Index(3)]
        public ActivoCategoria Categoria
        {
            get => categoria;
            set => SetPropertyValue(nameof(Categoria), ref categoria, value);
        }

        [XafDisplayName("Unidad"), RuleRequiredField("ActivoCatalogo.Unidad_Requerido", "Save")]
        [Index(4), VisibleInListView(false)]
        public EmpresaUnidad Unidad
        {
            get => unidad;
            set => SetPropertyValue(nameof(Unidad), ref unidad, value);
        }

        [XafDisplayName("Empleado")]
        [VisibleInListView(false), Index(5)]
        public Empleado.Module.BusinessObjects.Empleado Empleado
        {
            get => empleado;
            set => SetPropertyValue(nameof(Empleado), ref empleado, value);
        }

        [XafDisplayName("Proveedor")]
        [VisibleInListView(false), Index(6)]
        public Tercero.Module.BusinessObjects.Tercero Proveedor
        {
            get => proveedor;
            set => SetPropertyValue(nameof(Proveedor), ref proveedor, value);
        }

        [XafDisplayName("Fecha Compra"), RuleRequiredField("ActivoCatalogo.FechaCompra_Requerido", DefaultContexts.Save)]
        [DbType("datetime")]
        [ModelDefault("DisplayFormat", "{0:d}"), ModelDefault("EditMask", "d")]
        [Index(7)]
        public DateTime FechaCompra
        {
            get => fechaCompra;
            set => SetPropertyValue(nameof(FechaCompra), ref fechaCompra, value);
        }

        [Size(40), DbType("varchar(40)"), XafDisplayName("Marca")]
        [Index(8), VisibleInListView(false)]
        public string Marca
        {
            get => marca;
            set => SetPropertyValue(nameof(Marca), ref marca, value);
        }

        [Size(40), DbType("varchar(40)"), XafDisplayName("Modelo")]
        [VisibleInListView(false), Index(9)]
        public string Modelo
        {
            get => modelo;
            set => SetPropertyValue(nameof(Modelo), ref modelo, value);
        }

        [Size(25), DbType("varchar(25)"), XafDisplayName("No Serie")]
        [VisibleInListView(false), Index(10)]
        public string NoSerie
        {
            get => noSerie;
            set => SetPropertyValue(nameof(NoSerie), ref noSerie, value);
        }

        [DbType("datetime"), XafDisplayName("Inicio Depreciación")]
        [RuleRequiredField("ActivoCatalogo.InicioDepreciacion_Requerido", DefaultContexts.Save)]
        [ModelDefault("DisplayFormat", "{0:d}"), ModelDefault("EditMask", "d")]
        [Index(11)]
        public DateTime InicioDepreciacion
        {
            get => inicioDepreciacion;
            set => SetPropertyValue(nameof(InicioDepreciacion), ref inicioDepreciacion, value);
        }

        [XafDisplayName("Moneda"), RuleRequiredField("ActivoCatalogo.Moneda_Requerido", DefaultContexts.Save)]
        [Index(12)]
        public Moneda Moneda
        {
            get => moneda;
            set => SetPropertyValue(nameof(Moneda), ref moneda, value);
        }
        [DbType("numeric(14,2)"), XafDisplayName("Valor Moneda")]
        [RuleValueComparison("ActivoCatalogo.ValorMoneda_Valido", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0.0, "El valor de la moneda debe ser mayor a cero")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [VisibleInListView(false), Index(13)]
        public decimal ValorMoneda
        {
            get => valorMoneda;
            set => SetPropertyValue(nameof(ValorMoneda), ref valorMoneda, value);
        }

        [XafDisplayName("Vida Útil (meses)")]
        [RuleValueComparison("ActivoCatalogo.VidaUtil_Valido", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, "La vida útil del Activo debe ser mayor a cero")]
        [DbType("smallint"), Index(14)]
        public int VidaUtil
        {
            get => vidaUtil;
            set => SetPropertyValue(nameof(VidaUtil), ref vidaUtil, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Valor de Compra")]
        [ToolTip("Valor de compra del activo")]
        [RuleValueComparison("ActivoCatalogo.ValorCompra >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = false)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [Index(15)]
        public decimal ValorCompra
        {
            get => valorCompra;
            set => SetPropertyValue(nameof(ValorCompra), ref valorCompra, value);
        }

        [XafDisplayName("Ajustes"), PersistentAlias(nameof(totalAjuste))]
        [ToolTip("Total acumulado de ajustes al valor de compra del activo")]
        [ModelDefault("DisplayFormat", "{0:N2}")]
        [Index(16), VisibleInListView(false)]
        public decimal TotalAjuste => totalAjuste;

        [PersistentAlias(nameof(totalMejora)), XafDisplayName("Mejoras")]
        [ToolTip("Total acumulado de las mejoras realizadas al activo")]
        [ModelDefault("DisplayFormat", "{0:N2}")]
        [Index(17), VisibleInListView(false)]
        public decimal TotalMejora => totalMejora;

        [PersistentAlias(nameof(totalDepreciacion)), XafDisplayName("Depreciaciones")]
        [ToolTip("Total acumulado de las depreciaciones aplicadas al activo")]
        [ModelDefault("DisplayFormat", "{0:N2}")]
        [Index(18)]
        public decimal TotalDepreciacion => totalDepreciacion;

        [XafDisplayName("Valor Residual")]
        [RuleValueComparison("ActivoCatalogo.ValorResidual >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = false)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [VisibleInListView(false), Index(19)]
        public decimal ValorResidual
        {
            get => valorResidual;
            set => SetPropertyValue(nameof(ValorResidual), ref valorResidual, value);
        }

        [XafDisplayName("Estado Depreciación"), DbType("smallint")]
        [Index(20), VisibleInListView(false)]
        public EEstadoDepreciacion EstadoDepreciacion
        {
            get => estadoDepreciacion;
            set => SetPropertyValue(nameof(EstadoDepreciacion), ref estadoDepreciacion, value);
        }

        [DbType("datetime"), XafDisplayName("Fecha Descarga")]
        [ModelDefault("DisplayFormat", "{0:d}"), ModelDefault("EditMask", "d")]
        [VisibleInListView(false), Index(21)]
        public DateTime FechaDescarga
        {
            get => fechaDescarga;
            set => SetPropertyValue(nameof(FechaDescarga), ref fechaDescarga, value);
        }

        [DbType("smallint"), XafDisplayName("Meses Garantía")]
        [VisibleInListView(false), Index(22)]
        public int MesesGarantia
        {
            get => mesesGarantia;

            set => SetPropertyValue(nameof(MesesGarantia), ref mesesGarantia, value);
        }

        [DbType("datetime"), XafDisplayName("Fecha Inicio Garantía")]
        [ModelDefault("DisplayFormat", "{0:d}"), ModelDefault("EditMask", "d")]
        [VisibleInListView(false), Index(23)]
        public DateTime FechaInicioGarantia
        {
            get => fechaInicioGarantia;
            set => SetPropertyValue(nameof(FechaInicioGarantia), ref fechaInicioGarantia, value);
        }

        [PersistentAlias("AddMonths([FechaInicioGarantia], [MesesGarantia])")]
        [ModelDefault("DisplayFormat", "{0:d}"), ModelDefault("EditMask", "d")]
        [VisibleInListView(false), Index(24)]
        public DateTime FechaFinGarantia => Convert.ToDateTime(EvaluateAlias(nameof(FechaFinGarantia)));


        [XafDisplayName("Estado Uso Activo")]
        [DataSourceCriteria("[Categoria] == 1 && [Activo] == true")]
        [Index(25)]
        public Listas EstadoUso
        {
            get => estadoUso;
            set => SetPropertyValue(nameof(EstadoUso), ref estadoUso, value);
        }

        /// <summary>
        /// Orden de Compra. Hay que hacer la integracion con compras
        /// </summary>
        [DbType("bigint"), XafDisplayName("Orden de Compra")]
        [ToolTip("Orden de compra que corresponde a la adquisición del activo")]
        [VisibleInListView(false), Index(26)]
        public long OrdenCompra
        {
            get => ordenCompra;
            set => SetPropertyValue(nameof(OrdenCompra), ref ordenCompra, value);
        }


        [Size(250), DbType("varchar(250)"), XafDisplayName("Observaciones")]
        [VisibleInListView(false), Index(27)]
        public string Observaciones
        {
            get => observaciones;
            set => SetPropertyValue(nameof(Observaciones), ref observaciones, value);
        }

        [Size(SizeAttribute.Unlimited), ImageEditor(ListViewImageEditorMode = ImageEditorMode.PopupPictureEdit,
          DetailViewImageEditorMode = ImageEditorMode.PopupPictureEdit, ImageSizeMode = ImageSizeMode.StretchImage, ListViewImageEditorCustomHeight = 34)]
        [Index(5)]
        [Delayed(nameof(foto), true)]
        [VisibleInListView(false)]
        public byte[] Foto
        {
            get => (byte[])foto.Value;
            set
            {
                foto.Value = value;
            }
        }
        #endregion

        #region Propiedades
        [Association("ActivoCatalogo-Atributos"), DevExpress.Xpo.Aggregated, Index(0), XafDisplayName("Atributos")]
        public XPCollection<ActivoAtributo> Atributos => GetCollection<ActivoAtributo>(nameof(Atributos));

        [Association("ActivoCatalogo-Ajustes"), DevExpress.Xpo.Aggregated, Index(1), XafDisplayName("Ajustes")]
        public XPCollection<ActivoAjuste> Ajustes => GetCollection<ActivoAjuste>(nameof(Ajustes));

        [Association("ActivoCatalogo-Depreciaciones"), DevExpress.Xpo.Aggregated, Index(2), XafDisplayName("Depreciaciones")]
        public XPCollection<ActivoDepreciacion> Depreciaciones => GetCollection<ActivoDepreciacion>(nameof(Depreciaciones));

        [Association("ActivoCatalogo-Mejoras"), DevExpress.Xpo.Aggregated, Index(3), XafDisplayName("Mejoras")]
        public XPCollection<ActivoMejora> Mejoras => GetCollection<ActivoMejora>(nameof(Mejoras));
        [Association("ActivoCatalogo-Movimientos"), DevExpress.Xpo.Aggregated, Index(4), XafDisplayName("Movimientos")]
        public XPCollection<ActivoMovimiento> Movimientos => GetCollection<ActivoMovimiento>(nameof(Movimientos));
        [Association("ActivoCatalogo-SeguroDetalles"), Index(5), XafDisplayName("Seguros")]
        public XPCollection<ActivoSeguroDetalle> SeguroDetalles => GetCollection<ActivoSeguroDetalle>(nameof(SeguroDetalles));

        #endregion


        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}