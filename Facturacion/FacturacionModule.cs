using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using System;
using System.Collections.Generic;

namespace SBT.Apps.Facturacion.Module
{
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ModuleBase.
    public sealed partial class FacturacionModule : ModuleBase
    {
        public FacturacionModule()
        {
            InitializeComponent();
            BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
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

            //string [] NombreBOs = typesInfo.PersistentTypes.Where(item => item.FullName.Contains(this.AssemblyName)).Select(p => p.FullName).ToArray();

            // creamos una lista de columnas (tiene dos propiedades: el nombre del campo y el valor del atributo index)
            //List <DBColumn> columnas = new List<DBColumn>();
            //foreach(string bo in NombreBOs)
            //{
            //    columnas.Clear();
            //    ITypeInfo ITipo = typesInfo.FindTypeInfo(bo);
            //    foreach (IMemberInfo IMember in ITipo.Members)
            //    {
            //        IndexAttribute att = IMember.FindAttribute<DevExpress.Persistent.Base.IndexAttribute>();
            //        columnas.Add(new DBColumn(att != null ? att.Index : 0, IMember));
            //    }
            //    List<DBColumn> SortColumns = columnas.OrderBy(DBColumn => DBColumn.Index).ToList<DBColumn>();
            //    foreach (IMemberInfo IMember in ITipo.Members)
            //    {
            //    }
            //}
        }

        //protected override IEnumerable<Type> GetDeclaredExportedTypes()
        //{
        //    return base.GetDeclaredExportedTypes();
        //}
    }
}
