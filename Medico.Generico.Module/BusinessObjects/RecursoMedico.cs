using System;
using System.Linq;
using System.Text;
using System.Drawing;
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
using DevExpress.Persistent.Base.General;

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class RecursoMedico : BaseObject, IResource
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public RecursoMedico(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            color = Color.White.ToArgb();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        string caption;
        [Persistent(nameof(Color))]
        private int color;

        Medico medico;

        [XafDisplayName("Medico")]
        [DataSourceCriteria("[Activo] == True")]
        [Association("Medico-RecursoMedico")]
        public Medico Medico
        {
            get => medico;
            set => SetPropertyValue(nameof(Medico), ref medico, value);
        }

        #endregion

        #region IResource
        [Browsable(false)]
        public object Id => Oid;

        [Browsable(false)]
        public int OleColor
        {
            get { return ColorTranslator.ToOle(Color.FromArgb(color)); }
        }

        [Size(100), DbType("varchar(100)"), XafDisplayName("SubTitulo")]
        public string Caption
        {
            get => caption;
            set => SetPropertyValue(nameof(Caption), ref caption, value);
        }

        [Association("RecursoMedico-Citas", UseAssociationNameAsIntermediateTableName = true), XafDisplayName("Citas")]
        public XPCollection<CitaBase> Citas
        {
            get
            {
                return GetCollection<CitaBase>(nameof(Citas));
            }
        }
        #endregion



        #region Metodos

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