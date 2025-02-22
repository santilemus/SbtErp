﻿using DevExpress.Data.Filtering;
using SBT.Apps.Base.Module.BusinessObjects;
using System;

namespace SBT.Apps.Base.Module
{
    /// <summary>
    /// Implementar función personalizada que retorna el Oid del periodo de la sesion (periodo del sesion)
    /// PENDIENTE DE GUARDAR EN LA SESION EL PERIODO, para que esta funcion sea util
    /// </summary>
    public class PeriodoActualNumeroFunction : ICustomFunctionOperatorBrowsable
    {
        #region Implementacion de ICustomFunctionOperator
        /// <summary>
        /// Propiedad con el nombre de la funcion personalizada
        /// </summary>
        public string Name
        {
            get
            {
                return "PeriodoActualOid";
            }
        }

        /// <summary>
        /// Tipo de dato que retorna la funcion personalizada (codigo ejecutado en Evaluate)
        /// </summary>
        /// <param name="operands">Retorna el valor calculado por la funcion</param>
        /// <returns></returns>
        public object Evaluate(params object[] operands)
        {
            return DateTime.Now.Year;
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
        static PeriodoActualNumeroFunction()
        {
            PeriodoActualNumeroFunction instance = new PeriodoActualNumeroFunction();
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
            get { return $"PeriodoActualOid(){Environment.NewLine}Retorna el Oid del periodo de la sesion actual"; }
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
