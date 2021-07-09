using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Contabilidad.Module.BusinessObjects
{
    [NonPersistent]
    [ModelDefault("Caption", "Estados Financieros"), DefaultProperty(nameof(Nombre)), NavigationItem("Contabilidad")]
    //[Persistent(nameof(EstadoFinanciero))]
    //[ImageName("BO_Contact")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class EstadoFinanciero : XPObjectCustom
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public EstadoFinanciero(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        bool activo;
        DateTime fechaHasta;
        Moneda moneda;
        string nombre;
        Empresa empresa;

        [XafDisplayName("Empresa"), Browsable(false)]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }

        [Size(100), DbType("varchar(100)"), XafDisplayName("Nombre")]
        [RuleRequiredField("EstadoFinanciero.Nombre_Requerido", "Save")]
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        [XafDisplayName("Moneda")]
        [RuleRequiredField("EstadoFinanciero.Moneda_Requerido", "Save")]
        public Moneda Moneda
        {
            get => moneda;
            set => SetPropertyValue(nameof(Moneda), ref moneda, value);
        }

        [NonPersistent]
        [DbType("datetime"), XafDisplayName("Fecha Hasta")]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime FechaHasta
        {
            get => fechaHasta;
            set => SetPropertyValue(nameof(FechaHasta), ref fechaHasta, value);
        }

        [DbType("bit"), XafDisplayName("Activo"), RuleRequiredField("EstadoFinanciero.Activo_Requerido", "Save")]
        public bool Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }

        #endregion

        #region Collecciones
        //[Association("EstadoFinanciero-Detalles"), XafDisplayName("Detalles"), DevExpress.Xpo.Aggregated]
        //public XPCollection<EstadoFinanciero> Detalles => GetCollection<EstadoFinanciero>(nameof(Detalles));
        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}