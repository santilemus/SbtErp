using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace SBT.Apps.Medico.Expediente.Module.BusinessObjects
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// https://docs.devexpress.com/eXpressAppFramework/112658/task-based-help/business-model-design/express-persistent-objects-xpo/how-to-implement-file-data-properties
    /// </remarks>
    [DefaultClassOptions, DefaultProperty(nameof(Descripcion)), ModelDefault("Caption", "Archivos"), NavigationItem(false),
        Persistent(nameof(PacienteFileData)), CreatableItem(false)]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class PacienteFileData : FileAttachmentBase
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public PacienteFileData(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();

            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            Vigente = true;
        }

        Paciente paciente;
        //   MedicoLista categoria;
        string descripcion;
        DateTime fecha;
        bool vigente = true;


        #region Propiedades
        [Association("Paciente-ArchivosAdjuntos"), XafDisplayName("Paciente")]
        public Paciente Paciente
        {
            get => paciente;
            set => SetPropertyValue(nameof(Paciente), ref paciente, value);
        }

        //[XafDisplayName("Categoría"), ImmediatePostData(true), VisibleInLookupListView(true),
        //    RuleRequiredField("PacienteFileData.Categoria_Requerido", DefaultContexts.Save, ResultType = ValidationResultType.Information)]
        //public MedicoLista Categoria
        //{
        //    get => categoria;
        //    set => SetPropertyValue(nameof(Categoria), ref categoria, value);
        //}

        [Size(120), DbType("varchar(100)"), XafDisplayName("Descripción"),
            RuleRequiredField("PacienteFileData.Descripcion_Requerido", DefaultContexts.Save)]
        public string Descripcion
        {
            get => descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref descripcion, value);
        }

        [XafDisplayName("Fecha"), DbType("datetime2")]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
        }

        [XafDisplayName("Vigente")]
        public bool Vigente
        {
            get => vigente;
            set => SetPropertyValue(nameof(Vigente), ref vigente, value);
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}