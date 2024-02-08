using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;

namespace SBT.Apps.Facturacion.Module.Controllers
{
    /// <summary>
    /// Evaluar porque en este momento no se usa, borrar en caso de no usarse
    /// </summary>
    public class vcCxCTransaccionDetailView : ViewController
    {
        private NewObjectViewController controller;
        public vcCxCTransaccionDetailView() : base()
        {
            TargetObjectType = typeof(CxC.Module.BusinessObjects.CxCTransaccion);
            TargetViewType = ViewType.DetailView;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            controller = Frame.GetController<NewObjectViewController>();
            if (controller != null)
            {
                controller.ObjectCreated += controller_ObjectCreated;
            }
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            if (controller != null)
            {
                controller.ObjectCreated -= controller_ObjectCreated;
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();

        }

        private void controller_ObjectCreated(object sender, ObjectCreatedEventArgs e)
        {
            NestedFrame nestedFrame = Frame as NestedFrame;
            if (nestedFrame != null)
            {
                var createdItem = e.CreatedObject;
                if (createdItem != null)
                {
                    var parent = ((NestedFrame)Frame).ViewItem.CurrentObject;
                    if (parent != null)
                    {
                        ;
                    }
                }
            }
        }


    }
}
