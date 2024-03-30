using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Base.Module.Controllers;
using SBT.Apps.Contabilidad.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp.Validation;
using System.Collections;
using DevExpress.ExpressApp.Security;
using Microsoft.Extensions.DependencyInjection;

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
        private int fEmpresaOid;
        private string nombreUsuario;

        private int EmpresaOid
        {
            get
            {
                if (fEmpresaOid <= 0)
                {
                    if (SecuritySystem.CurrentUser == null)
                        fEmpresaOid = ObjectSpace.GetObjectByKey<Usuario>(ObjectSpace.ServiceProvider.GetRequiredService<ISecurityStrategyBase>().User).Empresa.Oid;
                    else
                        fEmpresaOid = ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid;
                }
                return fEmpresaOid;
            }
        }

        public vcPartida() : base()
        {
            TargetViewNesting = Nesting.Root;
            // vcPartida
            // 
            TargetObjectType = typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.Partida);
            //
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            if (string.IsNullOrEmpty(SecuritySystem.CurrentUserName))
                nombreUsuario = ObjectSpace.ServiceProvider.GetRequiredService<ISecurityStrategyBase>().UserName;
            else
                nombreUsuario = SecuritySystem.CurrentUserName;
            // Perform various tasks depending on the target View.
            pwaCierreDiario.Executing += PwaAbrirCierre_Executing;
            pwaCierreDiario.Execute += PwaCierreDiario_Execute;
            pwaCierreMes.Executing += PwaCierreMes_Executing;
            pwaCierreMes.Execute += PwaCierreMes_Execute;
            pwaAbrirDias.Executing += PwaAbrirCierre_Executing;
            pwaAbrirDias.Execute += PwaAbrirDias_Execute;
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
            pwaCierreDiario = new PopupWindowShowAction(this, "Partida_pwaCierreDiario", DevExpress.Persistent.Base.PredefinedCategory.RecordsNavigation)
            {
                AcceptButtonCaption = "Aceptar",
                ActionMeaning = DevExpress.ExpressApp.Actions.ActionMeaning.Accept,
                CancelButtonCaption = "Cerrar",
                Caption = "Cierre Diario",
                ImageName = "CierreDiario",
                TargetObjectType = typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.Partida),
                TargetViewType = DevExpress.ExpressApp.ViewType.ListView,
                ToolTip = "Realizar el cierre diario contable",
                TypeOfView = typeof(DevExpress.ExpressApp.ListView)
            };
            pwaCierreDiario.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.PwaCierreDiario_CustomizePopupWindowParams);
            // PwaAbrirDias
            pwaAbrirDias = new PopupWindowShowAction(this, "Partida_pwaAbrirDias", DevExpress.Persistent.Base.PredefinedCategory.RecordsNavigation)
            {
                AcceptButtonCaption = "Aceptar",
                ActionMeaning = DevExpress.ExpressApp.Actions.ActionMeaning.Accept,
                CancelButtonCaption = "Cerrar",
                Caption = "Abrir Días",
                ImageName = "abrircaja",
                TargetObjectType = typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.Partida),
                TargetViewType = DevExpress.ExpressApp.ViewType.ListView,
                ToolTip = "Abrir rango de dias que ya se encuentran cerrados",
                TypeOfView = typeof(DevExpress.ExpressApp.ListView)
            };
            pwaAbrirDias.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.PwaAbrirDias_CustomizePopupWindowParams);
            // PwaCierreMes
            pwaCierreMes = new PopupWindowShowAction(this, "Partida_pwaCierreMes", DevExpress.Persistent.Base.PredefinedCategory.RecordsNavigation)
            {
                AcceptButtonCaption = "Aceptar",
                ActionMeaning = DevExpress.ExpressApp.Actions.ActionMeaning.Accept,
                CancelButtonCaption = "Cerrar",
                Caption = "Cierre de Mes",
                ImageName = "service",
                TargetObjectType = typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.Partida),
                TargetViewType = DevExpress.ExpressApp.ViewType.ListView,
                ToolTip = "Cierre contable de un Mes. Este proceso no puede revertirse",
                TypeOfView = typeof(DevExpress.ExpressApp.ListView)
            };
            pwaCierreMes.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.PwaCierreMes_CustomizePopupWindowParams);
            // saPartidaApertura
            saApertura = new(this, "saPartidaApertura", DevExpress.Persistent.Base.PredefinedCategory.ObjectsCreation)
            {
                Caption = "Partida de Apertura",
                TargetObjectType = typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.Partida),
                TargetViewType = ViewType.DetailView,
                ToolTip = "Clic para generar la partida de apertura del período a partir de la partida de cierre del período anterior",
                TargetObjectsCriteria = "[Tipo] ==  0 And [Detalles][].Count() == 0",
                TargetObjectsCriteriaMode = TargetObjectsCriteriaMode.TrueForAll,
                ConfirmationMessage = "Desea generar Partida de Apertura?",
                ImageName = "book"
            };
            // saPartidaLiquidacion
            saLiquidacionCierre = new(this, "saPartidaLiquidacion", DevExpress.Persistent.Base.PredefinedCategory.ObjectsCreation)
            {
                Caption = "Liquidación y Cierre",
                TargetObjectType = typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.Partida),
                TargetViewType = ViewType.DetailView,
                ToolTip = "Clic para generar la partida de Liquidación o de cierre del período",
                TargetObjectsCriteria = "([Tipo] == 4 && [Detalles][].Count() == 0) || ([Tipo] == 5 && [Detalles][].Count() == 0)",
                TargetObjectsCriteriaMode = TargetObjectsCriteriaMode.TrueForAll,
                ConfirmationMessage = "Confirmar que desea generar la Partida?",
                ImageName = "recibo"
            };
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
            if (ptda.Detalles.Count > 0)
                return;
            var le = ((DetailView)View).FindItem("Detalles");
            var lv = ((ListPropertyEditor)le).ListView;
            string sMsg;
            if (le != null && lv != null)
            {
                if (ptda.Tipo == ETipoPartida.Apertura)
                {
                    if (string.IsNullOrEmpty(ptda.Concepto))
                    {
                        ptda.Concepto = $"Partida de Apertura del Ejercicio {ptda.Fecha.Year}";
                    }
                    PartidaAutomatica partidaAutomatica = new(ObjectSpace, ptda, lv);
                    partidaAutomatica.PartidaApertura(out sMsg);
                    if (!string.IsNullOrEmpty(sMsg))
                        Application.ShowViewStrategy.ShowMessage(sMsg, InformationType.Error);
                    lv.Refresh();
                    ptda.UpdateTotDebe(true);
                    ptda.UpdateTotHaber(true);
                }
            }
            else
            {
                MostrarInformacion("La partida no se va a generar porque no se encontró el ViewItem Detalles o el ListView correspondiente");
                return;
            }
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
            var ptda = ObjectSpace.FindObject<Partida>(CriteriaOperator.Parse("[Empresa.Oid] == ? && [Periodo.Oid] == ? && [Tipo] == ?",
                (View.CurrentObject as Partida).Empresa.Oid, (View.CurrentObject as Partida).Periodo.Oid, (View.CurrentObject as Partida).Tipo));
            if (ptda != null && (ptda != View.CurrentObject || !ObjectSpace.IsNewObject(View.CurrentObject)))
            {
                e.Cancel = true;
                MostrarError($"Ya existe una partida de {Convert.ToString((View.CurrentObject as Partida).Tipo)}. Solo puede existir una por período");
            }
        }

        private void saPartidaLiquidacionCierre_Execute(Object sender, SimpleActionExecuteEventArgs e)
        {
            Partida ptda = (e.CurrentObject as Partida);
            if (ptda.Detalles.Count != 0)
            {
                MostrarInformacion("La partida no se va a generar, porque ya existe uno o más detalles");
                return;
            }
            var le = ((DetailView)View).FindItem("Detalles");
            var lv = ((ListPropertyEditor)le).ListView;
            if (le != null && lv != null)
            {
                string sMsg = string.Empty;
                if (ptda.Tipo == ETipoPartida.Liquidacion)
                {
                    PartidaAutomatica partidaAutomatica = new (ObjectSpace, ptda, lv);
                    partidaAutomatica.PartidaLiquidacion(out sMsg);
                }
                else if (ptda.Tipo == ETipoPartida.Cierre)
                {
                    PartidaAutomatica partidaAutomatica = new (ObjectSpace, ptda, lv);
                    partidaAutomatica.PartidaDeCierre(out sMsg);
                }
                if (!string.IsNullOrEmpty(sMsg))
                    Application.ShowViewStrategy.ShowMessage(sMsg, InformationType.Error);
                lv.Refresh();
                ptda.UpdateTotDebe(true);
                ptda.UpdateTotHaber(true);
            }
            else
            {
                MostrarInformacion("La partida no se va a generar porque no se encontró el ViewItem Detalles o el ListView correspondiente");
                return;
            }
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
            var fechaDesde = ((CierreDiarioParam)e.PopupWindowViewCurrentObject).FechaDesde;
            var fechaHasta = ((CierreDiarioParam)e.PopupWindowViewCurrentObject).FechaHasta;
            DoBeforeExecute(ref e, EmpresaOid, fechaDesde, fechaHasta);
            if (EmpresaOid <= 0)
            {
                ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += $"No tiene valor la variable de la empresa de la sesion{Environment.NewLine}";
                ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += $"Hora Finalizó: {DateTime.Now:G} {Environment.NewLine}";
                Application.ShowViewStrategy.ShowMessage(@"Debe seleccionar una empresa para su sesión, cuando ingreso no seleccciono  una", InformationType.Error);
                return;
            }
            ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += $"{Environment.NewLine}Ejecutando el Proceso{Environment.NewLine}";
            var uow = new UnitOfWork((View.ObjectSpace as XPObjectSpace).Session.DataLayer);
            uow.BeginTransaction();
            uow.ExecuteNonQuery("exec spConCierreDiario @Empresa, @FechaDesde, @FechaHasta, @Usuario",
                new string[] { "@Empresa", "@FechaDesde", "@FechaHasta", "@Usuario" },
                new object[] { EmpresaOid, fechaDesde, fechaHasta, nombreUsuario });
            //uow.CommitChanges();
            ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += $"Hora Finalizó: {DateTime.Now:G}";
            //var OS = Application.CreateObjectSpace(typeof(AuditoriaProceso));
            AuditoriaProceso audit = new (uow);
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
            var fechaDesde = ((CierreDiarioParam)e.PopupWindowViewCurrentObject).FechaDesde;
            var fechaHasta = ((CierreDiarioParam)e.PopupWindowViewCurrentObject).FechaHasta;
            DoBeforeExecute(ref e, EmpresaOid, fechaDesde, fechaHasta);
            if (EmpresaOid <= 0)
            {
                ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += $"No tiene valor la variable de la empresa de la sesion{Environment.NewLine}";
                ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += $"Hora Finalizó: {DateTime.Now:G}{Environment.NewLine}";
                Application.ShowViewStrategy.ShowMessage(@"Debe seleccionar una empresa para su sesión, cuando ingreso no seleccciono  una", InformationType.Error);
                return;
            }
            ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += $"{Environment.NewLine}Ejecutando el Proceso{Environment.NewLine}";
            IObjectSpace ospace = Application.ObjectSpaceProvider.CreateObjectSpace();
            ((XPObjectSpace)ospace).Session.ExecuteNonQuery("exec spConAbrirDias @Empresa, @FechaDesde, @FechaHasta, @Usuario",
                new string[] { "@Empresa", "@FechaDesde", "@FechaHasta", "@Usuario" }, new object[] { EmpresaOid, fechaDesde, fechaHasta, nombreUsuario });
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
            if (!((CierreDiarioParam)vParam.CurrentObject).ValidarPeriodo)
            {
                Application.ShowViewStrategy.ShowMessage(@"El proceso no se ejecutará porque el Período no es válido", InformationType.Error);
                e.Cancel = true;
            }
        }

        private void PwaCierreMes_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace objectSpace = (NonPersistentObjectSpace)Application.ObjectSpaceProviders[1].CreateObjectSpace(); //Application.CreateObjectSpace(typeof(CierreDiarioParams));
            var pa = objectSpace.CreateObject<CierreMesParam>();
            e.View = Application.CreateDetailView(objectSpace, pa);
            e.Size = new System.Drawing.Size(500, 400);
            e.IsSizeable = false;
            vParam = e.View;
        }

        private void PwaCierreMes_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var pa = (CierreMesParam)vParam.CurrentObject;
            DateTime finMes = new DateTime(pa.FechaCierre.Year, pa.FechaCierre.Month, 1).AddMonths(1).AddDays(-1);
            int x = Convert.ToInt32(ObjectSpace.Evaluate(typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.Partida), "count(*)",
                         CriteriaOperator.Parse("Empresa.Oid == ? && Periodo.Oid == ? && Fecha <= ? && Mayorizada == false",
                         EmpresaOid, pa.FechaCierre.Year, finMes)));
            if (x >= 0)
            {
                MostrarError($@"El cierre de mes no se puede realizar, porque hay días previos al {string.Format("{0:dd/MM/yyyy}", finMes)} pendientes de cerrar");
                e.Cancel = true;
            }
        }

        private void PwaCierreMes_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            var fechaCierre = ((CierreMesParam)e.PopupWindowViewCurrentObject).FechaCierre;
            ((CierreMesParam)e.PopupWindowViewCurrentObject).Bitacora = $"{vParam.Caption}{Environment.NewLine}";
            ((CierreMesParam)e.PopupWindowViewCurrentObject).Bitacora += $"Hora Inicio: {DateTime.Now:G} {Environment.NewLine}";
            ((CierreMesParam)e.PopupWindowViewCurrentObject).Bitacora += $"{Environment.NewLine}P a r a m e t r o s{Environment.NewLine}";
            ((CierreMesParam)e.PopupWindowViewCurrentObject).Bitacora += $"Empresa ==> {EmpresaOid} {Environment.NewLine}";
            ((CierreMesParam)e.PopupWindowViewCurrentObject).Bitacora += $"Fecha Cierre ==> {fechaCierre:G} {Environment.NewLine}";
            if (EmpresaOid <= 0)
            {
                ((CierreMesParam)e.PopupWindowViewCurrentObject).Bitacora += $"No tiene valor la variable de la empresa de la sesion{Environment.NewLine}";
                ((CierreMesParam)e.PopupWindowViewCurrentObject).Bitacora += $"Hora Finalizó: {DateTime.Now:G} {Environment.NewLine}";
                MostrarError("No tiene valor la variable de la empresa de la sesion");
            }
            ((CierreMesParam)e.PopupWindowViewCurrentObject).Bitacora += $"{Environment.NewLine}Ejecutando el Proceso{Environment.NewLine}";
            UnitOfWork uow = new((View.ObjectSpace as XPObjectSpace).Session.DataLayer);
            // ***** OJO, REVISAR ESTA PARTE NO ES ESTO ****
            uow.ExecuteNonQuery("exec spConAbrirDias @Empresa, @FechaDesde, @FechaHasta, @Usuario",
                new string[] { "@Empresa", "@FechaDesde", "@FechaHasta", "@Usuario" }, new object[] { EmpresaOid, fechaCierre });
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
            /*
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
            */ 
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
            using (UnitOfWork uow = new((ObjectSpace as XPObjectSpace).Session.DataLayer))
            {
                var q1 = uow.Query<SaldoDiario>();
                var sdos = (from sc in q1
                            where sc.Fecha.Date == (View.CurrentObject as Partida).Fecha.Date && cuentas.Any(x => x.Oid == sc.Cuenta.Oid)
                            group sc by new { sc.Periodo, sc.Cuenta.Padre, sc.Fecha } into z
                            select new
                            {
                                z.Key.Periodo,
                                Fecha = z.Key.Fecha.Date,
                                Cuenta = z.Key.Padre,
                                Debe = z.Sum(x => x.Debe),
                                Haber = z.Sum(x => x.Haber),
                                DebeAjuste = z.Sum(x => x.DebeAjusteConsolida),
                                HaberAjuste = z.Sum(x => x.HaberAjusteConsolida)
                            });
                if (sdos.Any())
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
            using (UnitOfWork uow = new((ObjectSpace as XPObjectSpace).Session.DataLayer))
            {
                var q1 = uow.Query<SaldoMes>();
                var sdos = (from sc in q1
                            where sc.Periodo == per && sc.Mes == (View.CurrentObject as Partida).Fecha.Month && cuentas.Any(x => x.Oid == sc.Cuenta.Oid)
                            group sc by new { sc.Periodo, sc.Cuenta.Padre, sc.Mes } into z
                            select new
                            {

                                z.Key.Periodo,
                                z.Key.Mes,
                                Cuenta = z.Key.Padre,
                                Debe = z.Sum(x => x.Debe),
                                Haber = z.Sum(x => x.Haber)
                            });
                if (sdos.Any())
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