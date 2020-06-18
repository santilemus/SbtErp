using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using SBT.Apps.Base.Module.BusinessObjects;

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    [DefaultClassOptions, Persistent("Vacuna"), DefaultProperty("Nombre"), ModelDefault("Caption", "Vacuna"), NavigationItem("Salud"),]
    [ImageName(nameof(Vacuna))]
    // OJO. PENDIENTE VINCULARLA CON PRODUCTOS. ESTA ES LA INFORMACION TECNICA DE LA VACUNA, AHORA FALTA VINCULARLA AL PRODUCTO
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Vacuna : XPObjectBaseBO
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
            RuleRequiredField("Vacuna.CodigoPT_Requerido", "Save"),
            RuleUniqueValue("Vacuna.CodigoPT_Unico", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction,
                                                     SkipNullOrEmptyValues = false, ResultType = ValidationResultType.Error)]
        public string Codigo
        {
            get => codigoPT;
            set => SetPropertyValue(nameof(Codigo), ref codigoPT, value);
        }


        [Size(6), DbType("varchar(6)"), Persistent("CodigoCVX"), XafDisplayName("Código CVX"),
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


        [Size(200), DbType("varchar(200)"), Persistent("DescripcionCPT"), XafDisplayName("Descripción CPT")]
        public string DescripcionCPT
        {
            get => descripcionCPT;
            set => SetPropertyValue(nameof(DescripcionCPT), ref descripcionCPT, value);
        }

        
        [Size(150), DbType("varchar(150)"), Persistent("Comentario"), XafDisplayName("Comentario")]
        public string Comentario
        {
            get => comentario;
            set => SetPropertyValue(nameof(Comentario), ref comentario, value);
        }
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}