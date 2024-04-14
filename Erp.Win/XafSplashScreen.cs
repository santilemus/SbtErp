using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using DevExpress.ExpressApp.Win.Utils;
using DevExpress.Skins;
using DevExpress.Utils.Drawing;
using DevExpress.Utils.Svg;
using DevExpress.XtraSplashScreen;

namespace SBT.Apps.Erp.Win {
    public partial class XafSplashScreen : SplashScreen
    {

        [SupportedOSPlatform("windows")]
        protected override void DrawContent(GraphicsCache graphicsCache, Skin skin)
        {
            Rectangle bounds = ClientRectangle;
            bounds.Width--; bounds.Height--;
            graphicsCache.Graphics.DrawRectangle(graphicsCache.GetPen(Color.FromArgb(255, 87, 87, 87), 1), bounds);
        }

        [SupportedOSPlatform("windows")]
        protected void UpdateLabelsPosition()
        {
            labelApplicationName.CalcBestSize();
            int newLeft = (Width - labelApplicationName.Width) / 2;
            labelApplicationName.Location = new Point(newLeft, labelApplicationName.Top);
            labelSubtitle.CalcBestSize();
            newLeft = (Width - labelSubtitle.Width) / 2;
            labelSubtitle.Location = new Point(newLeft, labelSubtitle.Top);
        }

        [SupportedOSPlatform("windows")]
        public XafSplashScreen()
        {
            InitializeComponent();
            
            this.labelCopyright.Text = $"Copyright © {DateTime.Now.Year} {CompanyName}{System.Environment.NewLine}All Rights Reserved";
            UpdateLabelsPosition();
        }

        #region Overrides

        [SupportedOSPlatform("windows")]
        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
            if ((UpdateSplashCommand)cmd == UpdateSplashCommand.Description)
            {
                labelStatus.Text = (string)arg;
            }
        }

#endregion

        public enum SplashScreenCommand
        {
        }
    }
}