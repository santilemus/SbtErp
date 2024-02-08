using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace SBT.Apps.Tercero.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que representa las sucursales o agencias de un tercero. Por ejemplo: un banco
    /// </summary>
    [ModelDefault("Caption", "Sucursal"), Persistent("TerceroSucursal"), XafDefaultProperty("Nombre"),
        NavigationItem(false), ImageName("company-employee")]
    public class TerceroSucursal : XPObject
    {
        /// <summary>
        /// Metodo para la inicialización de propiedades y/o objetos del BO. Se ejecuta una sola vez después de la creación del objeto
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Activa = true;
        }

        private Tercero tercero;
        private System.String eMail;
        private System.String telefono;
        private System.Boolean activa;
        private System.String direccion;
        private System.String nombre;
        public TerceroSucursal(DevExpress.Xpo.Session session)
          : base(session)
        {
        }
        [RuleRequiredField("TerceroSucursal.Nombre_Requerido", "Save"), DbType("varchar(100)"), Index(0)]
        [RuleUniqueValue("TerceroSucursal.Nombre_Unico", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        public System.String Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        [DbType("varchar(100)")]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Dirección"), Index(1)]
        [RuleRequiredField("TerceroSucursal.Direccion_Requerido", "Save")]
        public System.String Direccion
        {
            get => direccion;
            set => SetPropertyValue(nameof(Direccion), ref direccion, value);
        }

        [DevExpress.Xpo.SizeAttribute(25), DbType("varchar(25)"), Index(2)]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Teléfono")]
        [RuleRequiredField("TerceroSucursal.Telefono_Requerido", "Save")]
        public System.String Telefono
        {
            get => telefono;
            set => SetPropertyValue(nameof(Telefono), ref telefono, value);
        }

        [DevExpress.Xpo.SizeAttribute(60), DbType("varchar(60)"), Index(3)]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Persistent.Base.VisibleInListViewAttribute(false)]
        public System.String EMail
        {
            get => eMail;
            set => SetPropertyValue(nameof(EMail), ref eMail, value);
        }

        [Index(4), VisibleInLookupListView(true)]
        public System.Boolean Activa
        {
            get => activa;
            set => SetPropertyValue(nameof(Activa), ref activa, value);
        }

        [DevExpress.Xpo.AssociationAttribute("Tercero-Sucursales"), Index(5), VisibleInListView(false)]
        public Tercero Tercero
        {
            get => tercero;
            set => SetPropertyValue(nameof(Tercero), ref tercero, value);
        }

    }
}
