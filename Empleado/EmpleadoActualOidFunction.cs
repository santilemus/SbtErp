using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using SBT.Apps.Base.Module;
using SBT.Apps.Base.Module.BusinessObjects;
using System;

namespace SBT.Apps.Empleado.Module
{
    /// <summary>
    /// Implementar función personalizada que retorna el Oid del Empleado relacionado con el usuario actualmente logeado
    /// </summary>
    /// <remarks>
    /// Implementar la interface ICustomFunctionOperatorBrowsable, ademas de la interface ICustomFunctionOperator para hacer que
    /// la funcion este disponible para el usuario final en el editor de expresiones. Si quiere usar la funcion como un CriteriaOperator
    /// para los criterios del lado del servidor (consultar a la base de datos), implementar ademas, ICustomFunctionOperatorFormattable.
    /// </remarks>
    /// <cambios>
    /// 26/junio/2024 por SELM
    /// Cambios por migracion a NET 6+ Blazor ASP.NET, porque las propiedades estaticas de SecuritySystem no funcionan en net 6+.
    /// Ejemplo: SecuritySystem.CurrentUser
    /// Mas información en:
    /// https://docs.devexpress.com/eXpressAppFramework/113480/filtering/in-list-view/custom-function-criteria-operators
    /// https://github.com/DevExpress-Examples/xaf-how-to-use-data-from-security-in-criterion/blob/23.1.6%2B/CS/XPO/CustomOperator/CustomOperator.Module/CurrentCompanyOidOperator.cs#L19
    /// Además basado en el cambio de EmpresaActualOidFunction que ya implementa la funcionalidad descrita en los enlaces anteriores y ha sido probada por varios meses
    /// </cambios>
    public class EmpleadoActualOidFunction : ICustomFunctionOperatorBrowsable
    {

        #region Implementacion ICustomFunctionOperator
        public const string FUNCTION_NAME = "EmpresaActualOid";
        public string Name => "EmpleadoActualOid";

        /// <summary>
        /// Evalua el codigo (expresion) que calcula el valor a retornar por la funcion personalizada cuyo nombre se define en Name
        /// </summary>
        /// <param name="operands"></param>
        /// <returns>Retorna el valor calculado por la funcion</returns>
        public object Evaluate(params object[] operands)
        {
            if (string.IsNullOrEmpty(SecuritySystem.CurrentUserName))
                return CurrentEmpleadoIdFunctionCore(SecuritySystem.Instance);
            else
            {
                Usuario usuario = ((Usuario)SecuritySystem.CurrentUser);
                var tInfo = usuario.ClassInfo;
                if (tInfo.FindMember("Empleado") != null)
                {
                    Empleado.Module.BusinessObjects.Empleado empleado = (Empleado.Module.BusinessObjects.Empleado)tInfo.GetMember("Empleado").GetValue(usuario);
                    return empleado?.Oid;
                }
                else
                    return null;
            }
        }

        public static void Evaluate(CustomCriteriaOperatorPatcherContext context)
        {
            context.Result = new ConstantValue(CurrentEmpleadoIdFunctionCore(context.Security));
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
        static EmpleadoActualOidFunction()
        {
            EmpleadoActualOidFunction instance = new EmpleadoActualOidFunction();
            if (CriteriaOperator.GetCustomFunction(instance.Name) == null)
            {
                CriteriaOperator.RegisterCustomFunction(instance);
            }
        }

        #endregion

        #region Implementacion de ICustomFunctionOperatorBrowsable

        public static bool CanEvaluate(CustomCriteriaOperatorPatcherContext context)
        {
            if (context.Operator is FunctionOperator functionOperator)
            {
                return functionOperator.Operands.Count == 1 &&
                                FUNCTION_NAME.Equals((functionOperator.Operands[0] as ConstantValue)?.Value?.ToString(), StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }

        private static int? CurrentEmpleadoIdFunctionCore(ISecurityStrategyBase security)
        {
            var usuario = (Usuario)security.User;
            var tInfo = usuario.ClassInfo;
            if (tInfo.FindMember("Empleado") != null)
            {
                Empleado.Module.BusinessObjects.Empleado empleado = (Empleado.Module.BusinessObjects.Empleado)tInfo.GetMember("Empleado").GetValue(usuario);
                return empleado?.Oid;
            }
            else
                return null;
        }


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
            get { return $"{FUNCTION_NAME}(){Environment.NewLine}Retorna el Oid del empleado vinculado al usuario logeado"; }
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
