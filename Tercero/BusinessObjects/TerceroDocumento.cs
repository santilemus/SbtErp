﻿using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Tercero.Module.BusinessObjects
{
    /// <summary>
    /// TerceroDocumento BO
    /// BO que corresponde a los documentos de los terceros. Es para permitir vincular a un tercero, que puede ser persona
    /// natural o jurídica, más de un documento. Por ejemplo: NIT, DUI, PASAPORTE. En el caso de personas jurídicas, aplican
    /// NIT y cualquier otro documento vinculado a su personería
    /// </summary>
    [ModelDefault("Caption", "Tercero Documentos"), NavigationItem(false), DefaultProperty(nameof(Numero))]
    [Persistent(nameof(TerceroDocumento)), CreatableItem(false)]
    [ImageName("user_id-info")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class TerceroDocumento : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public TerceroDocumento(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades
        Tercero tercero;
        private System.DateTime fechaEmision;
        private System.Boolean vigente;
        private System.String lugarEmision;
        private System.String nombre;
        private System.String numero;
        private Listas tipo;

        [ImmediatePostData(true), Index(0), VisibleInLookupListView(true)]
        [RuleRequiredField("TerceroDocumento.Tipo_Requerido", DefaultContexts.Save, "Tipo Documento es requerido")]
        [DataSourceCriteria("[Categoria] = 10"), XafDisplayName("Tipo")]
        [ExplicitLoading]
        public Listas Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        [Size(14), DbType("varchar(14)"), XafDisplayName("Número"), Index(2)]
        [RuleRequiredField("TerceroDocumento.Numero_Requerido", "Save"), Indexed("Tipo", Name = "idxTerceroNoDocumento")]
        public System.String Numero
        {
            get => numero;
            set => SetPropertyValue(nameof(Numero), ref numero, value);
        }

        /// <summary>
        /// Nombre del tercero en el documento. Cuando es diferente del nombre registrado en el sistema
        /// </summary>
        [Size(80), DbType("varchar(80)"), ToolTip("Nombre del tercero según el documento"), Index(3)]
        public System.String Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        [XafDisplayName("Lugar Emisión"), ToolTip("Lugar de emisión del documento"), VisibleInListView(false),
            VisibleInLookupListView(false), DbType("varchar(100)"), Index(4)]
        [RuleRequiredField("TerceroDocumento.LugarEmision_Requerido", "Save", TargetCriteria = "[Tipo.Codigo] In ('DUI', 'PAS', 'RES')")]
        public System.String LugarEmision
        {
            get => lugarEmision;
            set => SetPropertyValue(nameof(LugarEmision), ref lugarEmision, value);
        }

        [XafDisplayName("Fecha Emisión"), VisibleInLookupListView(false)]
        [RuleRequiredField("TerceroDocumento.FechaEmision_Requerido", "Save", TargetCriteria = "[Tipo.Codigo] In ('DUI', 'PAS', 'RES')")]
        [RuleRequiredField("TerceroDocumento.LugarEmision_Information", "Save",
            TargetCriteria = "!([Tipo.Codigo] In ('DUI', 'PAS', 'RES'))", ResultType = ValidationResultType.Information)]
        [Index(5), VisibleInListView(false)]
        public System.DateTime FechaEmision
        {
            get => fechaEmision;
            set => SetPropertyValue(nameof(FechaEmision), ref fechaEmision, value);
        }

        [VisibleInLookupListView(false), Index(6)]
        [RuleRequiredField("TerceroDocumento.Vigente_Requerido", "Save")]
        public System.Boolean Vigente
        {
            get => vigente;
            set => SetPropertyValue(nameof(Vigente), ref vigente, value);
        }


        [Association("Tercero-Documentos"), XafDisplayName("Tercero"), VisibleInListView(false), VisibleInLookupListView(false), Index(7)]
        public Tercero Tercero
        {
            get => tercero;
            set => SetPropertyValue(nameof(Tercero), ref tercero, value);
        }

        #endregion

        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }

}