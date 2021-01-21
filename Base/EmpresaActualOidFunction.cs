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
    /// Implementar función personalizada que retorna el Codigo de la empresa seleccionada en en Login (empresa de la sesion)
    /// </summary>
    /// <remarks>
    /// Implementar la interface ICustomFunctionOperatorBrowsable, ademas de la interface ICustomFunctionOperator para hacer que
    /// la funcion este disponible para el usuario final en el editor de expresiones. Si quiere usar la funcion como un CriteriaOperator
    /// para los criterios del lado del servidor (consultar a la base de datos), implementar ademas, ICustomFunctionOperatorFormattable.
    /// </remarks>
    public class EmpresaActualOidFunction: ICustomFunctionOperatorBrowsable
    {
        #region Implementacion de ICustomFunctionOperator
        /// <summary>
        /// Propiedad con el nombre de la funcion personalizada
        /// </summary>
        public string Name
        {
            get
            {
                return "EmpresaActualOid";
            }
        }

        /// <summary>
        /// Evalua el codigo (expresion) que calcula el valor a retornar por la funcion personalizada cuyo nombre se define en Name
        /// </summary>
        /// <param name="operands"></param>
        /// <returns>Retorna el valor calculado por la funcion</returns>
        public object Evaluate(params object[] operands)
        {
            return ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid;
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
