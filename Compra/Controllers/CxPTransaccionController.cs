using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Base.Module.Controllers;
using SBT.Apps.Compra.Module.BusinessObjects;
using SBT.Apps.CxP.Module.BusinessObjects;
using SBT.Apps.Inventario.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Compra.Module.Controllers
{
    /// <summary>
    /// View Controller para el BO CxPTransaccion (transacciones de cuentas por pagar)
    /// </summary>
    public class CxPTransaccionController : ViewControllerBase
    {
        public CxPTransaccionController() : base()
        {
            DoInitializeComponent();
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            ObjectSpace.Committing += ObjectSpace_Committing;
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            ObjectSpace.Committing -= ObjectSpace_Committing;
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
            var items = ObjectSpace.ModifiedObjects.Cast<IXPObject>().Where<IXPObject>(x => x.GetType() == typeof(CxPTransaccion) || x.GetType() == typeof(CxPDocumento));
            try
            {
                if (items != null && items.Count() > 0)
                    ActualizarCxP(items);
                items = ObjectSpace.ModifiedObjects.Cast<IXPObject>().Where<IXPObject>(x => x.GetType() == typeof(CxPDocumentoDetalle));
                if (items != null && items.Count() > 0)
                    
                {
                        DoProcesarDetalleCxP(items);  // aqui se actualiza el inventario
                }
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage($@"Error {ex.Message}", InformationType.Error);
            }
        }

        private void ActualizarCxP(IEnumerable<IXPObject> items)
        {
            foreach (CxPTransaccion item in items)
                if (item.Estado != ECxPTransaccionEstado.Anulado)
                    ActualizarSaldoFactura(item);
        }

        private bool DoProcesarDetalleCxP(IEnumerable<IXPObject> items)
        {
            /// 401 es el Codigo del Tipo de Movimiento de Inventario que corresponde a devoluciones de compras (notas de credito)
            /// las 2 siguientes lineas estan fuera del foreach para evitar ejecutarlo mas de una vez
            CriteriaOperator criteria = CriteriaOperator.FromLambda<InventarioTipoMovimiento>(x => x.Codigo == "401" && x.Activo);
            InventarioTipoMovimiento tipoMovimiento = ObjectSpace.FindObject<InventarioTipoMovimiento>(criteria);
            if (tipoMovimiento == null)
            {
                Application.ShowViewStrategy.ShowMessage($"No se encontró Tipo de Movimiento de Inventario con código 401 que debe corresponder a Devoluciones a proveedores, el Inventario no se puede actualizar ",
                    InformationType.Error);
                return false;
            }
            foreach (CxPDocumentoDetalle item in items)
            {
                if (item.Documento.Tipo.Oid == 2 || item.Documento.Tipo.Oid == 17)
                {
                    ActualizarInventario(item, tipoMovimiento);
                    //ActualizarKardex(item, tipoMovimiento);
                    //ActualizarLote
                }
            }
            return true;
        }

        private void ActualizarInventario(CxPDocumentoDetalle item, InventarioTipoMovimiento tipo)
        {
            // pendiente de implementar
            throw new NotImplementedException();
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
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace())
            {
                CompraFactura compraFactura = os.GetObject<CompraFactura>(item.Factura);
                if (fNcredito > 0 && item.Factura.Total == fNcredito)
                    compraFactura.ActualizarSaldo(0.0m, EEstadoFactura.Devolucion, true);
                else if ((item.Factura.Total - item.Factura.Renta - monto) == 0.0m)
                    compraFactura.ActualizarSaldo(0.0m, EEstadoFactura.Pagado, true);
                else
                    compraFactura.ActualizarSaldo(item.Factura.Total - item.Factura.Renta - monto, EEstadoFactura.Debe, true);
                compraFactura.Save();
                os.CommitChanges();
            }
            
        }
    }
}
