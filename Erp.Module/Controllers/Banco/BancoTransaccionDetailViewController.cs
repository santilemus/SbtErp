using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using System;
using SBT.Apps.Banco.Module.BusinessObjects;
using SBT.Apps.Compra.Module.Controllers;
using DevExpress.Data.Filtering;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Compra.Module.BusinessObjects;
using SBT.Apps.CxP.Module.BusinessObjects;
using System.Reflection;
using DevExpress.Xpo.Metadata;
using DevExpress.Xpo;

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
    private PopupWindowShowAction pwsaSelectProveedor;
    private AppearanceController appearanceController;
    private PopupWindowShowAction pwsaSelectFacturaCompra;
    private IObjectSpace osFactura;
    public BancoTransaccionDetailViewController()
    {
        TargetObjectType = typeof(SBT.Apps.Banco.Module.BusinessObjects.BancoTransaccion);
        pwsaSelectProveedor = new PopupWindowShowAction(this, "pwsaSelectProveedor", PredefinedCategory.RecordEdit);
        pwsaSelectProveedor.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
        pwsaSelectProveedor.Caption = "Tercero";
        pwsaSelectProveedor.ImageName = "Tercero";

        // accion para agregar factura de compra que se esta cancelando
        pwsaSelectFacturaCompra = new PopupWindowShowAction(this, "SeleccionarCompraPagar", PredefinedCategory.RecordEdit);
        pwsaSelectFacturaCompra.Caption = "Pagar Compra";
        pwsaSelectFacturaCompra.ImageName = "CompraFactura";
        pwsaSelectFacturaCompra.ToolTip = @"Clic para cancelar una o más facturas de compra con el movimiento de bancos";
        pwsaSelectFacturaCompra.TargetObjectsCriteria = @"[Clasificacion.Tipo] in (3, 4) And [Clasificacion.Oid] in (11, 16, 20)";
    }

    protected override void OnActivated()
    {
        base.OnActivated();
        ObjectSpace.Committed += ObjectSpace_Committed;
        pwsaSelectProveedor.CustomizePopupWindowParams += pwsaSelectProveedor_CustomizePopupWindowParams;
        pwsaSelectProveedor.Execute += pwsaSelectProveedorExecute;

        pwsaSelectFacturaCompra.CustomizePopupWindowParams += PwsaSelectFacturaCompra_CustomizePopupWindowParams;
        pwsaSelectFacturaCompra.Execute += PwsaSelectFacturaCompra_Execute;

        appearanceController = Frame.GetController<AppearanceController>();
        if (appearanceController != null)
        {
            appearanceController.CustomApplyAppearance +=
                new EventHandler<ApplyAppearanceEventArgs>(
                    appearanceController_CustomApplyAppearance);
        }
    }

    protected override void OnDeactivated()
    {
        pwsaSelectProveedor.CustomizePopupWindowParams -= pwsaSelectProveedor_CustomizePopupWindowParams;
        pwsaSelectProveedor.Execute -= pwsaSelectProveedorExecute;
        ObjectSpace.Committed -= ObjectSpace_Committed;
        pwsaSelectFacturaCompra.CustomizePopupWindowParams -= PwsaSelectFacturaCompra_CustomizePopupWindowParams;
        pwsaSelectFacturaCompra.Execute -= PwsaSelectFacturaCompra_Execute;

        if (appearanceController != null)
        {
            appearanceController.CustomApplyAppearance -=
                 new EventHandler<ApplyAppearanceEventArgs>(
                     appearanceController_CustomApplyAppearance);
        }

        base.OnDeactivated();
    }

    protected override void Dispose(bool disposing)
    {
        if (pwsaSelectProveedor != null)
            pwsaSelectProveedor.Dispose();
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

    private void pwsaSelectProveedor_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
    {
        IObjectSpace os = e.Application.CreateObjectSpace(typeof(Tercero.Module.BusinessObjects.Tercero));
        string idView = e.Application.FindLookupListViewId(typeof(Tercero.Module.BusinessObjects.Tercero));
        IModelListView modelListView = (IModelListView)Application.FindModelView(idView);
        modelListView.FilterEnabled = true;
        modelListView.DataAccessMode = CollectionSourceDataAccessMode.Server;
        //alternativa CollectionSourceDataAccessMode.ServerView es más light
        //modelListView.DataAccessMode = CollectionSourceDataAccessMode.ServerView;
        CollectionSourceBase collectionSource = Application.CreateCollectionSource(os, typeof(Tercero.Module.BusinessObjects.Tercero), idView);
        e.View = Application.CreateListView(modelListView, collectionSource, true);
    }

    private void pwsaSelectProveedorExecute(object sender, PopupWindowShowActionExecuteEventArgs e)
    {
        // cuando modelListView.DataAccessMode = CollectionSourceDataAccessMode.ServerView. Se tiene que obtener el tercero.
        //var tercero = ObjectSpace.GetObjectByKey<Tercero.Module.BusinessObjects.Tercero>(((DevExpress.ExpressApp.ObjectRecord)e.PopupWindowView.CurrentObject).ObjectKeyValue);
        var tercero = (e.PopupWindowView.CurrentObject as Tercero.Module.BusinessObjects.Tercero);
        (View.CurrentObject as BancoTransaccion).Beneficiario = tercero.Nombre;

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
                else if (bancoTransaccion.Clasificacion.Oid == 16 || bancoTransaccion.Clasificacion.Oid == 20)   // nota de cargo pago a proveedor
                    pagoFactura.Tipo = ObjectSpace.GetObjectByKey<CxCTipoTransaccion>(10);
            }
            pagoFactura.Moneda = ObjectSpace.GetObject<Moneda>(bancoTransaccion.Moneda);
            pagoFactura.ValorMoneda = bancoTransaccion.ValorMoneda;
            pagoFactura.Monto = item.Saldo;
            pagoFactura.Estado = ECxPTransaccionEstado.Digitado;
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
