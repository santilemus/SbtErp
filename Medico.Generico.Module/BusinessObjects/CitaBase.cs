using System;
using System.Linq;
using System.Text;
using System.Xml;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo.Metadata;
using DevExpress.Persistent.Base.General;
using DevExpress.XtraScheduler.Xml;
using DevExpress.ExpressApp.SystemModule.Notifications;
using DevExpress.ExpressApp.Filtering;
using DevExpress.XtraScheduler.Compatibility;
using SBT.Apps.Base.Module.BusinessObjects;


namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    [Persistent("Cita"), NavigationItem(false), DefaultProperty(nameof(Subject))]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CitaBase : XPObjectBaseBO, IEvent, IRecurrentEvent, IReminderEvent
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public CitaBase(Session session)
            : base(session)
        {
            Medicos.ListChanged += new ListChangedEventHandler(Medicos_ListChanged);
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            StartOn = DateTime.Now;
            EndOn = StartOn.AddHours(1);

            var ci = ((Usuario)SecuritySystem.CurrentUser).ClassInfo;
            if (ci.FindMember("Empleado") == null)
                return;   // ver que hacemos porque hay un problema no existe la propiedad empleado
            Empleado.Module.BusinessObjects.Empleado empleado = (((Usuario)SecuritySystem.CurrentUser).GetMemberValue("Empleado") as Empleado.Module.BusinessObjects.Empleado);
            if (empleado == null)
                return;   // la propiedad existe, pero no tiene valor
            var medico = Session.GetObjectByKey<Medico>(empleado.Oid);
            if (medico != null)
                Medicos.Add(medico);

            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();
            if (Medicos.IsLoaded && !Session.IsNewObject(this))
            {
                Medicos.Reload();
            }
        }

        //private EventImpl appointmentImpl = new EventImpl();

        bool isPostponed;
        DateTime? alarmTime;
        TimeSpan? remindIn;
        string reminderInfoXml;
        string recurrenceInfoXml;
        [Size(SizeAttribute.Unlimited), Persistent("ResourceIds")]
        string medicoIds;
        int type;
        int status;
        int label;
        string location;
        bool allDay;
        DateTime endOn;
        DateTime startOn;
        string description;
        string subject;
        [Persistent(nameof(RecurrencePattern))]
        CitaBase recurrencePattern;

        #region Miembros IEvent

        [Size(250), DbType("varchar(250)"), XafDisplayName("Nombre")]
        public string Subject
        {
            get => subject;
            set => SetPropertyValue(nameof(Subject), ref subject, value);
        }


        [Size(SizeAttribute.Unlimited), DbType("varchar(max)"), XafDisplayName("Descripción")]
        public string Description
        {
            get => description;
            set => SetPropertyValue(nameof(Description), ref description, value);
        }

        [XafDisplayName("Hora Inicio"), Indexed]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime StartOn
        {
            get => startOn;
            set => SetPropertyValue(nameof(StartOn), ref startOn, value);
        }

        [XafDisplayName("Hora Fin"), Indexed]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime EndOn
        {
            get => endOn;
            set => SetPropertyValue(nameof(EndOn), ref endOn, value);
        }

        [XafDisplayName("Todo el Día"), DbType("bit")]
        [ImmediatePostData]
        public bool AllDay
        {
            get => allDay;
            set => SetPropertyValue(nameof(AllDay), ref allDay, value);
        }

        [Size(100), DbType("varchar(100)"), XafDisplayName("Ubicación")]
        public string Location
        {
            get => location;
            set => SetPropertyValue(nameof(Location), ref location, value);
        }

        [XafDisplayName("Etiqueta")]
        public int Label
        {
            get => label;
            set => SetPropertyValue(nameof(Label), ref label, value);
        }

        [XafDisplayName("Estado")]
        public int Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }

        [XafDisplayName("Tipo"), Browsable(false)]
        public int Type
        {
            get => type;
            set => SetPropertyValue(nameof(Type), ref type, value);
        }

        [XafDisplayName("Id Recurso"), PersistentAlias(nameof(medicoIds)), Browsable(false)]
        public string ResourceId
        {
            get
            {
                if (medicoIds == null)
                    UpdateMedicoIds();
                return medicoIds;
            }
            set
            {
                if ((!IsLoading && !IsSaving) && string.Compare(medicoIds, value, StringComparison.Ordinal) != 0 && value != null)
                {
                    medicoIds = value;
                    UpdateMedicos();
                }
            }
        }

        [NonPersistent, Browsable(false)]
        public object AppointmentId
        {
            get => Oid;
        }

        [NonPersistent, Browsable(false)]
        [RuleFromBoolProperty("CitaBase_EsIntervaloValido", DefaultContexts.Save, 
            "La fecha y hora de inicio debe ser menor que la fecha y hora de finalización", 
            SkipNullOrEmptyValues = false, UsedProperties = "StartOn, EndOn")]
        public bool EsIntervaloValido { get { return StartOn <= EndOn; } }

        #endregion

        #region Miembros IRecurrentEvent
        [Browsable(false), PersistentAlias(nameof(recurrencePattern)), XafDisplayName("Patrón de Repetición")]
        public IRecurrentEvent RecurrencePattern
        {
            get => recurrencePattern;
            set => SetPropertyValue(nameof(RecurrencePattern), ref recurrencePattern, value as CitaBase);
        }

        [Size(SizeAttribute.Unlimited), Persistent("RecurrenceInfoXml"), XafDisplayName("Repetición")]
        public string RecurrenceInfoXml
        {
            get => recurrenceInfoXml;
            set => SetPropertyValue(nameof(RecurrenceInfoXml), ref recurrenceInfoXml, value);
        }

        #endregion

        #region Miembros IReminderEvent

        [Size(200), XafDisplayName("Recordatorio")]
        public string ReminderInfoXml
        {
            get => reminderInfoXml;
            set
            {
                SetPropertyValue(nameof(ReminderInfoXml), ref reminderInfoXml, value);
                if (!IsLoading && !IsSaving)
                {
                    UpdateAlarmTime();
                }
            }
        }

        [XafDisplayName("Recordar en"), Browsable(false)]
        public TimeSpan? RemindIn
        {
            get => remindIn;
            set => SetPropertyValue(nameof(RemindIn), ref remindIn, value);
        }

        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [XafDisplayName("Hora de Alarma")]
        public DateTime? AlarmTime
        {
            get => alarmTime;
            set
            {
                SetPropertyValue(nameof(AlarmTime), ref alarmTime, value);
                if (!IsLoading)
                {
                    if (value == null)
                    {
                        remindIn = null;
                        IsPostponed = false;
                    }
                    UpdateRemindersInfoXml(true);
                }
            }
        }

        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [XafDisplayName("Esta Pospuesto")]
        public bool IsPostponed
        {
            get => isPostponed;
            set => SetPropertyValue(nameof(IsPostponed), ref isPostponed, value);
        }

        [Browsable(false), NonPersistent]
        public object UniqueId
        {
            get { return Oid; }
        }

        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [NonPersistent, XafDisplayName("Mensaje Notificación")]
        public string NotificationMessage
        {
            get { return Subject; }
        }


        #endregion
                        
        [Association("Medico-Citas", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<Medico> Medicos
        {
            get { return GetCollection<Medico>(nameof(Medicos)); }
        }

        //Medico medico;
        //[Association("Medico-Citas"), VisibleInLookupListView(false), RuleRequiredField("AgendaBase.Medico_Requerido", "Save")]
        //public Medico Medico
        //{
        //    get => medico;
        //    set => SetPropertyValue(nameof(Medico), ref medico, value);
        //}


        #region Metodos
        protected override XPCollection<T> CreateCollection<T>(XPMemberInfo property)
        {
            XPCollection<T> result = base.CreateCollection<T>(property);
            if (string.Compare(property.Name, nameof(Medicos), StringComparison.Ordinal) == 0)
            {
                result.ListChanged += Medicos_ListChanged;
            }
            return result;
        }

        public void UpdateMedicoIds()
        {
            medicoIds = string.Empty;
            foreach (Medico activityUser in Medicos)
            {
                medicoIds += $@"<ResourceId Type=""{activityUser.Id.GetType().FullName}"" Value=""{activityUser.Id}"" />\r\n";
            }
            medicoIds = $"<ResourceIds>\r\n{medicoIds}</ResourceIds>";
        }
        private void UpdateMedicos()
        {
            Medicos.SuspendChangedEvents();
            try
            {
                while (Medicos.Count > 0)
                    Medicos.Remove(Medicos[0]);
                if (!String.IsNullOrEmpty(medicoIds))
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(medicoIds);
                    foreach (XmlNode xmlNode in xmlDocument.DocumentElement.ChildNodes)
                    {
                        Medico activityUser = Session.GetObjectByKey<Medico>(xmlNode.Attributes["Value"].Value);
                        if (activityUser != null)
                            Medicos.Add(activityUser);
                    }
                }
            }
            finally
            {
                Medicos.ResumeChangedEvents();
            }
        }
        private void Medicos_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted)
            {
                UpdateMedicoIds();
                OnChanged(nameof(ResourceId));
            }
        }


        private void UpdateRemindersInfoXml(bool UpdateAlarmTime)
        {
            if (RemindIn.HasValue && AlarmTime.HasValue)
            {
                AppointmentReminderInfo apptReminder = new AppointmentReminderInfo();
                ReminderInfo reminderInfo = new ReminderInfo();
                reminderInfo.TimeBeforeStart = RemindIn.Value;
                if (UpdateAlarmTime)
                {
                    reminderInfo.AlertTime = AlarmTime.Value;
                }
                else
                {
                    reminderInfo.AlertTime = StartOn - RemindIn.Value;
                }
                apptReminder.ReminderInfos.Add(reminderInfo);
                reminderInfoXml = apptReminder.ToXml();
            }
            else
            {
                reminderInfoXml = null;
            }
        }

        private void UpdateAlarmTime()
        {
            if (!string.IsNullOrEmpty(reminderInfoXml))
            {
                AppointmentReminderInfo appointmentReminderInfo = new AppointmentReminderInfo();
                try
                {
                    appointmentReminderInfo.FromXml(reminderInfoXml);
                    alarmTime = appointmentReminderInfo.ReminderInfos[0].AlertTime;
                }
                catch (XmlException e)
                {
                    Tracing.Tracer.LogError(e);
                }
            }
            else
            {
                alarmTime = null;
                remindIn = null;
                IsPostponed = false;
            }
        }

        public event EventHandler<CustomizeNotificationsPostponeTimeListEventArgs> CustomizeReminderTimeLookup;
        private IList<PostponeTime> CreatePostponeTimes()
        {
            IList<PostponeTime> result = PostponeTime.CreateDefaultPostponeTimesList();
            result.Add(new PostponeTime("None", null, "None"));
            result.Add(new PostponeTime("AtStartTime", TimeSpan.Zero, "0 minutes"));
            CustomizeNotificationsPostponeTimeListEventArgs args = new CustomizeNotificationsPostponeTimeListEventArgs(result);
            if (CustomizeReminderTimeLookup != null)
            {
                CustomizeReminderTimeLookup(this, args);
            }
            PostponeTime.SortPostponeTimesList(args.PostponeTimesList);
            return args.PostponeTimesList;
        }

        #endregion
    }
}