using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Linq;

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
    public class ViewControllerXPOBaseDoc : ViewController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public ViewControllerXPOBaseDoc()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            if ((string.Compare(View.GetType().Name, "ListView", StringComparison.Ordinal) == 0) && (((ListView)View).ObjectTypeInfo.FindMember("Empresa") != null))
                ((ListView)View).CollectionSource.Criteria["Empresa Actual"] = CriteriaOperator.Parse("[Empresa.Oid] = ?", ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid);
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

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // 
            // vcFilterDatosEmpresaDeSesion
            // 
            this.TargetObjectType = typeof(SBT.Apps.Base.Module.BusinessObjects.XPOBaseDoc);

        }
    }
}
