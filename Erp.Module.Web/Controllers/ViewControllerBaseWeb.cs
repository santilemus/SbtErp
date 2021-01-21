using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace SBT.Apps.Erp.Module.Web.Controllers
{
    /// <summary>
    /// ViewController 
    /// Controlador base para los controladores específicos de las aplicaciones ASP.NET.
    /// Heredar de este controlador para implementar los específicos de los BO, que pueden tener mayor funcionalidad.
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ViewControllerBaseWeb : ViewController
    {
        public ViewControllerBaseWeb()
        {
            DoInitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            // ajustes del View cuando se trata de un ListView
            if (string.Compare(View.GetType().Name, "ListView", StringComparison.Ordinal) == 0)
            {
                if (FixColumnWidthInListView == ETipoAjusteColumnaListView.BestFit)
                    SetWebGridAdaptative();
            }
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.

            // Ajuste a los controles cuando se trata de un ListView
            if (string.Compare(View.GetType().Name, "ListView", StringComparison.Ordinal) == 0)
            {
                if (FixColumnWidthInListView == ETipoAjusteColumnaListView.Model)
                    ShowColumnWidthFromModel();
                else if (FixColumnWidthInListView == ETipoAjusteColumnaListView.BestFit)
                    ShowOnlyColumnsFitInScreen();
            }
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        protected virtual void DoInitializeComponent()
        {

        }

        private ETipoAjusteColumnaListView fFixColumnWidthInListView = ETipoAjusteColumnaListView.Default;
        public ETipoAjusteColumnaListView FixColumnWidthInListView
        {
            get => fFixColumnWidthInListView;
            set => fFixColumnWidthInListView = value;
        }

        /// <summary>
        /// Poner el ancho de las columnas del grid, segun el ancho definido por el usuario en el Model en cada ListView
        /// </summary>
        /// <remarks>
        /// 1. Mas info en: https://supportcenter.devexpress.com/ticket/details/q439787/how-to-adjust-column-width-in-listview-on-the-web
        /// 2. Invocar en la implementacion del método OnViewControlsCreated despues de base.OnViewControlsCreated()
        /// </remarks>
        private void ShowColumnWidthFromModel()
        {
            if (string.Compare(View.GetType().Name, "ListView", StringComparison.Ordinal) != 0)
                return;
            ASPxGridListEditor gridListEditor = (View as ListView).Editor as ASPxGridListEditor;
            if (gridListEditor != null)
            {
                ASPxGridView gridView = gridListEditor.Grid;
                gridView.Settings.UseFixedTableLayout = true;
                gridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                gridView.Width = Unit.Percentage(100);
                foreach (WebColumnBase column in gridView.Columns)
                {
                    IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)gridListEditor).GetColumnInfo(column);
                    if (columnInfo != null)
                    {
                        IModelColumn modelColumn = (IModelColumn)columnInfo.Model;
                        column.Width = Unit.Pixel(modelColumn.Width);
                    }
                }
            }
        }

        /// <summary>
        /// Ajustar el ListView y mostrar solamente las columnas que caben segun el tamaño de la pantalla. Alternativa
        /// a ajustar el ancho de las columnas segun el modelo, como se hace en el metodo anterior
        /// </summary>
        /// <remarks>
        /// 1. Mas info en: https://supportcenter.devexpress.com/ticket/details/q439787/how-to-adjust-column-width-in-listview-on-the-web
        /// 2. Invocar en la implementacion del método OnViewControlsCreated despues de base.OnViewControlsCreated()
        /// </remarks>
        private void ShowOnlyColumnsFitInScreen()
        {
            if (string.Compare(View.GetType().Name, "ListView", StringComparison.Ordinal) != 0)
                return;
            ASPxGridListEditor gridListEditor = (View as ListView).Editor as ASPxGridListEditor;
            if (gridListEditor != null)
            {
                ASPxGridView gridView = gridListEditor.Grid;
                foreach (WebColumnBase column in gridView.Columns)
                {
                    var dataColumn = column as GridViewDataColumn;
                    if (dataColumn != null)
                    {
                        dataColumn.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
                    }
                }
            }
        }

        /// <summary>
        /// Habilitar el diseño adaptativo y responsive par el grid. Debe invocarse en el OnActivated cuando se utilice
        /// ShowOnlyColumnsFitInScreen
        /// </summary>
        private void SetWebGridAdaptative()
        {
            if (string.Compare(View.GetType().Name, "ListView", StringComparison.Ordinal) != 0)
                return;
            var webGridListEditor = (View as ListView)?.Editor as ASPxGridListEditor;
            if (webGridListEditor != null)
            {
                webGridListEditor.IsAdaptive = true;
            }
        }

    }
}
