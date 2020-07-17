using System;
using System.Linq;

namespace SBT.Apps.Medico.Module.Web.Controllers
{
    /// <summary>
    /// ViewController
    /// View Controller con la funcionalidad comun en aplicaciones web y que se aplica a BO heredados de XPOBaseDoc
    /// </summary>
    public class ViewControllerXPOBaseDocWeb : ViewControllerBaseWeb
    {
        public ViewControllerXPOBaseDocWeb() : base()
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

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
        }
        protected override void DoInitializeComponent()
        {
            base.DoInitializeComponent();
            this.TargetObjectType = typeof(SBT.Apps.Base.Module.BusinessObjects.XPOBaseDoc);
            this.FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }
    }
}
