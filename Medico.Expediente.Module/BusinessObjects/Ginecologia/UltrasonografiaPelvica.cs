using System;
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
using SBT.Apps.Medico.Generico.Module.BusinessObjects;

namespace SBT.Apps.Medico.Expediente.Module.BusinessObjects.Ginecologia
{
    [DefaultClassOptions, ModelDefault("Caption", "Ultrasonografía Pélvica"), NavigationItem(false), 
        DefaultProperty(nameof(Fecha)), Persistent(nameof(UltrasonografiaPelvica)), CreatableItem(false)]
    [ImageName("UltraSonografiaPelvica")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class UltrasonografiaPelvica : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public UltrasonografiaPelvica(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        string observaciones;
        string fondoSaco;
        string trompaFalopioDerecha;
        string trompaFalopioIzquierda;
        string ovarioDerecho;
        string ovarioIzquierdo;
        string utero;
        ETipoUltrasonografiaPelvica tipoUltrasonografia;
        DateTime fecha;
        Empleado.Module.BusinessObjects.Empleado tecnico;
        Consulta consulta;

        [Association("Consulta-UltrasonografiaPelvicas")]
        public Consulta Consulta
        {
            get => consulta;
            set => SetPropertyValue(nameof(Consulta), ref consulta, value);
        }

        [XafDisplayName("Técnico"), ToolTip("Técnico responsable de realizar la ultrasonografía")]
        public Empleado.Module.BusinessObjects.Empleado Tecnico
        {
            get => tecnico;
            set => SetPropertyValue(nameof(Tecnico), ref tecnico, value);
        }

        [DbType("datetime2"), XafDisplayName("Fecha")]
        [RuleRequiredField("UltrasonografiaPelvica.Fecha_Requerido", DefaultContexts.Save)]
        public DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
        }

        [DbType("smallint"), XafDisplayName("Tipo")]
        public ETipoUltrasonografiaPelvica TipoUltrasonografia
        {
            get => tipoUltrasonografia;
            set => SetPropertyValue(nameof(TipoUltrasonografia), ref tipoUltrasonografia, value);
        }


        [Size(150), DbType("varchar(150)"), XafDisplayName("Útero")]
        public string Ovario
        {
            get => utero;
            set => SetPropertyValue(nameof(Ovario), ref utero, value);
        }


        [Size(150), DbType("varchar(150)"), XafDisplayName("Ovario Izquierdo")]
        public string OvarioIzquierdo
        {
            get => ovarioIzquierdo;
            set => SetPropertyValue(nameof(OvarioIzquierdo), ref ovarioIzquierdo, value);
        }


        [Size(150), DbType("varchar(150)"), XafDisplayName("Ovario Derecho")]
        public string OvarioDerecho
        {
            get => ovarioDerecho;
            set => SetPropertyValue(nameof(OvarioDerecho), ref ovarioDerecho, value);
        }


        [Size(150), DbType("varchar(150)"), XafDisplayName("Trompa Falopio Izquierda"), VisibleInListView(false)]
        public string TrompaFalopioIzquierda
        {
            get => trompaFalopioIzquierda;
            set => SetPropertyValue(nameof(TrompaFalopioIzquierda), ref trompaFalopioIzquierda, value);
        }


        [Size(150), DbType("varchar(150)"), XafDisplayName("Trompa Falopio Derecha"), VisibleInListView(false)]
        public string TrompaFalopioDerecha
        {
            get => trompaFalopioDerecha;
            set => SetPropertyValue(nameof(TrompaFalopioDerecha), ref trompaFalopioDerecha, value);
        }


        [Size(150), DbType("varchar(150)"), XafDisplayName("Fondo de Saco"), VisibleInListView(false)]
        public string FondoSaco
        {
            get => fondoSaco;
            set => SetPropertyValue(nameof(FondoSaco), ref fondoSaco, value);
        }


        [Size(250), DbType("varchar(250)"), XafDisplayName("Observaciones"), VisibleInListView(false)]
        public string Observaciones
        {
            get => observaciones;
            set => SetPropertyValue(nameof(Observaciones), ref observaciones, value);
        }


        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}