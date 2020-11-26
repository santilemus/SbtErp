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
using System.IO;

namespace SBT.Apps.Medico.Expediente.Module.BusinessObjects
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// https://docs.devexpress.com/eXpressAppFramework/112658/task-based-help/business-model-design/express-persistent-objects-xpo/how-to-implement-file-data-properties
    /// </remarks>
    [DefaultProperty(nameof(Descripcion)), ModelDefault("Caption", "Archivos"), NavigationItem(false),
        Persistent(nameof(ArchivoAdjunto)), CreatableItem(false)]
    [FileAttachment(nameof(File))]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ArchivoAdjunto : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ArchivoAdjunto(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        Paciente paciente;
        MedicoLista categoria;
        string descripcion;
        DateTime fecha;
        protected FileData file;
        bool vigente = true;


        #region Propiedades
        [Association("Paciente-ArchivosAdjuntos"), XafDisplayName("Paciente")]
        public Paciente Paciente
        {
            get => paciente;
            set => SetPropertyValue(nameof(Paciente), ref paciente, value);
        }

        [XafDisplayName("Categoría"), ImmediatePostData(true),
            RuleRequiredField("ArchivoAdjunto.Categoria_Requerido", DefaultContexts.Save, ResultType = ValidationResultType.Information), VisibleInLookupListView(true)]
        public MedicoLista Categoria
        {
            get => categoria;
            set => SetPropertyValue(nameof(Categoria), ref categoria, value);
        }

        [Size(120), DbType("varchar(100)"), XafDisplayName("Descripción"), ImmediatePostData(true),
            RuleRequiredField("ArchivoAdjunto.Descripcion_Requerido", DefaultContexts.Save)]
        public string Descripcion
        {
            get => descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref descripcion, value);
        }

        [XafDisplayName("Fecha"), DbType("datetime2"), ImmediatePostData(true)]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
        }

        [DevExpress.Xpo.Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        public FileData File
        {
            get => file;
            set => SetPropertyValue<FileData>(nameof(File), ref file, value);
        }

        [XafDisplayName("Vigente"), RuleRequiredField("ArchivoAdjunto.Vigente_Requerido", "Save")]
        public bool Vigente
        {
            get => vigente;
            set => SetPropertyValue(nameof(Vigente), ref vigente, value);
        }

        //[Browsable(false), NonPersistent()]
        //public byte[] Data
        //{
        //    get
        //    {
        //        if (this.File != null)
        //        {
        //            using (var stream = new MemoryStream())
        //            {
        //                File.SaveToStream(stream);
        //                return stream.ToArray();
        //            }

        //        }
        //        else
        //            return null;
        //    }
        //}

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