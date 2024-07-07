using DevExpress.ExpressApp.Blazor.Components;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Utils;
using Microsoft.AspNetCore.Components;

namespace SBT.Apps.Erp.Blazor.Server.Editors
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// ver además: https://github.com/jjcolumb/DXUploadXAFBlazor/tree/master/DXUploadXAFBlazor.Module.Blazor/Editors
    /// </remarks>
    public class UploadFileAdapter : ComponentAdapterBase
    {
        public UploadFileAdapter(UploadFileModel componentModel)
            => ComponentModel = componentModel ?? throw new ArgumentNullException(nameof(componentModel));

        public override UploadFileModel ComponentModel { get; }

        public override void SetAllowEdit(bool allowEdit) => ComponentModel.ReadOnly = !allowEdit;

        public override object GetValue() => ComponentModel.Value;

        public override void SetValue(object value) { }

        protected override RenderFragment CreateComponent() => ComponentModelObserver.Create(ComponentModel, UploadFileComponent.Create(ComponentModel));

        public override void SetAllowNull(bool allowNull) { }

        public override void SetDisplayFormat(string displayFormat) { }

        public override void SetEditMask(string editMask) { }

        public override void SetEditMaskType(EditMaskType editMaskType) { }

        public override void SetErrorIcon(ImageInfo errorIcon) { }

        public override void SetErrorMessage(string errorMessage) { }

        public override void SetIsPassword(bool isPassword) { }

        public override void SetMaxLength(int maxLength) { }

        public override void SetNullText(string nullText) { }
    }
}
