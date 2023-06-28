namespace SBT.Apps.Erp.Module.Win {
    partial class ErpWindowsFormsModule {
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
            // ErpWindowsFormsModule
            // 
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Notifications.Win.NotificationsWindowsFormsModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ReportsV2.Win.ReportsWindowsFormsModuleV2));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Validation.Win.ValidationWindowsFormsModule));
            this.RequiredModuleTypes.Add(typeof(SBT.Apps.Erp.Module.ErpModule));
            this.RequiredModuleTypes.Add(typeof(SBT.Apps.Base.Module.BaseModule));
            this.RequiredModuleTypes.Add(typeof(SBT.Apps.Tercero.Module.TerceroModule));
            this.RequiredModuleTypes.Add(typeof(SBT.Apps.Empleado.Module.EmpleadoModule));
            this.RequiredModuleTypes.Add(typeof(SBT.Apps.Producto.Module.ProductoModule));
            this.RequiredModuleTypes.Add(typeof(SBT.Apps.Contabilidad.ContabilidadModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Dashboards.Win.DashboardsWindowsFormsModule));

        }

        #endregion
    }
}