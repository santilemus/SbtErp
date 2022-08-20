using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Empleado.Module.BusinessObjects;

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
    public class EmpleadoActualOidFunction : ICustomFunctionOperatorBrowsable
    {

        #region Implementacion ICustomFunctionOperator
        public string Name => "EmpleadoActualOid";

        public object Evaluate(params object[] operands)
        {
            Usuario usuario = ((Usuario)SecuritySystem.CurrentUser);
            var tInfo = usuario.ClassInfo;
            if (tInfo.FindMember("Empleado") != null)
            {
                Empleado.Module.BusinessObjects.Empleado empleado = (Empleado.Module.BusinessObjects.Empleado)tInfo.GetMember("Empleado").GetValue(usuario);
                return (empleado != null) ? empleado.Oid : -1;
            }
            else
                return -1;
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
            get { return $"EmpleadoActualOid(){Environment.NewLine}Retorna el Oid del empleado vinculado al usuario logeado"; }
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
