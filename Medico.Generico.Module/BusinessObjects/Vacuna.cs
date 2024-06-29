using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    /// <summary>
    /// BO que corresponde a las vacunas. Se relaciona con el BO Medicamentos y por lo tanto a Producto
    /// </summary>
    [DefaultClassOptions, Persistent("Vacuna"), DefaultProperty(nameof(Nombre)), ModelDefault("Caption", "Vacuna"), NavigationItem("Salud"),]
    [ImageName(nameof(Vacuna)), CreatableItem(false)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Vacuna : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Vacuna(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        Medicamento medicamento;
        string comentario;
        string descripcionCPT;
        string nombre;
        string codigoCVX;
        string codigoPT;


        [Association("Medicamento-Vacunas"), DbType("int"), Persistent("Medicamento"), XafDisplayName("Medicamento"),
            RuleRequiredField("Vacuna.Medicamento_Requerido", "Save")]
        public Medicamento Medicamento
        {
            get => medicamento;
            set => SetPropertyValue(nameof(Medicamento), ref medicamento, value);
        }

        [Size(8), DbType("varchar(8)"), Persistent("CodigoPT"), XafDisplayName("Código PT"),
            RuleRequiredField("Vacuna.CodigoPT_Requerido", "Save"), VisibleInListView(false),
            RuleUniqueValue("Vacuna.CodigoPT_Unico", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction,
                                                     SkipNullOrEmptyValues = false, ResultType = ValidationResultType.Error)]
        public string Codigo
        {
            get => codigoPT;
            set => SetPropertyValue(nameof(Codigo), ref codigoPT, value);
        }


        [Size(6), DbType("varchar(6)"), Persistent("CodigoCVX"), XafDisplayName("Código CVX"), VisibleInListView(false),
            RuleRequiredField("Vacuna.CodigoCVX_Requerido", DefaultContexts.Save)]
        public string CodigoCVX
        {
            get => codigoCVX;
            set => SetPropertyValue(nameof(CodigoCVX), ref codigoCVX, value);
        }


        [Size(100), DbType("varchar(100)"), Persistent("Nombre"), XafDisplayName("Nombre"),
            RuleRequiredField("Vacuna.Nombre_Requerido", DefaultContexts.Save)]
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }


        [Size(200), DbType("varchar(250)"), Persistent("DescripcionCPT"), XafDisplayName("Descripción CPT")]
        public string DescripcionCPT
        {
            get => descripcionCPT;
            set => SetPropertyValue(nameof(DescripcionCPT), ref descripcionCPT, value);
        }


        [Size(150), DbType("varchar(150)"), Persistent("Comentario"), XafDisplayName("Comentario"), VisibleInListView(false)]
        public string Comentario
        {
            get => comentario;
            set => SetPropertyValue(nameof(Comentario), ref comentario, value);
        }

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}