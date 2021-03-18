using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Linq;

namespace SBT.Apps.Base.Module
{
    /// <summary>
    /// Implementar función personalizada que retorna el Oid de la sucursal actual (sucursal de la sesion)
    /// </summary>
    public class AgenciaActualOidFunction : ICustomFunctionOperatorBrowsable
    {
        #region Implementacion de ICustomFunctionOperator
        /// <summary>
        /// Propiedad con el nombre de la funcion personalizada
        /// </summary>
        public string Name
        {
            get
            {
                return "AgenciaActualOid";
            }
        }


        /// <summary>
        /// Evalua el codigo (expresion) que calcula el valor a retornar por la funcion personalizada cuyo nombre se define en Name
        /// </summary>
        /// <param name="operands"></param>
        /// <returns>Retorna el valor calculado por la funcion</returns>
        public object Evaluate(params object[] operands)
        {
            return ((Usuario)SecuritySystem.CurrentUser).Agencia.Oid;
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
        static AgenciaActualOidFunction()
        {
            AgenciaActualOidFunction instance = new AgenciaActualOidFunction();
            if (CriteriaOperator.GetCustomFunction(instance.Name) == null)
            {
                CriteriaOperator.RegisterCustomFunction(instance);
            }
        }

        #endregion

        #region Implementacion de  ICustomFunctionOperatorBrowsable
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
            get { return $"AgenciaActualOid(){Environment.NewLine}Retorna el Oid de la agencia o sucursal de la sesion actual (seleccionada en LogIn"; }
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
