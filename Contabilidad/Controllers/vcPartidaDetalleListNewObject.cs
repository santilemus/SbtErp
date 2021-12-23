using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Validation;
using SBT.Apps.Contabilidad.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBT.Apps.Contabilidad.Module.Controllers
{
    class vcPartidaDetalleListNewObject : ObjectViewController<ListView, PartidaDetalle>
    {
        private ListEditorNewObjectController listEditorNewObjectController;
        private NewItemRowListViewController newItemRowListViewController;
        private ListEditor editor;
        public vcPartidaDetalleListNewObject() : base()
        {

        }

        protected override void OnActivated()
        {
            base.OnActivated();
            listEditorNewObjectController = Frame.GetController<ListEditorNewObjectController>();
            listEditorNewObjectController.ViewControlsCreated += ListEditorNewObjectController_ViewControlsCreated;
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            editor = View.Editor;           
            if (editor != null)
            {
                editor.ValidateObject += Editor_ValidateObject;
                editor.NewObjectAdding += Editor_NewObjectAdding;
            }
        }

        private void Editor_NewObjectAdding(object sender, NewObjectAddingEventArgs e)
        {
            
        }

        private void Editor_ValidateObject(object sender, ValidateObjectEventArgs e)
        {
            if (ObjectSpace.ModifiedObjects.Count == 1 && View.ObjectSpace.IsNewObject(ObjectSpace.ModifiedObjects[0])) 
            {
                var reglas = Validator.RuleSet.GetRules(typeof(Partida), "Save");
                RuleSetValidationResult result = Validator.RuleSet.ValidateTarget(ObjectSpace, ObjectSpace.ModifiedObjects[0], reglas, "1er Detalle");
                if (result.ValidationOutcome > ValidationOutcome.Information)
                {
                    string sError = string.Empty;
                    foreach (var reglaRes in result.Results)
                        if (reglaRes.State == ValidationState.Invalid)
                            sError += reglaRes.ErrorMessage + Environment.NewLine;
                    if (!string.IsNullOrEmpty(sError))
                    {
                        MessageOptions options = new MessageOptions
                        {
                            Duration = 2000,
                            Message = sError,
                            Type = InformationType.Error,
                            
                        };
                        options.Web.Position = InformationPosition.Bottom;
                        options.Win.Caption = "Error";
                        options.Win.Type = WinMessageType.Flyout;
                        Application.ShowViewStrategy.ShowMessage(options);
                    }
                    e.Valid = false;
                }
                {
                    e.Valid = true;
                }
            }
        }

        /// <summary>
        /// Se utiliza este evento para excluir dos reglas (por su id) 
        /// </summary>
        /// <param name="sender">el objeto que dispara el evento</param>
        /// <param name="e">parametro para indicar las reglas que se van a ejecutar (en este caso se ignoran dos)</param>
        private void RuleSet_CustomNeedToValidateRule(object sender, CustomNeedToValidateRuleEventArgs e)
        {
            if (!e.Handled && !(e.Rule.Id == "Partida Cuadre" || e.Rule.Id == "XPOBaseDocs.Numero_Requerido"))
            {
                e.NeedToValidateRule = true;
                e.Handled = true;
            }
        }
    }

}
