using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Xpo;
using SBT.Apps.Contabilidad.Module.BusinessObjects;
using System;

namespace SBT.Apps.Contabilidad.Module
{
    public class SaldoDeCuentaFunction : ICustomFunctionOperatorBrowsable
    {
        #region Implementacion de ICustomFunctionOperator
        public string Name
        {
            get => "SaldoDeCuenta";
        }

        public object Evaluate(params object[] operands)
        {

            if (operands[0] is XPBaseObject)
            {
                IObjectSpace os = (operands[0] as IObjectSpace);
                CriteriaOperator criteria = CriteriaOperator.FromLambda<SaldoMes>(x => x.Cuenta.Empresa.Oid == (int)operands[1] &&
                                            x.Periodo.Oid == (int)operands[2] && x.Mes == (int)operands[3] && x.Cuenta.CodigoCuenta == (string)operands[4]);
                return Convert.ToDecimal(os.Evaluate(typeof(SaldoMes), CriteriaOperator.Parse("Sum([SaldoFin])"), criteria));
            }
            else
            {
                return 0.0m;
            }
        }

        public Type ResultType(params Type[] operands)
        {
            return typeof(decimal);
        }

        static SaldoDeCuentaFunction()
        {
            SaldoDeCuentaFunction instance = new SaldoDeCuentaFunction();
            if (CriteriaOperator.GetCustomFunction(instance.Name) == null)
            {
                CriteriaOperator.RegisterCustomFunction(instance);
            }
        }

        #endregion

        #region Implementacion de ICustomFunctionOperatorBrowsable
        public bool IsValidOperandCount(int count)
        {
            return count == 5;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operandIndex"></param>
        /// <param name="operandCount"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <remarks>
        /// operands[0] es el BO
        /// operands[1] es la empresa
        /// operands[2] es el Periodo
        /// operands[3] es el mes
        /// operands[4] es la cuenta
        /// </remarks>
        public bool IsValidOperandType(int operandIndex, int operandCount, Type type)
        {
            return ((operandIndex == 0 && type.GetType() == typeof(XPBaseObject)) && (operandIndex == 1 && type.GetType() == typeof(int)) &&
                    (operandIndex == 2 && type.GetType() == typeof(int)) && (operandIndex == 3 && type.GetType() == typeof(int)) &&
                    (operandIndex == 4 && type.GetType() == typeof(string)));
        }

        public int MinOperandCount
        {
            get { return 5; }
        }

        public int MaxOperandCount
        {
            get { return 5; }
        }

        public string Description
        {
            get { return $"{Name}() {Environment.NewLine}Retorna el saldo de una cuenta contable"; }
        }

        public FunctionCategory Category
        {
            get { return FunctionCategory.Text; }
        }

        #endregion
    }
}
