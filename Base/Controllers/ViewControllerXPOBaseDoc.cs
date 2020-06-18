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
using SBT.Apps.Base.Module.BusinessObjects;

namespace SBT.Apps.Base.Module.Controllers
{
    /// <summary>
    /// Para filtrar los datos por empresa, para los BO que hereden de XPOBaseDoc, porque estos contienen la propiedad empresa
    /// </summary>
    /// <remarks>
    /// *** L E E M E ***
    /// Los BO heredadoes de XPOBaseDoc muy probablemnte todos incorporen su propio viewcontroller por los procesos de negocio
    /// particulares de cada uno, por lo tanto es posible que este viewcontroller, no sea necesario y debe removerse
    /// </remarks>
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ViewControllerXPOBaseDoc : ViewController
    {
        public ViewControllerXPOBaseDoc()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();          
            if ((View.GetType().Name == "ListView") && (((ListView)View).ObjectTypeInfo.FindMember("Empresa") != null)) 
                ((ListView)View).CollectionSource.Criteria["Empresa Actual"] = CriteriaOperator.Parse("Empresa = ?", ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid);
            // otra manera es la siguiente
            //((ListView)View).CollectionSource.Criteria["Filtro Defecto"] = CriteriaOperator.Parse("Empresa.Oid = EmpresaActualOid()"); 
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
