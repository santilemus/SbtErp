using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Scheduler.Win;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Exchange;
using SBT.Apps.Medico.Expediente.Module.BusinessObjects;
using System;
using System.Runtime.Versioning;

namespace SBT.Apps.Medico.Module.Win.Controllers
{
    /// <summary>
    /// View controller del tipo OjectViewController y que aplica al control de tipo XtraScheduler del BO cita
    /// El proposito es tener acceso al control para syncronizar la informacion de las citas con outlook
    /// </summary>
    /// <remarks>
    /// Mas info en
    /// https://docs.devexpress.com/WindowsForms/3937/controls-and-libraries/scheduler/import-and-export/synchronization-with-microsoft-outlook
    /// https://github.com/DevExpress-Examples/how-to-synchronize-appointments-with-outlook-in-a-multi-user-application-e699 // revisar este 
    /// https://github.com/DevExpress-Examples/synchronization-with-ms-outlook-a-demonstration-example-t158895
    /// </remarks>
    [SupportedOSPlatform("windows")]
    public class vcCitaWin : ObjectViewController<DevExpress.ExpressApp.ListView, Cita>
    {
        private const string OutlookEntryIDFieldName = "OutlookID"; // revisarlo despues porque sera un campo del BO y aun no se tiene
        private SimpleAction saExportToOutlook;
        private SimpleAction saImportFromOutlook;
        private SchedulerListEditor listEditor;

        public vcCitaWin() : base()
        {
            saExportToOutlook = new SimpleAction(this, "saExportToOutlook", DevExpress.Persistent.Base.PredefinedCategory.RecordEdit);
            saExportToOutlook.Caption = "Exportar a Outlook";
            saExportToOutlook.ToolTip = "Clic para enviar las citas con Outlook";
            saExportToOutlook.ConfirmationMessage = "Confirmar envío a Outlook?";
            saExportToOutlook.ImageName = "book";
            saImportFromOutlook = new SimpleAction(this, "saImportFromOutlook", DevExpress.Persistent.Base.PredefinedCategory.RecordEdit);
            saImportFromOutlook.Caption = "Importar de Outlook";
            saImportFromOutlook.ToolTip = "Clic para recibir las citas con Outlook";
            saImportFromOutlook.ConfirmationMessage = "Confirmar recibo de Outlook?";
            saImportFromOutlook.ImageName = "book";
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            listEditor = View.Editor as SchedulerListEditor;
            if (listEditor != null)
            {
                SchedulerControl scheduler = listEditor.SchedulerControl;
                scheduler.Views.DayView.VisibleTime = new TimeOfDayInterval(new TimeSpan(8, 0, 0), new TimeSpan(17, 0, 0));
            }
            InitAppointments();
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            saExportToOutlook.Execute += ExportToOutlook_Execute;
            saImportFromOutlook.Execute += ImportFromOutlook_Execute;
        }

        protected override void OnDeactivated()
        {
            saExportToOutlook.Execute -= ExportToOutlook_Execute;
            saImportFromOutlook.Execute -= ImportFromOutlook_Execute;
            base.OnDeactivated();
        }

        protected override void Dispose(bool disposing)
        {
            saExportToOutlook.Dispose();
            saImportFromOutlook.Dispose();
            base.Dispose(disposing);
        }


        private void ExportToOutlook_Execute(Object sender, SimpleActionExecuteEventArgs e)
        {
            // AQUI FALTA FILTRAR PARA QUE SOLO SE EXPORTEN LAS CITAS DEL MEDICO DE LA SESION
            // SE TIENE QUE HACER AL FILTRAR SOLO LAS CITAS CON ELEMENTOS EN LA COLECCION Medicos del BO Cita (CitaBase)
            // Y QUE CORRESPONDEN AL MEDICO DE LA SESION
            AppointmentExportSynchronizer synchronizer = listEditor.StorageBase.CreateOutlookExportSynchronizer();
            // Specify the field that contains appointment identifier used by a third-party application.
            synchronizer.ForeignIdFieldName = OutlookEntryIDFieldName;
            // The AppointmentSynchronizing event allows you to control the operation for an individual appointment.
            synchronizer.AppointmentSynchronizing += new AppointmentSynchronizingEventHandler(exportSynchronizer_AppointmentSynchronizing);
            // The AppointmentSynchronized event indicates that the operation for a particular appointment is complete.
            synchronizer.AppointmentSynchronized += new AppointmentSynchronizedEventHandler(exportSynchronizer_AppointmentSynchronized);
            // Specify MS Outlook calendar path.

            //((ISupportCalendarFolders)synchronizer).CalendarFolderName = comboBoxEdit1.EditValue.ToString();  SELM QUITAR EL COMENTARIO Y REVISAR

            // Perform the operation.
            synchronizer.Synchronize();
        }

        private void ImportFromOutlook_Execute(Object sender, SimpleActionExecuteEventArgs e)
        {
            AppointmentImportSynchronizer synchronizer = listEditor.StorageBase.CreateOutlookImportSynchronizer();
            // Specify the field that contains appointment identifier used by a third-party application.
            synchronizer.ForeignIdFieldName = OutlookEntryIDFieldName;
            // The AppointmentSynchronizing event allows you to control the operation for an individual appointment.
            synchronizer.AppointmentSynchronizing += new AppointmentSynchronizingEventHandler(importSynchronizer_AppointmentSynchronizing);
            // The AppointmentSynchronized event indicates that the operation for a particular appointment is complete.
            synchronizer.AppointmentSynchronized += new AppointmentSynchronizedEventHandler(importSynchronizer_AppointmentSynchronized);
            // Specify MS Outlook calendar path.
            //((ISupportCalendarFolders)synchronizer).CalendarFolderName = comboBoxEdit1.EditValue.ToString(); SELM QUITAR EL COMENTARIO Y REVISAR
            // Perform the operation.
            synchronizer.Synchronize();
        }

        void importSynchronizer_AppointmentSynchronized(object sender, AppointmentSynchronizedEventArgs e)
        {
            // Your code here.  
        }

        void importSynchronizer_AppointmentSynchronizing(object sender, AppointmentSynchronizingEventArgs e)
        {
            AnalyzeAndHandleCurrentOperation(e as DevExpress.XtraScheduler.Outlook.OutlookAppointmentSynchronizingEventArgs, true);
        }

        void exportSynchronizer_AppointmentSynchronizing(object sender, AppointmentSynchronizingEventArgs e)
        {
            AnalyzeAndHandleCurrentOperation(e as DevExpress.XtraScheduler.Outlook.OutlookAppointmentSynchronizingEventArgs, false);
        }

        void exportSynchronizer_AppointmentSynchronized(object sender, AppointmentSynchronizedEventArgs e)
        {
            // Your code here.  
        }

        void AnalyzeAndHandleCurrentOperation(DevExpress.XtraScheduler.Outlook.OutlookAppointmentSynchronizingEventArgs eventArgs, bool toScheduler)
        {
            string logInfo = "";
            string appointmentInfoScheduler = "";
            string appointmentInfoOutlook = "";

            if (eventArgs.Appointment != null)
                appointmentInfoScheduler = String.Format("Subject: {0}, Start: {1}, End: {2}, ForeignID: {3}",
                    eventArgs.Appointment.Subject, eventArgs.Appointment.Start.ToString(), eventArgs.Appointment.End.ToString(),
                    eventArgs.Appointment.GetValue(listEditor.StorageBase, OutlookEntryIDFieldName));
            else
                appointmentInfoScheduler = "<No information>";

            if (eventArgs.OutlookAppointment != null)
                appointmentInfoOutlook = String.Format("Subject: {0}, Start: {1}, End: {2}, ID:{3}",
                    eventArgs.OutlookAppointment.Subject, eventArgs.OutlookAppointment.Start.ToString(), eventArgs.OutlookAppointment.End.ToString(),
                    eventArgs.OutlookAppointment.EntryID);
            else
                appointmentInfoOutlook = "<No information>";

            switch (eventArgs.Operation)
            {
                case SynchronizeOperation.Create:
                    logInfo += (toScheduler ? "->Scheduler (Creating)" : "->Outlook (Creating)");
                    logInfo += "\r\n";
                    logInfo += String.Format("({0}) appointment - {1}", "Scheduler", appointmentInfoScheduler);
                    logInfo += "\r\n";
                    logInfo += String.Format("({0}) appointment - {1}", "Outlook   ", appointmentInfoOutlook);
                    // Decide whether to perform an operation.
                    /*  COMENTARIO POR SELM
                    if ((toScheduler && !SyncronizationOptionForm.AllowCreateAppointmentInScheduler) || (!toScheduler && !SyncronizationOptionForm.AllowCreateAppointmentInOutlook))
                    {
                        eventArgs.Cancel = true;
                        logInfo += " (Operation canceled)";
                    }
                       FIN COMENTARIO SELM
                    */
                    break;
                case SynchronizeOperation.Delete:
                    logInfo += (toScheduler ? "->Scheduler (Deleting)" : "->Outlook (Deleting)");
                    logInfo += "\r\n";
                    logInfo += String.Format("({0}) appointment - {1}", "Scheduler", appointmentInfoScheduler);
                    logInfo += "\r\n";
                    logInfo += String.Format("({0}) appointment - {1}", "Outlook    ", appointmentInfoOutlook);
                    // Decide whether to perform an operation.
                    /* COMENTARIO SELM
                    if ((toScheduler && !SyncronizationOptionForm.AllowRemoveAppointmentInScheduler) || (!toScheduler && !SyncronizationOptionForm.AllowRemoveAppointmentInOutlook))
                    {
                        eventArgs.Cancel = true;
                        logInfo += " (Operation canceled)";
                    } FIN COMENTARIO SELM
                    */
                    break;
                case SynchronizeOperation.Replace:
                    logInfo += (toScheduler ? "->Scheduler (Updating)" : "->Outlook (Updating)");
                    logInfo += "\r\n";
                    logInfo += String.Format("({0}) appointment - {1}", "Scheduler", appointmentInfoScheduler);
                    logInfo += "\r\n";
                    logInfo += String.Format("({0}) appointment - {1}", "Outlook   ", appointmentInfoOutlook);
                    // Decide whether to perform an operation.
                    /* COMENTARIO SELM
                    if ((toScheduler && !SyncronizationOptionForm.AllowUpdateAppointmentInScheduler) || (!toScheduler && !SyncronizationOptionForm.AllowUpdateAppointmentInOutlook))
                    {
                        eventArgs.Cancel = true;
                        logInfo += " (Operation canceled)";
                    } FIN COMENTARIO SELM
                    */
                    break;
                default:
                    break;
            }
            //memoEdit1.Text += logInfo + "\r\n";
        }

        private void InitAppointments()
        {
            /// completar y adaptar al BO Cita. Ademas revisar SystemInformation.es necesario
            /// en caso que no funcione probar sin el mapeo porque el BO esta vinculado al control schedule y puede
            /// que no sea necesario
            if (listEditor == null || listEditor.Appointments == null)
                return;
            AppointmentMappingInfo aptmappings = listEditor.Appointments.Mappings;
            aptmappings.Start = "StartOn";
            aptmappings.End = "EndOn";
            aptmappings.Subject = "Subject";
            aptmappings.AllDay = "AllDay";
            aptmappings.Description = "Description";
            aptmappings.Label = "Label";
            aptmappings.Location = "Location";
            aptmappings.RecurrenceInfo = "RecurrencePattern";
            aptmappings.ReminderInfo = "ReminderInfoXml";
            aptmappings.Status = "Status";
            aptmappings.Type = "Type";
            aptmappings.ResourceId = "ResourceId";

            ResourceMappingInfo resmappings = listEditor.Resources.Mappings;
            resmappings.Id = "Oid";
            resmappings.Caption = "Caption";
            resmappings.Color = "Color";
        }
    }
}
