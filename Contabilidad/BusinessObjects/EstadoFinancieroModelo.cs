﻿using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Contabilidad.Module.BusinessObjects
{
    [DefaultClassOptions, ModelDefault("Caption", "Estado Financiero Modelo"), DefaultProperty(nameof(Nombre)), NavigationItem("Contabilidad")]
    [Persistent(nameof(EstadoFinancieroModelo)), CreatableItem(false)]
    //[ImageName("BO_Contact")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class EstadoFinancieroModelo : XPObjectCustom
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public EstadoFinancieroModelo(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        ReportDataV2 reporte;
        bool activo;
        string nombre;
        Empresa empresa;

        [XafDisplayName("Empresa"), VisibleInListView(false), VisibleInDetailView(false), Index(0)]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }

        [Size(100), DbType("varchar(100)"), XafDisplayName("Nombre"), Index(1)]
        [RuleRequiredField("EstadoFinanciero.Nombre_Requerido", "Save")]
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        [DbType("bit"), XafDisplayName("Activo"), RuleRequiredField("EstadoFinanciero.Activo_Requerido", "Save")]
        [Index(3)]
        public bool Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }

        
        [XafDisplayName("Reporte")]
        public ReportDataV2 Reporte
        {
            get => reporte;
            set => SetPropertyValue(nameof(Reporte), ref reporte, value);
        }

        #endregion

        #region Collecciones
        [Association("EstadoFinanciero-Detalles"), XafDisplayName("Detalles"), DevExpress.Xpo.Aggregated]
        public XPCollection<EstadoFinancieroModeloDetalle> Detalles => GetCollection<EstadoFinancieroModeloDetalle>(nameof(Detalles));
        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}