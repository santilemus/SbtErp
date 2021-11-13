using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Base.Module.Controllers;
using SBT.Apps.Contabilidad.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;


namespace SBT.Apps.Contabilidad.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public class vcPartida : ViewControllerBase
    {
        DevExpress.ExpressApp.View vParam;

        private PopupWindowShowAction pwaCierreDiario;
        private PopupWindowShowAction pwaAbrirDias;
        private PopupWindowShowAction pwaCierreMes;
        private SimpleAction saApertura;
        private SimpleAction saLiquidacionCierre;

        public vcPartida() : base()
        {
            TargetViewNesting = Nesting.Root;
            // vcPartida
            // 
            TargetObjectType = typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.Partida);
            //
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            pwaCierreDiario.Executing += PwaAbrirCierre_Executing;
            pwaCierreDiario.Execute += PwaCierreDiario_Execute;
            pwaCierreMes.Executing += PwaCierreMes_Executing;
            pwaCierreMes.Execute += PwaCierreMes_Execute;
            pwaAbrirDias.Executing += PwaAbrirCierre_Executing;
            pwaAbrirDias.Execute += PwaAbrirDias_Execute;
            pwaCierreDiario.Executing += PwaCierreMes_Executing;
            saApertura.Execute += saPartidaApertura_Execute;
            saApertura.Executing += saPartidaApertura_Executing;
            saLiquidacionCierre.Execute += saPartidaLiquidacionCierre_Execute;
            saLiquidacionCierre.Executing += saPartidaApertura_Executing;
            ObjectSpace.Committed += ObjectSpace_Commited;
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            pwaCierreDiario.Executing -= PwaAbrirCierre_Executing;
            pwaCierreDiario.Execute -= PwaCierreDiario_Execute;
            pwaAbrirDias.Executing -= PwaAbrirCierre_Executing;
            pwaAbrirDias.Execute -= PwaAbrirDias_Execute;
            pwaCierreMes.Executing -= PwaCierreMes_Executing;
            pwaCierreMes.Execute -= PwaCierreMes_Execute;
            saApertura.Execute -= saPartidaApertura_Execute;
            saApertura.Executing -= saPartidaApertura_Executing;
            saLiquidacionCierre.Execute -= saPartidaLiquidacionCierre_Execute;
            saLiquidacionCierre.Executing -= saPartidaApertura_Executing;
            ObjectSpace.Committed -= ObjectSpace_Commited;
            base.OnDeactivated();
        }

        protected override void DoInitializeComponent()
        {
            base.DoInitializeComponent();
            // PwaCierreDiario
            pwaCierreDiario = new PopupWindowShowAction(this, "Partida_pwaCierreDiario", DevExpress.Persistent.Base.PredefinedCategory.RecordsNavigation);
            pwaCierreDiario.AcceptButtonCaption = "Aceptar";
            pwaCierreDiario.ActionMeaning = DevExpress.ExpressApp.Actions.ActionMeaning.Accept;
            pwaCierreDiario.CancelButtonCaption = "Cerrar";
            pwaCierreDiario.Caption = "Cierre Diario";
            pwaCierreDiario.ImageName = "CierreDiario";
            pwaCierreDiario.TargetObjectType = typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.Partida);
            pwaCierreDiario.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            pwaCierreDiario.ToolTip = "Realizar el cierre diario contable";
            pwaCierreDiario.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            pwaCierreDiario.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.PwaCierreDiario_CustomizePopupWindowParams);
            // PwaAbrirDias
            pwaAbrirDias = new PopupWindowShowAction(this, "Partida_pwaAbrirDias", DevExpress.Persistent.Base.PredefinedCategory.RecordsNavigation);
            pwaAbrirDias.AcceptButtonCaption = "Aceptar";
            pwaAbrirDias.ActionMeaning = DevExpress.ExpressApp.Actions.ActionMeaning.Accept;
            pwaAbrirDias.CancelButtonCaption = "Cerrar";
            pwaAbrirDias.Caption = "Abrir Días";
            pwaAbrirDias.ImageName = "abrircaja";
            pwaAbrirDias.TargetObjectType = typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.Partida);
            pwaAbrirDias.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            pwaAbrirDias.ToolTip = "Abrir rango de dias que ya se encuentran cerrados";
            pwaAbrirDias.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            pwaAbrirDias.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.PwaAbrirDias_CustomizePopupWindowParams);
            // PwaCierreMes
            pwaCierreMes = new PopupWindowShowAction(this, "Partida_pwaCierreMes", DevExpress.Persistent.Base.PredefinedCategory.RecordsNavigation);
            pwaCierreMes.AcceptButtonCaption = "Aceptar";
            pwaCierreMes.ActionMeaning = DevExpress.ExpressApp.Actions.ActionMeaning.Accept;
            pwaCierreMes.CancelButtonCaption = "Cerrar";
            pwaCierreMes.Caption = "Cierre de Mes";
            pwaCierreMes.ImageName = "service";
            pwaCierreMes.TargetObjectType = typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.Partida);
            pwaCierreMes.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            pwaCierreMes.ToolTip = "Cierre contable de un Mes. Este proceso no puede revertirse";
            pwaCierreMes.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            pwaCierreMes.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.PwaCierreMes_CustomizePopupWindowParams);
            // saPartidaApertura
            saApertura = new SimpleAction(this, "saPartidaApertura", DevExpress.Persistent.Base.PredefinedCategory.ObjectsCreation);
            saApertura.Caption = "Partida de Apertura";
            saApertura.TargetObjectType = typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.Partida);
            saApertura.TargetViewType = ViewType.DetailView;
            saApertura.ToolTip = "Clic para generar la partida de apertura del período a partir de la partida de cierre del período anterior";
            saApertura.TargetObjectsCriteria = "[Tipo] ==  0 And [Detalles][].Count() == 0";
            saApertura.TargetObjectsCriteriaMode = TargetObjectsCriteriaMode.TrueForAll;
            saApertura.ConfirmationMessage = "Desea generar Partida de Apertura?";
            saApertura.ImageName = "book";
            // saPartidaLiquidacion
            saLiquidacionCierre = new SimpleAction(this, "saPartidaLiquidacion", DevExpress.Persistent.Base.PredefinedCategory.ObjectsCreation);
            saLiquidacionCierre.Caption = "Partida Liquidación";
            saLiquidacionCierre.TargetObjectType = typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.Partida);
            saLiquidacionCierre.TargetViewType = ViewType.DetailView;
            saLiquidacionCierre.ToolTip = "Clic para generar la partida de Liquidación o de cierre del período";
            saLiquidacionCierre.TargetObjectsCriteria = "[Tipo] == 4 && [Detalles][].Count() == 0";
            saLiquidacionCierre.TargetObjectsCriteriaMode = TargetObjectsCriteriaMode.TrueForAll;
            saLiquidacionCierre.ConfirmationMessage = "Confirmar que desea generar la Partida?";
            saLiquidacionCierre.ImageName = "recibo";
        }

        protected override void DoDispose()
        {
            pwaCierreDiario.Dispose();
            pwaAbrirDias.Dispose();
            pwaCierreMes.Dispose();

            saApertura.Dispose();
            saLiquidacionCierre.Dispose();
        }

        private void saPartidaApertura_Execute(Object sender, SimpleActionExecuteEventArgs e)
        {
            Partida ptda = (e.CurrentObject as Partida);
            if (ptda.Detalles.Count() == 0)
                return;
            UnitOfWork uow = new UnitOfWork(((XPObjectSpace)ObjectSpace).Session.DataLayer);
            PartidaAutomatica partidaAutomatica = new PartidaAutomatica(uow, ptda.Empresa);
            string sMsg = string.Empty;
            partidaAutomatica.PartidaApertura(ptda, out sMsg);
        }

        /// <summary>
        /// Validar previo a la ejecucion de la generacion de partidas de Apertura, Liquidacion o Cierre que no exista otra similar
        /// </summary>
        /// <param name="sender">El BO que dispara el evento</param>
        /// <param name="e">los parametros del evento</param>
        private void saPartidaApertura_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if ((View.CurrentObject == null || (View.CurrentObject as Partida).Empresa == null || (View.CurrentObject as Partida).Periodo == null))
            {
                e.Cancel = true;
                MostrarError($"El proceso se cancela porque una propiedad para buscar la partida de {Convert.ToString((View.CurrentObject as Partida).Tipo)} en el período actual es nula");
            }
            var ptda = ObjectSpace.FindObject<Partida>(CriteriaOperator.Parse("[Empresa.Oid] == ? && [Periodo.Oid] == ? && [Tipo] == 0",
                (View.CurrentObject as Partida).Empresa.Oid, (View.CurrentObject as Partida).Periodo.Oid));
            if (ptda != null)
            {
                e.Cancel = true;
                MostrarError($"Ya existe una partida de {Convert.ToString((View.CurrentObject as Partida).Tipo)}. Solo puede existir una por período");
            }
        }

        private void saPartidaLiquidacionCierre_Execute(Object sender, SimpleActionExecuteEventArgs e)
        {
            Partida ptda = (e.CurrentObject as Partida);
            if (ptda.Detalles.Count() == 0)
                return;
            UnitOfWork uow = new UnitOfWork(((XPObjectSpace)ObjectSpace).Session.DataLayer);
            PartidaAutomatica partidaAutomatica = new PartidaAutomatica(uow, ptda.Empresa);
            string sMsg = string.Empty;
            partidaAutomatica.PartidaLiquidacionOCierre(ptda, out sMsg);
        }

        private void DoCustomizePopupWindowCierre(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace objectSpace = (NonPersistentObjectSpace)Application.ObjectSpaceProviders[1].CreateObjectSpace(); //Application.CreateObjectSpace(typeof(CierreDiarioParams));
            var pa = objectSpace.CreateObject<CierreDiarioParam>();
            //pa.FechaDesde = DateTime.Now;
            //pa.FechaHasta = DateTime.Now;
            e.View = Application.CreateDetailView(objectSpace, pa);
            e.Size = new System.Drawing.Size(500, 400);
            e.IsSizeable = false;
            vParam = e.View;
        }

        private void DoBeforeExecute(ref PopupWindowShowActionExecuteEventArgs e, int emp, DateTime d1, DateTime d2)
        {
            ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora = $"{vParam.Caption}{Environment.NewLine}";
            ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += $"Hora Inicio: {DateTime.Now:G}  {Environment.NewLine}";
            ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += $"{Environment.NewLine}P a r a m e t r o s{Environment.NewLine}";
            ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += $"Empresa ==> {emp} {Environment.NewLine}";
            ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += $"Fecha Desde ==> {d1:G} {Environment.NewLine}";
            ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += $"Fecha Hasta ==> {d2:G} {Environment.NewLine}";
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
                ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += $"No tiene valor la variable de la empresa de la sesion{Environment.NewLine}";
                ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += $"Hora Finalizó: {DateTime.Now:G} {Environment.NewLine}";
                throw new UserFriendlyException("No tiene valor la variable de la empresa de la sesion");
            }
            ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += $"{Environment.NewLine}Ejecutando el Proceso{Environment.NewLine}";
            var uow = new UnitOfWork((View.ObjectSpace as XPObjectSpace).Session.DataLayer);
            uow.BeginTransaction();
            uow.ExecuteNonQuery("exec spConCierreDiario @Empresa, @FechaDesde, @FechaHasta, @Usuario",
                new string[] { "@Empresa", "@FechaDesde", "@FechaHasta", "@Usuario" }, 
                new object[] { EmpresaOid, fechaDesde, fechaHasta, sUsuario });
            //uow.CommitChanges();
            ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += $"Hora Finalizó: {DateTime.Now:G}";
            //var OS = Application.CreateObjectSpace(typeof(AuditoriaProceso));
            AuditoriaProceso audit = new AuditoriaProceso(uow);
            audit.AuditarProceso(pwaCierreDiario.Caption, "", ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora);
            uow.CommitTransaction();
            uow.Disconnect();
            uow.Dispose();
            View.ObjectSpace.Refresh();
            e.CanCloseWindow = false;
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
                ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += $"No tiene valor la variable de la empresa de la sesion{Environment.NewLine}";
                ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += $"Hora Finalizó: {DateTime.Now:G}{Environment.NewLine}";
                throw new UserFriendlyException("No tiene valor la variable de la empresa de la sesion");
            }
            ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += $"{Environment.NewLine}Ejecutando el Proceso{Environment.NewLine}";
            IObjectSpace ospace = Application.ObjectSpaceProvider.CreateObjectSpace();
            ((XPObjectSpace)ospace).Session.ExecuteNonQuery("exec spConAbrirDias @Empresa, @FechaDesde, @FechaHasta, @Usuario",
                new string[] { "@Empresa", "@FechaDesde", "@FechaHasta", "@Usuario" }, new object[] { EmpresaOid, fechaDesde, fechaHasta, sUsuario });
            ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += $"Hora Finalizó: {DateTime.Now:G}";
            var obj = ospace.CreateObject<AuditoriaProceso>();
            obj.AuditarProceso(pwaAbrirDias.Caption, "", ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora);
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

        private void PwaAbrirCierre_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RuleSetValidationResult result = Validator.RuleSet.ValidateTarget(vParam.ObjectSpace, vParam.CurrentObject, "Accept");
            if (!((CierreDiarioParam)vParam.CurrentObject).ValidarPeriodo)
                result.AddResult(new RuleSetValidationResultItem(vParam.CurrentObject, "Accept",
                    new RuleCriteria(), new RuleValidationResult(new RuleCriteria(), ValidationState.Invalid, "El proceso no se ejecutará, porque el Periodo no es Valido")));
            if (result.State == ValidationState.Invalid)
            {
                e.Cancel = true;
                ((CierreDiarioParam)vParam.CurrentObject).Bitacora = result.GetFormattedErrorMessage();
                MostrarError($"El proceso no se ejecutó porque el periodo entre {((CierreDiarioParam)vParam.CurrentObject).FechaDesde:G} y {((CierreDiarioParam)vParam.CurrentObject).FechaHasta:G} no es válido");
            }
        }

        private void PwaCierreMes_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace objectSpace = (NonPersistentObjectSpace)Application.ObjectSpaceProviders[1].CreateObjectSpace(); //Application.CreateObjectSpace(typeof(CierreDiarioParams));
            var pa = objectSpace.CreateObject<CierreMesParam>();
            //pa.FechaDesde = DateTime.Now;
            //pa.FechaHasta = DateTime.Now;
            e.View = Application.CreateDetailView(objectSpace, pa);
            e.Size = new System.Drawing.Size(500, 400);
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
            ((CierreMesParam)e.PopupWindowViewCurrentObject).Bitacora = $"{vParam.Caption}{Environment.NewLine}";
            ((CierreMesParam)e.PopupWindowViewCurrentObject).Bitacora += $"Hora Inicio: {DateTime.Now:G} {Environment.NewLine}";
            ((CierreMesParam)e.PopupWindowViewCurrentObject).Bitacora += $"{Environment.NewLine}P a r a m e t r o s{Environment.NewLine}";
            ((CierreMesParam)e.PopupWindowViewCurrentObject).Bitacora += $"Empresa ==> {empresaOid} {Environment.NewLine}";
            ((CierreMesParam)e.PopupWindowViewCurrentObject).Bitacora += $"Fecha Cierre ==> {fechaCierre:G} {Environment.NewLine}";
            if (empresaOid <= 0)
            {
                ((CierreMesParam)e.PopupWindowViewCurrentObject).Bitacora += $"No tiene valor la variable de la empresa de la sesion{Environment.NewLine}";
                ((CierreMesParam)e.PopupWindowViewCurrentObject).Bitacora += $"Hora Finalizó: {DateTime.Now:G} {Environment.NewLine}";
                MostrarError("No tiene valor la variable de la empresa de la sesion");
            }
            ((CierreMesParam)e.PopupWindowViewCurrentObject).Bitacora += $"{Environment.NewLine}Ejecutando el Proceso{Environment.NewLine}";
            UnitOfWork uow = new UnitOfWork((View.ObjectSpace as XPObjectSpace).Session.DataLayer);
            // ***** OJO, REVISAR ESTA PARTE NO ES ESTO ****
            uow.ExecuteNonQuery("exec spConAbrirDias @Empresa, @FechaDesde, @FechaHasta, @Usuario",
                new string[] { "@Empresa", "@FechaDesde", "@FechaHasta", "@Usuario" }, new object[] { empresaOid, fechaCierre });
            // --
            ((CierreMesParam)e.PopupWindowViewCurrentObject).Bitacora += $"Hora Finalizó: {DateTime.Now:G}";
            var obj = new AuditoriaProceso(uow);
            obj.AuditarProceso(pwaAbrirDias.Caption, "", ((CierreMesParam)e.PopupWindowViewCurrentObject).Bitacora);
            (View.ObjectSpace as XPObjectSpace).Refresh();
            e.CanCloseWindow = false;
            uow.Disconnect();
            uow.Dispose();
            View.ObjectSpace.Refresh();
        }

        /// <summary>
        /// Actualizar el saldo de las cuentas de control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ObjectSpace_Commited(object sender, EventArgs e)
        {
            //var detalles = ObjectSpace.ModifiedObjects
            //                 .Cast<PartidaDetalle>()
            //                 .Where(x => x.GetType() == typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.PartidaDetalle));
            using (UnitOfWork uow = new UnitOfWork(((XPObjectSpace)ObjectSpace).Session.DataLayer))
            {
                var res = from d in (View.CurrentObject as Partida).Detalles
                          group d by new { d.Partida.Empresa, d.Partida.Periodo, d.Cuenta, Fecha = d.Partida.Fecha.Date } into x
                          select new
                          {
                              Empresa = x.Key.Empresa,
                              Periodo = x.Key.Periodo,
                              Fecha = x.Key.Fecha,
                              Cuenta = x.Key.Cuenta,
                              TotDebe = x.Sum(y => y.ValorDebe),
                              TotHaber = x.Sum(y => y.ValorHaber),
                              AjusteDebe = x.Where(y => y.AjusteConsolidacion == ETipoOperacionConsolidacion.Cargo).Sum(y => y.ValorHaber),
                              AjusteHaber = x.Where(y => y.AjusteConsolidacion == ETipoOperacionConsolidacion.Abono).Sum(y => y.ValorDebe)
                          };
                foreach (var item in res)
                {
                    SaldoDiario sd = uow.FindObject<SaldoDiario>(
                        CriteriaOperator.Parse("[Fecha] == ? && [Cuenta.Oid] == ? && [TipoSaldoDia] == 1", item.Fecha, item.Cuenta.Oid));
                    if (sd == null)
                        sd = new SaldoDiario(uow, item.Periodo, item.Cuenta, item.Fecha, item.TotDebe, item.TotHaber, ETipoSaldoDia.Operaciones,
                            item.AjusteDebe, item.AjusteHaber);
                    else
                        sd.Update(item.TotDebe, item.TotHaber, item.AjusteDebe, item.AjusteHaber);
                    sd.Save();
                }
                uow.CommitChanges();
                var ctas = (from cta in (View.CurrentObject as Partida).Detalles
                            select cta.Cuenta).Distinct().ToList<SBT.Apps.Contabilidad.BusinessObjects.Catalogo>();
                // acumular en los niveles de resumen, hasta llegar al nivel 1, por eso la funcion AcumularSaldos es recursiva
                IList<SBT.Apps.Contabilidad.BusinessObjects.Catalogo> cctas = ctas;
                AcumularSaldosDiarios(ctas, ref cctas);
                AcumularSaldosMes(cctas);
            }
        }

        /// <summary>
        /// Acumular el saldo de las cuentas del detalle de la partida en sus cuentas padre de forma recursiva hasta llegar al nivel 1.
        /// El objetivo es realizar la mayorización cuando se guarda la partida y no tener que esperar hasta el cierre del dia
        /// </summary>
        /// <param name="cuentas">Lista de Cuentas Contables cuyos saldos del dia seran acumulados</param>
        /// <param name="ouctas">Cuentas padre del parametro cuentas que seran acumuladas en la siguiente iteraccion hasta llegar al nivel 1</param>
        private void AcumularSaldosDiarios(IList<SBT.Apps.Contabilidad.BusinessObjects.Catalogo> cuentas, ref IList<SBT.Apps.Contabilidad.BusinessObjects.Catalogo> ouctas)
        {
            if (cuentas == null && cuentas.Count == 0)
                return;
            var emp = (View.CurrentObject as Partida).Empresa;
            using (UnitOfWork uow = new UnitOfWork((ObjectSpace as XPObjectSpace).Session.DataLayer))
            {
                var q1 = uow.Query<SaldoDiario>();
                var sdos = (from sc in q1
                            where sc.Fecha.Date == (View.CurrentObject as Partida).Fecha.Date && cuentas.Any(x => x.Oid == sc.Cuenta.Oid)
                            group sc by new { sc.Periodo, sc.Cuenta.Padre, sc.Fecha } into z
                            select new
                            {
                                Periodo = z.Key.Periodo,
                                Fecha = z.Key.Fecha.Date,
                                Cuenta = z.Key.Padre,
                                Debe = z.Sum(x => x.Debe),
                                Haber = z.Sum(x => x.Haber),
                                DebeAjuste = z.Sum(x => x.DebeAjusteConsolida),
                                HaberAjuste = z.Sum(x => x.HaberAjusteConsolida)
                            });
                if (sdos.Count() > 0)
                {
                    foreach (var item in sdos)
                    {
                        var saldoCta = uow.FindObject<SaldoDiario>(CriteriaOperator.Parse("GetDate([Fecha]) == ?  && [Cuenta.Oid] == ? && [TipoSaldoDia] == 1",
                                       item.Fecha, item.Cuenta.Oid));
                        if (saldoCta == null)
                            saldoCta = new SaldoDiario(uow, item.Periodo, item.Cuenta, item.Fecha, item.Debe, item.Haber,
                                ETipoSaldoDia.Operaciones, item.DebeAjuste, item.HaberAjuste);
                        else
                            saldoCta.Update(item.Debe, item.Haber, item.DebeAjuste, item.HaberAjuste);
                        uow.Save(saldoCta);
                    }
                    uow.CommitChangesAsync();
                    var ctas = (from cta in sdos select cta.Cuenta).Distinct().ToList<SBT.Apps.Contabilidad.BusinessObjects.Catalogo>();
                    // para concatenar las cuentas de los diferentes niveles; que se usaran para actualizar o generar los saldos mensuales
                    var cta2 = (IList<SBT.Apps.Contabilidad.BusinessObjects.Catalogo>)ouctas.Concat(ctas);
                    // acumular en los niveles superiores
                    AcumularSaldosDiarios(ctas, ref cta2);
                }
            }
        }

        /// <summary>
        /// Acumular y actualizar los saldos mensuales para las cuentas involucradas en una partida contable y sus cuentas padres
        /// hasta el nivel 1.
        /// </summary>
        /// <param name="cuentas">Cuentas desde el nivel de detalle hasta el nivel 1 que fueron afectadas por la partida
        /// y quedeben actualizarse en los saldos mensuales</param>
        private void AcumularSaldosMes(IList<SBT.Apps.Contabilidad.BusinessObjects.Catalogo> cuentas)
        {
            var emp = (View.CurrentObject as Partida).Empresa;
            var per = (View.CurrentObject as Partida).Periodo;
            var fecha = (View.CurrentObject as Partida).Fecha;
            using (UnitOfWork uow = new UnitOfWork((ObjectSpace as XPObjectSpace).Session.DataLayer))
            {
                var q1 = uow.Query<SaldoMes>();
                var sdos = (from sc in q1
                            where sc.Periodo == per && sc.Mes == (View.CurrentObject as Partida).Fecha.Month && cuentas.Any(x => x.Oid == sc.Cuenta.Oid)
                            group sc by new { sc.Periodo, sc.Cuenta.Padre, sc.Mes } into z
                            select new
                            {
      
                                Periodo = z.Key.Periodo,
                                Mes = z.Key.Mes,
                                Cuenta = z.Key.Padre,
                                Debe = z.Sum(x => x.Debe),
                                Haber = z.Sum(x => x.Haber)
                            });
                if (sdos.Count() > 0)
                {
                    foreach (var item in sdos)
                    {
                        var saldoCtaMes = uow.FindObject<SaldoMes>(CriteriaOperator.Parse("[Periodo.Oid] == ? && [Mes] == ? && [Cuenta.Oid] == ?",
                                       per.Oid, item.Mes, item.Cuenta.Oid));
                        if (saldoCtaMes == null)
                            saldoCtaMes = new SaldoMes(uow, item.Periodo, item.Cuenta, fecha, item.Debe, item.Haber);
                        else
                            saldoCtaMes.Update(item.Debe, item.Haber);
                        uow.Save(saldoCtaMes);
                    }
                    uow.CommitChangesAsync();
                }
            }
        }

    }
}