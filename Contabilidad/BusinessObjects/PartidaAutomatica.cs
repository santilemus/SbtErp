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
    /// <summary>
    /// Implementar la generacion de partidas automaticas
    /// </summary>
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

        /// <summary>
        /// Validar que es permitido generar una partida automatica, cuando la partida no existe
        /// </summary>
        /// <param name="partida">del Tipo BO Partida y corresponde a los datos generales de la partida contable a generar</param>
        /// <param name="msg">Mensaje retornado por el metodo al realizar la validacion</param>
        /// <returns>Verdadero (true) cuando es permitido generar una partida, de otro modo retorna false</returns>
        public bool EsValidoGenerarPartida(Partida partida, out string msg)
        {
            if (partida.Periodo == null)
            {
                msg = "Periodo no existe";
                return false;
            }

            var ptda = uow.FindObject<Partida>(CriteriaOperator.Parse("[Empresa.Oid] == ? && [Periodo.Oid] == ? && [Tipo] == ?",
                ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid, partida.Fecha.Year, partida.Tipo));
            if (ptda != null && ptda.Detalles.Count > 0)
            {
                msg = string.Format("Ya existe una partida de {0}. No puede generar otra", Enum.GetName(typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.ETipoPartida), partida.Tipo));
                return false;
            }
            msg = string.Empty;
            return true;
        }


        private decimal ObtenerSaldoCuenta(ECuentaEspecial tipo)
        {
            var cta = uow.FindObject<Catalogo>(CriteriaOperator.Parse("[Empresa.Oid] == ? && [CuentaEspecial] == ?",
                fEmpresa.Oid, tipo));
            if (cta != null)
            {
                var fSaldo = saldosMes.Where(x => x.Cuenta == cta).FirstOrDefault();
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

        /// <summary>
        /// Metodo a invocar para generar la partida de liquidación o de cierre
        /// </summary>
        /// <param name="partida">BO del Tipo Partida y corresponde a los datos generales de la partida</param>
        /// <param name="msg">Mensaje retornado por el metodo</param>
        /// <remarks>
        /// Los datos generales de la partida (encabezado) deben existir y el detalle vacío para que el metodo se ejecute satisfactoriamente
        /// </remarks>
        public void PartidaLiquidacionOCierre(Partida partida, out string msg)
        {
            saldosMes = uow.Query<SaldoMes>().Where(x => x.Cuenta.Empresa.Oid == fEmpresa.Oid && x.Periodo.Oid == partida.Periodo.Oid && x.Mes == x.Periodo.FechaFin.Month);
            if (saldosMes == null || saldosMes.Count() == 0)
            {
                msg = string.Format("No hay datos para generar la partida de {0}", Convert.ToString(partida.Fecha));
                return;
            }

            foreach (var item in saldosMes)
            {
                partida.Detalles.Add(CreateDetalle(item, partida.Concepto));
            }
            /// cuando es partida de liquidacion se generan las entradas correspondiente al resultado del ejercicio y la liquidacion de 
            /// la perdidad o ganancia
            if (partida.Tipo == ETipoPartida.Liquidacion)
            {
                var fIngreso = saldosMes.Where(x => x.Cuenta.TipoCuenta == Contabilidad.BusinessObjects.ETipoCuentaCatalogo.Ingreso
                                && x.Cuenta.Nivel == 1).FirstOrDefault();
                var fCostoGasto = saldosMes.Where(x => (x.Cuenta.TipoCuenta == Contabilidad.BusinessObjects.ETipoCuentaCatalogo.Costo ||
                                x.Cuenta.TipoCuenta == Contabilidad.BusinessObjects.ETipoCuentaCatalogo.Gasto) && x.Cuenta.Nivel == 1).Sum(x => x.SaldoFin);
                var resultadoOperacion = fIngreso.SaldoFin - fCostoGasto;
                var ctasEspeciales = uow.Query<Catalogo>().Where(x => x.Empresa.Oid == fEmpresa.Oid && x.CuentaEspecial > 0 && x.Activa == true);
                decimal reserva = 0.0m;
                decimal renta = 0.0m;
                if (resultadoOperacion > 0)
                {
                    var reservaLegal = CalcularReservaLegal(ctasEspeciales, resultadoOperacion);
                    reserva = reservaLegal.ValorHaber;
                    if (reservaLegal != null)
                        partida.Detalles.Add(reservaLegal);
                    var rentaItem = CalcularImpuestoRenta(ctasEspeciales, resultadoOperacion, reserva);
                    renta = rentaItem.ValorHaber;
                    if (rentaItem != null)
                        partida.Detalles.Add(rentaItem);
                }
                var ctaLiquida = ctasEspeciales.FirstOrDefault(x => x.CuentaEspecial == ECuentaEspecial.Liquidacion);
                if (ctaLiquida != null)
                {
                    PartidaDetalle liquidaItem = new PartidaDetalle(uow);
                    liquidaItem.Cuenta = ctaLiquida;
                    liquidaItem.Concepto = ctaLiquida.Concepto;
                    if (resultadoOperacion > 0)
                        liquidaItem.ValorHaber = resultadoOperacion - reserva - renta;
                    else
                        liquidaItem.ValorDebe = resultadoOperacion;
                    partida.Detalles.Add(liquidaItem);
                    // aplicar la ganancia o la perdida
                    PartidaDetalle item = new PartidaDetalle(uow);
                    Catalogo cta;
                    if (resultadoOperacion > 0)
                    {
                        cta = ctasEspeciales.FirstOrDefault(x => x.CuentaEspecial == ECuentaEspecial.UtilidadEjercicio);
                        item.ValorHaber = resultadoOperacion - reserva - renta;
                    }
                    else
                    {
                        cta = ctasEspeciales.FirstOrDefault(x => x.CuentaEspecial == ECuentaEspecial.PerdidaEjercicio);
                        item.ValorDebe = resultadoOperacion;
                    }
                    item.Cuenta = cta;
                    item.Concepto = cta.Concepto;
                    partida.Detalles.Add(item);
                }
            }
            partida.Save();
            uow.CommitChanges();
            msg = string.Empty;
        }

        private PartidaDetalle CalcularReservaLegal(IQueryable<Catalogo> empresaCuentas, decimal utilidadOperacion)
        {
            var reservaAnterior = ObtenerSaldoCuenta(ECuentaEspecial.ReservaLegalAnterior);
            var capitalSocial = ObtenerSaldoCuenta(ECuentaEspecial.CapitalSocial);
            var totReserva = Math.Round(capitalSocial * fEmpresa.ClaseSociedad.PorcentajeCapital, 2);
            if (reservaAnterior < totReserva)
            {
                var ctaReserva = empresaCuentas.FirstOrDefault(x => x.CuentaEspecial == ECuentaEspecial.ReservaLegalEjercicio);
                if (ctaReserva == null)
                    return null;
                var reservaEjercicio = Math.Round(utilidadOperacion * fEmpresa.ClaseSociedad.PorcentajeAnual, 2);
                var detalle = new PartidaDetalle(uow);
                detalle.Cuenta = ctaReserva;
                detalle.Concepto = ctaReserva.Concepto;
                detalle.ValorHaber = (reservaAnterior + reservaEjercicio < totReserva) ? reservaEjercicio : totReserva - reservaAnterior;
                return detalle;
            }
            else
                return null;
        }

        private PartidaDetalle CalcularImpuestoRenta(IQueryable<Catalogo> empresaCuentas, decimal utilidadOperacion, decimal reservaLegal)
        {
            var ctaImpuesto = empresaCuentas.FirstOrDefault(x => x.CuentaEspecial == ECuentaEspecial.RentaPagar);
            if (ctaImpuesto != null)
            {
                var detalle = new PartidaDetalle(uow);
                detalle.Cuenta = ctaImpuesto;
                detalle.Concepto = ctaImpuesto.Concepto;
                detalle.ValorHaber = Math.Round((utilidadOperacion - reservaLegal) * fEmpresa.ClaseSociedad.PorcentajeRenta, 2);
                return detalle;
            }
            else
                return null;
        }

        /// <summary>
        /// Generar la partida de apertura del periodo actual a partir de la partida de cierre del periodo anterior
        /// </summary>
        /// <param name="partida">instancia del BO que se esta creando y que corresponde con la partida de apertura</param>
        /// <param name="msg">Mensaje retornado por el metodo</param>
        public void PartidaApertura(Partida partida, out string msg)
        {
            if (uow.GetObjectByKey<Periodo>(partida.Periodo.Oid - 1) == null)
            {
                msg = $"Periodo {partida.Periodo.Oid - 1} no existe";
                return;
            }
            Partida partidaCierre = uow.FindObject<Partida>(CriteriaOperator.Parse("[Empresa.Oid] == ? && [Periodo.Oid] == ? && [Tipo] == ?",
                                        partida.Empresa.Oid, partida.Periodo.Oid - 1, ETipoPartida.Cierre));
            if (partidaCierre == null)
            {
                msg = $"Partida {Convert.ToString(ETipoPartida.Cierre)} del Periodo {partida.Periodo.Oid - 1} no existe para generar partida de Apertura";
                return;
            }
            foreach (var item in partidaCierre.Detalles)
            {
                PartidaDetalle partidaDetalle = new PartidaDetalle(partida.Session);
                partidaDetalle.Cuenta = item.Cuenta;
                partidaDetalle.Concepto = partida.Concepto;
                if (item.Cuenta.TipoSaldoCta == Contabilidad.BusinessObjects.ETipoSaldoCuenta.Deudor)
                    partidaDetalle.ValorDebe = item.ValorHaber;
                else
                    partidaDetalle.ValorHaber = item.ValorDebe;
                partida.Detalles.Add(partidaDetalle);
            }
            partida.Save();
            msg = string.Empty;
        }

    }
}
