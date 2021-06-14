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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;



namespace SBT.Apps.Contabilidad.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class vcPartida : ViewControllerBase
    {
        DevExpress.ExpressApp.View vParam;
        public vcPartida()
        {
            InitializeComponent();
            TargetViewNesting = Nesting.Root;
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            PwaCierreDiario.Executing += Pwa_Executing;
            PwaAbrirDias.Executing += Pwa_Executing;
            PwaCierreDiario.Executing += PwaCierreMes_Executing;
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
            PwaCierreDiario.Executing -= Pwa_Executing;
            PwaAbrirDias.Executing -= Pwa_Executing;
            PwaCierreMes.Executing -= PwaCierreMes_Executing;
            ObjectSpace.Committed -= ObjectSpace_Commited;
            base.OnDeactivated();
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
            IObjectSpace ospace = Application.ObjectSpaceProvider.CreateObjectSpace();
            ((XPObjectSpace)ospace).Session.ExecuteNonQuery("exec spConCierreDiario @Empresa, @FechaDesde, @FechaHasta, @Usuario",
                new string[] { "@Empresa", "@FechaDesde", "@FechaHasta", "@Usuario" }, new object[] { EmpresaOid, fechaDesde, fechaHasta, sUsuario });
            ((CierreDiarioParam)e.PopupWindowViewCurrentObject).Bitacora += $"Hora Finalizó: {DateTime.Now:G}";
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
            IObjectSpace ospace = Application.ObjectSpaceProvider.CreateObjectSpace();
            // esta parte revisarla, porque no es esto
            ((XPObjectSpace)ospace).Session.ExecuteNonQuery("exec spConAbrirDias @Empresa, @FechaDesde, @FechaHasta, @Usuario",
                new string[] { "@Empresa", "@FechaDesde", "@FechaHasta", "@Usuario" }, new object[] { empresaOid, fechaCierre });
            // --
            ((CierreMesParam)e.PopupWindowViewCurrentObject).Bitacora += $"Hora Finalizó: {DateTime.Now:G}";
            var obj = ospace.CreateObject<AuditoriaProceso>();
            obj.AuditarProceso(PwaAbrirDias.Caption, "", ((CierreMesParam)e.PopupWindowViewCurrentObject).Bitacora);
            (View.ObjectSpace as XPObjectSpace).Refresh();
            e.CanCloseWindow = false;
            ospace.Dispose();
            View.ObjectSpace.Refresh();
        }

        /// <summary>
        /// Actualizar el saldo de las cuentas de control. Pendiente de actualizar las cuentas padre
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
                        sd = new SaldoDiario(uow, item.Empresa, item.Periodo, item.Cuenta, item.Fecha, item.TotDebe, item.TotHaber, ETipoSaldoDia.Operaciones, 
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

        private void AcumularSaldosDiarios(IList<SBT.Apps.Contabilidad.BusinessObjects.Catalogo> cuentas, ref IList<SBT.Apps.Contabilidad.BusinessObjects.Catalogo> ouctas)
        {
            if (cuentas == null && cuentas.Count == 0)
                return;
            var emp = (View.CurrentObject as Partida).Empresa;
            using (UnitOfWork uow = new UnitOfWork((ObjectSpace as XPObjectSpace).Session.DataLayer))
            {
                var q1 = uow.Query<SaldoDiario>();
                var sdos = (from sc in q1
                            where sc.Empresa == emp && sc.Fecha.Date == (View.CurrentObject as Partida).Fecha.Date && cuentas.Any(x => x.Oid == sc.Cuenta.Oid)
                            group sc by new { sc.Empresa, sc.Periodo, sc.Cuenta.Padre, sc.Fecha } into z
                            select new
                            {
                                Empresa = z.Key.Empresa,
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
                            saldoCta = new SaldoDiario(uow, item.Empresa, item.Periodo, item.Cuenta, item.Fecha, item.Debe, item.Haber,
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

        private void AcumularSaldosMes(IList<SBT.Apps.Contabilidad.BusinessObjects.Catalogo> cuentas)
        {
            var emp = (View.CurrentObject as Partida).Empresa;
            var per = (View.CurrentObject as Partida).Periodo;
            var fecha = (View.CurrentObject as Partida).Fecha;
            using (UnitOfWork uow = new UnitOfWork((ObjectSpace as XPObjectSpace).Session.DataLayer))
            {
                var q1 = uow.Query<SaldoMes>();
                var sdos = (from sc in q1
                            where sc.Empresa == emp && sc.Periodo == per && sc.Mes == (View.CurrentObject as Partida).Fecha.Month && cuentas.Any(x => x.Oid == sc.Cuenta.Oid)
                            group sc by new { sc.Empresa, sc.Periodo, sc.Cuenta.Padre, sc.Mes } into z
                            select new
                            {
                                Empresa = z.Key.Empresa,
                                Periodo = z.Key.Periodo,
                                Mes     = z.Key.Mes,
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
                            saldoCtaMes = new SaldoMes(uow, item.Empresa, item.Periodo, item.Cuenta, fecha, item.Debe, item.Haber);
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