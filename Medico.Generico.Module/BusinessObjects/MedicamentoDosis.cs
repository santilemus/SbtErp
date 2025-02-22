﻿using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que corresponde a Dosis de Medicamento. Es la clase para el objeto de negocios que corresponde a las dosis de medicamente
    /// </summary>
	[NavigationItem(false)]
    [DevExpress.Persistent.Base.CreatableItemAttribute(false)]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Dosis")]
    [DevExpress.ExpressApp.DC.XafDefaultPropertyAttribute("Comentario")]
    [DevExpress.Persistent.Base.ImageNameAttribute("capsula2")]
    public class MedicamentoDosis : XPObject
    {
        /// <summary>
        /// Metodo para la inicialización de propiedades y/o objetos del BO. Se ejecuta una sola vez después de la creación del objeto
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Dosis = 0;
        }

        private Medicamento _medicamento;
        private System.String _comentario;
        private System.String _edad;
        private System.Int16 _dosis;
        public MedicamentoDosis(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        [VisibleInLookupListView(true)]
        public System.Int16 Dosis
        {
            get
            {
                return _dosis;
            }
            set
            {
                SetPropertyValue("Dosis", ref _dosis, value);
            }
        }

        [DevExpress.Xpo.SizeAttribute(50), DbType("varchar(50)")]
        [RuleRequiredField("MedicamentoDosis.Edad_Requerido", "Save"), VisibleInLookupListView(true)]
        public System.String Edad
        {
            get
            {
                return _edad;
            }
            set
            {
                SetPropertyValue("Edad", ref _edad, value);
            }
        }
        [DevExpress.Xpo.SizeAttribute(200), DbType("varchar(200)")]
        public System.String Comentario
        {
            get
            {
                return _comentario;
            }
            set
            {
                SetPropertyValue("Comentario", ref _comentario, value);
            }
        }
        [DevExpress.Xpo.AssociationAttribute("MedicamentoDosises-Medicamento")]
        public Medicamento Medicamento
        {
            get
            {
                return _medicamento;
            }
            set
            {
                SetPropertyValue("Medicamento", ref _medicamento, value);
            }
        }
    }
}
