﻿using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace SBT.Apps.Producto.Module.BusinessObjects
{
    /// <summary>
    /// BO TributoCategoria. Es la relacion entre los tributos y las categorias de productos a los cuales se aplican
    /// </summary>
    [DefaultClassOptions, ModelDefault("Caption", "Productos"), NavigationItem(false), CreatableItem(false),
        DefaultProperty(nameof(Categoria)), Persistent(nameof(TributoCategoria))]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class TributoCategoria : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public TributoCategoria(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        Tributo tributo;
        Categoria categoria;

        [XafDisplayName("Categoría"), Index(0), RuleRequiredField("TributoCategoria.Categoria_Requerido", DefaultContexts.Save)]
        [Association("Categoria-TributosCategoria")]
        [ExplicitLoading]
        public Categoria Categoria
        {
            get => categoria;
            set => SetPropertyValue(nameof(Categoria), ref categoria, value);
        }


        [Association("Tributo-TributoCategorias"), XafDisplayName("Tributo")]
        //[ExplicitLoading]
        public Tributo Tributo
        {
            get => tributo;
            set => SetPropertyValue(nameof(Tributo), ref tributo, value);
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}