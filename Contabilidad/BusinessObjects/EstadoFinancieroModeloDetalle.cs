﻿using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Contabilidad.BusinessObjects;
using System;
using System.ComponentModel;

namespace SBT.Apps.Contabilidad.Module.BusinessObjects
{
    [NavigationItem(false), CreatableItem(false), ModelDefault("Caption", "Estado Financiero Detalle"),
        DefaultProperty(nameof(Nombre1)), Persistent(nameof(EstadoFinancieroModeloDetalle))]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class EstadoFinancieroModeloDetalle : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public EstadoFinancieroModeloDetalle(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades
        int orden;
        string formula2;
        string nombre2;
        Catalogo cuenta2;
        string formula1;
        string nombre1;
        Catalogo cuenta1;
        Type tipoBO;
        EstadoFinancieroModelo estadoFinanciero;

        [Association("EstadoFinanciero-Detalles"), XafDisplayName("Estado Financiero"), Index(0)]
        public EstadoFinancieroModelo EstadoFinanciero
        {
            get => estadoFinanciero;
            set => SetPropertyValue(nameof(EstadoFinanciero), ref estadoFinanciero, value);
        }

        [XafDisplayName("Orden"), Index(0)]
        public int Orden
        {
            get => orden;
            set => SetPropertyValue(nameof(Orden), ref orden, value);
        }

        /// <summary>
        /// El BO para el cual se implementa la formula
        /// </summary>
        /// <remarks>
        /// Mas info
        /// 1. EditorAliases.TypePropertyEditor implementa la lista de seleccion de los BO
        ///    Ver: https://docs.devexpress.com/eXpressAppFramework/113579/concepts/business-model-design/data-types-supported-by-built-in-editors/type-properties
        /// 2. ValueConverter, para hacer la propiedad persistente se tiliza la conversion a string y guardar el nombre del BO en la bd, incluyendo el namespace
        /// </remarks>
        [XafDisplayName("Tipo Business Object"), Persistent(nameof(TipoBO)), Index(1)]
        [EditorAlias(EditorAliases.TypePropertyEditor)]
        [ValueConverter(typeof(DevExpress.ExpressApp.Utils.TypeToStringConverter)), ImmediatePostData]
        [VisibleInListView(false)]
        public Type TipoBO
        {
            get => tipoBO;
            set => SetPropertyValue(nameof(TipoBO), ref tipoBO, value);
        }

        [XafDisplayName("Cuenta Columna 1"), Index(2)]
        public Catalogo Cuenta1
        {
            get => cuenta1;
            set
            {
                bool changed = SetPropertyValue(nameof(Cuenta1), ref cuenta1, value);
                if (!IsLoading && !IsSaving && changed && value != null)
                {
                    Nombre1 = value.Nombre;
                }
            }
        }

        [Size(150), DbType("varchar(150)"), XafDisplayName("Nombre Columna 1"), Index(3)]
        [RuleRequiredField("EstadoFinancieroDetalle.Nombre1_Requerido", "Save")]
        public string Nombre1
        {
            get => nombre1;
            set => SetPropertyValue(nameof(Nombre1), ref nombre1, value);
        }

        [Size(1000), DbType("varchar(1000)"), XafDisplayName("Fórmula 1"), Persistent(nameof(Formula1))]
        [ElementTypeProperty(nameof(TipoBO))]
        [EditorAlias(EditorAliases.PopupExpressionPropertyEditor)]
        [VisibleInListView(false), Index(4)]
        //[ModelDefault("Width", "50")]
        [ModelDefault("RowCount", "3")]
        [RuleRequiredField("EstadoFinancieroModeloDetalle.Formula1_Requerido", DefaultContexts.Save, TargetCriteria = "!([TipoBO] Is Null) && [Formula2] Is Null")]
        public string Formula1
        {
            get => formula1;
            set => SetPropertyValue(nameof(Formula1), ref formula1, value);
        }

        [XafDisplayName("Cuenta Columna 2"), Index(5)]
        public Catalogo Cuenta2
        {
            get => cuenta2;
            set
            {
                bool changed = SetPropertyValue(nameof(Cuenta2), ref cuenta2, value);
                if (!IsLoading && !IsSaving && changed && value != null)
                    Nombre2 = cuenta2.Nombre;
            }
        }

        [Size(150), DbType("varchar(150)"), XafDisplayName("Nombre Columna 2"), Index(6)]
        public string Nombre2
        {
            get => nombre2;
            set => SetPropertyValue(nameof(Nombre2), ref nombre2, value);
        }

        [Size(1000), DbType("varchar(1000)"), XafDisplayName("Fórmula 2"), Persistent(nameof(Formula2))]
        [ElementTypeProperty(nameof(TipoBO))]
        [EditorAlias(EditorAliases.PopupExpressionPropertyEditor)]
        [VisibleInListView(false), Index(7)]
        //[ModelDefault("Width", "50")]
        [ModelDefault("RowCount", "3")]
        [RuleRequiredField("EstadoFinancieroModeloDetalle.Formula2_Requerido", DefaultContexts.Save, TargetCriteria = "!([TipoBO] Is Null) && [Formula1] Is Null")]
        public string Formula2
        {
            get => formula2;
            set => SetPropertyValue(nameof(Formula2), ref formula2, value);
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}