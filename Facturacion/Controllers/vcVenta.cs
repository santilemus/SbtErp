using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Base.Module.Controllers;
using SBT.Apps.CxC.Module.BusinessObjects;
using SBT.Apps.Facturacion.Module.BusinessObjects;
using SBT.Apps.Inventario.Module.BusinessObjects;
using System;
using System.ComponentModel;

namespace SBT.Apps.Facturacion.Module.Controllers
{
    /// <summary>
    /// View Controller para el BO Ventas
    /// </summary>
    /// <remarks>
    /// Para el evento ObjectChanged mas info aqui: https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.IObjectSpace.ObjectChanged
    /// El objetivo es mostrar mensajes proactivos cuando cambia la propiedad AutorizacionCorrelativo. Calcular el NoFactura
    /// o mostrar mensaje de error si se han emitido todos los documentos autorizados
    /// </remarks>
    public class vcVenta : ViewControllerBase
    {
        InventarioTipoMovimiento tipoMovimiento;
        private NewObjectViewController newController;
        private SimpleAction imprimirDocumento;
        private PopupWindowShowAction pwsaAnular;
        private PopupWindowShowAction pwsaDevolucionTotal;

        public vcVenta() : base()
        {
            TargetObjectType = typeof(SBT.Apps.Facturacion.Module.BusinessObjects.Venta);
            TargetViewType = ViewType.Any;
            TargetViewNesting = Nesting.Root;

            pwsaDevolucionTotal = new PopupWindowShowAction(this, "pwsaDevolucionTotal", DevExpress.Persistent.Base.PredefinedCategory.RecordEdit);
            pwsaDevolucionTotal.Caption = "Devolución Total";
            pwsaDevolucionTotal.ToolTip = @"Clic para generar una nota de crédito que revierte la totalidad de la venta";
            pwsaDevolucionTotal.TargetObjectType = typeof(SBT.Apps.Facturacion.Module.BusinessObjects.Venta);
            pwsaDevolucionTotal.TargetViewType = ViewType.DetailView;
            pwsaDevolucionTotal.TargetViewId = "Venta_CxC_DetailView";
            //pwsaDevolucionTotal.TargetObjectsCriteria = "[Venta.Estado] == 'Debe' && [Venta.Saldo] != 0.0";
            pwsaDevolucionTotal.ImageName = "service";
            pwsaDevolucionTotal.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            pwsaDevolucionTotal.IsSizeable = true;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
            ObjectSpace.Committing += ObjectSpace_Committing;
            newController = Frame.GetController<NewObjectViewController>();
            if (newController != null)
                newController.ObjectCreated += NewController_ObjectCreated;
            pwsaAnular.Execute += PwsaAnular_Execute;
            pwsaAnular.CustomizePopupWindowParams += PwsaAnular_CustomizePopupWindowParams;
            pwsaDevolucionTotal.CustomizePopupWindowParams += PwsaDevolucionTotal_CustomizePopupWindowParams;
            pwsaDevolucionTotal.Execute += PwsaDevolucionTotal_Execute;
        }   

        protected override void OnDeactivated()
        {
            ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
            ObjectSpace.Committing -= ObjectSpace_Committing;
            if (newController != null)
                newController.ObjectCreated -= NewController_ObjectCreated;
            pwsaAnular.Execute -= PwsaAnular_Execute;
            pwsaAnular.CustomizePopupWindowParams -= PwsaAnular_CustomizePopupWindowParams;
            pwsaDevolucionTotal.CustomizePopupWindowParams -= PwsaDevolucionTotal_CustomizePopupWindowParams;
            pwsaDevolucionTotal.Execute -= PwsaDevolucionTotal_Execute;
            base.OnDeactivated();
        }

        /// <summary>
        /// Lanzar mensaje con advertencia que no existe un control de correlativos valido para el documento, y no esperar
        /// a que se ejecuten las validaciones
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void ObjectSpace_ObjectChanged(object Sender, ObjectChangedEventArgs e)
        {
            if (View == null || View.CurrentObject == null || e.Object == null)
                return;
            if (View.CurrentObject == e.Object && ObjectSpace.IsNewObject(View.CurrentObject))
            {
                if (e.PropertyName == "TipoFactura")
                {
                    if (((Venta)View.CurrentObject).Agencia == null)
                    {
                        Application.ShowViewStrategy.ShowMessage(@"Agencia es nula, porque el usuario no tiene asignada una. Seleccionar una en Mis Detalles", InformationType.Error);
                        return;
                    }
                }
                if (e.PropertyName == "AutorizacionDocumento")
                {
                    if (e.NewValue == null)
                    {
                        ((Venta)View.CurrentObject).NoFactura = null;
                        MostrarError($"No se encontró la autorización de correlativos para {((Venta)View.CurrentObject).TipoFactura.Nombre}. No conoce el número de documento");
                        return;
                    }
                    AutorizacionDocumento aud = (AutorizacionDocumento)e.NewValue;
                    int noFact = Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate<Venta>(CriteriaOperator.Parse("max([NoFactura])"),
                                 CriteriaOperator.Parse("Oid == ?", aud.Oid))) + 1;
                    if (noFact >= aud.NoDesde && noFact < aud.NoHasta)
                        ((Venta)View.CurrentObject).NoFactura = noFact;
                    else
                    {
                        ((Venta)View.CurrentObject).NoFactura = null;
                        MostrarError($"No hay autorización de correlativo disponible para {((Venta)View.CurrentObject).TipoFactura.Nombre}");
                    }
                }
            }
            // agregar aqui si se quiere procesar otros eventos
        }

        private void DoTipoFacturaChanged()
        {

        }


        /// <summary>
        /// realizar la actualizacion del inventario, el kardex y el lote cuando el metodo de costeo es PEPS o UEPS
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void ObjectSpace_Committing(object Sender, CancelEventArgs﻿ e)
        {
            System.Collections.IList items = ObjectSpace.ModifiedObjects;
            if (items.Count == 1 && items.GetType() != typeof(SBT.Apps.Facturacion.Module.BusinessObjects.VentaDetalle))
                return;

            if (items.Count > 0)
            {
                /// 301 es el Codigo del Tipo de Movimiento de Inventario que corresponde a facturacion
                tipoMovimiento = ObjectSpace.FindObject<InventarioTipoMovimiento>(CriteriaOperator.And(new BinaryOperator("Codigo", "301"), new BinaryOperator("Activo", true)));
                if (tipoMovimiento == null)
                {
                    MostrarError($"No se encontró Tipo de Movimiento de Inventario con código 301 que debe corresponder a Facturación, el Inventario no se puede actualizar ");
                    e.Cancel = true;
                    return;
                }

                foreach (object item in items)
                {
                    if (item.GetType() != typeof(SBT.Apps.Facturacion.Module.BusinessObjects.VentaDetalle))
                        continue;
                    VentaDetalle fItem = (VentaDetalle)item;
                    if (fItem.Producto.Categoria.Clasificacion == Producto.Module.BusinessObjects.EClasificacion.Servicios ||
                        fItem.Producto.Categoria.Clasificacion == Producto.Module.BusinessObjects.EClasificacion.Intangible ||
                        fItem.Producto.Categoria.Clasificacion == Producto.Module.BusinessObjects.EClasificacion.Otros)
                        continue;
                    DoActualizarInventario(fItem);
                    DoActualizarKardex(fItem);
                    if (fItem.Producto.Categoria.MetodoCosteo == Producto.Module.BusinessObjects.EMetodoCosteoInventario.PEPS ||
                        fItem.Producto.Categoria.MetodoCosteo == Producto.Module.BusinessObjects.EMetodoCosteoInventario.UEPS)
                        DoActualizarLote(fItem);
                }
            }
        }

        /// <summary>
        /// Delegate que se ejecuta cuando se crea un nuevo objeto de venta. Dependiendo del Id de la vista de detalle
        /// (view variant) asigna el tipo de factura (para que se asigne la factura correcta que corresponde al view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void NewController_ObjectCreated(object sender, ObjectCreatedEventArgs e)
        {
            string sCodigo;
            if (e.CreatedObject is Venta)
            {
                if (View.Id == "Venta_ListView_fcf" || View.Id == "Venta_DetailView_fcf")
                    sCodigo = "COVE02";
                else if (View.Id == "Venta_ListView_exportacion" || View.Id == "Venta_DetailView_exportacion")
                    sCodigo = "COVE03";
                else if (View.Id == "Venta_ListView_simplif" || View.Id == "Venta_DetailView_simplif")
                    sCodigo = "COVE04";
                else if (View.Id == "Venta_ListView_ticket" || View.Id == "Venta_DetailView_ticket")
                    sCodigo = "COVE04";
                else
                    sCodigo = "COVE01";
                Listas tipoFactura = e.ObjectSpace.GetObjectByKey<Listas>(sCodigo);
                if (tipoFactura != null)
                    ((Venta)e.CreatedObject).TipoFactura = tipoFactura;
            }
        }

        /// <summary>
        /// actualizar el inventario por cada linea del detalle del documento de venta. 
        /// El acumulado de las salidas por venta en el inventario se obtiene al sumar las cantidades vendida en el mes, 
        /// excepto para el detalle de venta que esta siendo procesado. Cuando es modificacion se suma item.Cantidad al
        /// al acumulado del mes.
        /// Cuando se ha eliminado el item de la venta, considerar solo la cantidad acumulada del mes como  se meciono
        /// antes, tiene el mismo efecto que restar de la salida la cantidad del item eliminado del mes
        /// </summary>
        /// <param name="item">La linea de detalle de la factura (instancia del BO VentaDetalle)</param>
        private void DoActualizarInventario(VentaDetalle item)
        {
            Inventario.Module.BusinessObjects.Inventario inventarioItem = ObjectSpace.FindObject<Inventario.Module.BusinessObjects.Inventario>(
                CriteriaOperator.Parse("[Bodega.Oid] == ? && [Producto.Oid] == ? && [TipoMovimiento.Oid] == ?", item.Bodega.Oid, item.Producto.Oid, tipoMovimiento.Oid));
            if (inventarioItem == null)
            {
                inventarioItem = ObjectSpace.CreateObject<Inventario.Module.BusinessObjects.Inventario>();
                inventarioItem.Bodega = item.Bodega;
                inventarioItem.Producto = item.Producto;
                inventarioItem.TipoMovimiento = tipoMovimiento;
                inventarioItem.Cantidad = 0.0m;
            }
            if (ObjectSpace.IsNewObject(item))
                inventarioItem.Cantidad += item.Cantidad;
            else
            {
                decimal cantidadMes = Convert.ToDecimal(ObjectSpace.Evaluate(typeof(VentaDetalle), CriteriaOperator.Parse("Sum([Cantidad])"),
                     CriteriaOperator.Parse("GetMonth([Venta.Fecha]) == ? && GetYear([Venta.Fecha]) == ? && [Producto.Oid] == ? && [Bodega.Oid] == ? && [Oid] != ?",
                     item.Venta.Fecha.Month, item.Venta.Fecha.Year, item.Producto.Oid, item.Bodega.Oid, item.Oid)));
                // Si la fila fue borrada, la nueva cantidad (de salidas por venta) es la cantidad acumulada del mes, sin considerar al item [Oid] != item.Oid
                cantidadMes += item.IsDeleted ? 0 : item.Cantidad;
                inventarioItem.Cantidad = cantidadMes;
            }
            inventarioItem.Save();
        }

        /// <summary>
        /// actualizar el kardex por cada linea del detalle del documento de venta. 
        /// Se verifica que el kardex no existe a traves del Producto, TipoMovimiento, Fecha y Referencia
        /// Cuando se ha eliminado el item de la venta, considerar solo la cantidad acumulada del mes como  se meciono
        /// antes, tiene el mismo efecto que restar de la salida la cantidad del item eliminado del mes
        /// </summary>
        /// <param name="item">La linea de detalle de la factura (instancia del BO VentaDetalle)</param>
        /// <remarks>
        /// Evaluar en las pruebas porque cuando pase por aqui es posible que item.Oid aun no tenga valor, en cuyo caso 
        /// tendremos que usar otro datos, por ejemplo item.Venta.Oid
        /// </remarks>
        private void DoActualizarKardex(VentaDetalle item)
        {
            Kardex kardexItem = ObjectSpace.FindObject<Kardex>(
                CriteriaOperator.Parse("[Producto.Oid] == ? && [TipoMovimiento] == ? && [Fecha] = ? && [Referencia] == ?",
                item.Producto.Oid, tipoMovimiento.Oid, item.Venta.Fecha, item.Oid));
            if (kardexItem == null)
            {
                kardexItem = ObjectSpace.CreateObject<Kardex>();
                kardexItem.Bodega = item.Bodega;
                kardexItem.Producto = item.Producto;
                kardexItem.TipoMovimiento = tipoMovimiento;
            }
            kardexItem.Cantidad = item.Cantidad;
            kardexItem.CostoUnidad = item.Costo;
            kardexItem.PrecioUnidad = item.PrecioUnidad;
            kardexItem.Referencia = item.Oid;
            kardexItem.Save();
        }

        /// <summary>
        /// Actualizar las salidas del lote cuando el metodo de costeo del inventario es PEPS o UEPS
        /// </summary>
        /// <param name="item">La linea de detalle de la factura (instancia del BO VentaDetalle)</param>
        private void DoActualizarLote(VentaDetalle item)
        {
            if (item.Lote != null)
            {
                InventarioLote lote = ObjectSpace.GetObjectByKey<InventarioLote>(item.Lote.Oid);
                if (lote == null)
                    return;
                if (ObjectSpace.IsNewObject(item))
                {
                    if (tipoMovimiento.Operacion == ETipoOperacionInventario.Salida)
                        lote.Salida += item.Cantidad;
                    else
                        lote.Entrada += item.Cantidad;
                    lote.Costo = item.Costo;
                }
                else
                {
                    if (item.IsDeleted)
                    {
                        if (tipoMovimiento.Operacion == ETipoOperacionInventario.Salida)
                            lote.Salida -= item.Cantidad;
                        else
                            lote.Entrada -= item.Cantidad;
                    }
                }
                lote.Save();
            }
        }

        protected override void DoInitializeComponent()
        {
            base.DoInitializeComponent();
            pwsaAnular = new PopupWindowShowAction(this, "Venta_Anular", DevExpress.Persistent.Base.PredefinedCategory.RecordEdit.ToString());
            pwsaAnular.TargetObjectType = typeof(SBT.Apps.Facturacion.Module.BusinessObjects.Venta);
            pwsaAnular.TargetViewType = ViewType.DetailView;
            pwsaAnular.ToolTip = "Clic para Anular el documento seleccionado";
            pwsaAnular.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            pwsaAnular.Caption = "Anular";
            pwsaAnular.ImageName = "Attention";
            pwsaAnular.TargetViewId = "Venta_DetailView;Venta_DetailView_ccf;Venta_DetailView_fcf;Venta_DetailView_exportacion;Venta_DetailView_simplif;" +
                "Venta_ListView;Venta_ListView_ccf;Venta_ListView_fcf;Venta_ListView_exportacion;Venta_ListView_Todo";
            pwsaAnular.TargetObjectsCriteriaMode = TargetObjectsCriteriaMode.TrueForAll;
            pwsaAnular.TargetObjectsCriteria = "[Estado] = 'Debe' && [Saldo] != 0.0";
            pwsaAnular.AcceptButtonCaption = "Anular";
            pwsaAnular.CancelButtonCaption = "Cancelar";
            pwsaAnular.ConfirmationMessage = "Esta segur@ de anular el documento seleccionado";


            imprimirDocumento = new SimpleAction(this, "Venta_ImprimirFactura", DevExpress.Persistent.Base.PredefinedCategory.RecordEdit.ToString());
            imprimirDocumento.Caption = "Imprimir";
            imprimirDocumento.TargetObjectType = typeof(SBT.Apps.Facturacion.Module.BusinessObjects.Venta);
            imprimirDocumento.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            imprimirDocumento.ToolTip = "Click para mostrar la vista previa de la impresión de la factura seleccionada";
            imprimirDocumento.Execute += ImprimirDocumento_Execute;
            imprimirDocumento.TargetObjectsCriteria = "[Oid] > 0";
            imprimirDocumento.ImageName = "ShowPrintPreview";
        }

        private void PwsaAnular_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            Venta vta = (Venta)View.CurrentObject;
            vta.Anular(((AnularParametros)e.PopupWindowView.CurrentObject));
            View.ObjectSpace.CommitChanges();
        }

        private void PwsaAnular_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace osParam = Application.CreateObjectSpace(typeof(AnularParametros));
            AnularParametros anularParams = osParam.CreateObject<AnularParametros>();
            anularParams.FechaAnulacion = DateTime.Now;
            e.View = Application.CreateDetailView(osParam, anularParams);
            e.View.Caption = "Anular Venta";
           
        }

        private void ImprimirDocumento_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View.CurrentObject == null)
            {
                Application.ShowViewStrategy.ShowMessage("No se encuentra ningún documento de venta para imprimir", InformationType.Error);
                return;
            }
            var reportOsProvider = ReportDataProvider.GetReportObjectSpaceProvider(this.Application.ServiceProvider);
            var reportStorage = ReportDataProvider.GetReportStorage(this.Application.ServiceProvider);

            Venta vta = (Venta)View.CurrentObject;
            IObjectSpace objectSpace = reportOsProvider.CreateObjectSpace(typeof(ReportDataV2));
            IReportDataV2 reportData = objectSpace.GetObject<IReportDataV2>(vta.AutorizacionDocumento.Reporte);
            string handle = reportStorage.GetReportContainerHandle(reportData);
            ReportServiceController controller = Frame.GetController<ReportServiceController>();
            CriteriaOperator objectsCriteria = ((BaseObjectSpace)objectSpace).GetObjectsCriteria(View.ObjectTypeInfo, e.SelectedObjects);
            if (controller != null)
            {
                controller.ShowPreview(handle, objectsCriteria);
            };
        }

        /// <summary>
        /// evento para generar una nota de crédito por el total de la venta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PwsaDevolucionTotal_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            //((CxCDocumento)e.PopupWindowViewCurrentObject).Save();
            ((Venta)View.CurrentObject).CxCTransacciones.Add(((CxCDocumento)e.PopupWindowViewCurrentObject));
        }

        private void PwsaDevolucionTotal_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            e.DialogController.SaveOnAccept = false;
            CxCDocumento doc = ObjectSpace.CreateObject<CxCDocumento>();
            doc.Venta = (Venta)View.CurrentObject;
            var view = Application.CreateDetailView(ObjectSpace, "CxCDocumento_DetailView", false, doc);
            var tipo = ObjectSpace.GetObjectByKey<CxCTipoTransaccion>(2);
            doc.Tipo = tipo ?? null;
            doc.Moneda = doc.Venta.Moneda;
            doc.ValorMoneda = doc.Venta.ValorMoneda;
            doc.Fecha = DateTime.Now;
            foreach (var item in doc.Venta.Detalles)
            {
                var detaCxC = ObjectSpace.CreateObject<CxCDocumentoDetalle>();
                detaCxC.VentaDetalle = item;
                detaCxC.CxCDocumento = doc;
                detaCxC.Cantidad = item.Cantidad;
                detaCxC.PrecioUnidad = item.PrecioUnidad;
                detaCxC.Exenta = item.Exenta;
                detaCxC.Gravada = item.Gravada;
                detaCxC.NoSujeta = item.NoSujeta;
                detaCxC.Iva = item.Iva;
                doc.Detalles.Add(detaCxC);
            }
            doc.Estado = ECxCTransaccionEstado.Digitado;
 
            e.View = view;
        }
    }
}
