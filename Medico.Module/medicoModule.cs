using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using SBT.Apps.Base.Module.BusinessObjects;
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
