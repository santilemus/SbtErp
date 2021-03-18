using System;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.ViewVariantsModule;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.Controllers;
using DevExpress.ExpressApp.Actions;
using SBT.Apps.Facturacion.Module.BusinessObjects;

namespace SBT.Apps.Facturacion.Module.Controllers
{
    public class vcVentaViewDetail : ViewControllerBase
    {
        private NewObjectViewController newObjectController;
        private ChangeVariantController changeVariantController;       

        public vcVentaViewDetail()
        {
            TargetObjectType = typeof(SBT.Apps.Facturacion.Module.BusinessObjects.Venta);
            TargetViewType = ViewType.DetailView;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            newObjectController = Frame.GetController<NewObjectViewController>();
            //if (newObjectController != null)
            //    newObjectController.ObjectCreated += NewObjectController_ObjectCreated;
            changeVariantController = Frame.GetController<ChangeVariantController>();
            ViewControlsCreated += VentaViewDetail_ViewControlsCreated;
            //changeVariantController.AllowChangeVariantWhenObjectSpaceIsModified = false;
            changeVariantController.ChangeVariantAction.Executing += ChangeVariantAction_Executing;
        }

        protected override void OnDeactivated()
        {
            //if (newObjectController != null)
            //    newObjectController.ObjectCreated -= NewObjectController_ObjectCreated;
            ViewControlsCreated -= VentaViewDetail_ViewControlsCreated;
            changeVariantController.ChangeVariantAction.Executing -= ChangeVariantAction_Executing;
            base.OnDeactivated();
        }

        private void NewObjectController_ObjectCreated(object sender, ObjectCreatedEventArgs e)
        {
            if (e.CreatedObject is Venta)
            {
                AsignarTipoFactura(e.CreatedObject, e.ObjectSpace);
            }
        }

        private void VentaViewDetail_ViewControlsCreated(object sender, EventArgs e)
        {
            if (View == null || View.CurrentObject == null)
                return;
            if (View.CurrentObject is Venta)
            {
                Venta ventaObj = View.CurrentObject as Venta;
                if (ObjectSpace.IsNewObject(ventaObj))
                {
                    AsignarTipoFactura(ventaObj, ObjectSpace);
                }
            }

        }

        private void AsignarTipoFactura(object createdObject, IObjectSpace os)
        {
            string fCodigo;
            fCodigo = "COVE01";
            if (View.Id == "Venta_DetailView_fcf")
                fCodigo = "COVE02";
            else if (View.Id == "Venta_DetailView_exportacion")
                fCodigo = "COVE03";
            var fTipoFactura = os.GetObjectByKey<SBT.Apps.Base.Module.BusinessObjects.Listas>(fCodigo);
            if (fTipoFactura != null)
                ((Venta)createdObject).TipoFactura = fTipoFactura;
        }

        private void ChangeVariantAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (View == null || View.CurrentObject == null)
                return;
            if (View.CurrentObject is Venta)
            {
                Venta ventaObj = View.CurrentObject as Venta;
                if (ObjectSpace.IsNewObject(ventaObj) || ObjectSpace.IsModified)
                {
                    MostrarError("No puede cambiar la vista para un objeto que nuevo o que está siendo editado");
                    e.Cancel = true;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (newObjectController != null)
                newObjectController.Dispose();
            if (changeVariantController != null)
                changeVariantController.Dispose();
            base.Dispose(disposing);
        }
    }
}
