using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Linq;

namespace SBT.Apps.Empleado.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que corresponde a Cargo. Es la clase para el objeto de negocios que corresponde al mantenimiento de Cargos
    /// </summary>
    [DefaultClassOptions]
    [DevExpress.Persistent.Base.NavigationItemAttribute("Catalogos")]
    [DevExpress.ExpressApp.DC.XafDefaultPropertyAttribute("Nombre")]
    [DevExpress.Persistent.Base.ImageNameAttribute("archive-man")]
    [RuleIsReferenced("Cargo_Referencia", DefaultContexts.Delete, typeof(Cargo), nameof(Oid),
        MessageTemplateMustBeReferenced = "Para borrar el objeto '{TargetObject}', debe estar seguro que no es utilizado (referenciado) en ningún lugar.",
        InvertResult = true, FoundObjectMessageFormat = "'{0}'", FoundObjectMessagesSeparator = ";")]
    public class Cargo : XPObjectBaseBO
    {
        /// <summary>
        /// Metodo para la inicialización de propiedades y/o objetos del BO. Se ejecuta una sola vez después de la creación del objeto
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Salario = 0.0m;
            TipoSalario = TipoSalario.Mensual;
            TipoContrato = TipoContrato.Indefinido;
            Activo = true;
        }

        private System.Decimal _salario;
        private System.Boolean _activo;
        private System.String _obligaciones;
        private TipoSalario _tipoSalario;
        private TipoContrato tipoContrato;
        private System.String _nombre;
        public Cargo(DevExpress.Xpo.Session session)
          : base(session)
        {
        }
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [RuleRequiredField("Cargo.Nombre_Requerido", DefaultContexts.Save, "Nombre del tipoContrato es requerido")]
        public System.String Nombre
        {
            get
            {
                return _nombre;
            }
            set
            {
                SetPropertyValue("Nombre", ref _nombre, value);
            }
        }

        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        public TipoContrato TipoContrato
        {
            get => tipoContrato;
            set => SetPropertyValue(nameof(TipoContrato), ref tipoContrato, value);
        }

        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        public System.Decimal Salario
        {
            get
            {
                return _salario;
            }
            set
            {
                SetPropertyValue("Salario", ref _salario, value);
            }
        }
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        public TipoSalario TipoSalario
        {
            get
            {
                return _tipoSalario;
            }
            set
            {
                SetPropertyValue("TipoSalario", ref _tipoSalario, value);
            }
        }
        [DevExpress.Xpo.SizeAttribute(500)]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Persistent.Base.VisibleInListViewAttribute(false)]
        public System.String Obligaciones
        {
            get
            {
                return _obligaciones;
            }
            set
            {
                SetPropertyValue("Obligaciones", ref _obligaciones, value);
            }
        }
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [RuleRequiredField("Cargo.Activo_Requerido", DefaultContexts.Save, "Activo es obligatorio")]
        public System.Boolean Activo
        {
            get
            {
                return _activo;
            }
            set
            {
                SetPropertyValue("Activo", ref _activo, value);
            }
        }

    }
}
