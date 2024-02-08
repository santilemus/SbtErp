using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    /// <summary>
    /// Clase para los BO con laDefinicion de los factores de riesgo, que posteriormente pueden asociarse a los pacientes para definir los factores
    /// que afectan a cada paciente en particular. 
    /// Ejemplos: Hipertension, Diabetes, etc y los cuales deben tenerse en cuenta al ser tratado
    /// </summary>
    [DefaultClassOptions, Persistent("FactorRiesgo"), ModelDefault("Caption", "Factor Riesgo"), NavigationItem("Salud"),
        XafDefaultProperty("Diagnostico"), CreatableItem(false)]
    [ImageName("FactorRiesgo")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class FactorRiesgo : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public FactorRiesgo(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        string descripcion;
        Enfermedad diagnostico;

        [Association("Enfermedad-FactorRiesgos"), Persistent("Diagnostico"), XafDisplayName("Diagnostico"),
            ToolTip("Enfermedad a la cual corresponde el Diagnostico de factor de riesgo (sinonimo de enfermedad)")]
        public Enfermedad Diagnostico
        {
            get => diagnostico;
            set
            {
                bool changed = SetPropertyValue(nameof(Diagnostico), ref diagnostico, value);
                if (!IsLoading && !IsSaving && changed)
                {
                    if (Diagnostico != null)
                        Descripcion = Diagnostico.Nombre;
                    else
                        Descripcion = string.Empty;
                }
            }
        }

        [Size(150), DbType("varchar(150)"), Persistent("Descripcion"), XafDisplayName("Descripción"),
            RuleRequiredField("FactorRiesgo.Descripcion_Requerido", DefaultContexts.Save)]
        public string Descripcion
        {
            get => descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref descripcion, value);
        }


        #endregion 

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}