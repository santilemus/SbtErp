using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    [DomainComponent]
    [DefaultClassOptions, CreatableItem(false), NavigationItem(false)]
    public class FileUploadParameter: NonPersistentBaseObject
    {
        private FileData fileData;
        private FileData data;

        [VisibleInListView(false)]
        [EditorAlias("FileData")]
        [System.ComponentModel.DisplayName("Upload Data")]
        public FileData Data
        {
            get => data;
            set => SetPropertyValue(ref data, value);
        }
        [VisibleInListView(true)]
        [VisibleInDetailView(true)]
        [ModelDefault("AllowEdit", "False")]
        [System.ComponentModel.DisplayName("Uploaded Data")]
        public FileData FileData
        {
            get => fileData;
            set => SetPropertyValue(ref fileData, value);
        }
    }
}
