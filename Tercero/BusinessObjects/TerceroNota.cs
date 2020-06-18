using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using SBT.Apps.Base.Module.BusinessObjects;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace SBT.Apps.Tercero.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que corresponde a las notas relacionadas a terceros
    /// </summary>
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Notas")]
    [DevExpress.Persistent.Base.CreatableItemAttribute(false)]
    [DevExpress.Persistent.Base.ImageNameAttribute("comment-info")]
    [DevExpress.ExpressApp.DC.XafDefaultProperty(nameof(Fecha))]
    public class TerceroNota : XPObjectBaseBO
    {
        /// <summary>
        /// Metodo para la inicialización de propiedades y/o objetos del BO. Se ejecuta una sola vez después de la creación del objeto
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Fecha = DateTime.Now;
            //Usuario = DevExpress.ExpressApp.SecuritySystem.CurrentUserName;
        }

        private Tercero tercero;
        //private System.String _usuario;
        private System.String comentario;
        private System.DateTime fecha;
        public TerceroNota(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        [DevExpress.Persistent.Base.ImmediatePostData(true), DevExpress.Persistent.Base.Index(0)]
        [RuleRequiredField("TerceroNota.Fecha_Requerido", "Save")]
        public System.DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
        }

        [DevExpress.Xpo.SizeAttribute(200), DbType("varchar(200)"), DevExpress.Persistent.Base.Index(1)]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [RuleRequiredField("TerceroNota.Comentario_Requerido", "Save")]
        public System.String Comentario
        {
            get => comentario;
            set => SetPropertyValue(nameof(Comentario), ref comentario, value);
        }

        [DevExpress.Xpo.AssociationAttribute("Tercero-Notas"), DevExpress.Persistent.Base.Index(2), VisibleInListView(false)]
        public Tercero Tercero
        {
            get => tercero;
            set => SetPropertyValue(nameof(Tercero), ref tercero, value);
        }

    }
}
