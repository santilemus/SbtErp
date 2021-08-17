using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Scheduler.Web;
using DevExpress.Web.ASPxScheduler;
using DevExpress.XtraScheduler;
using SBT.Apps.Medico.Expediente.Module.BusinessObjects;
using System;

namespace SBT.Apps.Medico.Module.Web.Controllers
{
    /// <summary>
    /// View controller del tipo OjectViewController y que aplica al control de tipo XtraScheduler del BO cita
    /// El proposito es tener acceso al control para syncronizar la informacion de las citas con outlook
    /// </summary>
    public class vcCitaWeb : ObjectViewController<ListView, Cita>
    {
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            ASPxSchedulerListEditor listEditor = View.Editor as ASPxSchedulerListEditor;
            if (listEditor != null)
            {
                ASPxScheduler scheduler = listEditor.SchedulerControl;
                scheduler.Views.DayView.VisibleTime = new TimeOfDayInterval(new TimeSpan(8, 0, 0), new TimeSpan(17, 0, 0));
            }
        }
    }
}
