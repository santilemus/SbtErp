using DevExpress.Persistent.Validation;
using SBT.Apps.Iva.Module.BusinessObjects;

namespace SBT.Apps.Erp.Module.Validations
{
    [CodeRule]
    public class BancoTransaccionCxPComparisonCodeRule: RuleValueComparison
    {
        private DevExpress.ExpressApp.DC.IMemberInfo MemberInfo { get; set; }
        public BancoTransaccionCxPComparisonCodeRule(): base()
        {
            Properties.TargetType = typeof(SBT.Apps.Banco.Module.BusinessObjects.BancoTransaccion);
            Properties.Id = "BancoTransaccion.Monto = Total CxP";
            Properties.OperatorType = ValueComparisonType.Equals;
            Properties.ResultType = ValidationResultType.Error;
            Properties.RightOperandExpression = @"[Pagos].Sum([Monto])";
            Properties.TargetCriteria = "[Pagos][].Exists()";
            Properties.CustomMessageTemplate = @"{TargetPropertyName} debe ser igual a la suma de los pagos";
        }

        public BancoTransaccionCxPComparisonCodeRule(IRuleValueComparisonProperties properties) : base(properties)
        {
            Properties.TargetType = typeof(SBT.Apps.Banco.Module.BusinessObjects.BancoTransaccion);
            Properties.Id = "BancoTransaccion.Monto = Total CxP";
            Properties.TargetCriteria = "[Pagos][].Exists()";
            Properties.CustomMessageTemplate = @"{TargetPropertyName} debe ser igual a la suma de los pagos";
        }
    }
}
