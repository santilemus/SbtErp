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

namespace SBT.Apps.Base.Module.BusinessObjects
{
    [DefaultClassOptions, NavigationItem(false), CreatableItem(false)]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ConsultaParametro : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ConsultaParametro(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        int prioridad;
        ETipoDato tipo = ETipoDato.tString;
        bool activo = true;
        bool visible = true;
        string descripcion;
        string nombre;
        SqlObject consulta;

        [Association("Consulta-Parametros"), DbType("int"), Persistent("Consulta"), XafDisplayName("Consulta"), Browsable(false)]
        public SqlObject Consulta
        {
            get => consulta;
            set => SetPropertyValue(nameof(Consulta), ref consulta, value);
        }

        [Size(30), DbType("varchar(30)"), Persistent("Nombre"), XafDisplayName("Nombre"), RuleRequiredField("Consulta.Nombre_Requerido", "Save")]
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        [Size(60), DbType("varchar(60)"), Persistent("Descripcion"), XafDisplayName("Descripción")]
        public string Descripcion
        {
            get => descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref descripcion, value);
        }

        [DbType("smallint"), Persistent("Tipo"), XafDisplayName("Tipo")]
        public ETipoDato Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }
        [DbType("smallint"), Persistent("Prioridad"), XafDisplayName("Prioridad")]
        public int Prioridad
        {
            get => prioridad;
            set => SetPropertyValue(nameof(Prioridad), ref prioridad, value);
        }
        [DbType("bit"), Persistent("Visible")]
        public bool Visible
        {
            get => visible;
            set => SetPropertyValue(nameof(Visible), ref visible, value);
        }

        [DbType("bit"), Persistent("Activo")]
        public bool Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }

        #endregion
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