using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using System;
using SBT.Apps.Banco.Module.BusinessObjects;
using DevExpress.Data.Filtering;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Compra.Module.BusinessObjects;
using SBT.Apps.CxP.Module.BusinessObjects;
using DevExpress.Xpo.Metadata;
using DevExpress.Xpo;
using SBT.Apps.Facturacion.Module.BusinessObjects;
using SBT.Apps.Banco.Module.Controllers;
using SBT.Apps.CxC.Module.BusinessObjects;

namespace SBT.Apps.Erp.Module.Controllers.Banco;

/// <summary>
/// View Controller que corresponde a la vista de detalle del BO BancoTransaccion
/// </summary>
/// <remarks>
///  Con relacion a la personalizacion de la apariencia, ver mas informacion en
///  https://docs.devexpress.com/eXpressAppFramework/113374/conditional-appearance/how-to-customize-the-conditional-appearance-module-behavior
/// </remarks>
public class BancoTransaccionDetailViewController : ViewController<DetailView>
{
    private AppearanceController appearanceController;
    private PopupWindowShowAction pwsaSelectFacturaCompra;
    private PopupWindowShowAction pwsaSelectVenta;
    private SimpleAction saEntregaCheque;
    private IObjectSpace osFactura;
    public BancoTransaccionDetailViewController()
    {
        // accion para agregar factura de compra que se esta cancelando
        pwsaSelectFacturaCompra = new PopupWindowShowAction(this, "SeleccionarCompraPagar", PredefinedCategory.RecordEdit);
        pwsaSelectFacturaCompra.Caption = "Compras";
        pwsaSelectFacturaCompra.ImageName = "CompraFactura";
        pwsaSelectFacturaCompra.ToolTip = @"Clic para cancelar una o más facturas de compra con el movimiento de bancos";
        pwsaSelectFacturaCompra.TargetObjectsCriteria = @"[Clasificacion.Tipo] in (3, 4) And [Clasificacion.Oid] in (11, 16, 20) And !IsNull([Tercero])";        

        saEntregaCheque = new SimpleAction(this, "BancoTransaccion_EntregaCheque", PredefinedCategory.RecordEdit);
        saEntregaCheque.Caption = "Entregar Cheque";
        saEntregaCheque.TargetObjectsCriteria = "[Clasificacion.Tipo] = 'Cheque'";
        saEntregaCheque.TargetObjectType = typeof(BancoTransaccion);
        saEntregaCheque.ImageName = "Attention";
        saEntregaCheque.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
        saEntregaCheque.ToolTip = "Marcar el cheque seleccionado como entregado";

        pwsaSelectVenta = new PopupWindowShowAction(this, "SeleccionarVentaCobrar", PredefinedCategory.RecordEdit);
        pwsaSelectVenta.Caption = "Ventas";
        pwsaSelectVenta.TargetObjectsCriteria = "[Clasificacion.Tipo] in (1, 2) And [Clasificacion.Oid] in (1, 3, 5, 7) And !IsNull([Tercero])";
        pwsaSelectVenta.ImageName = "factura";
        pwsaSelectVenta.ToolTip = @"Clic para registrar el ingreso a la cuenta de bancos, por cobro de uno o más documentos de venta";
    }

    protected override void OnActivated()
    {
        base.OnActivated();
        ObjectSpace.Committed += ObjectSpace_Committed;
        pwsaSelectFacturaCompra.CustomizePopupWindowParams += PwsaSelectFacturaCompra_CustomizePopupWindowParams;
        pwsaSelectFacturaCompra.Execute += PwsaSelectFacturaCompra_Execute;
        saEntregaCheque.Execute += SaEntregaCheque_Execute;

        appearanceController = Frame.GetController<AppearanceController>();
        if (appearanceController != null)
        {
            appearanceController.CustomApplyAppearance +=
                new EventHandler<ApplyAppearanceEventArgs>(
                    appearanceController_CustomApplyAppearance);
        }

        pwsaSelectVenta.CustomizePopupWindowParams += PwsaSelectVenta_CustomizePopupWindowParams;
        pwsaSelectVenta.Execute += PwsaSelectVenta_Execute;
    }

    private void SaEntregaCheque_Execute(object sender, SimpleActionExecuteEventArgs e)
    {
        if (e.CurrentObject is BancoTransaccion)
        {
            (e.CurrentObject as BancoTransaccion).Estado = EBancoTransaccionEstado.Entregado;
            (e.CurrentObject as BancoTransaccion).Save();
        }
    }

    protected override void OnDeactivated()
    {
        ObjectSpace.Committed -= ObjectSpace_Committed;
        pwsaSelectFacturaCompra.CustomizePopupWindowParams -= PwsaSelectFacturaCompra_CustomizePopupWindowParams;
        pwsaSelectFacturaCompra.Execute -= PwsaSelectFacturaCompra_Execute;

        if (appearanceController != null)
        {
            appearanceController.CustomApplyAppearance -=
                 new EventHandler<ApplyAppearanceEventArgs>(
                     appearanceController_CustomApplyAppearance);
        }

        pwsaSelectVenta.CustomizePopupWindowParams -= PwsaSelectVenta_CustomizePopupWindowParams;
        pwsaSelectVenta.Execute -= PwsaSelectVenta_Execute;

        base.OnDeactivated();
    }

    protected override void Dispose(bool disposing)
    {
        if (pwsaSelectFacturaCompra != null)
            pwsaSelectFacturaCompra.Dispose();
        if (osFactura != null)
            osFactura.Dispose();
        base.Dispose(disposing);
    }

    private void ObjectSpace_Committed(object sender, EventArgs e)
    {
        var bancoTransaccion = (this.View.CurrentObject as BancoTransaccion);
        if (bancoTransaccion != null && bancoTransaccion.BancoCuenta != null)
        {
            bancoTransaccion.BancoCuenta.CalcularSaldo(bancoTransaccion.Fecha);
        }
    }

    private void PwsaSelectFacturaCompra_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
    {
        osFactura = Application.CreateObjectSpace(typeof(CompraFactura));
        var bancoTransaccion = (View.CurrentObject as BancoTransaccion);
        var criteria = CriteriaOperator.FromLambda<CompraFactura>(x => x.Empresa.Oid == bancoTransaccion.BancoCuenta.Empresa.Oid &&
                        x.Proveedor.Oid == bancoTransaccion.Tercero.Oid && x.Estado == EEstadoFactura.Debe && x.Saldo >= 0.0m);
        CollectionSourceBase collectionSource = Application.CreateCollectionSource(osFactura, typeof(CompraFactura), "CompraFactura_LookupListView_CxP", 
            CollectionSourceDataAccessMode.Server, CollectionSourceMode.Normal);
        collectionSource.Criteria["Pendientes"] = criteria;
        e.View = Application.CreateListView("CompraFactura_LookupListView_CxP", collectionSource, true);
        e.View.AllowNew["key"] = false;
        e.View.AllowDelete["key"] = false;
    }

    private void PwsaSelectFacturaCompra_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
    {
        if (e.PopupWindowView.SelectedObjects.Count == 0)
            return;
        decimal total = 0.0m;
        BancoTransaccion bancoTransaccion = (e.CurrentObject as BancoTransaccion);
        XPMemberInfo pagos = bancoTransaccion.ClassInfo.FindMember("Pagos");
        if (pagos == null)
        {
            Application.ShowViewStrategy.ShowMessage(@"No se encontro en transacción de bancos la colección Pagos. No se pueden aplicar los pagos a las facturas seleccionadas", 
                InformationType.Error);
            return;
        }
        
        foreach (SBT.Apps.Compra.Module.BusinessObjects.CompraFactura item in e.PopupWindowView.SelectedObjects)
        {
            var pagoFactura = ObjectSpace.CreateObject<CxPTransaccion>();
            pagoFactura.Factura = ObjectSpace.GetObject<CompraFactura>(item);
            pagoFactura.Fecha = (View.CurrentObject as BancoTransaccion).Fecha;
            pagoFactura.BancoTransaccion = bancoTransaccion;
            // NOTA 26/01/2024. Modificar el BO CxCTipoTransaccion, para incluir el Oid de la transaccion de bancos correspondiente cuando es pago a
            // proveedor (CxP) y también cuando es pago del cliente (CxC), para que no queden fijos acá, sino que se obtengan 
            // en este caso el filtro la CxCTipoTransaccion se obtendría buscando el Oid relacionado de la operación de bancos (que se ingreso antes)
            // de acuerdo al flujo de ingreso de datos
            if (bancoTransaccion.Clasificacion != null)
            {
                if (bancoTransaccion.Clasificacion.Oid == 11)            // cheque pago a proveedor
                    pagoFactura.Tipo = ObjectSpace.GetObjectByKey<CxCTipoTransaccion>(7);
                else //if (bancoTransaccion.Clasificacion.Oid == 16 || bancoTransaccion.Clasificacion.Oid == 20)   // nota de cargo pago a proveedor
                    pagoFactura.Tipo = ObjectSpace.GetObjectByKey<CxCTipoTransaccion>(10);
            }
            pagoFactura.Moneda = ObjectSpace.GetObject<Moneda>(bancoTransaccion.Moneda);
            pagoFactura.ValorMoneda = bancoTransaccion.ValorMoneda;
            pagoFactura.Monto = item.Saldo;
            pagoFactura.Estado = ECxPTransaccionEstado.Aplicado;
            string sComentario = $@"Pago a {item.Proveedor.Nombre} de factura {item.NumeroFactura}";
            pagoFactura.Comentario = (sComentario.Trim().Length <= 200) ? sComentario.Trim(): sComentario.Trim().Substring(0, 200);
            total += item.Saldo;           
            pagoFactura.Save();
            (bancoTransaccion["Pagos"] as XPCollection<CxPTransaccion>).Add(pagoFactura);
        }
        bancoTransaccion.Monto = total;
        bancoTransaccion.Save();
        ObjectSpace.CommitChanges();
        if (osFactura != null)
            osFactura.Dispose();  
    }

    private void PwsaSelectVenta_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
    {
        osFactura = Application.CreateObjectSpace(typeof(Venta));
        var bancoTransaccion = (View.CurrentObject as BancoTransaccion);
        var criteria = CriteriaOperator.FromLambda<Venta>(x => x.Empresa.Oid == bancoTransaccion.BancoCuenta.Empresa.Oid &&
                x.Cliente.Oid == bancoTransaccion.Tercero.Oid && x.Estado == EEstadoFactura.Debe && x.Saldo >= 0.0m);
        CollectionSourceBase collectionSource = Application.CreateCollectionSource(osFactura, typeof(Venta), "Venta_LookupListView_CxC",
            CollectionSourceDataAccessMode.Server, CollectionSourceMode.Normal);
        collectionSource.Criteria["Pendientes"] = criteria;
        e.View = Application.CreateListView("Venta_LookupListView_CxC", collectionSource, true);
        e.View.AllowNew["key"] = false;
        e.View.AllowDelete["key"] = false;
    }

    private void PwsaSelectVenta_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
    {
        if (e.PopupWindowView.SelectedObjects.Count == 0)
            return;
        decimal total = 0.0m;
        BancoTransaccion bancoTransaccion = (e.CurrentObject as BancoTransaccion);
        XPMemberInfo pagos = bancoTransaccion.ClassInfo.FindMember("Cobros");
        if (pagos == null)
        {
            Application.ShowViewStrategy.ShowMessage(@"No se encontro en Transacción de Bancos, la colección Cobros. No se pueden aplicar los pagos de las ventas seleccionadas",
                InformationType.Error);
            return;
        }
        foreach (Venta item in e.PopupWindowView.SelectedObjects)
        {
            var cobroFactura = ObjectSpace.CreateObject<CxCTransaccion>();
            cobroFactura.Venta = ObjectSpace.GetObject<Venta>(item);
            cobroFactura.Fecha = (View.CurrentObject as BancoTransaccion).Fecha;
            cobroFactura.BancoTransaccion = bancoTransaccion;
            // NOTA 26/01/2024. Modificar el BO CxCTipoTransaccion, para incluir el Oid de la transaccion de bancos correspondiente cuando es pago de
            // cliente (CxC) para que no queden fijos acá, sino que se obtengan en este caso el filtro la CxCTipoTransaccion se obtendría buscando el
            // Oid relacionado de la operación de bancos (que se ingreso antes) de acuerdo al flujo de ingreso de datos
            if (bancoTransaccion.Clasificacion != null)
            {
                // Remesa Venta Cobro de Crédito (1), Remesa Venta de Contado (3), Nota de Abono Cobro Venta de Crédito (5), Noto de Abono Venta de Contado (7)
                //if (bancoTransaccion.Clasificacion.Oid == 1 || bancoTransaccion.Clasificacion.Oid == 3 ||
                //    bancoTransaccion.Clasificacion.Oid == 5 || bancoTransaccion.Clasificacion.Oid == 7)            
                    cobroFactura.Tipo = ObjectSpace.GetObjectByKey<CxCTipoTransaccion>(11);   // remesa a cuenta
            }
            cobroFactura.Moneda = ObjectSpace.GetObject<Moneda>(bancoTransaccion.Moneda);
            cobroFactura.ValorMoneda = bancoTransaccion.ValorMoneda;
            cobroFactura.Monto = item.Saldo;
            cobroFactura.Estado = ECxCTransaccionEstado.Aplicado;
            string sComentario = $@"Pago de {item.Cliente.Nombre}. Factura {item.CodigoGeneracion}";
            cobroFactura.Comentario = (sComentario.Trim().Length <= 200) ? sComentario.Trim() : sComentario.Trim().Substring(0, 200);
            total += item.Saldo;
            cobroFactura.Save();
            (bancoTransaccion["Cobros"] as XPCollection<CxCTransaccion>).Add(cobroFactura);
        }
        bancoTransaccion.Monto = total;
        bancoTransaccion.Save();
        ObjectSpace.CommitChanges();
        if (osFactura != null)
            osFactura.Dispose();
    }

    private void appearanceController_CustomApplyAppearance(object sender, ApplyAppearanceEventArgs e)
    {
        if (e.AppearanceObject.Enabled == false)
        {
            CustomizeDisabledEditorsAppearance(e);
            // e.Handled = true;
        }
    }

    protected virtual void CustomizeDisabledEditorsAppearance(ApplyAppearanceEventArgs e) { }
}
