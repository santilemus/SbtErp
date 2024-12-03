using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System.ComponentModel;

namespace SBT.Apps.Tercero.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que representa los teléfonos de un tercero. 
    /// </summary>
    [DevExpress.Persistent.Base.CreatableItemAttribute(false)]
    [DevExpress.Persistent.Base.ImageNameAttribute("phone")]
    [DefaultProperty(nameof(Telefono))]
    [RuleObjectExists("TerceroTelefono.Telefono existe", DefaultContexts.Save, "Numero = '@Telefono.Numero'", LooksFor = typeof(Telefono), 
        CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction, CustomMessageTemplate = "{TargetObject} no existe en Telefono")]
    public class TerceroTelefono : XPObject
    {
        /// <summary>
        /// Metodo para la inicialización de propiedades y/o objetos del BO. Se ejecuta una sola vez después de la creación del objeto
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        private Tercero tercero;
        Telefono telefono;

        public TerceroTelefono(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Xpo.AssociationAttribute("Tercero-Telefonos"), VisibleInListView(false)]
        public Tercero Tercero
        {
            get => tercero;
            set => SetPropertyValue(nameof(Tercero), ref tercero, value);
        }

        [XafDisplayName("Teléfono")]
        [ExplicitLoading]
        [RuleRequiredField("TerceroTelefono.Telefono_requerido", DefaultContexts.Save)]
        public Telefono Telefono
        {
            get => telefono;
            set => SetPropertyValue(nameof(Telefono), ref telefono, value);
        }
    }
}
