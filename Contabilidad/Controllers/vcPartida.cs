using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Model;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Contabilidad.Module.BusinessObjects;
using SBT.Apps.Base.Module.Controllers;


namespace SBT.Apps.Contabilidad.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class vcPartida : ViewControllerBase
    {
        DevExpress.ExpressApp.View vParam;
        public vcPartida()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            PwaCierreDiario.Executing += Pwa_Executing;
            PwaAbrirDias.Executing += Pwa_Executing;
            PwaCierreDiario.Executing += PwaCierreMes_Executing;
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            PwaCierreDiario.Executing -= Pwa_Executing;
            PwaAbrirDias.Executing -= Pwa_Executing;
            PwaCierreMes.Executing -= PwaCierreMes_Executing;
            base.OnDeactivated();
        }

        private void DoCustomizePopupWindowCierre(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace objectSpace = (NonPersistentObjectSpace)Application.ObjectSpaceProviders[1].CreateObjectSpace(); //Application.CreateObjectSpace(typeof(CierreDiarioParams));
            var pa = objectSpace.CreateObject<CierreDiarioParam>();
            //pa.FechaDesde = DateTime.Now;
            //pa.FechaHasta = DateTime.Now;
            e.View = Application.CreateDetailView(objectSpace, pa);
            e.Size = new System.Drawing.Size(500, 200);
            e.IsSizeable = false;
            vParam = e.View;
        }

        private void DoBeforeExecute(ref PopupWindowShowActionExecuteEventArgs e, int emp, DateTime d1, DateTime d2)
        {
            ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora = vParam.Caption + Environment.NewLine;
            ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += string.Format("Hora Inicio: {0:G}", DateTime.Now) + Environment.NewLine;
            ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += Environment.NewLine + "P a r a m e t r o s" + Environment.NewLine;
            ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += string.Format("Empresa ==> {0}", emp) + Environment.NewLine;
            ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += string.Format("Fecha Desde ==> {0:G}", d1) + Environment.NewLine;
            ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += string.Format("Fecha Hasta ==> {0:G}", d2) + Environment.NewLine;
        }

        private void PwaCierreDiario_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            DoCustomizePopupWindowCierre(sender, e);
            e.View.Caption = "Cierre Diario";
        }

        private void PwaCierreDiario_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            var EmpresaOid = ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid;
            var sUsuario = ((Usuario)SecuritySystem.CurrentUser).UserName;
            var fechaDesde = ((CierreDiarioParam)e.PopupWindowViewCurrentObject).FechaDesde;
            var fechaHasta = ((CierreDiarioParam)e.PopupWindowViewCurrentObject).FechaHasta;
            DoBeforeExecute(ref e, EmpresaOid, fechaDesde, fechaHasta);
            if (EmpresaOid <= 0)
            {
                ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += "No tiene valor la variable de la empresa de la sesion" + Environment.NewLine;
                ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += string.Format("Hora Finalizó: {0:G}", DateTime.Now) + Environment.NewLine;
                throw new UserFriendlyException("No tiene valor la variable de la empresa de la sesion");
            }
            ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += Environment.NewLine + "Ejecutando el Proceso" + Environment.NewLine;       
            IObjectSpace ospace = Application.ObjectSpaceProvider.CreateObjectSpace();
            ((XPObjectSpace)ospace).Session.ExecuteNonQuery("exec spConCierreDiario @Empresa, @FechaDesde, @FechaHasta, @Usuario",
                new string[] { "@Empresa", "@FechaDesde", "@FechaHasta", "@Usuario" }, new object[] { EmpresaOid, fechaDesde, fechaHasta, sUsuario });
            ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += string.Format("Hora Finalizó: {0:G}", DateTime.Now);
            //var OS = Application.CreateObjectSpace(typeof(AuditoriaProceso));
            var obj = ospace.CreateObject<AuditoriaProceso>();
            obj.AuditarProceso(PwaCierreDiario.Caption, "", ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora);
            (View.ObjectSpace as XPObjectSpace).Refresh();
            e.CanCloseWindow = false;
            ospace.Dispose();
            View.ObjectSpace.Refresh();
            MostrarMensajeResultado("El Cierre Diario del período ingresado se completo con éxito");
        }

        private void PwaAbrirDias_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            var EmpresaOid = ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid;
            var sUsuario = ((Usuario)SecuritySystem.CurrentUser).UserName;
            var fechaDesde = ((CierreDiarioParam)e.PopupWindowViewCurrentObject).FechaDesde;
            var fechaHasta = ((CierreDiarioParam)e.PopupWindowViewCurrentObject).FechaHasta;
            DoBeforeExecute(ref e, EmpresaOid, fechaDesde, fechaHasta);
            if (EmpresaOid <= 0)
            {
                ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += "No tiene valor la variable de la empresa de la sesion" + Environment.NewLine;
                ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += string.Format("Hora Finalizó: {0:G}", DateTime.Now) + Environment.NewLine;
                throw new UserFriendlyException("No tiene valor la variable de la empresa de la sesion");
            }
            ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += Environment.NewLine + "Ejecutando el Proceso" + Environment.NewLine;
            IObjectSpace ospace = Application.ObjectSpaceProvider.CreateObjectSpace();
            ((XPObjectSpace)ospace).Session.ExecuteNonQuery("exec spConAbrirDias @Empresa, @FechaDesde, @FechaHasta, @Usuario",
                new string[] { "@Empresa", "@FechaDesde", "@FechaHasta", "@Usuario" }, new object[] { EmpresaOid, fechaDesde, fechaHasta, sUsuario });
            ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += string.Format("Hora Finalizó: {0:G}", DateTime.Now);
            var obj = ospace.CreateObject<AuditoriaProceso>();
            obj.AuditarProceso(PwaAbrirDias.Caption, "", ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora);
            (View.ObjectSpace as XPObjectSpace).Refresh();
            e.CanCloseWindow = false;
            ospace.Dispose();
            View.ObjectSpace.Refresh();
            MostrarMensajeResultado("La apertura de los días se realizó con éxito");
        }

        private void PwaAbrirDias_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            DoCustomizePopupWindowCierre(sender, e);
            e.View.Caption = "Abrir Días";
        }

        private void Pwa_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RuleSetValidationResult result = Validator.RuleSet.ValidateTarget(vParam.ObjectSpace, vParam.CurrentObject, "Accept");
            if (!((CierreDiarioParam)vParam.CurrentObject).ValidarPeriodo)
                result.AddResult(new RuleSetValidationResultItem(vParam.CurrentObject, "Accept", 
                    new RuleCriteria(), new RuleValidationResult(new RuleCriteria(), ValidationState.Invalid, "El proceso no se ejecutará, porque el Periodo no es Valido")));
            if (result.State == ValidationState.Invalid)
            {
                e.Cancel = true;
                ((CierreDiarioParam)vParam.CurrentObject).Bitacora = result.GetFormattedErrorMessage();
                MostrarError(string.Format("El proceso no se ejecutó porque el periodo entre {0:G} y {1:G} no es válido",
                    ((CierreDiarioParam)vParam.CurrentObject).FechaDesde, ((CierreDiarioParam)vParam.CurrentObject).FechaHasta));
            }
        }

        private void PwaCierreMes_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace objectSpace = (NonPersistentObjectSpace)Application.ObjectSpaceProviders[1].CreateObjectSpace(); //Application.CreateObjectSpace(typeof(CierreDiarioParams));
            var pa = objectSpace.CreateObject<CierreMesParam>();
            //pa.FechaDesde = DateTime.Now;
            //pa.FechaHasta = DateTime.Now;
            e.View = Application.CreateDetailView(objectSpace, pa);
            e.Size = new System.Drawing.Size(500, 200);
            e.IsSizeable = false;
            vParam = e.View;
        }

        private void PwaCierreMes_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            RuleSetValidationResult result = Validator.RuleSet.ValidateTarget(vParam.ObjectSpace, vParam.CurrentObject, "Accept");
            //if (!((CierreMesParam)vParam.CurrentObject).ValidarMesCerrar())
            //    result.AddResult(new RuleSetValidationResultItem(vParam.CurrentObject, "Accept",
            //        new RuleCriteria(), new RuleValidationResult(new RuleCriteria(), ValidationState.Invalid, "El proceso no se ejecutará, no es válido realizar el cierre para el mes y periodo seleccionado")));
            if (result.State == ValidationState.Invalid)
            {
                e.Cancel = true;
                ((CierreMesParam)vParam.CurrentObject).Bitacora = result.GetFormattedErrorMessage();
                //throw new ValidationException(result); LINEA comentariada para no mostrar la excepción, porque el mensaje de error se muestra en la bitacora
            }
        }

        private void PwaCierreMes_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            var empresaOid = ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid;
            var fechaCierre = ((CierreMesParam)e.PopupWindowViewCurrentObject).FechaCierre;
            ((CierreMesParam)e.PopupWindowViewCurrentObject).Bitacora = vParam.Caption + Environment.NewLine;
            ((CierreMesParam)e.PopupWindowViewCurrentObject).Bitacora += string.Format("Hora Inicio: {0:G}", DateTime.Now) + Environment.NewLine;
            ((CierreMesParam)e.PopupWindowViewCurrentObject).Bitacora += Environment.NewLine + "P a r a m e t r o s" + Environment.NewLine;
            ((CierreMesParam)e.PopupWindowViewCurrentObject).Bitacora += string.Format("Empresa ==> {0}", empresaOid) + Environment.NewLine;
            ((CierreMesParam)e.PopupWindowViewCurrentObject).Bitacora += string.Format("Fecha Cierre ==> {0:G}", fechaCierre) + Environment.NewLine;
            if (empresaOid <= 0)
            {
                ((CierreMesParam)e.PopupWindowViewCurrentObject).Bitacora += "No tiene valor la variable de la empresa de la sesion" + Environment.NewLine;
                ((CierreMesParam)e.PopupWindowViewCurrentObject).Bitacora += string.Format("Hora Finalizó: {0:G}", DateTime.Now) + Environment.NewLine;
                MostrarError("No tiene valor la variable de la empresa de la sesion");
            }
            ((CierreMesParam)e.PopupWindowViewCurrentObject).Bitacora += Environment.NewLine + "Ejecutando el Proceso" + Environment.NewLine;
            IObjectSpace ospace = Application.ObjectSpaceProvider.CreateObjectSpace();
            // esta parte revisarla, porque no es esto
            ((XPObjectSpace)ospace).Session.ExecuteNonQuery("exec spConAbrirDias @Empresa, @FechaDesde, @FechaHasta, @Usuario",
                new string[] { "@Empresa", "@FechaDesde", "@FechaHasta", "@Usuario" }, new object[] { empresaOid, fechaCierre });
            // --
            ((CierreMesParam)e.PopupWindowViewCurrentObject).Bitacora += string.Format("Hora Finalizó: {0:G}", DateTime.Now);
            var obj = ospace.CreateObject<AuditoriaProceso>();
            obj.AuditarProceso(PwaAbrirDias.Caption, "", ((CierreMesParam)e.PopupWindowViewCurrentObject).Bitacora);
            (View.ObjectSpace as XPObjectSpace).Refresh();
            e.CanCloseWindow = false;
            ospace.Dispose();
            View.ObjectSpace.Refresh();
        }

    }
}