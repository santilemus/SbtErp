using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using SBT.Apps.Base.Module.Controllers;
using SBT.Apps.CxC.Module.BusinessObjects;

namespace SBT.Apps.CxC.Module.Controllers
{
    /// <summary>
    /// Cuenta por Cobrar. View Controller que corresponde a la Cartera de Cuenta por Cobrar
    /// </summary>
    public class vcCartera: ViewControllerBase
    {
        public vcCartera(): base()
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
            TargetObjectType = typeof(SBT.Apps.CxC.Module.BusinessObjects.Cartera);
        }

        protected override void Dispose(bool disposing)
        {
            // destruir los objetos aqui
            base.Dispose(disposing);
        }

    }
}
