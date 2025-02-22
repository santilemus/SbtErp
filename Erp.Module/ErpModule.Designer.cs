﻿namespace SBT.Apps.Erp.Module {
	partial class ErpModule {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            // 
            // ErpModule
            // 
            this.AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.ModelDifference));
            this.AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.ModelDifferenceAspect));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.SystemModule.SystemModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.CloneObject.CloneObjectModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Notifications.NotificationsModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ReportsV2.ReportsModuleV2));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Validation.ValidationModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ViewVariantsModule.ViewVariantsModule));
            this.RequiredModuleTypes.Add(typeof(SBT.Apps.Base.Module.BaseModule));
            this.RequiredModuleTypes.Add(typeof(SBT.Apps.Tercero.Module.TerceroModule));
            this.RequiredModuleTypes.Add(typeof(SBT.Apps.Empleado.Module.EmpleadoModule));
            this.RequiredModuleTypes.Add(typeof(SBT.Apps.Producto.Module.ProductoModule));
            this.RequiredModuleTypes.Add(typeof(SBT.Apps.Contabilidad.ContabilidadModule));
            this.RequiredModuleTypes.Add(typeof(SBT.Apps.RecursoHumano.RecursoHumanoModule));
            this.RequiredModuleTypes.Add(typeof(SBT.Apps.Facturacion.Module.FacturacionModule));
            this.RequiredModuleTypes.Add(typeof(SBT.Apps.Compra.CompraModule));
            this.RequiredModuleTypes.Add(typeof(SBT.Apps.Activo.ActivoModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Dashboards.DashboardsModule));

		}

		#endregion
	}
}
