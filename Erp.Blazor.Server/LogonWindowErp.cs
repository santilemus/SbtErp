using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Templates;
using DevExpress.ExpressApp.Blazor.Templates.Toolbar.ActionControls;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates.ActionControls;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;

namespace SBT.Apps.Erp.Blazor.Server
{
    public class LogonWindowErp : WindowTemplateBase, ITemplateToolbarProvider
    {
        public LogonWindowErp()
        {
            Toolbar = new DxToolbarAdapter(new DxToolbarModel() { AdaptivityAutoHideRootItems = false, AdaptivityAutoCollapseItemsToIcons = false });
            Toolbar.AddActionContainer(DialogController.DialogActionContainerName);

            AdditionalToolbar = new DxToolbarAdapter(new DxToolbarModel());
            AdditionalToolbar.ImageSize = 16;
            AdditionalToolbar.ToolbarModel.CssClass = "logon-bottom-toolbar";
            AdditionalToolbar.AddActionContainer("AdditionalLogonActions");
        }
        protected override IEnumerable<IActionControlContainer> GetActionControlContainers() =>
            Toolbar.ActionContainers.Concat(AdditionalToolbar.ActionContainers);
        protected override RenderFragment CreateComponent() => LogonWindowErpComponent.Create(this);
        public DxToolbarAdapter Toolbar { get; }
        public DxToolbarAdapter AdditionalToolbar { get; }
        public string HeaderCaption { get; set; }
        protected override void BeginUpdate()
        {
            base.BeginUpdate();
            ((ISupportUpdate)Toolbar).BeginUpdate();
            ((ISupportUpdate)AdditionalToolbar).BeginUpdate();
        }
        protected override void EndUpdate()
        {
            ((ISupportUpdate)AdditionalToolbar).EndUpdate();
            ((ISupportUpdate)Toolbar).EndUpdate();
            base.EndUpdate();
        }
    }
}
