using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Contabilidad.Module.BusinessObjects
{
    /// <summary>
    /// Contabilidad.
    /// BO que contiene el control de los cierres diarios 
    /// </summary>
    [DefaultClassOptions, NavigationItem(false), ModelDefault("Caption", "Cierre Diario"), DefaultProperty("FechaCierre")]
    [Persistent("ConCierre")]
    [ImageName(nameof(CierreDiario))]
    [CreatableItem(false)]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CierreDiario : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public CierreDiario(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        [Persistent(nameof(Empresa))]
        Empresa empresa = null;
        [Persistent(nameof(FechaCierre))]
        DateTime? fechaCierre = null;
        [Persistent(nameof(DiaCerrado)), DbType("bit")]
        bool diaCerrado = true;
        [Persistent(nameof(MesCerrado)), DbType("bit")]
        bool mesCerrado = false;
        [Persistent("FechaCierreAudit"), DbType("datetime2")]
        DateTime? fechaCierreAuditoria = null;


        [PersistentAlias(nameof(empresa)), XafDisplayName("Empresa")]
        public Empresa Empresa
        {
            get => empresa;
        }
        [PersistentAlias(nameof(fechaCierre)), XafDisplayName("Fecha Cierre")]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime? FechaCierre
        {
            get { return fechaCierre; }
        }

        [PersistentAlias(nameof(diaCerrado)), XafDisplayName("Día Cerrado")]
        public bool DiaCerrado
        {
            get { return diaCerrado; }
        }

        [PersistentAlias(nameof(mesCerrado)), XafDisplayName("Mes Cerrado")]
        public bool MesCerrado
        {
            get { return mesCerrado; }
        }

        [PersistentAlias(nameof(fechaCierreAuditoria))]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime? FechaCierreAuditoria
        {
            get => fechaCierreAuditoria;
        }

        #endregion

        #region Metodos
        public bool MesAbierto(int AMes, int AAnio)
        {
            var obj = Session.Evaluate<CierreDiario>(CriteriaOperator.Parse("Count()"),
                CriteriaOperator.Parse("[Empresa] = ? And GetMonth([FechaCierre]) = ? And GetYear([FechaCierre]) = ? And MesCerrado = False",
                ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid, AMes, AAnio));
            return Convert.ToInt32(obj) > 0;
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