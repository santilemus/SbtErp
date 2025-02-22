﻿using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    [DevExpress.ExpressApp.DC.XafDisplayName("Documentos")]
    [DevExpress.ExpressApp.DC.XafDefaultProperty("Número")]
    [DevExpress.Persistent.Base.CreatableItemAttribute(false)]
    [DevExpress.Persistent.Base.ImageNameAttribute("user_id-info")]
    [RuleCombinationOfPropertiesIsUnique("PersonaDocumento_DocumentoUnico", DefaultContexts.Save, "Tipo,Oid", SkipNullOrEmptyValues = false)]
    // nuevas agregadas el 29/03/2024
    [RuleCriteria("PersonaDocumento.Nit", DefaultContexts.Save, "len(trim(Numero)) = 14", TargetCriteria = "Tipo.Codigo == 'NIT'",
        CustomMessageTemplate = "Número de NIT no válido", SkipNullOrEmptyValues = true)]
    [RuleCriteria("PersonaDocumento.Dui", DefaultContexts.Save, "len(trim(Numero)) = 9", TargetCriteria = "Tipo.Codigo == 'DUI'",
        CustomMessageTemplate = "Número de DUI no válido", SkipNullOrEmptyValues = true)]
    public class PersonaDocumento : XPObject
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Vigente = true;
        }

        private Persona persona;
        private System.DateTime fechaEmision;
        private System.Boolean vigente;
        private System.String lugarEmision;
        private System.String nombre;
        private System.String numero;
        private Listas tipo;
        public PersonaDocumento(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        [VisibleInListView(true), Index(0), VisibleInLookupListView(true)]
        [RuleRequiredField("PersonaDocumento.Tipo_Requerido", DefaultContexts.Save, "Tipo Documento es requerido")]
        [DataSourceCriteria("Categoria = 'DocumentoIdentidad'"), XafDisplayName("Tipo Documento")]
        [ExplicitLoading]
        public Listas Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        [PersistentAlias("@Tipo.Codigo"), XafDisplayName("Código Documento"), VisibleInListView(false), Index(1)]
        public string CodigoDocumento
        {
            get => Convert.ToString(EvaluateAlias("CodigoDocumento"));
        }

        [Size(14), DbType("varchar(14)"), XafDisplayName("Número"), Index(2)]
        [RuleRequiredField("PersonaDocumento.Numero_Requerido", "Save")]
        public System.String Numero
        {
            get => numero;
            set => SetPropertyValue(nameof(Numero), ref numero, value);
        }

        /// <summary>
        /// Nombre de la persona en el documento. Cuando es diferente del nombre registrado en el sistema
        /// </summary>
        [DevExpress.Xpo.SizeAttribute(80), DbType("varchar(80)"), Index(3)]
        [DevExpress.Persistent.Base.ToolTipAttribute("Nombre de la persona según el documento")]
        [DevExpress.Xpo.IndexedAttribute]
        public System.String Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Lugar Emisión")]
        [DevExpress.Persistent.Base.ToolTipAttribute("Lugar de emisión del documento")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false), DbType("varchar(100)"), Index(4)]
        //[RuleRequiredField("PersonaDocumento.LugarEmision_Requerido", "Save", ResultType = ValidationResultType.Warning, SkipNullOrEmptyValues = true)]
        public System.String LugarEmision
        {
            get => lugarEmision;
            set => SetPropertyValue(nameof(LugarEmision), ref lugarEmision, value);
        }

        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Fecha de Emisión")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false), Index(5)]
        //[RuleRequiredField("PersonaDocumento.FechaEmision_Requerido", "Save", ResultType = ValidationResultType.Warning, SkipNullOrEmptyValues = true)]
        public System.DateTime FechaEmision
        {
            get => fechaEmision;
            set => SetPropertyValue(nameof(FechaEmision), ref fechaEmision, value);
        }

        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false), Index(6)]
        [RuleRequiredField("PersonaDocumento.Vigente_Requerido", "Save")]
        public System.Boolean Vigente
        {
            get => vigente;
            set => SetPropertyValue(nameof(Vigente), ref vigente, value);
        }

        [DevExpress.Xpo.AssociationAttribute("Documentos-Persona")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Persistent.Base.VisibleInListViewAttribute(false)]
        public Persona Persona
        {
            get => persona;
            set => SetPropertyValue(nameof(Persona), ref persona, value);
        }

    }
}
