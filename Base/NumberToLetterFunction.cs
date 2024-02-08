using DevExpress.Data.Filtering;
using System;

namespace SBT.Apps.Base.Module
{
    /// <summary>
    /// Implementar función personalizada que retorna el equivalente de una cantidad numérica en letras
    /// </summary>
    /// <remarks>
    /// Implementar la interface ICustomFunctionOperatorBrowsable, ademas de la interface ICustomFunctionOperator para hacer que
    /// la funcion este disponible para el usuario final en el editor de expresiones. 
    /// </remarks>
    public class NumberToLetterFunction : ICustomFunctionOperatorBrowsable
    {
        #region Implementacion de ICustomFunctionOperator
        /// <summary>
        /// Propiedad con el nombre de la funcion personalizada
        /// </summary>
        public string Name
        {
            get
            {
                return "NumberToLetter";
            }
        }

        /// <summary>
        /// Evalua el codigo (expresion) que calcula el valor a retornar por la funcion personalizada cuyo nombre se define en Name
        /// </summary>
        /// <param name="operands"></param>
        /// <returns>Retorna el valor calculado por la funcion</returns>
        public object Evaluate(params object[] operands)
        {
            NumeroALetras numLetter = new Base.Module.NumeroALetras();
            return numLetter.Convertir(Convert.ToDecimal(operands[0]), Convert.ToString(operands[1]));
        }

        /// <summary>
        /// Tipo de dato que retorna la funcion personalizada (codigo ejecutado en Evaluate)
        /// </summary>
        /// <param name="operands"></param>
        /// <returns></returns>
        public Type ResultType(params Type[] operands)
        {
            return typeof(string);
        }

        /// <summary>
        /// Constructor (estatico)
        /// </summary>
        static NumberToLetterFunction()
        {
            NumberToLetterFunction instance = new NumberToLetterFunction();
            if (CriteriaOperator.GetCustomFunction(instance.Name) == null)
            {
                CriteriaOperator.RegisterCustomFunction(instance);
            }
        }

        #endregion

        #region Implementacion de ICustomFunctionOperatorBrowsable
        public bool IsValidOperandCount(int count)
        {
            return count == 2;
        }

        public bool IsValidOperandType(int operandIndex, int operandCount, Type type)
        {
            return ((operandIndex == 0 && type.GetType() == typeof(decimal)) && (operandIndex == 1 && type.GetType() == typeof(string)) && operandCount == 2);
        }

        public int MinOperandCount
        {
            get { return 2; }
        }

        public int MaxOperandCount
        {
            get { return 2; }
        }

        public string Description
        {
            get { return $"{Name}() {Environment.NewLine}Retorna la cantidad del primer parametro convertida en letras y concatenado al segundo parametro que corresponde al plural de la moneda"; }
        }

        public FunctionCategory Category
        {
            get { return FunctionCategory.Text; }
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
