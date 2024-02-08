using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;

namespace SBT.Apps.RecursoHumano
{
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ModuleBase.
    public sealed partial class RecursoHumanoModule : ModuleBase
    {
        public RecursoHumanoModule()
        {
            InitializeComponent();
            BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;

            CriteriaOperator.RegisterCustomFunction(new SBT.Apps.RecursoHumano.Module.EmpleadoFunction());
            CriteriaOperator.RegisterCustomFunction(new SBT.Apps.RecursoHumano.Module.PlanillaEvaluarFunction());
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
            // personalizaciones en codigo al modelo, porque los BO estan en diferentes assemblies
            CreateAsociacionEmpleadoTipoPlanilla(typesInfo);
        }

        /// <summary>
        /// Crear la propiedad (coleccion) TipoPlanillas en el BO SBT.Apps.Empleado.Module.BusinessObjects.Empleado cuando no existe
        /// y agregar los atributos que corresponden a la Asociation, Aggregated y DisplayName.
        /// En el BO SBT.Apps.RecursoHumano.Module.BusinessObjects.EmpleadoTipoPlanilla crea la propiedade Empleado cuando no existe
        /// y agrega los atributos que corresponde a: Asociation y DisplayName
        /// </summary>
        /// <param name="typesInfo"></param>
        /// <remarks>
        /// mas info en: 
        /// https://docs.devexpress.com/eXpressAppFramework/113583/concepts/business-model-design/types-info-subsystem/use-metadata-to-customize-business-classes-dynamically
        /// https://supportcenter.devexpress.com/ticket/details/T284822/how-to-create-business-classes-at-runtime-based-on-predefined-configurations-or-allow
        /// </remarks>
        private void CreateAsociacionEmpleadoTipoPlanilla(ITypesInfo typesInfo)
        {
            ITypeInfo tInfoEmpleado = typesInfo.FindTypeInfo(typeof(SBT.Apps.Empleado.Module.BusinessObjects.Empleado));
            IMemberInfo mInfoTipoPlanillas = tInfoEmpleado.FindMember("TipoPlanillas");
            ITypeInfo tInfoEmpleadoTipoPlanilla = typesInfo.FindTypeInfo(typeof(SBT.Apps.RecursoHumano.Module.BusinessObjects.EmpleadoTipoPlanilla));
            IMemberInfo mInfoEmpleado = tInfoEmpleadoTipoPlanilla.FindMember("Empleado");
            if (mInfoTipoPlanillas == null)
            {
                mInfoTipoPlanillas = tInfoEmpleado.CreateMember("TipoPlanillas", typeof(XPCollection<SBT.Apps.RecursoHumano.Module.BusinessObjects.EmpleadoTipoPlanilla>));
                mInfoTipoPlanillas.AddAttribute(new DevExpress.Xpo.AssociationAttribute("Empleado-TipoPlanillas",
                                                     typeof(SBT.Apps.RecursoHumano.Module.BusinessObjects.EmpleadoTipoPlanilla)), true);
                mInfoTipoPlanillas.AddAttribute(new DevExpress.Xpo.AggregatedAttribute(), true);
                mInfoTipoPlanillas.AddAttribute(new DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Tipo Planillas"), true);
            }
            if (mInfoEmpleado == null)
                mInfoEmpleado = tInfoEmpleadoTipoPlanilla.CreateMember("Empleado", typeof(SBT.Apps.Empleado.Module.BusinessObjects.Empleado));
            if (mInfoEmpleado.FindAttribute<DevExpress.Xpo.AssociationAttribute>() == null)
                mInfoEmpleado.AddAttribute(new DevExpress.Xpo.AssociationAttribute("Empleado-TipoPlanillas",
                                                     typeof(SBT.Apps.Empleado.Module.BusinessObjects.Empleado)), true);
            if (mInfoEmpleado.FindAttribute<DevExpress.ExpressApp.DC.XafDisplayNameAttribute>() == null)
                mInfoEmpleado.AddAttribute(new DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Empleado"), true);
            ((XafMemberInfo)mInfoTipoPlanillas).Refresh();
            ((XafMemberInfo)mInfoEmpleado).Refresh();
        }
    }
}
