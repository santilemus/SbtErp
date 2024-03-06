using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using static DevExpress.CodeParser.CodeStyle.Formatting.Rules;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Net.Mime.MediaTypeNames;

namespace SBT.Apps.Base.Module
{
    /// <summary>
    /// Implementar función personalizada que retorna el Codigo de la empresa seleccionada en en Login (empresa de la sesion)
    /// </summary>
    /// <remarks>
    /// Implementar la interface ICustomFunctionOperatorBrowsable, ademas de la interface ICustomFunctionOperator para hacer que
    /// la funcion este disponible para el usuario final en el editor de expresiones. Si quiere usar la funcion como un CriteriaOperator
    /// para los criterios del lado del servidor (consultar a la base de datos), implementar ademas, ICustomFunctionOperatorFormattable.
    /// </remarks>
    /// <cambios>
    /// 02/marzo/2024 por SELM
    /// Cambios por migracion a NET 6+ Blazor ASP.NET, porque las propiedades estaticas de SecuritySystem no funcionan en net 6+.
    /// Ejemplo: SecuritySystem.CurrentUser
    /// Mas información en:
    /// https://docs.devexpress.com/eXpressAppFramework/113480/filtering/in-list-view/custom-function-criteria-operators
    /// https://github.com/DevExpress-Examples/xaf-how-to-use-data-from-security-in-criterion/blob/23.1.6%2B/CS/XPO/CustomOperator/CustomOperator.Module/CurrentCompanyOidOperator.cs#L19
    /// </cambios>
    public class EmpresaActualOidFunction : ICustomFunctionOperatorBrowsable
    {
        public const string FUNCTION_NAME = "EmpresaActualOid";

        IObjectSpaceFactory objectSpaceFactory;
        private IObjectSpace objectSpace;

        #region Implementacion de ICustomFunctionOperator
        /// <summary>
        /// Propiedad con el nombre de la funcion personalizada
        /// </summary>
        public string Name
        {
            get
            {
                return FUNCTION_NAME;
            }
        }

        /// <summary>
        /// Evalua el codigo (expresion) que calcula el valor a retornar por la funcion personalizada cuyo nombre se define en Name
        /// </summary>
        /// <param name="operands"></param>
        /// <returns>Retorna el valor calculado por la funcion</returns>
        public object Evaluate(params object[] operands)
        {
            return CurrentOrgIdFunctionCore(SecuritySystem.Instance);

            //return ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid;
        }

        public static void Evaluate(CustomCriteriaOperatorPatcherContext context)
        {
            context.Result = new ConstantValue(CurrentOrgIdFunctionCore(context.Security));
        }

        /// <summary>
        /// Tipo de dato que retorna la funcion personalizada (codigo ejecutado en Evaluate)
        /// </summary>
        /// <param name="operands"></param>
        /// <returns></returns>
        public Type ResultType(params Type[] operands)
        {
            return typeof(object);
        }

        /// <summary>
        /// Constructor (estatico)
        /// </summary>
        static EmpresaActualOidFunction()
        {
            EmpresaActualOidFunction instance = new EmpresaActualOidFunction();
            if (CriteriaOperator.GetCustomFunction(instance.Name) == null)
            {
                CriteriaOperator.RegisterCustomFunction(instance);
            }
        }

        public static bool CanEvaluate(CustomCriteriaOperatorPatcherContext context)
        {
            if (context.Operator is FunctionOperator functionOperator)
            {
                return functionOperator.Operands.Count == 1 &&
                                FUNCTION_NAME.Equals((functionOperator.Operands[0] as ConstantValue)?.Value?.ToString(), StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }

        private static int? CurrentOrgIdFunctionCore(ISecurityStrategyBase security) => ((Usuario)security.User)?.Empresa?.Oid ?? null;

        #endregion

        #region Implementacion de ICustomFunctionOperatorBrowsable
        public bool IsValidOperandCount(int count)
        {
            return count == 0;
        }

        public bool IsValidOperandType(int operandIndex, int operandCount, Type type)
        {
            return true;
        }

        public int MinOperandCount
        {
            get { return 0; }
        }

        public int MaxOperandCount
        {
            get { return 0; }
        }

        public string Description
        {
            get { return $"EmpresaActualOid(){Environment.NewLine}Retorna el Oid de la empresa de la sesion actual (seleccionada en LogIn"; }
        }

        public FunctionCategory Category
        {
            get { return FunctionCategory.All; }
        }
        #endregion

        /// <summary>
        ///  Metodo statico para registrar la funcion
        /// </summary>
        public static void Register()
        {
        }
    }
}
