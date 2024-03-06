using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace SBT.Apps.Medico.Expediente.Module.BusinessObjects
{
    [DefaultClassOptions, NavigationItem(false), CreatableItem(false), ModelDefault("Caption", "Consulta Nutrición")]
    [DefaultProperty(nameof(Fecha))]
    [Persistent(nameof(ConsultaNutricion))]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ConsultaNutricion : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        private Paciente paciente;
        private decimal glucosa;
        private decimal colesterol;
        private decimal triglicerido;
        private decimal acidoUrico;
        private decimal creatinina;
        private decimal hemoglobina;
        private string otros;
        private decimal peso;
        private decimal talla;
        private decimal cmb;
        private decimal cc;
        private decimal grasaCorporal;
        private decimal grasaVisceral;
        private decimal muscular;
        private decimal aguaCorporal;
        private decimal edadMetabolica;
        private decimal imc;
        private string clasificacion;
        private string diagnosticoNutricional;
        private decimal icc;
        private string observacion;
        private string frecuenciaAlimento;
        private string consumoAgua;
        private decimal pesoMeta;
        private decimal pesoIdeal;
        private decimal pesoUsual;
        private decimal caloria;
        private decimal cho;
        private decimal chon;
        private decimal cooh;
        private string dietaPrescrita;
        private string planNutricional;
        private DateTime fecha;

        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public ConsultaNutricion(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        [DetailViewLayout("Datos Generales")]
        [Association("Paciente_ConsultaNutricion")]
        public Paciente Paciente
        {
            get => paciente;
            set => SetPropertyValue(nameof(Paciente), ref paciente, value);
        }

        [DetailViewLayout("Datos Generales")]
        [DbType("datetime")]
        [ModelDefault("EditMask", "dd/MM/yyyy"), ModelDefault("DisplayFormat", "{0:dd/MM/yyy}")]
        public DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
        }

        [DetailViewLayout("Exámenes de Laboratorio")]
        [ModelDefault("EditMask", "n2"), ModelDefault("DisplayFormat", "{0:N2}")]
        [DbType("numeric(9,4)")]
        public decimal Glucosa
        {
            get => glucosa;
            set => SetPropertyValue(nameof(Glucosa), ref glucosa, value);
        }

        [DetailViewLayout("Exámenes de Laboratorio")]
        [ModelDefault("EditMask", "n2"), ModelDefault("DisplayFormat", "{0:N2}")]
        [DbType("numeric(9,4)")]
        public decimal Colesterol
        {
            get => colesterol;
            set => SetPropertyValue(nameof(Colesterol), ref colesterol, value);
        }

        [DetailViewLayout("Exámenes de Laboratorio")]
        [System.ComponentModel.DisplayName("Triglicérido")]
        [ModelDefault("EditMask", "n2"), ModelDefault("DisplayFormat", "{0:N2}")]
        [DbType("numeric(9,4)")]
        public decimal Triglicerido
        {
            get => triglicerido;
            set => SetPropertyValue(nameof(Triglicerido), ref triglicerido, value);
        }

        [DetailViewLayout("Exámenes de Laboratorio")]
        [System.ComponentModel.DisplayName("Ácido Úrico")]
        [ModelDefault("EditMask", "n2"), ModelDefault("DisplayFormat", "{0:N2}")]
        [DbType("numeric(9,4)")]
        public decimal AcidoUrico
        {
            get => acidoUrico;
            set => SetPropertyValue(nameof(AcidoUrico), ref acidoUrico, value);
        }

        [DetailViewLayout("Exámenes de Laboratorio")]
        [ModelDefault("EditMask", "n2"), ModelDefault("DisplayFormat", "{0:N2}")]
        [DbType("numeric(9,4)")]
        public decimal Creatinina
        {
            get => creatinina;
            set => SetPropertyValue(nameof(Creatinina), ref creatinina, value);
        }

        [DetailViewLayout("Exámenes de Laboratorio")]
        [ModelDefault("EditMask", "n2"), ModelDefault("DisplayFormat", "{0:N2}")]
        [DbType("numeric(9,4)")]
        public decimal Hemoglobina
        {
            get => hemoglobina;
            set => SetPropertyValue(nameof(Hemoglobina), ref hemoglobina, value);
        }

        [DetailViewLayout("Exámenes de Laboratorio")]
        [Size(100), DbType("varchar(100)")]
        public string Otros
        {
            get => otros;
            set => SetPropertyValue(nameof(Otros), ref otros, value);
        }

        [DetailViewLayout("Datos Antropométrica")]
        [ModelDefault("EditMask", "n4"), ModelDefault("DisplayFormat", "{0:N4}")]
        [DbType("numeric(6,2)")]
        public decimal Peso
        {
            get => peso;
            set => SetPropertyValue(nameof(Peso), ref peso, value);
        }

        [DetailViewLayout("Datos Antropométrica")]
        [ModelDefault("EditMask", "n4"), ModelDefault("DisplayFormat", "{0:N4}")]
        [PersistentAlias("Round([Peso] * 2.20462, 2)")]
        public decimal PesoLibra => Convert.ToDecimal(EvaluateAlias(nameof(PesoLibra)));

        [DetailViewLayout("Datos Antropométrica")]
        [ModelDefault("EditMask", "n4"), ModelDefault("DisplayFormat", "{0:N4}")]
        [DbType("numeric(6,2)")]

        public decimal Talla
        {
            get => talla;
            set => SetPropertyValue(nameof(Talla), ref talla, value);
        }

        [DetailViewLayout("Datos Antropométrica")]
        [ToolTip("Circunferencia media del brazo")]
        [ModelDefault("EditMask", "n4"), ModelDefault("DisplayFormat", "{0:N4}")]
        [DbType("numeric(9,4)")]
        public decimal Cmb
        {
            get => cmb;
            set => SetPropertyValue(nameof(Cmb), ref cmb, value);
        }

        [DetailViewLayout("Datos Antropométrica")]
        [System.ComponentModel.DisplayName("Circunferencia Cintura")]
        [ModelDefault("EditMask", "n4"), ModelDefault("DisplayFormat", "{0:N4}")]
        [DbType("numeric(9,4)")]
        public decimal CC
        {
            get => cc;
            set => SetPropertyValue(nameof(CC), ref cc, value);
        }

        [DetailViewLayout("Datos Antropométrica")]
        [System.ComponentModel.DisplayName("Indice Cintura Cadera")]
        [ModelDefault("EditMask", "n4"), ModelDefault("DisplayFormat", "{0:N4}")]
        [DbType("numeric(9,4)")]
        public decimal ICC
        {
            get => icc;
            set => SetPropertyValue(nameof(ICC), ref icc, value);
        }

        [DetailViewLayout("Bioimpedancia")]
        [System.ComponentModel.DisplayName("Porcentaje Grasa Corporal")]
        [ModelDefault("EditMask", "n4"), ModelDefault("DisplayFormat", "{0:N4}")]
        [DbType("numeric(9,4)")]
        public decimal GrasaCorporal
        {
            get => grasaCorporal;
            set => SetPropertyValue(nameof(GrasaCorporal), ref grasaCorporal, value);
        }

        [DetailViewLayout("Bioimpedancia")]
        [System.ComponentModel.DisplayName("Gracia Visceral")]
        [ModelDefault("EditMask", "n4"), ModelDefault("DisplayFormat", "{0:N4}")]
        [DbType("numeric(9,4)")]
        public decimal GrasaVisceral
        {
            get => grasaVisceral;
            set => SetPropertyValue(nameof(GrasaVisceral), ref grasaVisceral, value);
        }

        [DetailViewLayout("Bioimpedancia")]
        [System.ComponentModel.DisplayName("Porcentaje Muscular")]
        [ModelDefault("EditMask", "n4"), ModelDefault("DisplayFormat", "{0:N4}")]
        [DbType("numeric(9,4)")]
        public decimal Muscular
        {
            get => muscular;
            set => SetPropertyValue(nameof(Muscular), ref muscular, value);
        }

        [DetailViewLayout("Bioimpedancia")]
        [System.ComponentModel.DisplayName("Agua Corporal")]
        [ModelDefault("EditMask", "n4"), ModelDefault("DisplayFormat", "{0:N4}")]
        [DbType("numeric(9,4)")]
        public decimal AguaCorporal
        {
            get => aguaCorporal;
            set => SetPropertyValue(nameof(AguaCorporal), ref aguaCorporal, value);
        }

        [DetailViewLayout("Bioimpedancia")]
        [System.ComponentModel.DisplayName("Edad Metabólica")]
        [ModelDefault("EditMask", "n4"), ModelDefault("DisplayFormat", "{0:N4}")]
        [DbType("numeric(9,4)")]
        public decimal EdadMetabolica
        {
            get => edadMetabolica;
            set => SetPropertyValue(nameof(EdadMetabolica), ref edadMetabolica, value);
        }

        [DetailViewLayout("Evaluación Nutricional")]
        [DbType("numeric(9,4)")]
        [System.ComponentModel.DisplayName("Valor del IMC")]
        public decimal IMC
        {
            get => imc;
            set => SetPropertyValue(nameof(IMC), ref imc, value);
        }

        [DetailViewLayout("Evaluación Nutricional")]
        [Size(50), DbType("varchar(50)")]
        public string Clasificacion
        {
            get => clasificacion;
            set => SetPropertyValue(nameof(Clasificacion), ref clasificacion, value);
        }

        [DetailViewLayout("Evaluación Nutricional")]
        [Size(50), DbType("varchar(50)")]
        public string DiagnosticoNutricional
        {
            get => diagnosticoNutricional;
            set => SetPropertyValue(nameof(DiagnosticoNutricional), ref diagnosticoNutricional, value);
        }

        [DetailViewLayout("Evaluación Nutricional")]
        [Size(250), DbType("varchar(250)")]
        [System.ComponentModel.DisplayName("Observación")]
        public string Observacion
        {
            get => observacion;
            set => SetPropertyValue(nameof(Observacion), ref observacion, value);
        }

        [DetailViewLayout("Otros Datos")]
        [Size(250), DbType("varchar(250)")]
        [System.ComponentModel.DisplayName("Frecuencia Alimentos")]
        [ToolTip("Frecuencia de alimentos y alimentos que no le gustan")]
        public string FrecuenciaAlimento
        {
            get => frecuenciaAlimento;
            set => SetPropertyValue(nameof(FrecuenciaAlimento), ref frecuenciaAlimento, value);
        }

        [DetailViewLayout("Otros Datos")]
        [ToolTip("Consumo de agua al día")]
        public string ConsumoAgua
        {
            get => consumoAgua;
            set => SetPropertyValue(nameof(ConsumoAgua), ref consumoAgua, value);
        }

        [DetailViewLayout("Otros Datos")]
        [ModelDefault("EditMask", "n2"), ModelDefault("DisplayFormat", "{0:N2}")]
        [DbType("numeric(6,2)")]
        public decimal PesoMeta
        {
            get => pesoMeta;
            set => SetPropertyValue(nameof(PesoMeta), ref pesoMeta, value);
        }

        [DetailViewLayout("Otros Datos")]
        [ModelDefault("EditMask", "n2"), ModelDefault("DisplayFormat", "{0:N2}")]
        [DbType("numeric(6,2)")]
        public decimal PesoIdeal
        {
            get => pesoIdeal;
            set => SetPropertyValue(nameof(PesoIdeal), ref pesoIdeal, value);
        }

        [DetailViewLayout("Otros Datos")]
        [ModelDefault("EditMask", "n2"), ModelDefault("DisplayFormat", "{0:N2}")]
        [DbType("numeric(6,2)")]
        public decimal PesoUsual
        {
            get => pesoUsual;
            set => SetPropertyValue(nameof(PesoUsual), ref pesoUsual, value);
        }

        [DetailViewLayout("Otros Datos")]
        public decimal Caloria
        {
            get => caloria;
            set => SetPropertyValue(nameof(Caloria), ref caloria, value);
        }

        [DetailViewLayout("Otros Datos")]
        [System.ComponentModel.DisplayName("Carbohidratos")]
        [ModelDefault("EditMask", "n2"), ModelDefault("DisplayFormat", "{0:N2}")]
        [DbType("numeric(6,2)")]
        public decimal CHO
        {
            get => cho;
            set => SetPropertyValue(nameof(CHO), ref cho, value);
        }

        [DetailViewLayout("Otros Datos")]
        [System.ComponentModel.DisplayName("CHON")]
        [ModelDefault("EditMask", "n2"), ModelDefault("DisplayFormat", "{0:N2}")]
        [DbType("numeric(6,2)")]
        [ToolTip("Carbono, Hidrógeno, Oxígeno y Nitrógeno")]
        public decimal CHON
        {
            get => chon;
            set => SetPropertyValue(nameof(CHON), ref chon, value);
        }

        [DetailViewLayout("Otros Datos")]
        [System.ComponentModel.DisplayName("COOH")]
        [ModelDefault("EditMask", "n2"), ModelDefault("DisplayFormat", "{0:N2}")]
        [DbType("numeric(6,2)")]
        [ToolTip("Ácidos Orgánicos")]
        public decimal COOH
        {
            get => cooh;
            set => SetPropertyValue(nameof(COOH), ref cooh, value);
        }

        [DetailViewLayout("Otros Datos")]
        [Size(60), DbType("varchar(60)")]
        [System.ComponentModel.DisplayName("Dieta Prescrita")]
        public string DietaPrescrita
        {
            get => dietaPrescrita;
            set => SetPropertyValue(nameof(DietaPrescrita), ref dietaPrescrita, value);
        }

        [DetailViewLayout("Otros Datos")]
        [Size(250), DbType("varchar(250)")]
        [System.ComponentModel.DisplayName("Plan Nutricional")]
        public string PlanNutricional
        {
            get => planNutricional;
            set => SetPropertyValue(nameof(PlanNutricional), ref planNutricional, value);
        }

        #endregion

        #region collecciones

        [Association("ConsultaNutricion_Recuento24H"), DevExpress.Xpo.Aggregated]
        [System.ComponentModel.DisplayName("Recuentos 24H")]
        public XPCollection<NutricionRecuento24H> Recuentos24H => GetCollection<NutricionRecuento24H>(nameof(Recuentos24H));

        #endregion

        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue(nameof(PersistentProperty), ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}