using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System;
using System.Linq;
using DevExpress.Xpo;
using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    [DefaultClassOptions, CreatableItem(false)]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Constante")]
    [DevExpress.Persistent.Base.ImageNameAttribute("list-key")]
    [DevExpress.ExpressApp.DC.XafDefaultPropertyAttribute("Nombre")]
    [DevExpress.Persistent.Base.NavigationItemAttribute("Catalogos")]
    public class Constante : XPCustomBaseBO
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            TipoConstante = TipoConstante.Texto;
        }

        private System.String _valor;
        private TipoConstante _tipoConstante;
        private System.String _nombre;
        private System.String _codigo;
        public Constante(DevExpress.Xpo.Session session)
          : base(session)
        {
        }
        [Size(25), DbType("varchar(25)"), Persistent("Codigo")]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Código")]
        [DevExpress.Xpo.KeyAttribute, VisibleInLookupListView(true)]
        [RuleRequiredField("Constante.Codigo_Requerido", "Save")]
        [RuleUniqueValue("Constante.Codigo_Unico", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        public System.String Codigo
        {
            get
            {
                return _codigo;
            }
            set
            {
                SetPropertyValue("Codigo", ref _codigo, value);
            }
        }

        [RuleRequiredField("Constante.Nombre_Requerido", DefaultContexts.Save, "Nombre es requerido")]
        [Size(100), DbType("varchar(100)"), Persistent("Nombre"), XafDisplayName("Nombre")]
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
        public TipoConstante TipoConstante
        {
            get
            {
                return _tipoConstante;
            }
            set
            {
                SetPropertyValue("TipoConstante", ref _tipoConstante, value);
            }
        }
        [DevExpress.Xpo.SizeAttribute(150), DbType("varchar(150)"), Persistent("Valor")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [RuleRequiredField("Constante.Valor_Requerido", DefaultContexts.Save, "Valor es requerido")]
        public System.String Valor
        {
            get
            {
                return _valor;
            }
            set
            {
                SetPropertyValue("Valor", ref _valor, value);
            }
        }

    }
}
