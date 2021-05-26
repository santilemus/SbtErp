using System;
using DevExpress.ExpressApp.Actions;
using SBT.Apps.Base.Module.Controllers;


namespace SBT.Apps.Base.Module.Controllers
{
    /// <summary>
    /// View Controller para el BO Catalogo Contable. Como minimo debe existir el metodo OnActivated porque en base.OnActivated
    /// se filtra el catalogo a la empresa de la sesion, lo cual es importante en una implementacion multiempresa.
    /// </summary>
    public class vcCatalogo: ViewControllerBase
    {
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
            TargetObjectType = typeof(SBT.Apps.Contabilidad.BusinessObjects.Catalogo);
        }
    }
}
