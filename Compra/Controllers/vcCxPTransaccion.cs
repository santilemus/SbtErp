using System;
using System.Linq;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Actions;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Base.Module.Controllers;
using SBT.Apps.CxP.Module.BusinessObjects;
using DevExpress.ExpressApp;
using System.ComponentModel;

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
                return;

            foreach (object item in items)
            {
                if (item.GetType() != typeof(SBT.Apps.CxP.Module.BusinessObjects.CxPTransaccion))
                {
                    CxPTransaccion transaccion = (CxPTransaccion)item;
                    if (transaccion.Factura != null || transaccion.Factura.Estado == EEstadoFactura.Debe)
                    {
                        ;
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                            
                }                   
            }
        }
    }
}
