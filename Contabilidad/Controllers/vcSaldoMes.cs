﻿using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Base.Module.Controllers;
using System;

namespace SBT.Apps.Contabilidad.Module.Controllers
{
    /// <summary>
    /// ViewController para el BO SaldoMes
    /// </summary>
    /// <remarks>
    /// Ver desactivacion de action predefinidos
    /// https://docs.devexpress.com/eXpressAppFramework/112728/ui-construction/controllers-and-actions/actions/how-to-deactivate-hide-an-action-in-code
    /// </remarks>
    public class vcSaldoMes : ViewControllerBase
    {
        private DeleteObjectsViewController deleteObjectController;
        private NewObjectViewController newObjectController;
        private ModificationsController modificationController;
        private const string Key = "Desactivar";
        public vcSaldoMes() : base()
        {

        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Para filtrar los datos para la empresa de la sesion y evitar que se mezclen cuando hay más de una empresa
            if ((string.Compare(View.GetType().Name, "ListView", StringComparison.Ordinal) == 0) && (((ListView)View).ObjectTypeInfo.FindMember("Empresa") != null) &&
                !(((ListView)View).CollectionSource.Criteria.ContainsKey("Empresa Actual")))
                ((ListView)View).CollectionSource.Criteria["Empresa Actual"] = CriteriaOperator.Parse("[Cuenta.Empresa.Oid] = ?", ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid);

            deleteObjectController = Frame.GetController<DeleteObjectsViewController>();
            if (deleteObjectController != null)
                deleteObjectController.Active[Key] = !(View.ObjectTypeInfo.Type == typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.SaldoMes)); // && View is ListView) ;
            newObjectController = Frame.GetController<NewObjectViewController>();
            if (newObjectController != null)
                newObjectController.Active[Key] = !(View.ObjectTypeInfo.Type == typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.SaldoMes));
            modificationController = Frame.GetController<ModificationsController>();
            if (modificationController != null)
                modificationController.Active[Key] = !(View.ObjectTypeInfo.Type == typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.SaldoMes));
        }

        protected override void OnDeactivated()
        {
            if (deleteObjectController != null)
                deleteObjectController.Active.RemoveItem(Key);
            if (newObjectController != null)
                newObjectController.Active.RemoveItem(Key);
            if (modificationController != null)
                modificationController.Active.RemoveItem(Key);
            base.OnDeactivated();
        }

        protected override void DoInitializeComponent()
        {
            base.DoInitializeComponent();
            TargetObjectType = typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.SaldoMes);
        }
    }
}
