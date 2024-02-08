using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Base.Module.Controllers;
using System;

namespace SBT.Apps.Facturacion.Module.Controllers
{
    /// <summary>
    /// View Controller para el BO AutorizacionDocumento que contiene las autorizaciones para la emision de documentos
    /// de la autoridad tributaria
    /// </summary>
    public class vcAutorizacionDocumento : ViewControllerBase
    {
        public vcAutorizacionDocumento() : base()
        {

        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // el filtro ya se hace en ViewControllerBase
            if (string.Compare(View.GetType().Name, "ListView", StringComparison.Ordinal) == 0)
                ((ListView)View).CollectionSource.Criteria["Empresa Actual"] = CriteriaOperator.Parse("[Agencia.Empresa.Oid] == ?", ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid);
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }

        protected override void DoInitializeComponent()
        {
            base.DoInitializeComponent();
            TargetObjectType = typeof(SBT.Apps.Facturacion.Module.BusinessObjects.AutorizacionDocumento);
        }
    }
}
