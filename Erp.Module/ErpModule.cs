using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;

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
        }
        public override void CustomizeTypesInfo(ITypesInfo typesInfo)
        {
            base.CustomizeTypesInfo(typesInfo);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
            CreateAsociacionBancoTransaccionCompraFactura(typesInfo);
        }
        void application_CreateCustomLogonWindowObjectSpace(object sender, CreateCustomLogonWindowObjectSpaceEventArgs e)
        {
            IObjectSpace objectSpace = ((XafApplication)sender).CreateObjectSpace();
            ((SBT.Apps.Base.Module.BusinessObjects.CustomLogonParameters)e.LogonParameters).ObjectSpace = objectSpace;
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
        private void CreateAsociacionBancoTransaccionCompraFactura(ITypesInfo typesInfo)
        {
            ITypeInfo tInfoBancoTransaccionPago = typesInfo.FindTypeInfo(typeof(SBT.Apps.Banco.Module.BusinessObjects.BancoTransaccionPago));
            IMemberInfo mInfoCompraFactura = tInfoBancoTransaccionPago.FindMember("CompraFactura");
            ITypeInfo tInfoCompraFactura = typesInfo.FindTypeInfo(typeof(SBT.Apps.Compra.Module.BusinessObjects.CompraFactura));
            IMemberInfo mInfoPagos = tInfoCompraFactura.FindMember("Pagos");
            if (mInfoPagos == null)
            {
                mInfoPagos = tInfoCompraFactura.CreateMember("Pagos", typeof(XPCollection<SBT.Apps.Banco.Module.BusinessObjects.BancoTransaccionPago>));
                mInfoPagos.AddAttribute(new DevExpress.Xpo.AssociationAttribute("CompraFactura-BancoTransaccionPagos",
                                                     typeof(SBT.Apps.Banco.Module.BusinessObjects.BancoTransaccionPago)), true);
                mInfoPagos.AddAttribute(new DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Pagos"), true);
            }
            if (mInfoCompraFactura == null)
                mInfoCompraFactura = tInfoBancoTransaccionPago.CreateMember("CompraFactura", typeof(SBT.Apps.Compra.Module.BusinessObjects.CompraFactura));
            if (mInfoCompraFactura.FindAttribute<DevExpress.Xpo.AssociationAttribute>() == null)
                mInfoCompraFactura.AddAttribute(new DevExpress.Xpo.AssociationAttribute("CompraFactura-BancoTransaccionPagos",
                                                     typeof(SBT.Apps.Compra.Module.BusinessObjects.CompraFactura)), true);
            if (mInfoCompraFactura.FindAttribute<DevExpress.ExpressApp.DC.XafDisplayNameAttribute>() == null)
                mInfoCompraFactura.AddAttribute(new DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Compra Factura"), true);
            if (mInfoCompraFactura.FindAttribute<DevExpress.Persistent.Base.DataSourceCriteriaAttribute>() == null)
                mInfoCompraFactura.AddAttribute(new DevExpress.Persistent.Base.DataSourceCriteriaAttribute("[Proveedor] == '@This.BancoTransaccion.Proveedor' && [Saldo] > 0.0"));
            ((XafMemberInfo)mInfoPagos).Refresh();
            ((XafMemberInfo)mInfoCompraFactura).Refresh();
        }
    }
}
