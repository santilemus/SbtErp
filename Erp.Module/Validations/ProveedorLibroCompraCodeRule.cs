using DevExpress.Persistent.Validation;
using SBT.Apps.Iva.Module.BusinessObjects;
using System;

namespace SBT.Apps.Erp.Module.Validations
{
    
    [CodeRule]
    public class ProveedorLibroCompraCodeRule: RuleIsReferenced
    {
        public ProveedorLibroCompraCodeRule(): base("Proveedor_Referencia_LibroCompra", ContextIdentifier.Delete, typeof(Tercero.Module.BusinessObjects.Tercero))
        {
            Properties.CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction;
            Properties.LooksFor = typeof(LibroCompra);
            Properties.InvertResult = true;
            Properties.ReferencePropertyName = @"Proveedor";
            Properties.CustomMessageTemplate = @"No puede borrar el objeto '{TargetObject}' porque se utiliza en la aplicación";
        }

        public ProveedorLibroCompraCodeRule(IRuleIsReferencedProperties properties): base(properties)
        {
            properties.Id = "Proveedor_Referencia_LibroCompra";
            properties.LooksFor = typeof(LibroCompra);
            properties.InvertResult = true;
            properties.ReferencePropertyName = @"Proveedor";
            properties.CustomMessageTemplate = @"No puede borrar el objeto '{TargetObject}' porque se utiliza en la aplicación";
        }
    
        public ProveedorLibroCompraCodeRule(string id, ContextIdentifiers targetContextIDs, Type objectType): 
            base("Proveedor_Referencia_LibroCompra", ContextIdentifier.Delete, typeof(Tercero.Module.BusinessObjects.Tercero))
        {
            Properties.CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction;
            Properties.LooksFor = typeof(LibroCompra);
            Properties.InvertResult = true;
            Properties.ReferencePropertyName = @"Proveedor";
            Properties.CustomMessageTemplate = @"No puede borrar el objeto '{TargetObject}' porque se utiliza en la aplicación";
        }

        /*
        protected bool IsValidInternal(Tercero.Module.BusinessObjects.Tercero target, out string errorMessageTemplate)
        {
            string lastSearchResults = "";
            bool flag = IsSearchedObjectsExist(target, typeof(LibroCompra), out lastSearchResults);
            errorMessageTemplate = Properties.MessageTemplateMustBeReferenced;
            if (flag)
            {
                errorMessageTemplate += GetFoundObjectsString(lastSearchResults);
            }
            return flag;
        }
        */
    }
}
