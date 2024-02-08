using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace SBT.Apps.Tercero.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que corresponde a los contactos de terceros
    /// </summary>
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Contactos")]
    [DevExpress.Persistent.Base.ImageNameAttribute("bank-to_vendor_32x32")]
    [DevExpress.Persistent.Base.CreatableItemAttribute(false)]
    [DevExpress.ExpressApp.DC.XafDefaultProperty(nameof(Nombre))]
    public class TerceroContacto : XPObject
    {
        /// <summary>
        /// Metodo para la inicialización de propiedades y/o objetos. Se ejecuta una sola vez después de la creación del objeto
        /// </summary>

        public override void AfterConstruction()
        {
            Activo = true;
        }

        private Tercero tercero;
        private System.Boolean activo;
        private System.String eMail;
        private System.String telefono;
        private System.String nombre;
        public TerceroContacto(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        [RuleRequiredField("TerceroContacto.Nombre_Requerido", "Save"), DbType("varchar(100)")]
        [RuleUniqueValue("TerceroContacto.Nombre_Unico", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        public System.String Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        [DevExpress.Xpo.SizeAttribute(25), DbType("varchar(25)")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("No Telefono")]
        [RuleRequiredField("TerceroContacto.Telefono_Requerido", "Save")]
        public System.String Telefono
        {
            get => telefono;
            set => SetPropertyValue(nameof(Telefono), ref telefono, value);
        }

        [DevExpress.Xpo.SizeAttribute(60), DbType("varchar(60)")]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Correo Electrónico")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [RuleRequiredField("TerceroContacto.EMail_Requerido", "Save")]
        public System.String EMail
        {
            get => eMail;
            set => SetPropertyValue(nameof(EMail), ref eMail, value);
        }

        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(true)]
        public System.Boolean Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }
        [DevExpress.Xpo.AssociationAttribute("Tercero-Contactos"), VisibleInListView(false), VisibleInLookupListView(false)]
        public Tercero Tercero
        {
            get => tercero;
            set => SetPropertyValue(nameof(Tercero), ref tercero, value);
        }

    }
}
