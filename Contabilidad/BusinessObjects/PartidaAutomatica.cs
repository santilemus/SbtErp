using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Contabilidad.BusinessObjects;

namespace SBT.Apps.Contabilidad.Module.BusinessObjects
{
    public class PartidaAutomatica
    {
        UnitOfWork uow;
        IQueryable<SaldoMes> saldosMes;
        Empresa fEmpresa; 

        public PartidaAutomatica(UnitOfWork fUow, Empresa empresa)
        {
            uow = fUow;
            fEmpresa = empresa;
        }

        public bool EsValidoGenerarPartida(DateTime fecha, ETipoPartida tipo, out string msg)
        {
            var Periodo = uow.GetObjectByKey<Periodo>(fecha.Year);
            if (Periodo == null)
            {
                msg = "Periodo no existe";
                return false;
            }

            var ptda = uow.FindObject<Partida>(CriteriaOperator.Parse("[Empresa.Oid] == ? && [Periodo.Oid] == ? && [Tipo] == ?",
                ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid, fecha.Year, tipo));
            if (ptda != null)
            {
                msg = string.Format("Ya existe una partida de {0}. No puede generar otra", Enum.GetName(typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.ETipoPartida), tipo));
                return false;
            }
            msg = string.Empty;
            return true;
        }

        private decimal ObtenerSaldoCuenta(ETipoEmpresaCuenta tipo)
        {
            var cta = uow.FindObject<EmpresaCuenta>(CriteriaOperator.Parse("[Empresa.Oid] == ? && [TipoCuenta] == ?",
                fEmpresa.Oid, tipo));
            if (cta != null)
            {
                var fSaldo = saldosMes.Where(x => x.Cuenta == cta.Cuenta).FirstOrDefault();
                return (fSaldo != null) ? fSaldo.SaldoFin : 0.0m;
            }
            else 
                return 0.0m;
        }

        private PartidaDetalle CreateDetalle(SaldoMes item, string concepto)
        {
            PartidaDetalle partidaDetalle = new PartidaDetalle(uow);
            partidaDetalle.Cuenta = item.Cuenta;
            partidaDetalle.Concepto = concepto;
            if (item.Cuenta.TipoSaldoCta == Contabilidad.BusinessObjects.ETipoSaldoCuenta.Deudor)
                partidaDetalle.ValorHaber = item.SaldoFin;
            else
                partidaDetalle.ValorDebe = item.SaldoFin;
            return partidaDetalle;
        }

        public void PartidaLiquidacionOCierre(DateTime fecha, ETipoPartida tipo, string concepto, out string msg)
        {
            saldosMes = uow.Query<SaldoMes>().Where(x => x.Empresa.Oid == fEmpresa.Oid && x.Periodo.Oid == fecha.Year && x.Mes == x.Periodo.FechaFin.Month);
            if (saldosMes == null || saldosMes.Count() == 0)
            {
                msg = string.Format("No hay datos para generar la partida de {0}", Convert.ToString(tipo));
            }

            Partida ptda = new Partida(uow)
            {
                Fecha = fecha,
                Tipo = tipo,
                Concepto = concepto
            };

            foreach (var item in saldosMes)
            {
                ptda.Detalles.Add(CreateDetalle(item, concepto));
            }
            if (tipo == ETipoPartida.Liquidacion)
            {
                var fIngreso = saldosMes.Where(x => x.Cuenta.TipoCuenta == Contabilidad.BusinessObjects.ETipoCuentaCatalogo.Ingreso
                                && x.Cuenta.Nivel == 1).FirstOrDefault();
                var fCostoGasto = saldosMes.Where(x => (x.Cuenta.TipoCuenta == Contabilidad.BusinessObjects.ETipoCuentaCatalogo.Costo ||
                                x.Cuenta.TipoCuenta == Contabilidad.BusinessObjects.ETipoCuentaCatalogo.Gasto) && x.Cuenta.Nivel == 1).Sum(x => x.SaldoFin);
                var resultadoOperacion = fIngreso.SaldoFin - fCostoGasto;
                var empresaCuentas = uow.Query<EmpresaCuenta>().Where(x => x.Empresa.Oid == fEmpresa.Oid);
                decimal reserva = 0.0m;
                decimal renta = 0.0m;
                if (resultadoOperacion > 0)
                {
                    var reservaLegal = CalcularReservaLegal(empresaCuentas, resultadoOperacion);
                    reserva = reservaLegal.ValorHaber;
                    if (reservaLegal != null)
                        ptda.Detalles.Add(reservaLegal);
                    var rentaItem = CalcularImpuestoRenta(empresaCuentas, resultadoOperacion, reserva);
                    renta = rentaItem.ValorHaber;
                    if (rentaItem != null)
                        ptda.Detalles.Add(rentaItem);
                }
                var ctaLiquida = empresaCuentas.FirstOrDefault(x => x.TipoCuenta == ETipoEmpresaCuenta.Liquidacion);
                if (ctaLiquida != null)
                {
                    PartidaDetalle liquidaItem = new PartidaDetalle(uow);
                    liquidaItem.Cuenta = ctaLiquida.Cuenta;
                    liquidaItem.Concepto = ctaLiquida.Concepto;
                    if (resultadoOperacion > 0)
                        liquidaItem.ValorHaber = resultadoOperacion - reserva - renta;
                    else
                        liquidaItem.ValorDebe = resultadoOperacion;
                    ptda.Detalles.Add(liquidaItem);
                    // aplicar la ganancia o la perdida
                    PartidaDetalle item = new PartidaDetalle(uow);
                    EmpresaCuenta cta;
                    if (resultadoOperacion > 0)
                    {
                        cta = empresaCuentas.FirstOrDefault(x => x.TipoCuenta == ETipoEmpresaCuenta.UtilidadEjercicio);
                        item.ValorHaber = resultadoOperacion - reserva - renta;
                    }
                    else
                    {
                        cta = empresaCuentas.FirstOrDefault(x => x.TipoCuenta == ETipoEmpresaCuenta.PerdidaEjercicio);
                        item.ValorDebe = resultadoOperacion;
                    }
                    item.Cuenta = cta.Cuenta;
                    item.Concepto = cta.Concepto;
                    ptda.Detalles.Add(item);
                }
            }
            ptda.Save();
            uow.CommitChanges();
            msg = string.Empty;
        }

        private PartidaDetalle CalcularReservaLegal(IQueryable<EmpresaCuenta> empresaCuentas, decimal utilidadOperacion)
        {
            var reservaAnterior = ObtenerSaldoCuenta(ETipoEmpresaCuenta.ReservaLegalAnterior);
            var capitalSocial = ObtenerSaldoCuenta(ETipoEmpresaCuenta.CapitalSocial);
            var totReserva = Math.Round(capitalSocial * fEmpresa.ClaseSociedad.PorcentajeCapital, 2);
            if (reservaAnterior < totReserva)
            {
                var ctaReserva = empresaCuentas.FirstOrDefault(x => x.TipoCuenta == ETipoEmpresaCuenta.ReservaLegalEjercicio);
                if (ctaReserva == null)
                    return null;
                var reservaEjercicio = Math.Round(utilidadOperacion * fEmpresa.ClaseSociedad.PorcentajeAnual, 2);
                var detalle = new PartidaDetalle(uow);
                detalle.Cuenta = ctaReserva.Cuenta;
                detalle.Concepto = ctaReserva.Concepto;
                detalle.ValorHaber = (reservaAnterior + reservaEjercicio < totReserva) ? reservaEjercicio : totReserva - reservaAnterior;
                return detalle;
            }
            else
                return null;
        }

        private PartidaDetalle CalcularImpuestoRenta(IQueryable<EmpresaCuenta> empresaCuentas, decimal utilidadOperacion, decimal reservaLegal)
        {
            var ctaImpuesto = empresaCuentas.FirstOrDefault(x => x.TipoCuenta == ETipoEmpresaCuenta.RentaPagar);
            if (ctaImpuesto != null)
            {
                var detalle = new PartidaDetalle(uow);
                detalle.Cuenta = ctaImpuesto.Cuenta;
                detalle.Concepto = ctaImpuesto.Concepto;
                detalle.ValorHaber = Math.Round((utilidadOperacion - reservaLegal) * fEmpresa.ClaseSociedad.PorcentajeRenta, 2);
                return detalle;
            }
            else
                return null;
        }

    }
}
