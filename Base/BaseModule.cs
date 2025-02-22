using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using System;
using System.Collections.Generic;

namespace SBT.Apps.Base.Module
{
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppModuleBasetopic.
    public sealed partial class BaseModule : ModuleBase
    {
        public BaseModule()
        {
            InitializeComponent();
            BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
            /// se registran las funciones personalizadas que se han definido en la app
            CriteriaOperator.RegisterCustomFunction(new EmpresaActualOidFunction());
            //EmpresaActualOidFunction.Register();
            CriteriaOperator.RegisterCustomFunction(new AgenciaActualOidFunction());
            //AgenciaActualOidFunction.Register();
            CriteriaOperator.RegisterCustomFunction(new PeriodoActualNumeroFunction());
            //PeriodoActualNumeroFunction.Register();
            CriteriaOperator.RegisterCustomFunction(new NumberToLetterFunction());
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB)
        {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }
        public override void Setup(XafApplication application)
        {
            base.Setup(application);
            // Manage various aspects of the application UI and behavior at the module level.
        }
        public override void CustomizeTypesInfo(ITypesInfo typesInfo)
        {
            base.CustomizeTypesInfo(typesInfo);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
        }
    }
}
