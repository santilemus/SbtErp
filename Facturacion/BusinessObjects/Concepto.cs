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

namespace SBT.Apps.CxC.Module.BusinessObjects
{
    /// <summary>
    /// BO Conceptos de Cuentas por Cobrar. Contiene la parametrizacion de las diferentes tipos de transacciones de cuentas
    /// por cobrar. Ejemplo: Notas de Credito -> Devolucion, Descuento por Pronto Pago, etc, Pagos -> Pagos Efectivo, Transferencias, etc.
    /// </summary>

    [DefaultClassOptions, ModelDefault("Caption", "Concepto CxC y CxP"), DefaultProperty("Nombre"), NavigationItem("Cuenta por Cobrar"), 
        Persistent("CxCConcepto")]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Concepto : XPCustomBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Concepto(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        int oid;
        bool activo = true;
        Concepto padre;
        ETipoOperacion tipoOperacion = ETipoOperacion.Cargo;
        string codigo;
        string nombre;
        Listas tipoDocumento;

        [DbType("smallint"), Key(true), XafDisplayName("Oid")]
        public int Oid
        {
            get => oid;
            set => SetPropertyValue(nameof(Oid), ref oid, value);
        }

        [Association("Concepto-Conceptos"), XafDisplayName("Concepto Padre"), Index(0)]
        public Concepto Padre
        {
            get => padre;
            set
            {
                bool changed = SetPropertyValue(nameof(Padre), ref padre, value);
                if (!IsLoading && !IsSaving && changed && Session.IsNewObject(this) && Padre != null)
                {
                    Codigo = Padre.Codigo;
                    TipoOperacion = Padre.TipoOperacion;
                    TipoDocumento = Padre.TipoDocumento;
                }
            }
        }

        [Size(8), DbType("varchar(8)"), XafDisplayName("Código"), Index(1), RuleRequiredField("CxC-Concepto.Codigo_Requerido", "Save")]
        [Indexed(Name = "idxCxCConcepto_Codigo")]
        public string Codigo
        {
            get => codigo;
            set => SetPropertyValue(nameof(Codigo), ref codigo, value);
        }

        [Size(60), DbType("varchar(60)"), Index(2), RuleRequiredField("CxC-Concepto.Nombre_Requerido", "Save")]
        [RuleRequiredField("Concepto.Nombre_Requerido", "Save")]
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        /// <summary>
        /// Indica el tipo de operacion. Puede ser: un cargo o un abono
        /// </summary>
        [DbType("smallint"), XafDisplayName("Tipo Operación"), Index(3), VisibleInLookupListView(true)]
        public ETipoOperacion TipoOperacion
        {
            get => tipoOperacion;
            set => SetPropertyValue(nameof(TipoOperacion), ref tipoOperacion, value);
        }

        /// <summary>
        /// Solo aplica para los conceptos que requieren de una autorizacion de correlativos
        /// </summary>
        [XafDisplayName("Tipo Documento")]
        [RuleRequiredField("Concepto.TipoDocumento_Requerido", DefaultContexts.Save, ResultType = ValidationResultType.Information,
            CustomMessageTemplate = "Se sugiere que indique el tipo de documento para el registro")]
        [DataSourceCriteria("[Categoria] == 16 And [Activo] == True"), VisibleInLookupListView(true), Index(4)]
        public Listas TipoDocumento
        {
            get => tipoDocumento;
            set => SetPropertyValue(nameof(TipoDocumento), ref tipoDocumento, value);
        }

        [DbType("bit"), XafDisplayName("Activo"), Index(5), RuleRequiredField("Concepto.Activo_Requerido", "Save")]
        public bool Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }

        #endregion

        #region Colecciones
        [Association("Concepto-Conceptos"), XafDisplayName("Conceptos Hijos"), Index(0)]
        public XPCollection<Concepto> Conceptos
        {
            get
            {
                return GetCollection<Concepto>(nameof(Conceptos));
            }
        }

        [Association("Concepto-Transacciones"), XafDisplayName("Transacciones"), Index(1)]
        public XPCollection<CxCTransaccion> Transacciones
        {
            get
            {
                return GetCollection<CxCTransaccion>(nameof(Transacciones));
            }
        }


        #endregion


        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}