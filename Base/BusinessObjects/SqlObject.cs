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
    
    /// <summary>
    /// BO para las consultas dinamicas (incluyendo la ejecucion de procedimientos almacenados) cuya definicion se guarda en la base de datos
    /// para procesos dinamicos
    /// </summary>
    [DefaultClassOptions, ModelDefault("Caption", "Consulta Sql"), Persistent("SysConsulta"), DefaultProperty("Nombre"),
        NavigationItem(false), CreatableItem(false)]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class SqlObject : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SqlObject(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        bool activa = true;
        string ssql;
        string descripcion;
        string nombre;

        [Size(30), DbType("varchar(30)"), Persistent("Nombre"), XafDisplayName("Nombre"), RuleRequiredField("Consulta.Nombre_Requuerido", "Save")]
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        [Size(100), DbType("varchar(100)"), Persistent("Descripcion"), XafDisplayName("Descripción")]
        public string Descripcion
        {
            get => descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref descripcion, value);
        }

        [Size(2000), DbType("varchar(2000)"), Persistent("Ssql"), XafDisplayName("Sentencia SQL")]
        public string Ssql
        {
            get => ssql;
            set => SetPropertyValue(nameof(Ssql), ref ssql, value);
        }
        
        [DbType("bit"), XafDisplayName("Activa"), Persistent("Activa"), RuleRequiredField("Consulta.Activa_Requerido", DefaultContexts.Save)]
        public bool Activa
        {
            get => activa;
            set => SetPropertyValue(nameof(Activa), ref activa, value);
        }

        #endregion

        #region Colecciones
        [Association("Consulta-Parametros")]
        public XPCollection<ConsultaParametro> Parametros
        {
            get
            {
                return GetCollection<ConsultaParametro>(nameof(Parametros));
            }
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