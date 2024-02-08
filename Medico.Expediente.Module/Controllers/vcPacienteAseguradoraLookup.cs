using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using SBT.Apps.Base.Module.Controllers;
using SBT.Apps.Medico.Expediente.Module.BusinessObjects;
using SBT.Apps.Tercero.Module.BusinessObjects;

namespace SBT.Apps.Medico.Expediente.Module.Controllers
{
    /// <summary>
    /// View Controller para la vista con id = "Aseguradora_LookupListView" del BO Tercero.
    /// El propósito es implementar personalizaciones cuando se agrega una nueva aseguradora desde la propiedad Aseguradora
    /// del BO Paciente. Es para agregar de forma automatica el role de aseguradora del tercero
    /// </summary>
    public class vcPacienteAseguradoraLookup : ViewControllerBase
    {
        private NewObjectViewController newObjectController;
        public vcPacienteAseguradoraLookup() : base()
        {

        }

        protected override void DoInitializeComponent()
        {
            base.DoInitializeComponent();
            TargetObjectType = typeof(SBT.Apps.Tercero.Module.BusinessObjects.Tercero);
            TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            TargetViewId = "Aseguradora_LookupListView";
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            newObjectController = Frame.GetController<NewObjectViewController>();
            if (newObjectController != null)
            {
                newObjectController.ObjectCreated += newObjectController_ObjectCreated;
            }
        }

        protected override void OnDeactivated()
        {
            if (newObjectController != null)
            {
                newObjectController.ObjectCreated -= newObjectController_ObjectCreated;
            }
            base.OnDeactivated();
        }

        private void newObjectController_ObjectCreated(object sender, ObjectCreatedEventArgs e)
        {
            NestedFrame nestedFrame = Frame as NestedFrame;
            if (nestedFrame != null)
            {
                Tercero.Module.BusinessObjects.Tercero createdItem = e.CreatedObject as Tercero.Module.BusinessObjects.Tercero;
                if (createdItem != null)
                {
                    var parent = ((NestedFrame)Frame).ViewItem.CurrentObject as Paciente;
                    if (parent != null)
                    {
                        createdItem.TipoPersona = Base.Module.BusinessObjects.TipoPersona.Juridica;
                        createdItem.Activo = true;
                        createdItem.Origen = ETerceroOrigen.Nacional;
                        createdItem.Clasificacion = Base.Module.BusinessObjects.EClasificacionContribuyente.Otro;
                        var fTerceroRole = e.ObjectSpace.CreateObject<TerceroRole>();
                        fTerceroRole.IdRole = TipoRoleTercero.Aseguradora;
                        fTerceroRole.Activo = true;
                        createdItem.Roles.Add(fTerceroRole);
                    }
                }
            }
        }
    }
}
