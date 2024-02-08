using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;

namespace SBT.Apps.Contabilidad
{
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ModuleBase.
    public sealed partial class ContabilidadModule : ModuleBase
    {
        public ContabilidadModule()
        {
            InitializeComponent();
            BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;

            CriteriaOperator.RegisterCustomFunction(new Module.SaldoDeCuentaFunction());
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
            CreateAsociacionCatalogoPartidaDetalle(typesInfo);
            CreateAsociacionCatalogoSaldoDiario(typesInfo);
            CreateAsociacionCatalogoSaldoMes(typesInfo);
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
        private void CreateAsociacionCatalogoPartidaDetalle(ITypesInfo typesInfo)
        {
            ITypeInfo tInfoCatalogo = typesInfo.FindTypeInfo(typeof(SBT.Apps.Contabilidad.BusinessObjects.Catalogo));
            IMemberInfo mInfoPartidas = tInfoCatalogo.FindMember("Partidas");
            ITypeInfo tInfoPartidaDetalle = typesInfo.FindTypeInfo(typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.PartidaDetalle));
            IMemberInfo mInfoCuenta = tInfoPartidaDetalle.FindMember("Cuenta");
            if (mInfoPartidas == null)
            {
                mInfoPartidas = tInfoCatalogo.CreateMember("Partidas", typeof(XPCollection<SBT.Apps.Contabilidad.Module.BusinessObjects.PartidaDetalle>));
                mInfoPartidas.AddAttribute(new DevExpress.Xpo.AssociationAttribute("Catalogo-Partidas",
                                                     typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.PartidaDetalle)), true);
                mInfoPartidas.AddAttribute(new DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Partidas Detalle"), true);
            }
            if (mInfoCuenta == null)
                mInfoCuenta = tInfoPartidaDetalle.CreateMember("Cuenta", typeof(SBT.Apps.Contabilidad.BusinessObjects.Catalogo));
            if (mInfoCuenta.FindAttribute<DevExpress.Xpo.AssociationAttribute>() == null)
                mInfoCuenta.AddAttribute(new DevExpress.Xpo.AssociationAttribute("Catalogo-Partidas",
                                                     typeof(SBT.Apps.Contabilidad.BusinessObjects.Catalogo)), true);
            if (mInfoCuenta.FindAttribute<DevExpress.ExpressApp.DC.XafDisplayNameAttribute>() == null)
                mInfoCuenta.AddAttribute(new DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Cuenta"), true);
            ((XafMemberInfo)mInfoPartidas).Refresh();
            ((XafMemberInfo)mInfoCuenta).Refresh();
        }

        /// <summary>
        /// Crear la propiedad (coleccion) SaldosDiarios en el BO SBT.Apps.Contabilidad.BusinessObjects.Catalogo cuando no existe
        /// y agregar los atributos que corresponden a la Asociation, Aggregated y DisplayName.
        /// En el BO SBT.Apps.Contabilidad.Module.BusinessObjects.ConSaldoDiario crea la propiedade Cuenta cuando no existe
        /// y agrega los atributos que corresponde a: Asociation y DisplayName
        /// </summary>
        /// <param name="typesInfo"></param>
        /// <remarks>
        /// mas info en: 
        /// https://docs.devexpress.com/eXpressAppFramework/113583/concepts/business-model-design/types-info-subsystem/use-metadata-to-customize-business-classes-dynamically
        /// https://supportcenter.devexpress.com/ticket/details/T284822/how-to-create-business-classes-at-runtime-based-on-predefined-configurations-or-allow
        /// </remarks>
        private void CreateAsociacionCatalogoSaldoDiario(ITypesInfo typesInfo)
        {
            ITypeInfo tInfoCatalogo = typesInfo.FindTypeInfo(typeof(SBT.Apps.Contabilidad.BusinessObjects.Catalogo));
            IMemberInfo mInfoSaldos = tInfoCatalogo.FindMember("SaldosDiarios");
            ITypeInfo tInfoSaldoDiario = typesInfo.FindTypeInfo(typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.SaldoDiario));
            IMemberInfo mInfoCuenta = tInfoSaldoDiario.FindMember("Cuenta");
            if (mInfoSaldos == null)
            {
                mInfoSaldos = tInfoCatalogo.CreateMember("SaldosDiarios", typeof(XPCollection<SBT.Apps.Contabilidad.Module.BusinessObjects.SaldoDiario>));
                mInfoSaldos.AddAttribute(new DevExpress.Xpo.AssociationAttribute("Catalogo-SaldosDiarios",
                                                     typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.SaldoDiario)), true);
                mInfoSaldos.AddAttribute(new DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Saldos Diarios"), true);
            }
            if (mInfoCuenta == null)
                mInfoCuenta = tInfoSaldoDiario.CreateMember("Cuenta", typeof(SBT.Apps.Contabilidad.BusinessObjects.Catalogo));
            if (mInfoCuenta.FindAttribute<DevExpress.Xpo.AssociationAttribute>() == null)
                mInfoCuenta.AddAttribute(new DevExpress.Xpo.AssociationAttribute("Catalogo-SaldosDiarios",
                                                     typeof(SBT.Apps.Contabilidad.BusinessObjects.Catalogo)), true);
            if (mInfoCuenta.FindAttribute<DevExpress.ExpressApp.DC.XafDisplayNameAttribute>() == null)
                mInfoCuenta.AddAttribute(new DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Cuenta"), true);
            ((XafMemberInfo)mInfoSaldos).Refresh();
            ((XafMemberInfo)mInfoCuenta).Refresh();
        }

        /// <summary>
        /// Crear la propiedad (coleccion) SaldosMeses en el BO SBT.Apps.Contabilidad.BusinessObjects.Catalogo cuando no existe
        /// y agregar los atributos que corresponden a la Asociation, Aggregated y DisplayName.
        /// En el BO SBT.Apps.Contabilidad.Module.BusinessObjects.SaldoMes crea la propiedade Cuenta cuando no existe
        /// y agrega los atributos que corresponde a: Asociation y DisplayName
        /// </summary>
        /// <param name="typesInfo"></param>
        /// <remarks>
        /// mas info en: 
        /// https://docs.devexpress.com/eXpressAppFramework/113583/concepts/business-model-design/types-info-subsystem/use-metadata-to-customize-business-classes-dynamically
        /// https://supportcenter.devexpress.com/ticket/details/T284822/how-to-create-business-classes-at-runtime-based-on-predefined-configurations-or-allow
        /// </remarks>
        private void CreateAsociacionCatalogoSaldoMes(ITypesInfo typesInfo)
        {
            ITypeInfo tInfoCatalogo = typesInfo.FindTypeInfo(typeof(SBT.Apps.Contabilidad.BusinessObjects.Catalogo));
            IMemberInfo mInfoSaldosMes = tInfoCatalogo.FindMember("SaldosMeses");
            ITypeInfo tInfoSaldoMes = typesInfo.FindTypeInfo(typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.SaldoMes));
            IMemberInfo mInfoCuenta = tInfoSaldoMes.FindMember("Cuenta");
            if (mInfoSaldosMes == null)
            {
                mInfoSaldosMes = tInfoCatalogo.CreateMember("SaldosMeses", typeof(XPCollection<SBT.Apps.Contabilidad.Module.BusinessObjects.SaldoMes>));
                mInfoSaldosMes.AddAttribute(new DevExpress.Xpo.AssociationAttribute("Catalogo-SaldosMeses",
                                                     typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.SaldoMes)), true);
                mInfoSaldosMes.AddAttribute(new DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Saldos Mensuales"), true);
            }
            if (mInfoCuenta == null)
                mInfoCuenta = tInfoSaldoMes.CreateMember("Cuenta", typeof(SBT.Apps.Contabilidad.BusinessObjects.Catalogo));
            if (mInfoCuenta.FindAttribute<DevExpress.Xpo.AssociationAttribute>() == null)
                mInfoCuenta.AddAttribute(new DevExpress.Xpo.AssociationAttribute("Catalogo-SaldosMeses",
                                                     typeof(SBT.Apps.Contabilidad.BusinessObjects.Catalogo)), true);
            if (mInfoCuenta.FindAttribute<DevExpress.ExpressApp.DC.XafDisplayNameAttribute>() == null)
                mInfoCuenta.AddAttribute(new DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Cuenta"), true);
            ((XafMemberInfo)mInfoSaldosMes).Refresh();
            ((XafMemberInfo)mInfoCuenta).Refresh();
        }

    }
}
