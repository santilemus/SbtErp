using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using System;

namespace SBT.Apps.Medico.Expediente.Module.DatabaseUpdate
{
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppUpdatingModuleUpdatertopic
    public class Updater : ModuleUpdater
    {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion)
        {
        }
        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();
            //string name = "MyName";
            //DomainObject1 theObject = ObjectSpace.FindObject<DomainObject1>(CriteriaOperator.Parse("Name=?", name));
            //if(theObject == null) {
            //    theObject = ObjectSpace.CreateObject<DomainObject1>();
            //    theObject.Name = name;
            //}
        }
        public override void UpdateDatabaseBeforeUpdateSchema()
        {
            base.UpdateDatabaseBeforeUpdateSchema();
            //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}
        }
    }
}
