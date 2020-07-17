using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Empleado.Module.BusinessObjects;
using SBT.Apps.RecursoHumano.Module.BusinessObjects;

namespace SBT.Apps.RecursoHumano.Module
{
    /// <summary>
    /// Implementar clase para ejecutar funciones que calculan y retornan información relacionada con el empleado
    /// </summary>
    /// <remarks>
    /// Implementar la interface ICustomFunctionOperatorBrowsable, ademas de la interface ICustomFunctionOperator para hacer que
    /// la funcion este disponible para el usuario final en el editor de expresiones. Si quiere usar la funcion como un CriteriaOperator
    /// para los criterios del lado del servidor (consultar a la base de datos), implementar ademas, ICustomFunctionOperatorFormattable.
    /// Ver: https://docs.devexpress.com/XPO/9948/examples/how-to-implement-custom-functions-and-criteria-in-linq-to-xpo
    ///      https://docs.devexpress.com/XPO/5206/examples/how-to-implement-a-custom-criteria-language-function-operator
    ///      https://docs.devexpress.com/eXpressAppFramework/113480/concepts/filtering/custom-function-criteria-operators
    /// </remarks>
    public class EmpleadoFunction : ICustomFunctionOperatorBrowsable
    {
        #region Implementacion de ICustomFunctionOperator
        /// <summary>
        /// Propiedad con el nombre de la funcion personalizada
        /// </summary>
        public string Name
        {
            get
            {
                return "ObtenerDeEmpleado";
            }
        }

        /// <summary>
        /// Evalua el codigo (expresion) que calcula el valor a retornar por la funcion personalizada cuyo nombre se define en Name
        /// </summary>
        /// <param name="operands"></param>
        /// Arreglo con los operandos de la funcion. 
        /// El primero es el nombre de la función o metodo a ejecutar
        /// El segundo es el codigo del empleado
        /// El tercero en adelante son los parametros requeridos por la función a ejecutar.
        ///   donde el primero de esos parametros sera el Oid del Empleado
        /// <returns>Retorna el valor calculado por la funcion</returns>
        public object Evaluate(params object[] operands)
        {
            // aqui invocar la funcion correspondiente
            if (operands[0].GetType() == typeof(SBT.Apps.Empleado.Module.BusinessObjects.Empleado))
            {
                SBT.Apps.Empleado.Module.BusinessObjects.Empleado fEmpleado = (SBT.Apps.Empleado.Module.BusinessObjects.Empleado)operands[0];
                string methodName = Convert.ToString(operands[1]);
                object result = fEmpleado.GetType().GetMethod(methodName).Invoke(fEmpleado, new object[] { 6 });
                // generar excepcion aqui
                return result;
            }
            else 
                return 0; // ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid;
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

        static EmpleadoFunction()
        {
            EmpleadoFunction instance = new EmpleadoFunction();
            if (CriteriaOperator.GetCustomFunction(instance.Name) == null)
            {
                CriteriaOperator.RegisterCustomFunction(instance);
            }
        }

        #endregion

        #region Implementacion de  ICustomFunctionOperatorBrowsable
        public bool IsValidOperandCount(int count)
        {
            return count >= 2 && count <= 6;
        }

        public bool IsValidOperandType(int operandIndex, int operandCount, Type type)
        {
            return ((operandIndex == 0 && type.GetType() == typeof(SBT.Apps.Empleado.Module.BusinessObjects.Empleado)) 
                   && (operandIndex == 1 && type.GetType() == typeof(string)));
        }


        public int MinOperandCount
        {
            get { return 1; }

        }

        public int MaxOperandCount
        {
            get { return 9; }
        }

        public string Description
        {
            get { return "EmpleadoFunction()" + Environment.NewLine + "Retorna el valor de la funcion calculada en el primer parametro"; }
        }

        public FunctionCategory Category
        {
            get { return FunctionCategory.All; }
        }
        #endregion

    }
}
