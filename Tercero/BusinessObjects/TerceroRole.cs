﻿using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Linq;

namespace SBT.Apps.Tercero.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que representa los roles que desempeña un tercero
    /// </summary>
    [DevExpress.Persistent.Base.CreatableItemAttribute(false)]
    [DevExpress.Persistent.Base.ImageNameAttribute("credit_card-to_bank")]
    [DevExpress.ExpressApp.DC.XafDefaultProperty(nameof(Descripcion))]
    public class TerceroRole : XPObjectBaseBO
    {
        /// <summary>
        /// Metodo para la inicialización de propiedades y/o objetos del BO. Se ejecuta una sola vez después de la creación del objeto
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            this.Activo = true;
        }

        private Apps.Tercero.Module.BusinessObjects.Tercero tercero;
        private System.Boolean activo = true;
        private System.String descripcion;
        private TipoRoleTercero idRole = TipoRoleTercero.Cliente;
        public TerceroRole(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        [Index(0), XafDisplayNameAttribute("Id Role")]
        [RuleRequiredField("TerceroRole.IdRole_Requerido", "Save")]
        [RuleUniqueValue("TerceroRole.IdRole_Unico", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        public TipoRoleTercero IdRole
        {
            get => idRole;
            set => SetPropertyValue(nameof(IdRole), ref idRole, value);
        }

        string codigo;
#if Firebird
        [DbType("DM_CODIGO10"), Persistent("COD_ROLE")]
#else
        [DbType("varchar(10)"), Persistent("Codigo")]
#endif
        [Size(10), XafDisplayName("Código"), Index(1), VisibleInLookupListView(true),
            RuleUniqueValue("TerceroRole.Codigo_Unico", DefaultContexts.Save, SkipNullOrEmptyValues = true)]
        public string Codigo
        {
            get => codigo;
            set => SetPropertyValue("Codigo", ref codigo, value);
        }

        [DevExpress.Persistent.Base.ImmediatePostDataAttribute, DbType("varchar(100)")]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Descripción"), Index(2)]
        [RuleRequiredField("TerceroRole.Descripcion_Requerido", "Save")]
        public System.String Descripcion
        {
            get => descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref descripcion, value);
        }

        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(true), Index(3)]
        [RuleRequiredField("TerceroRole.Activo_Requerido", "Save")]
        public System.Boolean Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }

        [DevExpress.Xpo.AssociationAttribute("Tercero-Roles"), Index(4), VisibleInListView(false)]
        public Apps.Tercero.Module.BusinessObjects.Tercero Tercero
        {
            get => tercero;
            set => SetPropertyValue(nameof(Tercero), ref tercero, value);
        }

    }
}
