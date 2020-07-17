using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using SBT.Apps.Base.Module.BusinessObjects;

namespace SBT.Apps.Base.Module
{
    /// <summary>
    /// Implementar función personalizada que retorna el valor de un parámetro de inicio de sesión
    /// Utiliza para esta finalidad la clase estatica SessionDataHelper
    /// </summary>
    public class SessionParameterFunction: ICustomFunctionOperatorBrowsable
    {
        #region Implementacion de ICustomFunctionOperator
        /// <summary>
        /// Propiedad con el nombre de la funcion personalizada
        /// </summary>
        public string Name
        {
            get
            {
                return "GetSessionParameter";
            }
        }

        /// <summary>
        /// Evalua la expresion) para calcular y retornar el valor de un parametro de sesion
        /// </summary>
        /// <param name="operands"></param>
        /// <returns>Retorna el valor calculado por la funcion</returns>
        public object Evaluate(params object[] operands)
        {
            if (operands == null || operands.Length == 0)
                return null;
            return SesionDataHelper.ObtenerValor(Convert.ToString(operands[0]));
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
        static SessionParameterFunction()
        {
            SessionParameterFunction instance = new SessionParameterFunction();
            if (CriteriaOperator.GetCustomFunction(instance.Name) == null)
            {
                CriteriaOperator.RegisterCustomFunction(instance);
            }
        }


        #endregion

        #region Implementacion de  ICustomFunctionOperatorBrowsable
        public bool IsValidOperandCount(int count)
        {
            return count == 1;
        }

        public bool IsValidOperandType(int operandIndex, int operandCount, Type type)
        {
            return (operandCount == 1 && (operandIndex == 0 && type.GetType() == typeof(string)));
        }



        public int MinOperandCount
        {
            get { return 1; }
        }

        public int MaxOperandCount
        {
            get { return 1; }
        }

        public string Description
        {
            get { return "GetSessionParameter(Value)" + Environment.NewLine + "Retorna el valor del parametro de la sesion, cuyo nombre se recibe en el argumento value"; }
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
