using DevExpress.Persistent.Base;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Linq;

namespace SBT.Apps.Tercero.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que representa los teléfonos de un tercero. 
    /// </summary>
    [DevExpress.Persistent.Base.CreatableItemAttribute(false)]
    [DevExpress.Persistent.Base.ImageNameAttribute("phone")]
    public class TerceroTelefono : Telefono
    {
        /// <summary>
        /// Metodo para la inicialización de propiedades y/o objetos del BO. Se ejecuta una sola vez después de la creación del objeto
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Tipo = TipoTelefono.Fijo;
        }

        private Tercero tercero;
        public TerceroTelefono(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]

        [DevExpress.Xpo.AssociationAttribute("Tercero-Telefonos"), VisibleInListView(false)]
        public Tercero Tercero
        {
            get => tercero;
            set => SetPropertyValue(nameof(Tercero), ref tercero, value);
        }

    }
}
