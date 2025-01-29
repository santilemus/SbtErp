using DevExpress.Persistent.Validation;
using SBT.Apps.Banco.Module.BusinessObjects;
using SBT.Apps.CxP.Module.BusinessObjects;
using System;

namespace SBT.Apps.Erp.Module.Validations
{
    [CodeRule]
    public class BancoTransaccionCxPReferenceCodeRule: RuleIsReferenced
    {
        public BancoTransaccionCxPReferenceCodeRule(): base("BancoTransaccion_Reference_CxP", ContextIdentifier.Delete, typeof(BancoTransaccion))
        {
            Properties.CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction;
            Properties.LooksFor = typeof(CxPTransaccion);
            Properties.InvertResult = true;
            Properties.ReferencePropertyName = @"BancoTransaccion";
            Properties.CustomMessageTemplate = @"No puede borrar el objeto '{TargetObject}' porque se utiliza en la aplicación";
        }

        public BancoTransaccionCxPReferenceCodeRule(IRuleIsReferencedProperties properties) : base(properties)
        {
            Properties.Id = "BancoTransaccion_Reference_CxP";
            Properties.CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction;
            Properties.LooksFor = typeof(CxPTransaccion);
            Properties.InvertResult = true;
            Properties.ReferencePropertyName = @"BancoTransaccion";
            Properties.CustomMessageTemplate = @"No puede borrar el objeto '{TargetObject}' porque se utiliza en la aplicación";
        }

        public BancoTransaccionCxPReferenceCodeRule(string id, ContextIdentifiers targetContextIDs, Type objectType) :
            base("Proveedor_Referencia_LibroCompra", ContextIdentifier.Delete, typeof(Tercero.Module.BusinessObjects.Tercero))
        {
            Properties.CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction;
            Properties.LooksFor = typeof(CxPTransaccion);
            Properties.InvertResult = true;
            Properties.ReferencePropertyName = @"BancoTransaccion";
            Properties.CustomMessageTemplate = @"No puede borrar el objeto '{TargetObject}' porque se utiliza en la aplicación";
        }
    }
}
