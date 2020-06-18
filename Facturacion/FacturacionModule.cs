using System;
using System.Text;
using System.Linq;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.DomainLogics;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;

namespace SBT.Apps.Facturacion.Module
{
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ModuleBase.
    public sealed partial class FacturacionModule : ModuleBase {
        public FacturacionModule() {
            InitializeComponent();
			BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }
        public override void Setup(XafApplication application) {
            base.Setup(application);
            // Manage various aspects of the application UI and behavior at the module level.
        }
        public override void CustomizeTypesInfo(ITypesInfo typesInfo)
        {
            base.CustomizeTypesInfo(typesInfo);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
            // creamos una lista de columnas (tiene dos propiedades: el nombre del campo y el valor del atributo index)
            List<DBColumn> columnas = new List<DBColumn>();
            int x = 0;
            XPDictionary dictionary = DevExpress.ExpressApp.Xpo.XpoTypesInfoHelper.GetXpoTypeInfoSource().XPDictionary;
            dictionary.GetClassInfo(typeof(DevExpress.ExpressApp.Xpo.Updating.ModuleInfo));
            foreach (XPClassInfo ci in dictionary.Classes)
            {
                // limpiamos antes de iterar por cada propiedad persistente de la clase (XPObject o similar)
                columnas.Clear();
                foreach (XPMemberInfo mi in ci.PersistentProperties)
                {
                    Attribute att = mi.FindAttributeInfo("Index");
                    x = (att != null) ? (att as IndexAttribute).Index : -1;
                    if (x >= 0)
                        columnas.Add(new DBColumn(x, mi.Name));
                }
                // si hay propiedades con el atributo index, esas se van a tratar de ordenar
                // NOTAR que esta implementacion no resuelve el caso de propiedades persistentes que no tienen el atributo
                // index, por lo tanto al crear la tabla en la bd, esas seran las primeras. Sugerencia usar siempre el atributo index
                if (columnas.Count() > 0)
                {
                    // primero vamos a ordenar la lista de propiedades persistentes, usando el atributo Index, para crearlas nuevamente
                    // en el orden que esta definido en el atributo Index
                    List<DBColumn> SortColumns = columnas.OrderBy(DBColumn => DBColumn.Index).ToList<DBColumn>();
                    foreach (DBColumn column in SortColumns)
                    {
                        XPMemberInfo mi = ((IEnumerable<XPMemberInfo>)ci.PersistentProperties).FirstOrDefault(XPMemberInfo => XPMemberInfo.Name == "Name");
                        if (mi != null)
                        {
                            mi.RemoveAttribute(typeof(PersistentAttribute));
                            mi.AddAttribute(new PersistentAttribute(mi.Name.ToUpper()));
                        }
                    }
                    typesInfo.RefreshInfo(ci.ClassType);
                }
            }
        }
    }

    public class DBColumn
    {
        public DBColumn()
        {

        }

        public DBColumn(int AIndex, string AName)
        {
            Index = AIndex;
            FieldName = AName;
        }

        public int Index { get; set; }
        public string FieldName { get; set; }
    }
    
}
