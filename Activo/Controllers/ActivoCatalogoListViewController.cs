using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo.DB;
using SBT.Apps.Activo.Module.BusinessObjects;
using SBT.Apps.Base.Module.Controllers;
using System;

namespace SBT.Apps.Activo.Module.Controllers
{
    public class ActivoCatalogoListViewController : ViewControllerBase
    {
        private PopupWindowShowAction pwsaCalcularDepreciacion;
        private PopupWindowShowAction pwsaRevertirDepreciacion;
        public ActivoCatalogoListViewController() : base()
        {
            TargetObjectType = typeof(SBT.Apps.Activo.Module.BusinessObjects.ActivoCatalogo);
            TargetViewType = ViewType.ListView;

            // accion calcular la depreciacion
            pwsaCalcularDepreciacion = new PopupWindowShowAction(this, "Activo_Calcular_Depreciacion",
                DevExpress.Persistent.Base.PredefinedCategory.RecordEdit.ToString());
            pwsaCalcularDepreciacion.Caption = @"Calcular Depreciación";
            pwsaCalcularDepreciacion.TargetObjectType = typeof(SBT.Apps.Activo.Module.BusinessObjects.ActivoCatalogo);
            pwsaCalcularDepreciacion.ToolTip = @"Clic para ejecutar el proceso de cálculo de la depreciación";
            pwsaCalcularDepreciacion.AcceptButtonCaption = @"Calcular";
            pwsaCalcularDepreciacion.ImageName = "service";
            pwsaCalcularDepreciacion.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;

            // accion revertir la depreciacion
            pwsaRevertirDepreciacion = new PopupWindowShowAction(this, "Activo_Revertir_Depreciacion",
                DevExpress.Persistent.Base.PredefinedCategory.RecordEdit.ToString());
            pwsaRevertirDepreciacion.Caption = @"Aceptar";
            pwsaRevertirDepreciacion.TargetObjectType = typeof(SBT.Apps.Activo.Module.BusinessObjects.ActivoCatalogo);
            pwsaRevertirDepreciacion.ToolTip = @"Clic para ejecutar proceso que permite revetir la depreciación";
            pwsaRevertirDepreciacion.ImageName = "undo";
            pwsaRevertirDepreciacion.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
        }

        protected override void OnActivated()
        {
            pwsaCalcularDepreciacion.CustomizePopupWindowParams += PwsaCalcularDepreciacion_CustomizePopupWindowParams;
            pwsaCalcularDepreciacion.Execute += PwsaCalcularDepreciacion_Execute;
            pwsaRevertirDepreciacion.CustomizePopupWindowParams += PwsaRevertirDepreciacion_CustomizePopupWindowParams;
            pwsaRevertirDepreciacion.Execute += PwsaRevertirDepreciacion_Execute;
        }

        protected override void OnDeactivated()
        {
            pwsaCalcularDepreciacion.CustomizePopupWindowParams -= PwsaCalcularDepreciacion_CustomizePopupWindowParams;
            pwsaCalcularDepreciacion.Execute -= PwsaCalcularDepreciacion_Execute;
            pwsaRevertirDepreciacion.CustomizePopupWindowParams -= PwsaRevertirDepreciacion_CustomizePopupWindowParams;
            pwsaRevertirDepreciacion.Execute -= PwsaRevertirDepreciacion_Execute;
        }

        private void PwsaCalcularDepreciacion_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            var pp = (e.PopupWindowViewCurrentObject as DepreciacionParams);
            DateTime fechaDepreciacion = new DateTime(pp.MesDepreciacion.Year, pp.MesDepreciacion.Month, 01).AddMonths(1).AddSeconds(-1);
            var uow = (ObjectSpace as XPObjectSpace).Session;
            try
            {
                // PENDIENTE DE CREAR EL PROCEDIMIENTO EN LA BD
                uow.ExecuteNonQuery(@"execute spGenerarDepreciacion @empresaOid, @fechaDepreciacion, @valorMinimo, @vidaUtilMinima",
                    new object[] { EmpresaOid, fechaDepreciacion, pp.ValorMinimo, pp.VidaUtilMinima });
                Application.ShowViewStrategy.ShowMessage(@"El cálculo de la depreciación finalizó");
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage($@"La depreciación no se calculó por el siguiente error {ex.Message}");
            }
        }

        private void PwsaCalcularDepreciacion_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(SBT.Apps.Activo.Module.BusinessObjects.DepreciacionParams));
            var pa = os.CreateObject<SBT.Apps.Activo.Module.BusinessObjects.DepreciacionParams>();
            e.View = Application.CreateDetailView(os, pa);
        }

        private void PwsaRevertirDepreciacion_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            var pp = (e.PopupWindowViewCurrentObject as DepreciacionRevertirParams);
            DateTime fechaDepreciacion = new DateTime(pp.MesDepreciacion.Year, pp.MesDepreciacion.Month, 01).AddMonths(1).AddSeconds(-1);
            var uow = (ObjectSpace as XPObjectSpace).Session;
            try
            {
                // PENDIENTE DE CREAR EL PROCEDIMIENTO EN LA BD
                uow.ExecuteNonQuery(@"execute spRevertirDepreciacion @empresaOid, @fechaDepreciacion",
                    new object[] { EmpresaOid, fechaDepreciacion});
                Application.ShowViewStrategy.ShowMessage(@"La reversión de la última depreciación finalizó");
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage($@"La reversión de la depreciación falló por el siguiente error {ex.Message}");
            }
        }

        private void PwsaRevertirDepreciacion_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(SBT.Apps.Activo.Module.BusinessObjects.DepreciacionRevertirParams));
            var pa = os.CreateObject<SBT.Apps.Activo.Module.BusinessObjects.DepreciacionRevertirParams>();
            e.View = Application.CreateDetailView(os, pa);
        }

    }
}
