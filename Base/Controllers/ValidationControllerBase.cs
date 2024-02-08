using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Validation;
using System.Collections;
using DevExpress.Persistent.Validation;

namespace SBT.Apps.Base.Module.Controllers
{
    /// <summary>
    /// View Controller base para BO en los cuales se necesita comprobar las reglas cuando se activa la vista o se cambia el objeto de la vista. 
    /// Para ello se implementa el ImmediateValidationController. Para evitar verificar estas reglas cuando el objeto comercial se guarda o elimina, 
    /// su contexto se establece en custom
    /// </summary>
    /// <remarks>
    /// mas info en https://github.com/DevExpress-Examples/XAF_validation-how-to-highlight-invalid-properties-when-the-view-is-shown-e1524/tree/2c1107b106afb51e1fefecaf6b88b4caea1fa8c9
    /// </remarks>
    public class ValidationControllerBase : ViewControllerBase
    {
        public ValidationControllerBase() : base()
        {

        }
        protected override void OnActivated()
        {
            base.OnActivated();
            ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
            ObjectSpace.ObjectReloaded += ObjectSpace_ObjectReloaded;
            View.CurrentObjectChanged += View_CurrentObjectChanged;
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            ValidateViewObjects();
        }
        void View_CurrentObjectChanged(object sender, EventArgs e)
        {
            ValidateViewObjects();
        }
        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            ValidateViewObjects();
        }
        private void ObjectSpace_ObjectReloaded(object sender, ObjectManipulatingEventArgs e)
        {
            ValidateViewObjects();
        }
        private void ValidateViewObjects()
        {
            if (View is ListView view)
            {
                if (!view.CollectionSource.IsServerMode)
                {
                    ValidateObjects(view.CollectionSource.List);
                }
            }
            else if (View is DetailView)
            {
                ImmediateValidationTargetObjectsSelector objectsSelector = new ();
                ValidateObjects(objectsSelector.GetObjectsToValidate(View.ObjectSpace, View.CurrentObject));
            }
        }
        private void ValidateObjects(IEnumerable targets)
        {
            if (targets == null) return;
            ResultsHighlightController resultsHighlightController = Frame.GetController<ResultsHighlightController>();
            if (resultsHighlightController != null)
            {
                IRuleSet ruleSet = Validator.GetService(Application.ServiceProvider);
                if (ruleSet != null)
                {
                    RuleSetValidationResult result = ruleSet.ValidateAllTargets(ObjectSpace, targets, DefaultContexts.Save);
                    if (result.ValidationOutcome == ValidationOutcome.Error || result.ValidationOutcome == ValidationOutcome.Warning || result.ValidationOutcome == ValidationOutcome.Information)
                    {
                        resultsHighlightController.HighlightResults(result);
                    }
                    else
                    {
                        resultsHighlightController.ClearHighlighting();
                    }
                }
            }
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
            ObjectSpace.ObjectReloaded -= ObjectSpace_ObjectReloaded;
            View.CurrentObjectChanged -= View_CurrentObjectChanged;
        }
    }
}
