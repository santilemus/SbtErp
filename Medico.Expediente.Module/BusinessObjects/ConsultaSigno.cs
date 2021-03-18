using System;
using System.ComponentModel;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Medico.Generico.Module.BusinessObjects;

namespace SBT.Apps.Medico.Expediente.Module.BusinessObjects
{
    /// <summary>
    /// Informacion de los signos vitales y especificos para el problema medico del paciente, tomados durante la consulta
    /// </summary>
    [DefaultClassOptions, ModelDefault("Caption", "Signo"), NavigationItem(false), CreatableItem(false), DefaultProperty(nameof(Signo)),
     Persistent(nameof(ConsultaSigno))]
    [ImageName("ConsultaSigno")]
    [Appearance("ConsultaSigno.Imc_Hide", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any",
        Criteria = "[Signo.Oid] != 9 || [Edad] < 20", TargetItems = "Imc")]
    // [Appearance("ConsultaSigno.Imc_Show", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show, Context = "Any",
    //     Criteria = "[Signo.Oid] == 9 && [Edad] > 20", TargetItems = "Imc")]
    [Appearance("ConsultaSigno.Percentil_Hide", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any",
        Criteria = "[EdadMeses] >= 240 && !([Signo.Oid] In (1, 2, 9))", TargetItems = "PercentilTablaDetalle")]
    //[Appearance("ConsultaSigno.Percentil_Show", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show, Context = "Any",
    //    Criteria = "[EdadMeses] < 240 && [Signo.Oid] In (1, 2, 9)", TargetItems = "PercentilTablaDetalle")]
    public class ConsultaSigno : XPObjectBaseBO
    {

        public ConsultaSigno(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        protected override void OnSaving()
        {
            base.OnSaving();
        }


        #region Propiedades
        [Persistent(nameof(PercentilTablaDetalle)), ExplicitLoading]
        PercentilTablaDetalle percentilTablaDetalle;
        [Persistent(nameof(Imc)), ExplicitLoading]
        TablaIMC imc;
        private Consulta consulta;
        private Signo signo;
        private decimal valor;

        [Association("ConsultaSigno-Consulta")]
        public Consulta Consulta
        {
            get => consulta;
            set => SetPropertyValue(nameof(Consulta), ref consulta, value);
        }

        [ImmediatePostData(true), RuleRequiredField("ConsultaSigno.Signo_Requerido", DefaultContexts.Save)]
        [ExplicitLoading]
        public Signo Signo
        {
            get => signo;
            set
            {
                bool changed = SetPropertyValue(nameof(Signo), ref signo, value);
                if (!IsLoading && !IsSaving && changed)
                {
                    ActualizarPercentil(true);
                    if (Signo.Oid == 9)
                    {
                        decimal oldValue = Valor;
                        Valor = CalcularIMC();
                        OnChanged(nameof(Valor), oldValue, Valor);
                    }
                }
            }
        }

        [RuleRequiredField("ConsultaSigno.Valor_Requerido", DefaultContexts.Save)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [DbType("numeric(12,2)"), ImmediatePostData(true)]
        public decimal Valor
        {
            get => valor;
            set
            {
                bool changed = SetPropertyValue(nameof(valor), ref valor, value);
                if (!IsLoading && !IsSaving && changed && Signo.Oid == 9 && Consulta.Paciente.EdadMeses > 228)
                    ActualizarRelacionTablaImc(true);
            }
        }

        [PersistentAlias("[Signo.Unidad]"), XafDisplayName("Unidad")]
        public string Unidad => Convert.ToString(EvaluateAlias(nameof(Unidad)));


        [PersistentAlias("[Consulta.Paciente.EdadMeses]")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        public decimal EdadMeses => Convert.ToDecimal(EvaluateAlias(nameof(EdadMeses)));

        [PersistentAlias("[Consulta.Paciente.Edad]")]
        [ModelDefault("DisplayFormat", "{0:N2}")]
        public decimal Edad => Convert.ToDecimal(EvaluateAlias(nameof(Edad)));

        [PersistentAlias(nameof(imc)), XafDisplayName("IMC")]
        public TablaIMC Imc => imc;

        [PersistentAlias(nameof(percentilTablaDetalle)), XafDisplayName("Percentil Oid")]
        public PercentilTablaDetalle PercentilTablaDetalle => percentilTablaDetalle;

        #endregion

        #region Metodos
        private void ActualizarPercentil(bool forceChangeEvents)
        {
            if (Consulta.Paciente.EdadMeses > 228)
                return;
            PercentilTablaDetalle pd = Session.FindObject<PercentilTablaDetalle>(
                CriteriaOperator.Parse("[PercentilTabla.Signo.Oid] == ? && [PercentilTabla.Genero] == ? && [EdadMes] == ?", 
                Signo.Oid, Consulta.Paciente.Genero, Consulta.Paciente.EdadMeses));
            PercentilTablaDetalle old = PercentilTablaDetalle;
            percentilTablaDetalle = pd;
            if (forceChangeEvents)
                OnChanged(nameof(PercentilTablaDetalle), old, PercentilTablaDetalle);
        }

        /// <summary>
        /// Calcular el IMC de una persona de los dos años en adelante. Utiliza los datos que ya estan registrados en los signos para hacerlo, 
        /// aplicando la siguiente formula: IMC = Peso (kg) / Estatura² (m2)
        /// Al calcular el IMC se inserta el correspondiente objeto en la colleccion signos
        /// </summary>
        /// <remarks>
        /// de los 2 a los 19 años, ver instrucciones en (considerar que las tablas del sistema corresponde a Who
        /// (World Health Organization)
        /// https://www.cdc.gov/nccdphp/dnpao/growthcharts/resources/growthchart.pdf
        /// https://apps.who.int/iris/bitstream/handle/10665/43601/9789241595070_BoysGrowth_eng.pdf?sequence=9&isAllowed=y
        ///  BMI (body mass index) = weight in kilograms divided by length or height in meters squared (kg/m2);
        /// </remarks>
        /// <returns>El IMc calculado para pacientes de 24 meses (2 años) o mas </returns>
        public decimal CalcularIMC()
        {
            ConsultaSigno peso = Consulta.ConsultaSignos.FirstOrDefault<ConsultaSigno>(item => item.Signo.Oid == 1);
            ConsultaSigno estatura = Consulta.ConsultaSignos.FirstOrDefault<ConsultaSigno>(item => item.Signo.Oid == 2);
            //if (Consulta.Paciente.EdadMeses <= 24.00m || peso == null || estatura == null)
            if (peso == null || estatura == null)
                return 0.0m;
            //BMI(body mass index) = weight in kilograms divided by length or height in meters squared(kg/ m2);
            return peso.Valor / Convert.ToDecimal(Math.Pow((double)estatura.Valor / 100.00, 2.0));
        }

        private void ActualizarRelacionTablaImc(bool fForceChangeEvents)
        {
            TablaIMC obj = Session.FindObject<TablaIMC>(CriteriaOperator.Parse("? >= [Desde] && ? <= [Hasta]", Valor, Valor));
            TablaIMC old = imc;
            imc = obj;
            if (fForceChangeEvents)
                OnChanged(nameof(Imc), old, imc);
        }

        #endregion

    }

}