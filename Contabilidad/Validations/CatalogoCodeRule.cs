using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Validation;
using SBT.Apps.Contabilidad.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SBT.Apps.Contabilidad.Module.Validations
{
    /// <summary>
    /// Implementar regla de validación en código 
    /// <br>más info en https://docs.devexpress.com/eXpressAppFramework/113051/validation/implement-custom-rules </br>
    /// </summary>
    [CodeRule]
    public class CatalogoCodeRule: RuleBase<Catalogo>
    {
        private string usedProperty;
        protected override bool IsValidInternal(Catalogo target, out string errorMessageTemplate)
        {
            errorMessageTemplate = string.Empty;
            usedProperty = "CodigoCuenta";
            if (!target.CtaResumen)
            {
                if (string.IsNullOrEmpty(target.CodigoCuenta))
                {
                    errorMessageTemplate = "Debe ingresar el Código de la Cuenta.";
                    return false;
                }
                // obtener las veces que la cuenta de detalle aparece como cuenta padre de otras. Debería ser cero si es diferente es un error
                IObjectSpace os = ((IObjectSpaceLink)this).ObjectSpace;
                CriteriaOperator criteria = CriteriaOperator.FromLambda<Catalogo>(x => x.Empresa.Oid == target.Empresa.Oid && x.Padre == target);
                int cantCtas = Convert.ToInt32(os.Evaluate(typeof(Catalogo), CriteriaOperator.Parse("Count()"), criteria));
                if (cantCtas > 0)
                {
                    errorMessageTemplate = $@"La Cuenta {target.Nombre} esta registrada como de detalle, pero hay {cantCtas} que dependen o son hijas...revisar";
                    return cantCtas == 0;
                }
            }
            return true;
        }

        public CatalogoCodeRule() : base("", "Save") 
        {
            
        }

        public CatalogoCodeRule(IRuleBaseProperties properties) : base(properties) { }
        public override ReadOnlyCollection<string> UsedProperties
        {
            get => new ReadOnlyCollection<string>(new List<string>() { "CodigoCuenta", "Nombre", "Padre" });
        }
    }
}

