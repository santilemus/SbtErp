﻿using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using SBT.Apps.Base.Module.BusinessObjects;

namespace SBT.Apps.Tercero.Module.BusinessObjects
{
    [DefaultClassOptions, ModelDefault("Caption", "Tercero Dirección"), NavigationItem(false), DefaultProperty(nameof(Direccion))]
    [Persistent("TerceroDireccion"), CreatableItem(false)]
    [ImageName("address")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class TerceroDireccion : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public TerceroDireccion(Session session)
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
        private ZonaGeografica pais;
        private ZonaGeografica provincia;
        private ZonaGeografica ciudad;
        private System.String direccion;
        bool activa;

        [Association("Tercero-Direcciones"), XafDisplayName("Tercero"), Index(0)]
        public Tercero Tercero
        {
            get => tercero;
            set => SetPropertyValue(nameof(Tercero), ref tercero, value);
        }

        [VisibleInLookupListView(false), XafDisplayName("País"), ImmediatePostData, Index(1), VisibleInListView(false)]
        [DataSourceCriteria("ZonaPadre is null and Activa = true")]
        [ExplicitLoading]
        
        public ZonaGeografica Pais
        {
            get => pais;
            set => SetPropertyValue(nameof(Pais), ref pais, value);
        }

        [XafDisplayName("Provincia"), ImmediatePostData(true), VisibleInLookupListView(false), Index(2)]
        [DataSourceCriteria("ZonaPadre = '@This.Pais' and Activa = true")]
        public ZonaGeografica Provincia
        {
            get => provincia;
            set => SetPropertyValue(nameof(Provincia), ref provincia, value);
        }

        [XafDisplayName("Ciudad"), VisibleInLookupListView(true), Index(4)]
        [DataSourceCriteria("ZonaPadre = '@This.Provincia' and Activa = true")]
        public ZonaGeografica Ciudad
        {
            get => ciudad;
            set => SetPropertyValue(nameof(Ciudad), ref ciudad, value);
        }

        [XafDisplayName("Dirección"), Size(200), DbType("varchar(200)")]
        [RuleRequiredField("Tercero.Direccion_Requerida", "Save"), Index(5)]
        public System.String Direccion
        {
            get => direccion;
            set => SetPropertyValue(nameof(Direccion), ref direccion, value);
        }

        [DbType("bit"), XafDisplayName("Activa"), RuleRequiredField("TerceroDireccion.Activa", "Save"), Index(6), 
            VisibleInListView(false), VisibleInLookupListView(false)]
        public bool Activa
        {
            get => activa;
            set => SetPropertyValue(nameof(Activa), ref activa, value);
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}