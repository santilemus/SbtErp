using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Contabilidad.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Contabilidad.Module.BusinessObjects
{
    /// <summary>
    /// Contabilidad. BO con los saldos diarios contables. Es calculado por un SP, aqui no se ingresan datos desde la interfaz de usuario
    /// </summary>
    [ModelDefault("Caption", "Saldos Diarios"), Persistent("ConSaldoDiario"), NavigationItem(false), CreatableItem(false)]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ConSaldoDiario : XPCustomObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ConSaldoDiario(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        #region Propiedades

        [Persistent("Oid"), Key(true), DbType("bigint")]
        Int64 oid = -1;
        [Persistent(nameof(Empresa)), FetchOnly]
        Empresa empresa;
        [DbType("int"), Persistent(nameof(Periodo)), FetchOnly]
        Periodo periodo;
        [Persistent(nameof(Cuenta)), DbType("int"), Indexed(nameof(fecha), Name = "idx_Cuenta_Fecha")]
        Catalogo cuenta;
        [Persistent(nameof(Fecha)), DbType("datetime"), FetchOnly]
        DateTime fecha = DateTime.Today;
        [Persistent(nameof(Debe)), DbType("money"), FetchOnly]
        decimal debe;
        [Persistent(nameof(Haber)), DbType("money"), FetchOnly]
        decimal haber;
        [Persistent(nameof(TipoSaldoDia)), DbType("smallint"), FetchOnly]
        ETipoSaldoDia tipoSaldoDia = ETipoSaldoDia.Operaciones;
        [Persistent(nameof(DebeAjusteConsolida)), DbType("money"), FetchOnly]
        decimal debeAjusteConsolida;
        [Persistent(nameof(HaberAjusteConsolida)), DbType("money"), FetchOnly]
        decimal haberAjusteConsolida;

        [PersistentAlias(nameof(oid)), XafDisplayName("Oid")]
        public Int64 Oid
        {
            get => oid;
        }


        [PersistentAlias(nameof(empresa)), XafDisplayName("Empresa"), Browsable(false)]
        public Empresa Empresa
        {
            get => empresa;
        }

        [PersistentAlias(nameof(periodo)), XafDisplayName("Período")]
        public Periodo Periodo
        {
            get => periodo;
        }

        [PersistentAlias(nameof(cuenta)), XafDisplayName("Cuenta")]
        public Catalogo Cuenta
        {
            get { return cuenta; }
        }


        [PersistentAlias("[Cuenta.Nombre]")]
        public string Nombre
        {
            get { return Convert.ToString(EvaluateAlias(nameof(Nombre))); }
        }

        [PersistentAlias(nameof(fecha)), XafDisplayName("Fecha")]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime Fecha
        {
            get { return fecha; }
        }

        [PersistentAlias(nameof(debe)), XafDisplayName("Valor Debe")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Debe
        {
            get { return debe; }
        }

        [PersistentAlias(nameof(haber)), XafDisplayName("Valor Haber")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Haber
        {
            get { return haber; }
        }

        [PersistentAlias(nameof(tipoSaldoDia)), XafDisplayName("Tipo Saldo")]
        public ETipoSaldoDia TipoSaldoDia
        {
            get { return tipoSaldoDia; }
        }

        [PersistentAlias(nameof(debeAjusteConsolida)), XafDisplayName("Debe Ajuste")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal DebeAjusteConsolida
        {
            get { return debeAjusteConsolida; }
        }

        [PersistentAlias(nameof(haberAjusteConsolida)), XafDisplayName("Haber Ajuste")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal HaberAjusteConsolida
        {
            get { return haberAjusteConsolida; }
        }

        #endregion


        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}