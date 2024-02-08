using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;

namespace SBT.Apps.Base.Module.Controllers
{
    /// <summary>
    /// View Controller para el BO PermissionPolicyUser.
    /// Se usa para que la creción de usuarios solamente se permita para objetos cuya clase es el ultimo descendiente de
    /// PermissionPolicyUser, en nuestro caso SBT.Apps.Base.Module.BusinessObjects.Usuario
    /// </summary>
    /// <remarks>
    /// Ver: https://docs.devexpress.com/eXpressAppFramework/112912/task-based-help/actions/how-to-initialize-an-object-created-using-the-new-action
    /// </remarks>
    public class vcPermissionPolicyUser : ViewController
    {
        private NewObjectViewController newObjectController;
        public vcPermissionPolicyUser()
        {
            TargetObjectType = typeof(DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyUser);
            TargetViewType = ViewType.Any;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            newObjectController = Frame.GetController<NewObjectViewController>();
            newObjectController.NewObjectActionItemListMode = NewObjectActionItemListMode.LastDescendantsOnly;
        }

        protected override void OnDeactivated()
        {

            base.OnDeactivated();
        }
    }
}
