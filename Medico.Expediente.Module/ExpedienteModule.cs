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
using DevExpress.Xpo.Metadata;
using DevExpress.Xpo;
using SBT.Apps.Medico.Expediente.Module.BusinessObjects;
using System.IO;

namespace SBT.Apps.Medico.Expediente.Module {
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppModuleBasetopic.
    public sealed partial class ExpedienteModule : ModuleBase {
        public ExpedienteModule() {
            InitializeComponent();
			BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            ModuleUpdater updater = new SBT.Apps.Medico.Expediente.Module.DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }
        public override void Setup(XafApplication application) {
            base.Setup(application);
            // Manage various aspects of the application UI and behavior at the module level.
        }
		public override void CustomizeTypesInfo(ITypesInfo typesInfo) {
			base.CustomizeTypesInfo(typesInfo);
			CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
            ///// NO BORRAR, ES EJEMPLO, SIRVE, EXCEPTO QUE HAY UN PROBLEMA POSIBLEMENTE POR LA HERENCIA DE Event en citas, pero con clases normales funciona
            //ITypeInfo tInfoMedico = typesInfo.FindTypeInfo(typeof(SBT.Apps.Medico.Generico.Module.BusinessObjects.Medico));
            //IMemberInfo memberCitasInfo = tInfoMedico.FindMember("Citas");
            //ITypeInfo tInfoCita = typesInfo.FindTypeInfo(typeof(SBT.Apps.Medico.Expediente.Module.BusinessObjects.Cita));
            //IMemberInfo memberMedicoInfo = tInfoCita.FindMember("Medico");
            //if (memberCitasInfo == null)
            //{
            //    memberCitasInfo = tInfoMedico.CreateMember("Citas", typeof(XPCollection<SBT.Apps.Medico.Expediente.Module.BusinessObjects.Cita>));
            //    memberCitasInfo.AddAttribute(new DevExpress.Xpo.AssociationAttribute("Medico-Citas", typeof(SBT.Apps.Medico.Expediente.Module.BusinessObjects.Cita)), true);
            //    memberCitasInfo.AddAttribute(new DevExpress.Xpo.AggregatedAttribute(), true);
            //    memberCitasInfo.AddAttribute(new DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Citas"), true);
            //}
            //if (memberMedicoInfo == null)
            //{
            //    memberMedicoInfo = tInfoCita.CreateMember("Medico", typeof(SBT.Apps.Medico.Generico.Module.BusinessObjects.Medico));
            //    memberMedicoInfo.AddAttribute(new DevExpress.Xpo.AssociationAttribute("Medico-Citas", typeof(SBT.Apps.Medico.Generico.Module.BusinessObjects.Medico)));
            //    memberMedicoInfo.AddAttribute(new DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Medico"), true);
            //}
            //else
            //{
            //    if (memberMedicoInfo.FindAttribute<DevExpress.Xpo.AssociationAttribute>() == null)
            //        memberMedicoInfo.AddAttribute(new DevExpress.Xpo.AssociationAttribute("Medico-Citas", typeof(SBT.Apps.Medico.Generico.Module.BusinessObjects.Medico)), true);
            //}
            //((XafMemberInfo)memberMedicoInfo).Refresh();
            //((XafMemberInfo)memberCitasInfo).Refresh();
            //typesInfo.RefreshInfo(tInfoMedico);
            //typesInfo.RefreshInfo(tInfoCita);
        }

        public static ExpedienteModule FindExpedienteModule(ModuleList modules)
        {
            ExpedienteModule expedienteModule = null;
            foreach (ModuleBase module in modules)
            {
                if (module is ExpedienteModule)
                {
                    expedienteModule = (ExpedienteModule)module;
                }
            }          
            return expedienteModule;
        }

        public byte[] GetPacienteFileDataByHandle(string handle)
        {

            MemoryStream ms = new MemoryStream();
            using (IObjectSpace os = this.Application.CreateObjectSpace(typeof(SBT.Apps.Medico.Expediente.Module.BusinessObjects.PacienteFileData)))
            {
                var fileAttachment = (os.GetObjectByHandle(handle) as PacienteFileData);
                if (fileAttachment != null)
                {
                    fileAttachment.File.SaveToStream(ms);
                    return ms.ToArray();
                }
                else
                    return null;
            }
        }

    }
}
