using System;
using System.Linq;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Actions;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Base.Module.Controllers;
using SBT.Apps.CxP.Module.BusinessObjects;
using DevExpress.ExpressApp;
using System.ComponentModel;
using SBT.Apps.Compra.Module.BusinessObjects;

namespace SBT.Apps.Compra.Module.Controllers
{
    /// <summary>
    /// View Controller para el BO CxPTransaccion (transacciones de cuentas por pagar)
    /// </summary>
    public class vcCxPTransaccion : ViewControllerBase
    {
        public vcCxPTransaccion() : base()
        {

        }

        protected override void OnActivated()
        {
            base.OnActivated();
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }

        protected override void DoInitializeComponent()
        {
            base.DoInitializeComponent();
            TargetObjectType = typeof(SBT.Apps.CxP.Module.BusinessObjects.CxPTransaccion);
            TargetViewType = ViewType.Any;
        }

        /// <summary>
        /// realizar la actualizacion de la factura de compra cuando se guarda la transacción de cuenta por pagar
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void ObjectSpace_Committing(object Sender, CancelEventArgs﻿ e)
        {
            System.Collections.IList items = ObjectSpace.ModifiedObjects;
            if (items == null || items.Count == 0)
            {
                e.Cancel = true;
                return;
            }
            foreach (object item in items)
            {
                if (item.GetType() == typeof(SBT.Apps.CxP.Module.BusinessObjects.CxPTransaccion) ||
                    item.GetType() == typeof(SBT.Apps.CxP.Module.BusinessObjects.CxPDocumento))
                {
                    if (((CxPTransaccion)item).Factura.Estado != EEstadoFactura.Anulado)
                        ActualizarSaldoFactura((CxPTransaccion)item);
                    else
                        e.Cancel = true;
                }
                else
                {
                    // aqui cuando es el CxPDocumentoDetalle podria ser necesario actualizar el inventario, kardex, lote
                    // solo si esos documentos afectan el inventario (por ejemplo notas de credito por devolucion al proveedor) 
                }
            }
        }

        /// <summary>
        /// Actualizar el saldo de la factura
        /// </summary>
        /// <param name="item">Es un documento de CxP</param>
        private void ActualizarSaldoFactura(CxPTransaccion item)
        {
            if (item.Factura.Estado == EEstadoFactura.Anulado)
                return;
            decimal fCargo = Convert.ToDecimal(item.Factura.CxPTransacciones.Where(x => x.Factura == item.Factura && x.Tipo.TipoOperacion == ETipoOperacion.Cargo &&
                                               x.Estado != ECxPTransaccionEstado.Anulado).Sum(x => x.Monto));
            decimal fAbono = Convert.ToDecimal(item.Factura.CxPTransacciones.Where(x => x.Factura == item.Factura && x.Tipo.TipoOperacion == ETipoOperacion.Abono &&
                                                           x.Estado != ECxPTransaccionEstado.Anulado).Sum(x => x.Monto));
            decimal fNcredito = Convert.ToDecimal(item.Factura.CxPTransacciones.Where(
                x => x.Factura == item.Factura && x.Tipo.Oid > 1 && x.Tipo.Oid <= 4 && x.Estado != ECxPTransaccionEstado.Anulado).Sum(x => x.Monto));
            decimal monto = Math.Abs(fCargo - fAbono);
            if (fNcredito > 0 && item.Factura.Total == fNcredito)
                item.Factura.ActualizarSaldo(0.0m, EEstadoFactura.Devolucion, true);
            else if ((item.Factura.Total - monto) == 0.0m)
            {
                item.Factura.ActualizarSaldo(0.0m, EEstadoFactura.Pagado, true);
            }
            else
                item.Factura.ActualizarSaldo(item.Factura.Total - monto, EEstadoFactura.Debe, true);
            item.Factura.Save();
        }
    }
}
