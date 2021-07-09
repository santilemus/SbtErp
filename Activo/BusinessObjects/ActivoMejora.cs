using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System.ComponentModel;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace SBT.Apps.Activo.Module.BusinessObjects
{
    [DefaultClassOptions, ModelDefault("Caption", "Mejora"), NavigationItem(false), DefaultProperty(nameof(Nombre))]
    [CreatableItem(false), Persistent(nameof(ActivoMejora))]
    [ImageName(nameof(ActivoMejora))]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ActivoMejora : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ActivoMejora(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Valor = 0.0m;
            VidaUtil = 0;
            ValoResidual = 0.0m;
            Tipo = ETipoMejoraActivo.Mejora;
            Depreciacion = 0.0m;
            MesesGarantia = 0;
            Activa = true;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        bool activa;
        string comentario;
        int mesesGarantia;
        [Persistent(nameof(Depreciacion)), DbType("numeric(14,2)")]
        decimal depreciacion;
        ETipoMejoraActivo tipo;
        decimal valoResidual;
        DateTime inicioDepreciacion;
        int vidaUtil;
        decimal valor;
        string nombre;
        Tercero.Module.BusinessObjects.Tercero tercero;
        DateTime fecha;
        ActivoCatalogo activo;

        [Association("ActivoCatalogo-Mejoras"), XafDisplayName("Activo")]
        public ActivoCatalogo Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }

        [DbType("datetime"), XafDisplayName("Fecha"), RuleRequiredField("ActivoMejora.Fecha_Requerido", DefaultContexts.Save)]
        [ModelDefault("DisplayFormat", "{0:d}"), ModelDefault("EditMask", "d")]
        public DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
        }

        [XafDisplayName("Proveedor"), RuleRequiredField("ActivoMejora.Proveedor_Requerido", DefaultContexts.Save)]
        public Tercero.Module.BusinessObjects.Tercero Tercero
        {
            get => tercero;
            set => SetPropertyValue(nameof(Tercero), ref tercero, value);
        }

        [Size(100), DbType("varchar(100)"), XafDisplayName("Nombre")]
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        [XafDisplayName("Tipo")]
        public ETipoMejoraActivo Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Valor")]
        [RuleValueComparison("ActivoMejora.Valor > 0", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, SkipNullOrEmptyValues = false)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Valor
        {
            get => valor;
            set => SetPropertyValue(nameof(Valor), ref valor, value);
        }

        [XafDisplayName("Vida Útil (meses)"), RuleRequiredField("ActivoMejora.VidaUtil_Requerido", "Save")]
        [DbType("smallint")]
        public int VidaUtil
        {
            get => vidaUtil;
            set => SetPropertyValue(nameof(VidaUtil), ref vidaUtil, value);
        }

        [DbType("datetime"), XafDisplayName("Inicio Depreciación")]
        [RuleValueComparison("ActivoMejora.InicioDepreciacion >= Fecha", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual,
            "[Fecha]", ParametersMode.Expression, SkipNullOrEmptyValues = false)]
        [ModelDefault("DisplayFormat", "{0:d}"), ModelDefault("EditMask", "d")]
        public DateTime InicioDepreciacion
        {
            get => inicioDepreciacion;
            set => SetPropertyValue(nameof(InicioDepreciacion), ref inicioDepreciacion, value);
        }

        [XafDisplayName("Valor Residual")]
        [RuleValueComparison("ActivoMejora.ValorResidual >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = false)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal ValoResidual
        {
            get => valoResidual;
            set => SetPropertyValue(nameof(ValoResidual), ref valoResidual, value);
        }

        [PersistentAlias(nameof(depreciacion)), XafDisplayName("Depreciación")]
        [ToolTip("Depreciación aplicada a la mejora o versión")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Depreciacion
        {
            get => depreciacion;
            set => SetPropertyValue(nameof(Depreciacion), ref depreciacion, value);
        }

        [DbType("smallint"), XafDisplayName("Meses Garantía")]
        public int MesesGarantia
        {
            get => mesesGarantia;
            set => SetPropertyValue(nameof(MesesGarantia), ref mesesGarantia, value);
        }


        [Size(200), DbType("varchar(200)"), XafDisplayName("Comentario")]
        public string Comentario
        {
            get => comentario;
            set => SetPropertyValue(nameof(Comentario), ref comentario, value);
        }

        [DbType("bit"), XafDisplayName("Activa"), RuleRequiredField("ActivoMejora.Activa_Requerido", "Save")]
        public bool Activa
        {
            get => activa;
            set => SetPropertyValue(nameof(Activa), ref activa, value);
        }

        #endregion

        #region Colecciones
        [Association("ActivoMejora-Depreciaciones"), XafDisplayName("Depreciaciones"), DevExpress.Xpo.Aggregated]
        public XPCollection<ActivoDepreciacion> Depreciaciones => GetCollection<ActivoDepreciacion>(nameof(Depreciaciones));

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

    public enum ETipoMejoraActivo
    {
        [XafDisplayName("Versión de Intangible")]
        Version = 0,
        [XafDisplayName("Mejora de Activo")]
        Mejora = 1,
        [XafDisplayName("Reparación")]
        Reparacion = 2,
        [XafDisplayName("Mantenimiento")]
        Mantenimiento = 3
    }
}