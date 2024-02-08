using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Validation;
using SBT.Apps.Base.Module.Controllers;
using SBT.Apps.Facturacion.Module.BusinessObjects;
using SBT.Apps.Producto.Module.BusinessObjects;
using System;
using System.Linq;

namespace SBT.Apps.Facturacion.Module.Controllers
{
    /// <summary>
    /// View Controller que corresponde al BO VentaDetalle
    /// </summary>
    public class vcVentaDetalle : ViewControllerBase
    {
        private NewObjectViewController newController;
        private readonly PopupWindowShowAction pwsaSeleccionarPrecio;
        private int productoNuevoOid;

        public vcVentaDetalle() : base()
        {
            TargetObjectType = typeof(SBT.Apps.Facturacion.Module.BusinessObjects.VentaDetalle);
            TargetViewType = ViewType.Any;

            pwsaSeleccionarPrecio = new(this, "Venta_ListaDePrecios", DevExpress.Persistent.Base.PredefinedCategory.RecordEdit)
            {
                Caption = "Precios",
                ImageName = "ProductoPrecio",
                AcceptButtonCaption = "Aceptar",
                CancelButtonCaption = "Cancelar",
                ActionMeaning = ActionMeaning.Accept,
                TargetObjectType = typeof(SBT.Apps.Facturacion.Module.BusinessObjects.VentaDetalle),
                //pwsaSeleccionarPrecio.TargetObjectsCriteriaMode = TargetObjectsCriteriaMode.TrueAtLeastForOne;
                //pwsaSeleccionarPrecio.TargetObjectsCriteria = "!IsNull([Producto])";
                SelectionDependencyType = SelectionDependencyType.Independent,
                ToolTip = "Lista de precios que aplican al producto"
            };
        }

        private void PwsaSeleccionarPrecio_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            if (e.PopupWindowViewCurrentObject != null)
            {
                (View.CurrentObject as VentaDetalle).PrecioUnidad = (e.PopupWindowViewCurrentObject as ProductoPrecio).PrecioUnitario;
            }
        }

        private void PwsaSeleccionarPrecio_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var objectType = typeof(SBT.Apps.Producto.Module.BusinessObjects.ProductoPrecio);
            IObjectSpace os = Application.CreateObjectSpace(objectType);
            string listViewId = "Producto_Precios_Seleccion";
            IModelListView precioListView = (IModelListView)Application.FindModelView(listViewId);
            if (precioListView == null)
            {
                listViewId = Application.FindLookupListViewId(objectType);
                precioListView = (IModelListView)Application.FindModelView(listViewId);
            }
            CollectionSourceBase collectionSource = Application.CreateCollectionSource(os, objectType, listViewId, CollectionSourceDataAccessMode.Server, CollectionSourceMode.Normal);

            int ProductoOid = (productoNuevoOid > 0) ? productoNuevoOid : (View.CurrentObject as VentaDetalle).Producto.Oid;
            collectionSource.Criteria["Producto"] = CriteriaOperator.Parse("[Producto] == ? && [Activo] == True", ProductoOid);
            e.View = Application.CreateListView(precioListView, collectionSource, true);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
            newController = Frame.GetController<NewObjectViewController>();
            if (newController != null)
            {
                newController.ObjectCreating += NewController_ObjectCreating;
            }
            pwsaSeleccionarPrecio.CustomizePopupWindowParams += PwsaSeleccionarPrecio_CustomizePopupWindowParams;
            pwsaSeleccionarPrecio.Execute += PwsaSeleccionarPrecio_Execute;
            ObjectSpace.Committed += ObjectSpace_Committed;
        }

        private void ObjectSpace_Committed(object sender, EventArgs e)
        {
            productoNuevoOid = -1;
        }

        protected override void OnDeactivated()
        {
            if (newController != null)
            {
                //    newController.ObjectCreated -= NewController_ObjectCreated;
                newController.ObjectCreating -= NewController_ObjectCreating;
            }
            ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
            pwsaSeleccionarPrecio.CustomizePopupWindowParams -= PwsaSeleccionarPrecio_CustomizePopupWindowParams;
            pwsaSeleccionarPrecio.Execute -= PwsaSeleccionarPrecio_Execute;
            base.OnDeactivated();
        }

        /// <summary>
        /// Generar alerta cuando la existencia disponible en el inventario o en lote, no es suficiente para cubrir la
        /// cantidad (acumulada) del producto que esta siendo facturado. El proposito no es esperar hasta la validacion
        /// para que el usuario se de cuenta
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void ObjectSpace_ObjectChanged(object Sender, ObjectChangedEventArgs e)
        {
            if (View == null || View.CurrentObject == null || e.Object == null || e.NewValue == null)
                return;
            // cuando se crea un nuevo Producto, para obtener la lista de precios de producto que esta agregando
            if (View.CurrentObject == e.Object && e.PropertyName == "Producto" && ObjectSpace.IsNewObject(View.CurrentObject))
            {
                productoNuevoOid = e.NewValue != null ? (e.NewValue as Apps.Producto.Module.BusinessObjects.Producto).Oid : -1;
                return;
            }
            if (View.CurrentObject == e.Object && e.PropertyName == "Cantidad" && ObjectSpace.IsNewObject(View.CurrentObject))
            {
                var itemVenta = ((VentaDetalle)View.CurrentObject);
                if (itemVenta.Producto.Categoria.Clasificacion >= Producto.Module.BusinessObjects.EClasificacion.Servicios ||
                    itemVenta.Producto.Categoria.MetodoCosteo == Producto.Module.BusinessObjects.EMetodoCosteoInventario.Ninguno ||
                    itemVenta.Bodega == null)
                    return;
                decimal cantVta;
                if (itemVenta.Producto.Categoria.MetodoCosteo == Producto.Module.BusinessObjects.EMetodoCosteoInventario.PEPS ||
                    itemVenta.Producto.Categoria.MetodoCosteo == Producto.Module.BusinessObjects.EMetodoCosteoInventario.UEPS)
                {
                    if (itemVenta.Lote == null)
                        return;
                    cantVta = itemVenta.Venta.Detalles.Where(x => x.Bodega == itemVenta.Bodega && x.Producto == itemVenta.Producto && x.Lote == itemVenta.Lote).Sum(x => x.Cantidad);
                    if (cantVta > itemVenta.Lote.Existencia)
                        MostrarError($"Una validación no se cumple, la cantidad a vender {cantVta} debe ser menor o igual a la existencia del lote {itemVenta.Lote.Existencia}");
                }
                else
                {
                    cantVta = itemVenta.Venta.Detalles.Where(x => x.Bodega == itemVenta.Bodega && x.Producto == itemVenta.Producto).Sum(x => x.Cantidad);
                    decimal existencia = Convert.ToDecimal(ObjectSpace.Evaluate(typeof(Inventario.Module.BusinessObjects.Inventario),
                        CriteriaOperator.Parse("Sum(Iif([TipoMovimiento.Operacion] In (0, 1), [Cantidad], -[Cantidad]))"),
                        CriteriaOperator.Parse("[Bodega.Oid] == ? && [Producto.Oid] == ?", itemVenta.Bodega.Oid, itemVenta.Producto.Oid)));
                    if (cantVta > existencia)
                        MostrarError($"Una validación no se cumple, La cantidad a vender {cantVta} debe ser menor o igual al disponible en inventario {existencia}");
                }
            }
        }

        //void NewController_ObjectCreated(object sender, ObjectCreatedEventArgs e)
        //{
        //    if (e.CreatedObject is VentaDetalle)
        //    {
        //        var obj = e.CreatedObject as VentaDetalle;
        //        if (obj.Venta == null || obj.Venta.Cliente == null)
        //        {
        //            MostrarError($"Revise que ha seleccionado un cliente, antes de ingresar el detalle del documento");
        //        }
        //    }
        //}

        private void NewController_ObjectCreating(object sender, ObjectCreatingEventArgs e)
        {
            if (e.NewObject != null && ((VentaDetalle)e.NewObject).Venta.Cliente == null)
            {
                Application.ShowViewStrategy.ShowMessage(@"Debe seleccionar en el encabezado del documento de venta al cliente", InformationType.Error);
                e.Cancel = true;
                return;
            }
            DoValidateVenta(e.ObjectSpace);
        }

        private void DoValidateVenta(IObjectSpace os)
        {
            if (os.ModifiedObjects.Count == 1 && os.IsNewObject(os.ModifiedObjects[0]))
            {
                try
                {
                    IRuleSet ruleSet = Validator.GetService(Application.ServiceProvider);
                    ruleSet?.Validate(os, os.ModifiedObjects[0], ContextIdentifier.Save);
                }
                catch
                {
                    // evaluar que esto funcione bien en plataforma web
                    MostrarError("Debe ingresar los datos requeridos del encabezado del documento, antes de ingresar el detalle");
                }
            }
        }
    }
}
