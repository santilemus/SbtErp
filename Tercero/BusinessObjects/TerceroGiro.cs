using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System.ComponentModel;

namespace SBT.Apps.Tercero.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que corresponde a los números de registro fiscal asociados a un tercero
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <cambios>
    /// 29/febrero/2024 por SELM
    /// 700-DGII-GTR-2024-0001. Líneamientos que entran en vigencia para declaraciones de IVA y pago a cuenta de febrero del 2024.
    /// </cambios>
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Registro Fiscal")]
    [DevExpress.Persistent.Base.CreatableItemAttribute(false)]
    [DevExpress.Persistent.Base.ImageNameAttribute("bill-key")]
    [DefaultProperty(nameof(ActEconomica))]
    public class TerceroGiro : XPObject
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Sector = ESectorSujetoPasivo.Servicio;
        }

        private Tercero tercero;
        bool vigente = true;
        private ActividadEconomica actEconomica;
        private ESectorSujetoPasivo sector;

        public TerceroGiro(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        [VisibleInListView(true), VisibleInLookupListView(true)]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Actividad Económica")]
        [RuleRequiredField("TerceroGiro.ActEconomica_Requerido", DefaultContexts.Save, "Actividad Económica es requerida")]
        [ExplicitLoading]
        public ActividadEconomica ActEconomica
        {
            get => actEconomica;
            set => SetPropertyValue(nameof(ActEconomica), ref actEconomica, value);
        }

        [DbType("bit"), DevExpress.ExpressApp.DC.XafDisplayName("Vigente")]
        public bool Vigente
        {
            get => vigente;
            set => SetPropertyValue(nameof(Vigente), ref vigente, value);
        }

        [DevExpress.Xpo.AssociationAttribute("Tercero-Giros")]
        public Tercero Tercero
        {
            get => tercero;
            set => SetPropertyValue(nameof(Tercero), ref tercero, value);
        }

        /// <summary>
        /// Rubro o actividad económica genérica a la que se dedica la empresa
        /// </summary>
        /// <remarks>
        /// 700-DGII-GTR-2024-0001. Líneamientos que entran en vigencia para declaraciones de IVA
        /// y pago a cuenta de febrero del 2024.
        /// </remarks>
        [System.ComponentModel.DisplayName("Sector")]
        [DbType("smallint"), VisibleInListView(true)]
        [ToolTip("Rubro o actividad económica genérica a la que se dedica la empresa")]
        public ESectorSujetoPasivo Sector
        {
            get => sector;
            set => SetPropertyValue(nameof(Sector), ref sector, value);
        }

    }
}
