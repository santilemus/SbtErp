using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Producto.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class vcProducto : ViewController
    {
        public vcProducto()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            if (string.Compare(View.GetType().Name, "ListView", StringComparison.Ordinal) == 0)
            {
                ((ListView)View).CollectionSource.Criteria["Empresa Actual"] = new BinaryOperator("Empresa", ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid);
            }

            ObjectSpace.ObjectChanged += ObjectSpace_Changed;
            ObjectSpace.Committing += ObjectSpace_Committing;
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            ObjectSpace.ObjectChanged -= ObjectSpace_Changed;
            ObjectSpace.Committing -= ObjectSpace_Committing;

            base.OnDeactivated();
        }

        private void ObjectSpace_Changed(object Sender, ObjectChangedEventArgs e)
        {
            string propName = e.PropertyName;
            string algo = string.Empty;
            if (ObjectSpace.IsNewObject(View.CurrentObject))
            {
                algo = propName;
            }
        }

        private void ObjectSpace_Committing(object Sender, CancelEventArgs﻿ e)
        {
            System.Collections.IList objects = ObjectSpace.ModifiedObjects;
            foreach (object obj in objects)
            {
                Type tipo = obj.GetType();
                if (tipo == typeof(SBT.Apps.Producto.Module.BusinessObjects.ProductoPrecio))
                {
                    var pp = ((BusinessObjects.ProductoPrecio)obj).PrecioUnitario;
                    bool deleted = ((BusinessObjects.ProductoPrecio)obj).IsDeleted;
                }
            }
        }
    }
}
