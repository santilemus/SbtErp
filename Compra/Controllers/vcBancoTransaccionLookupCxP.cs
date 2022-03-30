using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using SBT.Apps.Banco.Module.BusinessObjects;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Base.Module.Controllers;
using SBT.Apps.Compra.Module.BusinessObjects;
using SBT.Apps.CxP.Module.BusinessObjects;

namespace SBT.Apps.Contabilidad.Module.Controllers
{
    /// <summary>
    /// View Controller para la vista con id = "BancoTransaccion_LookupListView_CxP" del BO BancoTransaccion.
    /// El propósito es implementar personalizaciones de la integracion CxP y Bancos. Cuando se agrega una transaccion
    /// de cuentas por cobrar que corresponde al pago de una compra, en la propiedad tipo lookup de la transacción
    /// de bancos, debe inicializarse con datos de la compra para evitar que el usuario los digite nuevamente
    /// </summary>
    public class vcBancoTransaccionLookupCxP: ViewControllerBase
    {
        private NewObjectViewController newObjectController;
        public vcBancoTransaccionLookupCxP(): base()
        {

        }

        protected override void DoInitializeComponent()
        {
            base.DoInitializeComponent();
            TargetObjectType = typeof(SBT.Apps.Banco.Module.BusinessObjects.BancoTransaccion);
            TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            TargetViewId = "BancoTransaccion_LookupListView_CxP";
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            newObjectController = Frame.GetController<NewObjectViewController>();
            if (newObjectController != null)
            {
                newObjectController.ObjectCreated += newObjectController_ObjectCreated;
            }
        }

        protected override void OnDeactivated()
        {
            if (newObjectController != null)
            {
                newObjectController.ObjectCreated -= newObjectController_ObjectCreated;
            }
            base.OnDeactivated();
        }

        private void newObjectController_ObjectCreated(object sender, ObjectCreatedEventArgs e)
        {
            NestedFrame nestedFrame = Frame as NestedFrame;
            if (nestedFrame != null)
            {
                BancoTransaccion createdItem = e.CreatedObject as BancoTransaccion;
                if (createdItem != null)
                {
                    var parent = ((NestedFrame)Frame).ViewItem.CurrentObject as CxPTransaccion;
                    if (parent != null)
                    {
                        createdItem.Fecha = parent.Fecha;
                        int OidProveedor = parent.Factura.Proveedor.Oid;
                        var prov = e.ObjectSpace.GetObjectByKey<Tercero.Module.BusinessObjects.Tercero>(parent.Factura.Proveedor.Oid);
                        createdItem.Beneficiario = prov.Nombre ?? string.Empty;
                        int oidBancoTipoTransaccion = 0;
                        if (parent.Tipo.Oid == 7)          // es cheque
                            oidBancoTipoTransaccion = 11; // cheque de pago a proveedor
                        if (parent.Tipo.Oid == 10 || parent.Tipo.Oid == 11) // es transferencia bancaria o remesa a cuenta
                            oidBancoTipoTransaccion = 20;  // nota de cargo transferencia tercero
                        else if (parent.Tipo.Oid == 12)    // es pago electronico
                            oidBancoTipoTransaccion = 16;  // nota de cargo pago a proveedor
                        if (oidBancoTipoTransaccion > 0)
                        {
                            BancoTipoTransaccion tipoTrans = ObjectSpace.GetObjectByKey<BancoTipoTransaccion>(oidBancoTipoTransaccion);
                            createdItem.Clasificacion = tipoTrans;
                        }
                        if (parent.Moneda.Codigo != createdItem.Moneda.Codigo)
                            createdItem.Moneda = createdItem.Session.GetLoadedObjectByKey<Moneda>(parent.Moneda.Codigo);
                        createdItem.Monto = parent.Monto;
                        createdItem.Concepto = $"Pago de Factura {parent.Factura.Numero} del {parent.Factura.Fecha}";
                    }
                }
            }
        }

    }
}
