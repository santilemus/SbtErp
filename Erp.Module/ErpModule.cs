using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Collections.Generic;

namespace SBT.Apps.Erp.Module
{
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ModuleBase.
    public sealed partial class ErpModule : ModuleBase
    {
        public ErpModule()
        {
            InitializeComponent();
            BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB)
        {
            ModuleUpdater updater = new SBT.Apps.Erp.Module.DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }
        public override void Setup(XafApplication application)
        {
            base.Setup(application);
            // Manage various aspects of the application UI and behavior at the module level.
            application.CreateCustomLogonWindowObjectSpace += application_CreateCustomLogonWindowObjectSpace;
            // agregado el 13/nov/2021 por selm
            application.ObjectSpaceCreated += Application_ObjectSpaceCreated;
        }

        // agregado el 13/nov/2021 por selm
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
        //--

        public override void CustomizeTypesInfo(ITypesInfo typesInfo)
        {
            base.CustomizeTypesInfo(typesInfo);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
            CreateAssociationBancoTransaccionCxC(typesInfo);
            CreateAssociationBancoTransaccionCxP(typesInfo);
        }
        void application_CreateCustomLogonWindowObjectSpace(object sender, CreateCustomLogonWindowObjectSpaceEventArgs e)
        {
            IObjectSpace objectSpace = ((XafApplication)sender).CreateObjectSpace(typeof(CustomLogonParameters));
            //((SBT.Apps.Base.Module.BusinessObjects.CustomLogonParameters)e.LogonParameters).ObjectSpace = objectSpace;
            e.ObjectSpace = objectSpace;
        }

        /// <summary>
        /// Crear la propiedad (coleccion) PartidaDetalles en el BO SBT.Apps.Contabilidad.BusinessObjects.Catalogo cuando no existe
        /// y agregar los atributos que corresponden a la Asociation, Aggregated y DisplayName.
        /// En el BO SBT.Apps.Contabilidad.Module.BusinessObjects.PartidaDetalle crea la propiedade Cuenta cuando no existe
        /// y agrega los atributos que corresponde a: Asociation y DisplayName
        /// </summary>
        /// <param name="typesInfo"></param>
        /// <remarks>
        /// mas info en: 
        /// https://docs.devexpress.com/eXpressAppFramework/113583/concepts/business-model-design/types-info-subsystem/use-metadata-to-customize-business-classes-dynamically
        /// https://supportcenter.devexpress.com/ticket/details/T284822/how-to-create-business-classes-at-runtime-based-on-predefined-configurations-or-allow
        /// </remarks>
        private void CreateAssociationBancoTransaccionCxC(ITypesInfo typesInfo)
        {
            ITypeInfo tInfoBancoTransaccion = typesInfo.FindTypeInfo(typeof(SBT.Apps.Banco.Module.BusinessObjects.BancoTransaccion));
            IMemberInfo mInfoCobros = tInfoBancoTransaccion.FindMember("Cobros");
            if (mInfoCobros == null)
            {
                mInfoCobros = tInfoBancoTransaccion.CreateMember("Cobros", typeof(XPCollection<SBT.Apps.CxC.Module.BusinessObjects.CxCTransaccion>));
                mInfoCobros.AddAttribute(new DevExpress.Xpo.AssociationAttribute("BancoTransaccion-CxCTransacciones",
                                                     typeof(SBT.Apps.CxC.Module.BusinessObjects.CxCTransaccion)), true);
                mInfoCobros.AddAttribute(new DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Cobros"), true);
                mInfoCobros.AddAttribute(new DevExpress.ExpressApp.ConditionalAppearance.AppearanceAttribute("BancoTransaccion.Cobros",
                    DevExpress.ExpressApp.ConditionalAppearance.AppearanceItemType.ViewItem,
                    "([Clasificacion.Tipo] == 1 || [Clasificacion.Tipo] == 2) && [Oid] In (1, 3, 5, 7, 8)")
                { Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide }, true);
            }
            ITypeInfo tInfoCxCTransaccion = typesInfo.FindTypeInfo(typeof(SBT.Apps.CxC.Module.BusinessObjects.CxCTransaccion));
            IMemberInfo mInfoBancoTransac = tInfoCxCTransaccion.FindMember("BancoTransaccion");
            if (mInfoCobros != null && mInfoBancoTransac != null && mInfoBancoTransac.FindAttribute<AssociationAttribute>() == null)
            {
                mInfoBancoTransac.AddAttribute(new DevExpress.Xpo.AssociationAttribute("BancoTransaccion-CxCTransacciones"), true);
            }
            ((XafMemberInfo)mInfoCobros).Refresh();
            ((XafMemberInfo)mInfoBancoTransac).Refresh();
        }

        private void CreateAssociationBancoTransaccionCxP(ITypesInfo typesInfo)
        {
            ITypeInfo tInfoBancoTransaccion = typesInfo.FindTypeInfo(typeof(SBT.Apps.Banco.Module.BusinessObjects.BancoTransaccion));
            IMemberInfo mInfoPagos = tInfoBancoTransaccion.FindMember("Pagos");
            if (mInfoPagos == null)
            {
                mInfoPagos = tInfoBancoTransaccion.CreateMember("Pagos", typeof(XPCollection<SBT.Apps.CxP.Module.BusinessObjects.CxPTransaccion>));
                mInfoPagos.AddAttribute(new DevExpress.Xpo.AssociationAttribute("BancoTransaccion-CxPTransacciones",
                                                     typeof(SBT.Apps.CxP.Module.BusinessObjects.CxPTransaccion)), true);
                mInfoPagos.AddAttribute(new DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Pagos"), true);
                mInfoPagos.AddAttribute(new DevExpress.ExpressApp.ConditionalAppearance.AppearanceAttribute("BancoTransaccion.Pagos",
                    DevExpress.ExpressApp.ConditionalAppearance.AppearanceItemType.ViewItem,
                    "([Clasificacion.Tipo] == 3 || [Clasificacion.Tipo] == 4) && [Oid] In (11, 14, 16, 19, 20)")
                { Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide }, true);
            }
            ITypeInfo tInfoCxPTransaccion = typesInfo.FindTypeInfo(typeof(SBT.Apps.CxP.Module.BusinessObjects.CxPTransaccion));
            IMemberInfo mInfoBancoTransac = tInfoCxPTransaccion.FindMember("BancoTransaccion");
            if (mInfoPagos != null && mInfoBancoTransac != null && mInfoBancoTransac.FindAttribute<AssociationAttribute>() == null)
            {
                mInfoBancoTransac.AddAttribute(new DevExpress.Xpo.AssociationAttribute("BancoTransaccion-CxPTransacciones"), true);
            }
            ((XafMemberInfo)mInfoPagos).Refresh();
            ((XafMemberInfo)mInfoBancoTransac).Refresh();
        }
    }
}
