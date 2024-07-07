using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Notifications;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Empleado.Module;
using SBT.Apps.Medico.Expediente.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace SBT.Apps.Medico.Module
{
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppModuleBasetopic.
    public sealed partial class medicoModule : ModuleBase
    {
        public medicoModule()
        {
            InitializeComponent();
            BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB)
        {
            ModuleUpdater updater = new SBT.Apps.Medico.Module.DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }
        public override void Setup(XafApplication application)
        {
            base.Setup(application);
            // Manage various aspects of the application UI and behavior at the module level.
            application.CreateCustomLogonWindowObjectSpace += application_CreateCustomLogonWindowObjectSpace;
            application.ObjectSpaceCreated += Application_ObjectSpaceCreated;

            // para redirigir las notificaciones de consultas al profesional de la salud asignado
            application.LoggedOn += Application_LoggedOn;
        }

        /// <summary>
        /// Evento que se dispara despues de autenticarse y el objeto SecuritySystem es inicializado.
        /// </summary>
        /// <remarks>
        /// Para mostrar la notificación al profesional de la salud que corresponde
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_LoggedOn(object sender, LogonEventArgs e)
        {
            NotificationsModule notificationsModule = Application.Modules.FindModule<NotificationsModule>();
            DefaultNotificationsProvider notificationsProvider = notificationsModule.DefaultNotificationsProvider;
            notificationsProvider.CustomizeNotificationCollectionCriteria += NotificationsProvider_CustomizeNotificationCollectionCriteria;

        }

        /// <summary>
        /// Para filtrar las notificaciones que corresponden solo al profesional de la salud autenticado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotificationsProvider_CustomizeNotificationCollectionCriteria(object sender, DevExpress.Persistent.Base.General.CustomizeCollectionCriteriaEventArgs e)
        {
            if (e.Type == typeof(ConsultaNutricion))
            {
                e.Criteria = CriteriaOperator.FromLambda<ConsultaNutricion>(x => (x.Asignado == null || x.Asignado.Oid == (Guid)CurrentUserIdOperator.CurrentUserId()) && 
                x.Estado == EEstadoConsulta.Espera);
            }
            if (e.Type == typeof(Consulta))
            {
                e.Criteria = CriteriaOperator.FromLambda<Consulta>(x => (x.Asignado == null || x.Asignado.Oid == (Guid)CurrentUserIdOperator.CurrentUserId()) &&
                x.Estado == EEstadoConsulta.Espera);
            }
        }

        private void Application_ObjectSpaceCreated(object sender, ObjectSpaceCreatedEventArgs e)
        {
            if (e.ObjectSpace is CompositeObjectSpace compositeObjectSpace)
            {
                if (compositeObjectSpace.Owner is not CompositeObjectSpace)
                {
                    compositeObjectSpace.PopulateAdditionalObjectSpaces((XafApplication)sender);
                }
            }
        }

        public override void CustomizeTypesInfo(ITypesInfo typesInfo)
        {
            base.CustomizeTypesInfo(typesInfo);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
        }
        void application_CreateCustomLogonWindowObjectSpace(object sender, CreateCustomLogonWindowObjectSpaceEventArgs e)
        {
            IObjectSpace objectSpace = ((XafApplication)sender).CreateObjectSpace(typeof(CustomLogonParameters));
            //((SBT.Apps.Base.Module.BusinessObjects.CustomLogonParameters)e.LogonParameters).ObjectSpace = objectSpace;
            e.ObjectSpace = objectSpace;
        }
    }
}
