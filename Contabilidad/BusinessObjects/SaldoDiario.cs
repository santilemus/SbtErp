using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Contabilidad.BusinessObjects;
using System;

namespace SBT.Apps.Contabilidad.Module.BusinessObjects
{
    /// <summary>
    /// Contabilidad. BO con los saldos diarios contables. Es calculado por un SP, aqui no se ingresan datos desde la interfaz de usuario
    /// </summary>
    [ModelDefault("Caption", "Saldo Diario"), Persistent("ConSaldoDiario"), NavigationItem(false), CreatableItem(false), VisibleInReports(true)]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class SaldoDiario : XPCustomObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SaldoDiario(Session session)
            : base(session)
        {
        }

        public SaldoDiario(Session session, Periodo per, Catalogo cta, DateTime dia, decimal valdebe, decimal valhaber, ETipoSaldoDia tipo,
            decimal debeAjusteCons, decimal haberAjusteCons)
            : base(session)
        {
            periodo = per;
            cuenta = cta;
            fecha = dia;
            debe = valdebe;
            haber = valhaber;
            tipoSaldoDia = tipo;
            debeAjusteConsolida = debeAjusteCons;
            haberAjusteConsolida = haberAjusteCons;
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            periodo = null;
            cuenta = null;
            debe = 0.0m;
            haber = 0.0m;
            debeAjusteConsolida = 0.0m;
            haberAjusteConsolida = 0.0m;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        #region Propiedades

        [Persistent("Oid"), Key(true), DbType("bigint")]
        Int64 oid = -1;
        [DbType("int"), Persistent(nameof(Periodo)), FetchOnly]
        Periodo periodo;
        Catalogo cuenta;
        [Persistent(nameof(Fecha)), DbType("datetime"), FetchOnly]
        DateTime fecha = DateTime.Today;
        [Persistent(nameof(Debe)), DbType("money"), FetchOnly]
        decimal debe;
        [Persistent(nameof(Haber)), DbType("money"), FetchOnly]
        decimal haber;
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

        [PersistentAlias(nameof(periodo)), XafDisplayName("Período")]
        public Periodo Periodo
        {
            get => periodo;
        }

        [XafDisplayName("Cuenta")]
        [Indexed(nameof(fecha), nameof(TipoSaldoDia), Name = "idxCuentaFecha_ConSaldoDiario", Unique = true)]
        public Catalogo Cuenta
        {
            get => cuenta;
            set => SetPropertyValue(nameof(Cuenta), ref cuenta, value);
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

        [XafDisplayName("Tipo Saldo")]
        public ETipoSaldoDia TipoSaldoDia
        {
            get => tipoSaldoDia;
            set => SetPropertyValue(nameof(TipoSaldoDia), ref tipoSaldoDia, value);
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

        #region Metodos
        public void Update(decimal totDebe, decimal totHaber, decimal ajusteDebe, decimal ajusteHaber)
        {
            debe = totDebe;
            haber = totHaber;
            debeAjusteConsolida = ajusteDebe;
            haberAjusteConsolida = ajusteHaber;
        }
        #endregion 


        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}