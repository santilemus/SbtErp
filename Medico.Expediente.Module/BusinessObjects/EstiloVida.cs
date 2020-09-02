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
using SBT.Apps.Medico.Generico.Module.BusinessObjects;

namespace SBT.Apps.Medico.Expediente.Module.BusinessObjects
{
    /// <summary>
    /// Detalle con el estilo de vida del paciente. Se ingresa aqui la informacion relacionada con las actividades 
    /// que la persona realiza, que corresponden con su estilo de vida y que pueden ser factores que afectan su salud, 
    /// o los diagnosticos
    /// </summary>
    [DefaultClassOptions, Persistent("EstiloVida"), NavigationItem(false), ModelDefault("Caption", "Estilo Vida"),
        DefaultProperty("Factor")]
    [ImageName(nameof(EstiloVida))]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class EstiloVida : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public EstiloVida(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        EEstadoEstiloVida estado;
        string descripcion;
        Paciente paciente;
        MedicoLista factor;


        [Association("Paciente-EstilosVida"), Persistent("Paciente"), XafDisplayName("Paciente"), Index(0)]
        public Paciente Paciente
        {
            get => paciente;
            set => SetPropertyValue(nameof(Paciente), ref paciente, value);
        }

        [DataSourceCriteria("[Categoria] = 9")]  // 9 es estilo de vida
        [Size(10), DbType("varchar(10)"), Persistent("Factor"), XafDisplayName("Factor"), ToolTip("Clasificación del habito de estilo de vida"),
            Index(1)]
        public MedicoLista Factor
        {
            get => factor;
            set => SetPropertyValue(nameof(Factor), ref factor, value);
        }


        [Size(200), DbType("varchar(200)"), Persistent("Descripcion"), XafDisplayName("Descripción")]
        public string Descripcion
        {
            get => descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref descripcion, value);
        }

        [DbType("smallint"), Persistent("Estado"), XafDisplayName("Estado")]
        public EEstadoEstiloVida Estado
        {
            get => estado;
            set => SetPropertyValue(nameof(Estado), ref estado, value);
        }
        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}