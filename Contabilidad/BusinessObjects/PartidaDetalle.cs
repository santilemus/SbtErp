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
using SBT.Apps.Contabilidad.BusinessObjects;

namespace SBT.Apps.Contabilidad.Module.BusinessObjects
{
    [DefaultClassOptions, NavigationItem(false), ModelDefault("Caption", "Partida Detalle")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    [Persistent("ConPartidaDetalle")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class PartidaDetalle : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public PartidaDetalle(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades
        Partida partida;
        Catalogo catalogo;
        string concepto;
        decimal valorDebe = 0.0m;
        decimal valorHaber = 0.0m;
        int ctaPresupuesto;
        ETipoOperacionConsolidacion ajusteConsolidacion = ETipoOperacionConsolidacion.Ninguno;

        [Association("Partida-Detalles"), DbType("int"), Persistent("Partida"), Browsable(false)]
        public Partida Partida
        {
            get => partida;
            set
            {
                Partida oldPartida = partida;
                bool changed = SetPropertyValue(nameof(Partida), ref partida, value);
                if (!IsLoading && !IsSaving && changed && Partida != null)
                {
                    oldPartida = oldPartida ?? partida;
                    oldPartida.UpdateTotDebe(true);
                    oldPartida.UpdateTotHaber(true);
                }
            }
        }


        [DbType("int"), Persistent("Cuenta"), XafDisplayName("Cuenta")]
        public Catalogo Cuenta
        {
            get => catalogo;
            set => SetPropertyValue(nameof(Cuenta), ref catalogo, value);
        }


        [PersistentAlias("[Cuenta.Nombre]")]
        public string NombreCuenta
        {
            get { return Convert.ToString(EvaluateAlias(nameof(NombreCuenta))); }
        }

        [Size(150), DbType("varchar(150)"), Persistent("Concepto"), XafDisplayName("Concepto"),
            RuleRequiredField("PartidaDetalle.Concepto_Requerido", "Save")]
        public string Concepto
        {
            get => concepto;
            set => SetPropertyValue(nameof(Concepto), ref concepto, value);
        }

        [DbType("money"), Persistent("Debe"), XafDisplayName("Debe")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal ValorDebe
        {
            get => valorDebe;
            set
            {
                bool changed = SetPropertyValue(nameof(ValorDebe), ref valorDebe, value);
                if (!IsLoading && !IsSaving && changed && Partida != null)
                    Partida.UpdateTotDebe(true);
            }
        }

        [DbType("money"), Persistent("Haber"), XafDisplayName("Haber")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal ValorHaber
        {
            get => valorHaber;
            set
            {
                bool changed = SetPropertyValue(nameof(ValorHaber), ref valorHaber, value);
                if (!IsLoading && !IsSaving && changed && Partida != null)
                    Partida.UpdateTotHaber(true);
            }
        }

        [DbType("int"), Persistent("CtaPresupuesto"), XafDisplayName("Cuenta Presupuesto")]
        public int CtaPresupuesto
        {
            get => ctaPresupuesto;
            set => SetPropertyValue(nameof(CtaPresupuesto), ref ctaPresupuesto, value);
        }


        [DbType("smallint"), Persistent("AjusteConsolida"), XafDisplayName("Ajuste Consolidación")]
        public ETipoOperacionConsolidacion AjusteConsolidacion
        {
            get => ajusteConsolidacion;
            set => SetPropertyValue(nameof(AjusteConsolidacion), ref ajusteConsolidacion, value);
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}