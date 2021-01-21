using DevExpress.Data.Filtering;
using SBT.Apps.RecursoHumano.Module.BusinessObjects;
using System;
using System.Linq;

namespace SBT.Apps.RecursoHumano.Module
{
    /// <summary>
    /// Implementar funcion para permitir en el editor de expresiones, invocar metodos del BO PlanillaDetalle, 
    /// necesarios para el calculo de la planilla
    /// </summary>
    /// <remarks>
    /// Implementar la interface ICustomFunctionOperatorBrowsable, ademas de la interface ICustomFunctionOperator para hacer que
    /// la funcion este disponible para el usuario final en el editor de expresiones. Si quiere usar la funcion como un CriteriaOperator
    /// para los criterios del lado del servidor (consultar a la base de datos), implementar ademas, ICustomFunctionOperatorFormattable.
    /// Ver: https://docs.devexpress.com/XPO/9948/examples/how-to-implement-custom-functions-and-criteria-in-linq-to-xpo
    ///      https://docs.devexpress.com/XPO/5206/examples/how-to-implement-a-custom-criteria-language-function-operator
    ///      https://docs.devexpress.com/eXpressAppFramework/113480/concepts/filtering/custom-function-criteria-operators
    /// </remarks>
    public class PlanillaEvaluarFunction : ICustomFunctionOperatorBrowsable
    {
        #region Implementacion de ICustomFunctionOperator
        /// <summary>
        /// Propiedad con el nombre de la funcion personalizada
        /// </summary>
        public string Name => "PlanillaEvaluar";

        /// <summary>
        /// Evalua el codigo (expresion) que calcula el valor a retornar por la funcion personalizada cuyo nombre se define en Name
        /// </summary>
        /// <param name="operands"></param>
        /// Arreglo con los operandos de la funcion. 
        /// El perimero es un objeto (instancia) del BO PlanillaDetalle 
        /// El segundo es el nombre del metodo de PlanillaDetalle a ejecutar
        /// El tercero en adelante son los parametros requeridos por el metodo
        /// <returns>Retorna el valor calculado por la funcion</returns>
        public object Evaluate(params object[] operands)
        {
            // aqui invocar la funcion correspondiente
            string nombreMetodo = Convert.ToString(operands[1]);
            if (operands[0].GetType() == typeof(SBT.Apps.RecursoHumano.Module.BusinessObjects.PlanillaDetalle) &&
                (operands[1].GetType() == typeof(string) && ExistMethod(nombreMetodo)) && operands[0] != null)
            {
                PlanillaDetalle fPlanillaDetalle = (SBT.Apps.RecursoHumano.Module.BusinessObjects.PlanillaDetalle)operands[0];
                object result = fPlanillaDetalle.GetType().GetMethod(nombreMetodo).Invoke(fPlanillaDetalle, new object[] { 3 });
                // generar excepcion aqui
                return result;
            }
            else
                return 0; // ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid;
        }

        private bool ExistMethod(string AMethodName)
        {
            Type t = typeof(SBT.Apps.RecursoHumano.Module.BusinessObjects.PlanillaDetalle);
            return (t.GetMethod(AMethodName) != null);
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


        static PlanillaEvaluarFunction()
        {
            PlanillaEvaluarFunction instance = new PlanillaEvaluarFunction();
            if (CriteriaOperator.GetCustomFunction(instance.Name) == null)
            {
                CriteriaOperator.RegisterCustomFunction(instance);
            }
        }

        #endregion

        #region Implementacion de  ICustomFunctionOperatorBrowsable
        public bool IsValidOperandCount(int count)
        {
            return count >= 1 && count <= 4;
        }

        /// <summary>
        /// Valida el primer operando sea un string, que deberia de corresponde al nombre de un metodo en el BO PlanillaDetalle
        /// Los siguientes operandos no hay forma de validarlos, porque pueden ser cualquier cantidad y tipo
        /// </summary>
        /// <param name="operandIndex">El Indice del operando a validar</param>
        /// <param name="operandCount">La cantidad de operandos</param>
        /// <param name="type">El tipo de dato del operando</param>
        /// <returns></returns>
        public bool IsValidOperandType(int operandIndex, int operandCount, Type type)
        {
            return ((operandIndex == 0 && type.GetType() == typeof(SBT.Apps.RecursoHumano.Module.BusinessObjects.PlanillaDetalle)) &&
                   (operandIndex == 1 && type.GetType() == typeof(string) && operandCount >= MinOperandCount && operandCount <= MaxOperandCount));
        }

        public int MinOperandCount
        {
            get { return 2; }

        }

        public int MaxOperandCount
        {
            get { return 4; }
        }

        public string Description
        {
            get { return $"{nameof(PlanillaEvaluarFunction)}(){System.Environment.NewLine}Retorna el valor de la funcion calculada en el primer parametro"; }
        }

        public FunctionCategory Category
        {
            get { return FunctionCategory.All; }
        }

        #endregion
    }
}
