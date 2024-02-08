using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Editors;

namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class BaseListViewControllerWeb: ViewController<ListView>
    {
        public BaseListViewControllerWeb() 
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
