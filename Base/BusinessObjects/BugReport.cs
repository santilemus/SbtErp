using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// BO BugReport. Registrar los errores encontrados en la aplicacion
    /// </summary>
    /// 
    [DefaultClassOptions, ModelDefault("Caption", "Bug Report"), DefaultProperty(nameof(Titulo)), CreatableItem(false),
        Persistent(nameof(BugReport))]
    [ImageName("BugReport")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class BugReport : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public BugReport(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        string notas;
        DateTime fechaCorrecion;
        string correccion;
        EBugEstado estado = EBugEstado.Abierto;
        string asignadoA;
        EBugPrioridad defectoPrioridad = EBugPrioridad.Bloquea;
        EBugSeveridad defectoSeveridad = EBugSeveridad.Critico;
        string resultadoActual;
        string resultadoEsperado;
        string pasosReproducir;
        EBugNavegadorWeb navegador = EBugNavegadorWeb.Chrome;
        EBugSistemaOperativo sistemaOperativo = EBugSistemaOperativo.Windows10;
        EBugPlataforma plataforma = EBugPlataforma.Web;
        byte[] capturaPantalla;
        string url;
        byte[] descripcion;
        [Persistent(nameof(ReportadoPor)), Size(50), DbType("varchar(50)")]
        string reportadoPor = DevExpress.ExpressApp.SecuritySystem.CurrentUserName;
        DateTime fechaReporte;
        string titulo;

        [Size(100), DbType("varchar(100)"), XafDisplayName("Título"), RuleRequiredField("BugReport.Titulo_Requerido", "Save")]
        [Index(0), DetailViewLayout("Bug ID", LayoutGroupType.SimpleEditorsGroup, 0)]
        public string Titulo
        {
            get => titulo;
            set => SetPropertyValue(nameof(Titulo), ref titulo, value);
        }

        [DbType("datetime2"), XafDisplayName("Fecha Reporte"), RuleRequiredField("BugReport.FechaReporte_Requerido", "Save")]
        [Index(1), DetailViewLayout("Bug ID", LayoutGroupType.SimpleEditorsGroup, 0)]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime FechaReporte
        {
            get => fechaReporte;
            set => SetPropertyValue(nameof(FechaReporte), ref fechaReporte, value);
        }

        [XafDisplayName("Reportado Por"), PersistentAlias(nameof(reportadoPor))]
        [Index(2), DetailViewLayout("Bug ID", LayoutGroupType.SimpleEditorsGroup, 0)]
        public string ReportadoPor => reportadoPor;

        /// <summary>
        /// Mostrar un editor de texto enriquecido para esta propiedad
        /// </summary>
        /// <remarks>
        /// Mas info: https://docs.devexpress.com/eXpressAppFramework/401419/concepts/extra-modules/office-module/ways-to-improve-the-performance-of-asp-net-office-module-editors#rich-text-editor
        /// https://docs.devexpress.com/eXpressAppFramework/400004/concepts/extra-modules/office-module/use-rich-text-documents-in-business-objects
        /// https://docs.devexpress.com/eXpressAppFramework/401210/task-based-help/office/how-to-customize-the-rich-text-editors
        /// </remarks>
        [XafDisplayName("Descripción"), VisibleInListView(false)]
        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.RichTextPropertyEditor)]
        [Index(3), DetailViewLayout("Resumen del Error", LayoutGroupType.SimpleEditorsGroup, 1)]
        public byte[] Descripcion
        {
            get => descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref descripcion, value);
        }

        [Size(80), DbType("varchar(80)"), XafDisplayName("Url")]
        [Index(4), DetailViewLayout("Resumen del Error", LayoutGroupType.SimpleEditorsGroup, 1)]
        public string Url
        {
            get => url;
            set => SetPropertyValue(nameof(Url), ref url, value);
        }

        [Size(SizeAttribute.Unlimited), ImageEditor(ListViewImageEditorMode = ImageEditorMode.PopupPictureEdit,
            DetailViewImageEditorMode = ImageEditorMode.PopupPictureEdit, ListViewImageEditorCustomHeight = 40)]
        [XafDisplayName("Captura Pantalla")]
        [Index(5), DetailViewLayout("Resumen del Error", LayoutGroupType.SimpleEditorsGroup, 1)]
        public byte[] CapturaPantalla
        {
            get => capturaPantalla;
            set => SetPropertyValue(nameof(CapturaPantalla), ref capturaPantalla, value);
        }

        [DbType("smallint"), XafDisplayName("Plataforma")]
        [Index(6), DetailViewLayout("Ambiente", LayoutGroupType.SimpleEditorsGroup, 2)]
        public EBugPlataforma Plataforma
        {
            get => plataforma;
            set => SetPropertyValue(nameof(Plataforma), ref plataforma, value);
        }

        [DbType("smallint"), XafDisplayName("Sistema Operativo")]
        [Index(7), DetailViewLayout("Ambiente", LayoutGroupType.SimpleEditorsGroup, 2)]
        public EBugSistemaOperativo SistemaOperativo
        {
            get => sistemaOperativo;
            set => SetPropertyValue(nameof(SistemaOperativo), ref sistemaOperativo, value);
        }

        [DbType("smallint"), XafDisplayName("Navegador")]
        [Index(8), DetailViewLayout("Ambiente", LayoutGroupType.SimpleEditorsGroup, 2)]
        public EBugNavegadorWeb Navegador
        {
            get => navegador;
            set => SetPropertyValue(nameof(Navegador), ref navegador, value);
        }

        [XafDisplayName("Pasos Reproducir"), VisibleInListView(false)]
        [Size(SizeAttribute.Unlimited), DbType("varchar(max)")]
        [EditorAlias(EditorAliases.RichTextPropertyEditor)]
        [Index(9), DetailViewLayout("Detalle del Error", LayoutGroupType.SimpleEditorsGroup, 3)]
        public string PasosReproducir
        {
            get => pasosReproducir;
            set => SetPropertyValue(nameof(PasosReproducir), ref pasosReproducir, value);
        }

        [Size(100), DbType("varchar(100)"), XafDisplayName("Resultado Esperado")]
        [Index(10), DetailViewLayout("Detalle del Error", LayoutGroupType.SimpleEditorsGroup, 3)]
        public string ResultadoEsperado
        {
            get => resultadoEsperado;
            set => SetPropertyValue(nameof(ResultadoEsperado), ref resultadoEsperado, value);
        }

        [Size(100), DbType("varchar(100)"), XafDisplayName("Resultado Actual")]
        [Index(11), DetailViewLayout("Detalle del Error", LayoutGroupType.SimpleEditorsGroup, 3)]
        public string ResultadoActual
        {
            get => resultadoActual;
            set => SetPropertyValue(nameof(ResultadoActual), ref resultadoActual, value);
        }

        [DbType("smallint"), XafDisplayName("Severidad")]
        [Index(12), DetailViewLayout("Seguimiento", LayoutGroupType.SimpleEditorsGroup, 4)]
        public EBugSeveridad DefectoSeveridad
        {
            get => defectoSeveridad;
            set => SetPropertyValue(nameof(DefectoSeveridad), ref defectoSeveridad, value);
        }

        [DbType("smallint"), XafDisplayName("Prioridad")]
        [Index(13), DetailViewLayout("Seguimiento", LayoutGroupType.SimpleEditorsGroup, 4)]
        public EBugPrioridad DefectoPrioridad
        {
            get => defectoPrioridad;
            set => SetPropertyValue(nameof(DefectoPrioridad), ref defectoPrioridad, value);
        }

        [Size(50), DbType("varchar(50)"), XafDisplayName("Asignado A")]
        [Index(14), DetailViewLayout("Seguimiento", LayoutGroupType.SimpleEditorsGroup, 4)]
        public string AsignadoA
        {
            get => asignadoA;
            set => SetPropertyValue(nameof(AsignadoA), ref asignadoA, value);
        }

        [DbType("Smallint"), XafDisplayName("Estado")]
        [Index(15), DetailViewLayout("Seguimiento", LayoutGroupType.SimpleEditorsGroup, 4)]
        public EBugEstado Estado
        {
            get => estado;
            set => SetPropertyValue(nameof(Estado), ref estado, value);
        }

        [Size(100), DbType("varchar(100)"), XafDisplayName("Corrección")]
        [Index(16), DetailViewLayout("Correción", LayoutGroupType.SimpleEditorsGroup, 5)]
        public string Correccion
        {
            get => correccion;
            set => SetPropertyValue(nameof(Correccion), ref correccion, value);
        }

        [DbType("datetime2"), XafDisplayName("Fecha Corrección"),
            RuleValueComparison("BugReport.FechaCorrecion >= FechaReporte", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual,
            "[FechaReporte]", ParametersMode.Expression, SkipNullOrEmptyValues = true)]
        [Index(17), DetailViewLayout("Correción", LayoutGroupType.SimpleEditorsGroup, 5)]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime FechaCorrecion
        {
            get => fechaCorrecion;
            set => SetPropertyValue(nameof(FechaCorrecion), ref fechaCorrecion, value);
        }


        [Size(400), DbType("varchar(400)"), XafDisplayName("Notas")]
        [Index(18), DetailViewLayout("Correción", LayoutGroupType.SimpleEditorsGroup, 5)]
        public string Notas
        {
            get => notas;
            set => SetPropertyValue(nameof(Notas), ref notas, value);
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}