using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Validation;
using SBT.Apps.Base.Module.Controllers;
using SBT.Apps.Contabilidad.Module.BusinessObjects;
using System;

namespace SBT.Apps.Contabilidad.Module.Controllers
{
    /// <summary>
    /// ViewController que corresponde a la vista de detalle del BO PartidaDetalle
    /// </summary>
    public class vcPartidaDetalle : ViewControllerBase
    {
        private NewObjectViewController newController;
        public vcPartidaDetalle() : base()
        {

        }

        protected override void OnActivated()
        {
            base.OnActivated();
            newController = Frame.GetController<NewObjectViewController>();
            if (newController != null)
            {
                newController.ObjectCreating += NewController_ObjectCreating;
            }
        }

        protected override void OnDeactivated()
        {
            if (newController != null)
            {
                newController.ObjectCreating -= NewController_ObjectCreating;
            }
            base.OnDeactivated();
        }

        protected override void DoInitializeComponent()
        {
            base.DoInitializeComponent();
            TargetObjectType = typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.PartidaDetalle);
            //TargetViewId = "PartidaDetalle_ListView";
            //TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
        }

        /// <summary>
        /// Ejecuta las validaciones de la partida contable, cuando se crea la primera linea de detalle de la partida
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// View.ObjectSpace es el padre (Partida)
        /// e.ObjectSpace es el detalle (PartidaDetalle)
        /// </remarks>
        private void NewController_ObjectCreating(object sender, ObjectCreatingEventArgs e)
        {
            if (e.ObjectType == typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.PartidaDetalle) &&
                ObjectSpace.ModifiedObjects.Count == 1 && View.ObjectSpace.IsNewObject(ObjectSpace.ModifiedObjects[0])) /* solo el encabezado de la partida contable y es nueva*/
            {
                try
                {
                    RuleSet.CustomNeedToValidateRule += RuleSet_CustomNeedToValidateRule;
                    IRuleSet ruleSet = Validator.GetService(Application.ServiceProvider);
                    if (ruleSet != null)
                    {
                        var reglas = ruleSet.GetRules(typeof(Partida), DefaultContexts.Save);
                        RuleSetValidationResult result = ruleSet.ValidateTarget(View.ObjectSpace, View.ObjectSpace.ModifiedObjects[0], reglas, "Custom");
                        if (result.ValidationOutcome > ValidationOutcome.Information)
                        {
                            string sError = string.Empty;
                            foreach (var reglaRes in result.Results)
                                if (reglaRes.State == ValidationState.Invalid)
                                    sError += reglaRes.ErrorMessage + Environment.NewLine;
                            if (!string.IsNullOrEmpty(sError))
                                MostrarError(sError);
                            e.Cancel = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, 4000);
                    throw;
                }
                finally
                {
                    RuleSet.CustomNeedToValidateRule -= RuleSet_CustomNeedToValidateRule;
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
            if (!e.Handled && !(e.Rule.Id == "Partida Cuadre" || e.Rule.Id == "XPOBaseDocs.Numero_Requerido") ||
                e.Rule.Id == "Partida de Liquidación del ejercicio" || e.Rule.Id == "Partida.Tipo Liquidaciones manuales validas solo fin del periodo")
                e.NeedToValidateRule = false;
            else
                e.NeedToValidateRule = true;
            e.Handled = true;
        }
    }
}
