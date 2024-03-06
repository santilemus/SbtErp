using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp;

namespace SBT.Medico.Blazor.Server.Controllers
{
    public class CustomListViewControllerWeb : ViewController<ListView>
    {
        public CustomListViewControllerWeb()
        {

        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            if (View.Editor is DxGridListEditor gridListEditor)
            {
                IDxGridAdapter dataGridAdapter = gridListEditor.GetGridAdapter();
                dataGridAdapter.GridModel.ColumnResizeMode = DevExpress.Blazor.GridColumnResizeMode.ColumnsContainer;
            }
        }
    }
}
