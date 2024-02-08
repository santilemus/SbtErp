using DevExpress.ExpressApp.Validation;
using DevExpress.ExpressApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBT.Apps.Base.Module
{
    /// <summary>
    /// Para implementar la ejecución de validaciones en codigo en los viewcontroller
    /// </summary>
    /// <remarks>Se deja acá para que este disponible para todos los projectos</remarks>
    public class ImmediateValidationTargetObjectsSelector : ValidationTargetObjectSelector
    {
        protected override bool NeedToValidateObject(IObjectSpace objectSpace, object targetObject)
        {
            return true;
        }
    }
}
