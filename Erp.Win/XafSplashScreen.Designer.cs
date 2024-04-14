namespace SBT.Apps.Erp.Win {
    partial class XafSplashScreen
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XafSplashScreen));
            progressBarControl = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            labelCopyright = new DevExpress.XtraEditors.LabelControl();
            labelStatus = new DevExpress.XtraEditors.LabelControl();
            peImage = new DevExpress.XtraEditors.PictureEdit();
            peLogo = new DevExpress.XtraEditors.PictureEdit();
            pcApplicationName = new DevExpress.XtraEditors.PanelControl();
            labelSubtitle = new DevExpress.XtraEditors.LabelControl();
            labelApplicationName = new DevExpress.XtraEditors.LabelControl();
            lblOwner = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)progressBarControl.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)peImage.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)peLogo.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pcApplicationName).BeginInit();
            pcApplicationName.SuspendLayout();
            SuspendLayout();
            // 
            // progressBarControl
            // 
            progressBarControl.EditValue = 0;
            progressBarControl.Location = new System.Drawing.Point(28, 304);
            progressBarControl.Margin = new System.Windows.Forms.Padding(4);
            progressBarControl.Name = "progressBarControl";
            progressBarControl.Properties.Appearance.BorderColor = System.Drawing.Color.FromArgb(195, 194, 194);
            progressBarControl.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            progressBarControl.Properties.EndColor = System.Drawing.Color.LightSteelBlue;
            progressBarControl.Properties.LookAndFeel.SkinName = "Visual Studio 2013 Blue";
            progressBarControl.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            progressBarControl.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            progressBarControl.Properties.ProgressViewStyle = DevExpress.XtraEditors.Controls.ProgressViewStyle.Solid;
            progressBarControl.Properties.StartColor = System.Drawing.Color.MediumBlue;
            progressBarControl.Size = new System.Drawing.Size(535, 22);
            progressBarControl.TabIndex = 5;
            // 
            // labelCopyright
            // 
            labelCopyright.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            labelCopyright.Location = new System.Drawing.Point(28, 401);
            labelCopyright.Margin = new System.Windows.Forms.Padding(4);
            labelCopyright.Name = "labelCopyright";
            labelCopyright.Size = new System.Drawing.Size(143, 16);
            labelCopyright.TabIndex = 6;
            labelCopyright.Text = "Copyright © 2020 - 2024";
            // 
            // labelStatus
            // 
            labelStatus.Location = new System.Drawing.Point(28, 280);
            labelStatus.Margin = new System.Windows.Forms.Padding(4);
            labelStatus.Name = "labelStatus";
            labelStatus.Size = new System.Drawing.Size(67, 16);
            labelStatus.TabIndex = 7;
            labelStatus.Text = "Cargando...";
            // 
            // peImage
            // 
            peImage.EditValue = resources.GetObject("peImage.EditValue");
            peImage.Location = new System.Drawing.Point(14, 15);
            peImage.Margin = new System.Windows.Forms.Padding(4);
            peImage.Name = "peImage";
            peImage.Properties.AllowFocused = false;
            peImage.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            peImage.Properties.Appearance.Options.UseBackColor = true;
            peImage.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            peImage.Properties.ShowMenu = false;
            peImage.Size = new System.Drawing.Size(497, 222);
            peImage.TabIndex = 9;
            peImage.Visible = false;
            // 
            // peLogo
            // 
            peLogo.EditValue = resources.GetObject("peLogo.EditValue");
            peLogo.Location = new System.Drawing.Point(261, 344);
            peLogo.Margin = new System.Windows.Forms.Padding(4);
            peLogo.Name = "peLogo";
            peLogo.Properties.AllowFocused = false;
            peLogo.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            peLogo.Properties.Appearance.Options.UseBackColor = true;
            peLogo.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            peLogo.Properties.ShowMenu = false;
            peLogo.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            peLogo.Size = new System.Drawing.Size(302, 97);
            peLogo.TabIndex = 8;
            // 
            // pcApplicationName
            // 
            pcApplicationName.Appearance.BackColor = System.Drawing.Color.SteelBlue;
            pcApplicationName.Appearance.Options.UseBackColor = true;
            pcApplicationName.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            pcApplicationName.Controls.Add(labelSubtitle);
            pcApplicationName.Controls.Add(labelApplicationName);
            pcApplicationName.Dock = System.Windows.Forms.DockStyle.Top;
            pcApplicationName.Location = new System.Drawing.Point(1, 1);
            pcApplicationName.LookAndFeel.UseDefaultLookAndFeel = false;
            pcApplicationName.Margin = new System.Windows.Forms.Padding(4);
            pcApplicationName.Name = "pcApplicationName";
            pcApplicationName.Size = new System.Drawing.Size(576, 271);
            pcApplicationName.TabIndex = 10;
            // 
            // labelSubtitle
            // 
            labelSubtitle.Appearance.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            labelSubtitle.Appearance.ForeColor = System.Drawing.Color.FromArgb(255, 216, 188);
            labelSubtitle.Appearance.Options.UseFont = true;
            labelSubtitle.Appearance.Options.UseForeColor = true;
            labelSubtitle.Appearance.Options.UseTextOptions = true;
            labelSubtitle.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            labelSubtitle.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            labelSubtitle.Location = new System.Drawing.Point(69, 106);
            labelSubtitle.Margin = new System.Windows.Forms.Padding(4);
            labelSubtitle.Name = "labelSubtitle";
            labelSubtitle.Size = new System.Drawing.Size(441, 32);
            labelSubtitle.TabIndex = 1;
            labelSubtitle.Text = "Versión 2024";
            // 
            // labelApplicationName
            // 
            labelApplicationName.Appearance.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            labelApplicationName.Appearance.ForeColor = System.Drawing.SystemColors.Window;
            labelApplicationName.Appearance.Options.UseFont = true;
            labelApplicationName.Appearance.Options.UseForeColor = true;
            labelApplicationName.Appearance.Options.UseTextOptions = true;
            labelApplicationName.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            labelApplicationName.Location = new System.Drawing.Point(69, 61);
            labelApplicationName.Margin = new System.Windows.Forms.Padding(4);
            labelApplicationName.Name = "labelApplicationName";
            labelApplicationName.Size = new System.Drawing.Size(441, 37);
            labelApplicationName.TabIndex = 0;
            labelApplicationName.Text = "Sistema Administrativo Financiero";
            // 
            // lblOwner
            // 
            lblOwner.AutoSize = true;
            lblOwner.Location = new System.Drawing.Point(25, 363);
            lblOwner.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblOwner.Name = "lblOwner";
            lblOwner.Size = new System.Drawing.Size(145, 32);
            lblOwner.TabIndex = 13;
            lblOwner.Text = "Diseñado Por\r\nSantiago Enrique Lemus\r\n";
            // 
            // XafSplashScreen
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(578, 455);
            Controls.Add(lblOwner);
            Controls.Add(pcApplicationName);
            Controls.Add(peImage);
            Controls.Add(peLogo);
            Controls.Add(labelStatus);
            Controls.Add(labelCopyright);
            Controls.Add(progressBarControl);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "XafSplashScreen";
            Padding = new System.Windows.Forms.Padding(1);
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)progressBarControl.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)peImage.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)peLogo.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)pcApplicationName).EndInit();
            pcApplicationName.ResumeLayout(false);
            pcApplicationName.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DevExpress.XtraEditors.MarqueeProgressBarControl progressBarControl;
        private DevExpress.XtraEditors.LabelControl labelCopyright;
        private DevExpress.XtraEditors.LabelControl labelStatus;
        private DevExpress.XtraEditors.PictureEdit peLogo;
        private DevExpress.XtraEditors.PictureEdit peImage;
        private DevExpress.XtraEditors.PanelControl pcApplicationName;
        private DevExpress.XtraEditors.LabelControl labelSubtitle;
        private DevExpress.XtraEditors.LabelControl labelApplicationName;
        private System.Windows.Forms.Label lblOwner;
    }
}
