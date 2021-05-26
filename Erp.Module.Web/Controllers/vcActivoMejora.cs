﻿using System;
using DevExpress.ExpressApp.Actions;
using SBT.Apps.Base.Module.Controllers;

namespace SBT.Apps.Erp.Module.Web.Controllers
{
    /// <summary>
    /// ViewController que aplica al BO ActivoMejora para la plataforma web
    /// </summary>
    public class vcActivoMejora: ViewControllerBaseWeb
    {
        public vcActivoMejora(): base()
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
            TargetObjectType = typeof(SBT.Apps.Activo.Module.BusinessObjects.ActivoMejora);
            FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }
    }
}
