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
using DevExpress.ExpressApp.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;

namespace SBT.Apps.Base.Module.Controllers
{
    /// <summary>
    /// Controller, que corresponde al BO de informacion de auditoria. 
    /// El propósito es poner de solo lectura el BO
    /// </summary>
    /// <typeparam name="AuditoObjectInfo"></typeparam>
    public class vcAuditObjectInfo<AuditoObjectInfo>: ViewController<DetailView>
    {
        public vcAuditObjectInfo(): base()
        {
            TargetObjectType = typeof(AuditObjectInfo);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            View.AllowEdit["ReadOnly"] = true;
            View.AllowDelete["Delete"] = false;
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
    }
}
