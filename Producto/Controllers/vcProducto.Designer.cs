namespace SBT.Apps.Producto.Module.Controllers
{
    partial class vcProducto
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.simpleAction1 = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // simpleAction1
            // 
            this.simpleAction1.Caption = "Test";
            this.simpleAction1.ConfirmationMessage = null;
            this.simpleAction1.Id = "454cca82-ee3c-4967-b50a-07691a1c948d";
            this.simpleAction1.TargetObjectType = typeof(SBT.Apps.Producto.Module.BusinessObjects.Producto);
            this.simpleAction1.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.simpleAction1.ToolTip = null;
            this.simpleAction1.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            // 
            // vcProducto
            // 
            this.Actions.Add(this.simpleAction1);
            this.TargetObjectType = typeof(SBT.Apps.Producto.Module.BusinessObjects.Producto);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction simpleAction1;
    }
}
