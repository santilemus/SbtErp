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
    public class SaldoLocal
    {
        public Catalogo Cuenta { get; set; }
        public ETipoSaldoCuenta TipoSaldoCta { get; set; }
        public ETipoCuentaCatalogo TipoCuenta { get; set; }
        public decimal SaldoFin { get; set; }
        public ECuentaEspecial CuentaEspecial { get; set; }
        public bool Activa { get; set; }
    }

    /// <summary>
    /// Implementar la generacion de partidas automaticas
    /// </summary>
    public class PartidaAutomatica
    {
        IObjectSpace fObjectSpace;
        IQueryable<SaldoMes> querySaldos;
        Partida fPartida;
        ListView fListaDetalle;

        public PartidaAutomatica(IObjectSpace objectSpace, Partida partida, ListView listaDetalle)
        {
            fObjectSpace = objectSpace;
            fPartida = partida;
            fListaDetalle = listaDetalle;
        }

        /// <summary>
        /// Validar que es permitido generar una partida automatica, cuando la partida no existe
        /// </summary>
        /// <param name="msg">Mensaje retornado por el metodo al realizar la validacion</param>
        /// <returns>Verdadero (true) cuando es permitido generar una partida, de otro modo retorna false</returns>
        /// <remarks>revisar porque estas validaciaones deberian realizarse en el viewcontroller</remarks>
        public bool EsValidoGenerarPartida(out string msg)
        {
            if (fPartida.Periodo == null)
            {
                msg = "Periodo no existe";
                return false;
            }

            var ptda = fObjectSpace.FindObject<Partida>(CriteriaOperator.Parse("[Empresa.Oid] == ? && [Periodo.Oid] == ? && [Tipo] == ?",
                ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid, fPartida.Fecha.Year, fPartida.Tipo));
            if (ptda != null && ptda.Detalles.Count > 0)
            {
                msg = string.Format("Ya existe una partida de {0}. No puede generar otra", Enum.GetName(typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.ETipoPartida), fPartida.Tipo));
                return false;
            }
            msg = string.Empty;
            return true;
        }


        private decimal ObtenerSaldoCuenta(ECuentaEspecial tipo)
        {
            IQueryable<SaldoMes> qrySaldoMes = fObjectSpace.GetObjectsQuery<SaldoMes>();
            SaldoMes saldoMes = qrySaldoMes.Where(y => y.Cuenta.Empresa.Oid == fPartida.Empresa.Oid && y.Cuenta.CuentaEspecial == tipo && y.Cuenta.Activa == true).FirstOrDefault();
            return (saldoMes != null) ? saldoMes.SaldoFin : 0.0m;
        }

        private void CreateDetalle(Catalogo cta, string concepto, decimal debe, decimal haber)
        {
            var detalle = fObjectSpace.CreateObject<PartidaDetalle>();
            //detalle.Partida = fPartida;
            detalle.Cuenta = cta;
            detalle.Concepto = concepto.IndexOf('{') > 0 && (concepto.IndexOf('}') > concepto.IndexOf("{")) ? string.Format(concepto, fPartida.Fecha.Year) : concepto;
            detalle.ValorDebe = debe;
            detalle.ValorHaber = haber;
            fListaDetalle.CollectionSource.Add(detalle);
            //fPartida.Detalles.Add(detalle);
        }

        /// <summary>
        /// Metodo a invocar para generar la partida de liquidación o de cierre
        /// </summary>
        /// <param name="partida">BO del Tipo Partida y corresponde a los datos generales de la partida</param>
        /// <param name="msg">Mensaje retornado por el metodo</param>
        /// <remarks>
        /// Los datos generales de la partida (encabezado) deben existir y el detalle vacío para que el metodo se ejecute satisfactoriamente
        /// Cuando hay utilidad la cuenta de Perdidas y Ganancias se va a cargar y cuando hay perdidas se abonara
        /// porque cuando hay utilidad se abona (haber) la cuenta de capital correspondiente (tipo saldo acreedor aumenta susaldo con el abono) y cuando hay 
        /// perdida se carga (debe) la cuenta de capital que corresponde (tipo saldo acreedor disminuye su saldo con un cargo)
        /// </remarks>
        public void PartidaLiquidacion(out string msg)
        {
            IQueryable<SaldoLocal> saldos = ObtenerSaldos(new ETipoCuentaCatalogo[] { ETipoCuentaCatalogo.Costo, ETipoCuentaCatalogo.Gasto, ETipoCuentaCatalogo.Ingreso });
            if (saldos == null || saldos.Count() == 0)
            {
                msg = string.Format("No hay datos para generar la partida de {0}", Convert.ToString(fPartida.Fecha));
                return;
            }
            foreach (var item in saldos)
                CreateDetalle(item.Cuenta, fPartida.Concepto, (item.TipoSaldoCta == ETipoSaldoCuenta.Acreedor) ? item.SaldoFin : 0.0m, /* debe */
                                                               (item.TipoSaldoCta == ETipoSaldoCuenta.Deudor) ? item.SaldoFin : 0.0m   /* haber */);
            var fIngreso = fObjectSpace.Evaluate(typeof(SaldoMes), CriteriaOperator.Parse("Sum([SaldoFin])"),
                           CriteriaOperator.Parse("[Cuenta.Empresa.Oid] == ? && [Periodo.Oid] == ? && [Mes] == ? &&[Cuenta.TipoCuenta] == ? && IsNull([Cuenta.Padre])",
                           fPartida.Empresa.Oid, fPartida.Fecha.Year, fPartida.Fecha.Month, ETipoCuentaCatalogo.Ingreso));
            var fCostoGasto = fObjectSpace.Evaluate(typeof(SaldoMes), CriteriaOperator.Parse("Sum([SaldoFin])"),
                           CriteriaOperator.Parse("[Cuenta.Empresa.Oid] == ? && [Periodo.Oid] == ? && [Mes] == ? && ([Cuenta.TipoCuenta] == ?  || [Cuenta.TipoCuenta] == ?) && IsNull([Cuenta.Padre])",
                           fPartida.Empresa.Oid, fPartida.Fecha.Year, fPartida.Fecha.Month, ETipoCuentaCatalogo.Costo, ETipoCuentaCatalogo.Gasto));
            var resultadoBruto = Convert.ToDecimal(fIngreso) - Convert.ToDecimal(fCostoGasto);
            decimal reserva = 0.0m;
            decimal renta = 0.0m;
            //decimal impuestoNoDeducible = ImpuestoGastoFinancieroNoDeducible();
            if (resultadoBruto > 0)
            {
                reserva = CalcularReservaLegal(resultadoBruto);
                renta = CalcularImpuestoRenta(resultadoBruto, reserva); //, impuestoNoDeducible);
            }
            var ctaLiquida = ObtenerCuentaEspecial(ECuentaEspecial.Liquidacion); // perdidas y ganancias 
            if (ctaLiquida != null)
            {
                decimal resultadoNeto = resultadoBruto - reserva - renta; // - impuestoNoDeducible;
                // se lleva el resultado a perdidas y ganancias
                CreateDetalle(ctaLiquida, ctaLiquida.Concepto, (resultadoNeto > 0) ? resultadoNeto : 0.0m, (resultadoNeto < 0) ? 0.0m : resultadoNeto);
                ECuentaEspecial ctaUtilidadPerdida = (resultadoBruto > 0) ? ECuentaEspecial.UtilidadEjercicio : ECuentaEspecial.PerdidaEjercicio;
                var cta = ObtenerCuentaEspecial((resultadoBruto > 0) ? ECuentaEspecial.UtilidadEjercicio : ECuentaEspecial.PerdidaEjercicio);
                if (cta != null)
                {
                    if (resultadoNeto > 0)           // utilidad se lleva a cuenta de patrimonio que corresponde a utilidades del periodo
                        CreateDetalle(cta, cta.Concepto, 0.0m, resultadoNeto);
                    else if (resultadoBruto < 0)     // perdida se lleva a cuenta de patrimonio que corresponde a perdidas del periodo
                        CreateDetalle(cta, cta.Concepto, resultadoNeto, 0.0m);
                }
            }           
            fPartida.Save();
            msg = string.Empty;
        }

        /// <summary>
        /// Calcular la reserva legal y generar el asiento correspondiente en la partida de liquidacion -- NOTA: Revisar con Lorena
        /// </summary>
        /// <param name="itemReservaAnterior"></param>
        /// <param name="itemCapital"></param>
        /// <param name="utilidadOperacion"></param>
        private decimal CalcularReservaLegal(decimal utilidadOperacion)
        {
            decimal reservaAplicar = 0.0m;
            Catalogo ctaReserva = ObtenerCuentaEspecial(ECuentaEspecial.ReservaLegalEjercicio);
            // la cuenta de capital en este caso es la de resumen, por esa razon no se usa la funcion ObtenerCuentaEspecial porque trae cuentas de detalle
            // y revisar con Lorena si pueden ser mas de 1 cuenta
            Catalogo ctaCapital = fObjectSpace.FindObject<Catalogo>(CriteriaOperator.Parse("[Empresa.Oid] == ? && [CuentaEspecial] == ? && [Activa] == True",
                                                     fPartida.Empresa.Oid, ECuentaEspecial.CapitalSocial));  // revisar con Lorena si puede ser mas de una
            if (ctaReserva != null && ctaCapital != null)
            {
                decimal fReservaAnterior = 0.0m;
                decimal fReserva = 0.0m;
                if (ctaReserva != null)
                    fReserva = Convert.ToDecimal(fObjectSpace.Evaluate(typeof(SaldoMes), CriteriaOperator.Parse("Sum([SaldoFin])"),
                                   CriteriaOperator.Parse("[Cuenta.Oid] == ? && [Periodo.Oid] == ? && [Mes] == ?", ctaReserva.Oid, fPartida.Fecha.Year, fPartida.Fecha.Month)));
                decimal fCapital = Convert.ToDecimal(fObjectSpace.Evaluate(typeof(SaldoMes), CriteriaOperator.Parse("Sum([SaldoFin])"),
                               CriteriaOperator.Parse("[Cuenta.Oid] == ? && [Periodo.Oid] == ? && [Mes] == ?", ctaCapital.Oid, fPartida.Fecha.Year, fPartida.Fecha.Month)));
                decimal totalReserva = Math.Round(fCapital * ctaCapital.Empresa.ClaseSociedad.PorcentajeCapital, 2); // OJO: Revisar esto con Lorena
                Catalogo ctaReservaAnterior = ObtenerCuentaEspecial(ECuentaEspecial.ReservaLegalAnterior);
                if (ctaReservaAnterior != null)
                    fReservaAnterior = Convert.ToDecimal(fObjectSpace.Evaluate(typeof(SaldoMes), CriteriaOperator.Parse("Sum([SaldoFin])"),
                               CriteriaOperator.Parse("[Cuenta.Oid] == ? && [Periodo.Oid] == ? && [Mes] == ?", ctaReservaAnterior.Oid, fPartida.Fecha.Year, fPartida.Fecha.Month)));
                if (fReservaAnterior < totalReserva)
                {
                    decimal reservaEjercicio = Math.Round(utilidadOperacion * ctaCapital.Empresa.ClaseSociedad.PorcentajeAnual, 2);
                    reservaAplicar = (fReservaAnterior + reservaEjercicio < totalReserva) ? reservaEjercicio : totalReserva - fReservaAnterior;
                    CreateDetalle(ctaReserva, ctaReserva.Concepto, 0.0m, reservaAplicar);
                    return reservaAplicar;
                }
            }
            return reservaAplicar;
        }

        /// <summary>
        /// Calcular el impuesto sobre la renta por pagar cuando hay utilidad de operacion e insertar el asiento correspondiente en la partida de liquidacion
        /// </summary>
        /// <param name="utilidadOperacion"></param>
        /// <param name="reservaLegal"></param>
        /// <param name="impuestoNoDeducible"></param>    
        /// <returns>el monto neto del impuesto de la renta a pagar</returns>
        /// <remarks>
        /// De la renta por pagar se debe deducir (restar) el pago a cuenta (anticipo de renta pagada) 
        /// </remarks>
        private decimal CalcularImpuestoRenta(decimal utilidadOperacion, decimal reservaLegal) //, decimal impuestoNoDeducible)
        {
            var ctaRenta = ObtenerCuentaEspecial(ECuentaEspecial.RentaPagar);
            var ctaPagoCta = ObtenerCuentaEspecial(ECuentaEspecial.RentaPagada);
            if (ctaRenta != null)
            {
                //decimal pagoCta = 0.0m;
                //if (ctaPagoCta != null)
                //{
                //    pagoCta = Convert.ToDecimal(fObjectSpace.Evaluate(typeof(SaldoMes), CriteriaOperator.Parse("Sum([SaldoFin])"),
                //               CriteriaOperator.Parse("[Cuenta.Oid] == ? && [Periodo.Oid] == ? && [Mes] == ?", ctaPagoCta.Oid, fPartida.Fecha.Year, fPartida.Fecha.Month)));
                //    if (pagoCta > 0)
                //        CreateDetalle(ctaPagoCta, ctaPagoCta.Concepto, 0.0m, pagoCta);
                //}
                decimal renta = Math.Round((utilidadOperacion - reservaLegal) * ctaRenta.Empresa.ClaseSociedad.PorcentajeRenta, 2);
                CreateDetalle(ctaRenta, ctaRenta.Concepto, 0.0m, renta); // - pagoCta + impuestoNoDeducible);
                return renta;
            }
            else
                return 0.0m;
        }

        /// <summary>
        /// Calcular el gasto financiero no deducible del impuesto sobre la renta (se suma) pero se resta de la utilidad del ejerccio
        /// </summary>
        /// <returns>El monto del gasto financiero no deducible despues de aplicar el porcentaje de renta que corresponde</returns>
        /// <remarks>
        /// En este caso la propiedad de cuenta especial debe ser sobre la cuenta de mayor porque los gastos no deducibles estan 
        /// separados en sub-cuentas. 
        /// OJO que estas sub-cuentas y cuentas de detalle siempre deben de comportarse de la misma manera para que los calculos
        /// sean correctos
        /// </remarks>
        //private decimal ImpuestoGastoFinancieroNoDeducible()
        //{
        //    var ctaGastoNoDeducible = fObjectSpace.FindObject<Catalogo>(CriteriaOperator.Parse("[Empresa.Oid] == ? && [CuentaEspecial] == ? && [Activa] == True",
        //                                             fPartida.Empresa.Oid, ECuentaEspecial.GastoNoDeducible));
        //    decimal gastoNoDeducible = 0.0m;
        //    if (ctaGastoNoDeducible != null)
        //    {
        //        gastoNoDeducible = Convert.ToDecimal(fObjectSpace.Evaluate(typeof(SaldoMes), CriteriaOperator.Parse("Sum([SaldoFin])"),
        //                       CriteriaOperator.Parse("[Cuenta.Oid] == ? && [Periodo.Oid] == ? && [Mes] == ?", ctaGastoNoDeducible.Oid, fPartida.Fecha.Year, fPartida.Fecha.Month)));
        //        gastoNoDeducible = gastoNoDeducible * ctaGastoNoDeducible.Empresa.ClaseSociedad.PorcentajeRenta;
        //    }
        //    return gastoNoDeducible;
        //}

        private Catalogo ObtenerCuentaEspecial(ECuentaEspecial ctaEspecial)
        {
            //fObjectSpace.GetObjectsQuery
            return fObjectSpace.FindObject<Catalogo>(CriteriaOperator.Parse("[Empresa.Oid] == ? && [CuentaEspecial] == ? && [Activa] == True && [CtaResumen] == False",
                                                     fPartida.Empresa.Oid, ctaEspecial));
        }

        public void PartidaDeCierre(out string msg)
        {
            IQueryable<SaldoLocal> saldos = ObtenerSaldos(new ETipoCuentaCatalogo[] {ETipoCuentaCatalogo.Activo, ETipoCuentaCatalogo.Pasivo,
                                                               ETipoCuentaCatalogo.Patrimonio, ETipoCuentaCatalogo.Orden});
            if (saldos != null || saldos.Count() > 0)
            {
                foreach (var item in saldos)
                {
                    CreateDetalle(item.Cuenta, fPartida.Concepto, (item.TipoSaldoCta == ETipoSaldoCuenta.Acreedor) ? item.SaldoFin : 0.0m, /* debe */
                                                                   (item.TipoSaldoCta == ETipoSaldoCuenta.Deudor) ? item.SaldoFin : 0.0m   /* haber */);
                }
            }
            msg = string.Empty;
        }

        private IQueryable<SaldoLocal> ObtenerSaldos(ETipoCuentaCatalogo[] tipoCtas)
        {
            querySaldos = fObjectSpace.GetObjectsQuery<SaldoMes>();
            IQueryable<SaldoLocal> saldos = querySaldos.Where(y => y.Cuenta.Empresa.Oid == fPartida.Empresa.Oid && y.Periodo.Oid == fPartida.Periodo.Oid &&
                                           y.Mes == fPartida.Periodo.FechaFin.Month && y.Cuenta.CtaResumen == false && tipoCtas.Contains(y.Cuenta.TipoCuenta))
                                    .Select(y => new SaldoLocal
                                    {
                                        Cuenta = y.Cuenta,
                                        TipoSaldoCta = y.Cuenta.TipoSaldoCta,
                                        TipoCuenta = y.Cuenta.TipoCuenta,
                                        SaldoFin = y.SaldoFin,
                                        CuentaEspecial = y.Cuenta.CuentaEspecial,
                                        Activa = y.Cuenta.Activa
                                    });
            return saldos;
        }

        /// <summary>
        /// Generar la partida de apertura del periodo actual a partir de la partida de cierre del periodo anterior
        /// </summary>
        /// <param name="partida">instancia del BO que se esta creando y que corresponde con la partida de apertura</param>
        /// <param name="msg">Mensaje retornado por el metodo</param>
        public void PartidaApertura(out string msg)
        {
            if (fObjectSpace.GetObjectByKey<Periodo>(fPartida.Periodo.Oid - 1) == null)
            {
                msg = $"Periodo {fPartida.Periodo.Oid - 1} no existe";
                return;
            }
            Partida partidaCierre = fObjectSpace.FindObject<Partida>(CriteriaOperator.Parse("[Empresa.Oid] == ? && [Periodo.Oid] == ? && [Tipo] == ?",
                                        fPartida.Empresa.Oid, fPartida.Periodo.Oid - 1, ETipoPartida.Cierre));
            if (partidaCierre == null)
            {
                msg = $"Partida {Convert.ToString(ETipoPartida.Cierre)} del Periodo {fPartida.Periodo.Oid - 1} no existe para generar partida de Apertura";
                return;
            }
            foreach (var item in partidaCierre.Detalles)
            {
                fPartida.Detalles.Add(new PartidaDetalle(fPartida.Session)
                {
                    Cuenta = item.Cuenta,
                    Concepto = fPartida.Concepto,
                    ValorDebe = (item.Cuenta.TipoSaldoCta == ETipoSaldoCuenta.Deudor) ? item.ValorHaber : 0.0m,
                    ValorHaber = (item.Cuenta.TipoSaldoCta == ETipoSaldoCuenta.Acreedor) ? item.ValorDebe : 0.0m
                });
            }
            fPartida.Save();
            msg = string.Empty;
        }

    }
}
