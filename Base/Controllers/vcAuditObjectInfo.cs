using DevExpress.ExpressApp;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Linq;

namespace SBT.Apps.Base.Module.Controllers
{
    /// <summary>
    /// Controller, que corresponde al BO de informacion de auditoria. 
    /// El propósito es poner de solo lectura el BO
    /// </summary>
    /// <typeparam name="AuditoObjectInfo"></typeparam>
    public class vcAuditObjectInfo<AuditoObjectInfo> : ViewController<DetailView>
    {
        public vcAuditObjectInfo() : base()
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
