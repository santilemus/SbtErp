using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Contabilidad.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Activo.Module.BusinessObjects
{
    [DefaultClassOptions]
    [ModelDefault("Caption", "Catálogo Activos"), NavigationItem("Activo Fijo"), CreatableItem(false), DefaultProperty(nameof(Nombre))]
    [Persistent(nameof(Catalogo))]
    //[ImageName("BO_Contact")]
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

        private XPDelayedProperty foto = new XPDelayedProperty();
        string observaciones;
        long ordenCompra;
        Listas estadoUsoActivo;
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
        Tercero.Module.BusinessObjects.Tercero tercero;
        Empleado.Module.BusinessObjects.Empleado empleado;
        EmpresaUnidad unidad;
        ActivoCategoria categoria;
        string nombre;
        string codigo;
        Empresa empresa;

        [XafDisplayName("Empresa"), Browsable(false)]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }

        [Size(20), DbType("varchar(20)"), XafDisplayName("Código"), RuleRequiredField("ActivoCatalogo.Codigo_Requerido", "Save")]
        [Indexed(nameof(Empresa), Name = "idxEmpresaActivo_ActivoCatalogo", Unique = true)]
        [VisibleInLookupListView(true)]
        public string Codigo
        {
            get => codigo;
            set => SetPropertyValue(nameof(Codigo), ref codigo, value);
        }

        [Size(120), DbType("varchar(120)"), XafDisplayName("Nombre"), RuleRequiredField("ActivoCatalogo.Nombre_Requerido", "Save")]
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        [Association("Categoria-Activos"), XafDisplayName("Categoría"), RuleRequiredField("ActivoCatalogo.Categoria_Requerido", "Save")]
        public ActivoCategoria Categoria
        {
            get => categoria;
            set => SetPropertyValue(nameof(Categoria), ref categoria, value);
        }

        [XafDisplayName("Unidad"), RuleRequiredField("ActivoCatalogo.Unidad_Requerido", "Save")]
        public EmpresaUnidad Unidad
        {
            get => unidad;
            set => SetPropertyValue(nameof(Unidad), ref unidad, value);
        }

        [XafDisplayName("Empleado")]
        public Empleado.Module.BusinessObjects.Empleado Empleado
        {
            get => empleado;
            set => SetPropertyValue(nameof(Empleado), ref empleado, value);
        }

        [XafDisplayName("Proveedor")]
        public Tercero.Module.BusinessObjects.Tercero Tercero
        {
            get => tercero;
            set => SetPropertyValue(nameof(Tercero), ref tercero, value);
        }

        [XafDisplayName("Fecha Compra"), RuleRequiredField("ActivoCatalogo.FechaCompra_Requerido", DefaultContexts.Save)]
        [DbType("datetime")]
        [ModelDefault("DisplayFormat", "{0:d}"), ModelDefault("EditMask", "d")]
        public DateTime FechaCompra
        {
            get => fechaCompra;
            set => SetPropertyValue(nameof(FechaCompra), ref fechaCompra, value);
        }

        [Size(40), DbType("varchar(40)"), XafDisplayName("Marca")]
        public string Marca
        {
            get => marca;
            set => SetPropertyValue(nameof(Marca), ref marca, value);
        }

        [Size(40), DbType("varchar(40)"), XafDisplayName("Modelo")]
        public string Modelo
        {
            get => modelo;
            set => SetPropertyValue(nameof(Modelo), ref modelo, value);
        }

        [Size(25), DbType("varchar(25)"), XafDisplayName("No Serie")]
        public string NoSerie
        {
            get => noSerie;
            set => SetPropertyValue(nameof(NoSerie), ref noSerie, value);
        }

        [DbType("datetime"), XafDisplayName("Inicio Depreciación")]
        [RuleRequiredField("ActivoCatalogo.InicioDepreciacion_Requerido", DefaultContexts.Save)]
        [ModelDefault("DisplayFormat", "{0:d}"), ModelDefault("EditMask", "d")]
        public DateTime InicioDepreciacion
        {
            get => inicioDepreciacion;
            set => SetPropertyValue(nameof(InicioDepreciacion), ref inicioDepreciacion, value);
        }

        [XafDisplayName("Moneda"), RuleRequiredField("ActivoCatalogo.Moneda_Requerido", DefaultContexts.Save)]
        public Moneda Moneda
        {
            get => moneda;
            set => SetPropertyValue(nameof(Moneda), ref moneda, value);
        }
        [DbType("numeric(14,2)"), XafDisplayName("Valor Moneda"), RuleRequiredField("ActivoCatalogo.ValorMoneda_Requerido", DefaultContexts.Save)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal ValorMoneda
        {
            get => valorMoneda;
            set => SetPropertyValue(nameof(ValorMoneda), ref valorMoneda, value);
        }

        [XafDisplayName("Vida Útil (meses)"), RuleRequiredField("ActivoCatalogo.VidaUtil_Requerido", "Save")]
        [DbType("smallint")]
        public int VidaUtil
        {
            get => vidaUtil;
            set => SetPropertyValue(nameof(VidaUtil), ref vidaUtil, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Valor de Compra")]
        [ToolTip("Valor de compra del activo")]
        [RuleValueComparison("ActivoCatalogo.ValorCompra >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = false)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal ValorCompra
        {
            get => valorCompra;
            set => SetPropertyValue(nameof(ValorCompra), ref valorCompra, value);
        }

        [XafDisplayName("Ajustes"), PersistentAlias(nameof(totalAjuste))]
        [ToolTip("Total acumulado de ajustes al valor de compra del activo")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal TotalAjuste => totalAjuste;

        [PersistentAlias(nameof(totalMejora)), XafDisplayName("Mejoras")]
        [ToolTip("Total acumulado de las mejoras realizadas al activo")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal TotalMejora => totalMejora;

        [PersistentAlias(nameof(totalDepreciacion)), XafDisplayName("Depreciaciones")]
        [ToolTip("Total acumulado de las depreciaciones aplicadas al activo")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal TotalDepreciacion => totalDepreciacion;

        [XafDisplayName("Valor Residual"), RuleRequiredField("ActivoCatalogo.ValorResidual_Requerido", DefaultContexts.Save)]
        [RuleValueComparison("ActivoCatalogo.ValorResidual >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = false)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal ValorResidual
        {
            get => valorResidual;
            set => SetPropertyValue(nameof(ValorResidual), ref valorResidual, value);
        }

        [XafDisplayName("Estado Depreciación"), DbType("smallint"), RuleRequiredField("ActivoCatalogo.EstadoDepreciacion_Requerido", "Save")]
        public EEstadoDepreciacion EstadoDepreciacion
        {
            get => estadoDepreciacion;
            set => SetPropertyValue(nameof(EstadoDepreciacion), ref estadoDepreciacion, value);
        }

        [DbType("datetime"), XafDisplayName("Fecha Descarga")]
        [ModelDefault("DisplayFormat", "{0:d}"), ModelDefault("EditMask", "d")]
        public DateTime FechaDescarga
        {
            get => fechaDescarga;
            set => SetPropertyValue(nameof(FechaDescarga), ref fechaDescarga, value);
        }

        [DbType("smallint"), XafDisplayName("Meses Garantía")]
        public int MesesGarantia
        {
            get => mesesGarantia;

            set => SetPropertyValue(nameof(MesesGarantia), ref mesesGarantia, value);
        }

        [DbType("datetime"), XafDisplayName("Fecha Inicio Garantía")]
        [ModelDefault("DisplayFormat", "{0:d}"), ModelDefault("EditMask", "d")]
        public DateTime FechaInicioGarantia
        {
            get => fechaInicioGarantia;
            set => SetPropertyValue(nameof(FechaInicioGarantia), ref fechaInicioGarantia, value);
        }

        [PersistentAlias("AddMonths([FechaInicioGarantia], [MesesGarantia])")]
        [ModelDefault("DisplayFormat", "{0:d}"), ModelDefault("EditMask", "d")]
        public DateTime FechaFinGarantia => Convert.ToDateTime(EvaluateAlias(nameof(FechaFinGarantia)));


        [XafDisplayName("Estado Uso Activo")]
        [DataSourceCriteria("[Categoria] == 1 && [Activo] == true")]
        public Listas EstadoUsoActivo
        {
            get => estadoUsoActivo;
            set => SetPropertyValue(nameof(EstadoUsoActivo), ref estadoUsoActivo, value);
        }

        /// <summary>
        /// Orden de Compra. Hay que hacer la integracion con compras
        /// </summary>
        [DbType("bigint"), XafDisplayName("Orden de Compra")]
        [ToolTip("Orden de compra que corresponde a la adquisición del activo")]
        public long OrdenCompra
        {
            get => ordenCompra;
            set => SetPropertyValue(nameof(OrdenCompra), ref ordenCompra, value);
        }


        [Size(250), DbType("varchar(250)"), XafDisplayName("Observaciones")]
        public string Observaciones
        {
            get => observaciones;
            set => SetPropertyValue(nameof(Observaciones), ref observaciones, value);
        }

        [Size(SizeAttribute.Unlimited), ImageEditor(ListViewImageEditorMode = ImageEditorMode.PopupPictureEdit,
          DetailViewImageEditorMode = ImageEditorMode.PopupPictureEdit, ListViewImageEditorCustomHeight = 34)]
        [Index(5)]
        [Delayed(nameof(foto), true)]
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
        public XPCollection<ActivoAjuste> Ajustes => GetCollection<ActivoAjuste>(nameof(ActivoAjuste));

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