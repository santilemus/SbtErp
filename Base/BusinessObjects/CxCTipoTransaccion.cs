﻿using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// BO Conceptos de Cuentas por Cobrar y por Pagar. Contiene la parametrizacion de las diferentes tipos de transacciones de cuentas
    /// por cobrar y pagar. Ejemplo: Notas de Credito -> Devolucion, Descuento por Pronto Pago, etc, Pagos -> Pagos Efectivo, Transferencias, etc.
    /// </summary>

    [DefaultClassOptions, ModelDefault("Caption", "Tipo Transaccion (CxC - CxP)"), DefaultProperty("Nombre"), NavigationItem("Catalogos"),
        Persistent(nameof(CxCTipoTransaccion))]
    [ImageName("Concepto")]
    [CreatableItem(false)]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CxCTipoTransaccion : XPCustomObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public CxCTipoTransaccion(Session session)
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
        CxCTipoTransaccion padre;
        ETipoOperacion tipoOperacion = ETipoOperacion.Cargo;
        string codigo;
        string nombre;
        Listas tipoDocumento;
        //private int bancoTipoTransaccionCargo;
        //private int bancoTipoTransaccionAbono;

        [DbType("smallint"), Key(true), XafDisplayName("Oid")]
        public int Oid
        {
            get => oid;
            set => SetPropertyValue(nameof(Oid), ref oid, value);
        }

        [Association("TipoTransaccion-TipoTransacciones"), XafDisplayName("Padre"), Index(0)]
        public CxCTipoTransaccion Padre
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
        [DataSourceCriteria("[Categoria] == 16 And [Activo] == True"), Index(4)]
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

        /*
        public int BancoTipoTransaccionCargo
        {
            get => bancoTipoTransaccionCargo;
            set => SetPropertyValue(nameof(BancoTipoTransaccionCargo), ref bancoTipoTransaccionCargo, value);
        } 

        public int BancoTipoTransaccionAbono
        {
            get => bancoTipoTransaccionAbono;
            set => SetPropertyValue(nameof(BancoTipoTransaccionAbono), ref bancoTipoTransaccionAbono, value);
        }
        */

        #endregion

        #region Colecciones
        [Association("TipoTransaccion-TipoTransacciones"), XafDisplayName("Transacciones Hijas"), Index(0)]
        public XPCollection<CxCTipoTransaccion> Conceptos
        {
            get
            {
                return GetCollection<CxCTipoTransaccion>(nameof(Conceptos));
            }
        }

        //[Association("TipoTransaccion-Transacciones"), XafDisplayName("Transacciones"), Index(1)]
        //public XPCollection<CxCTransaccion> Transacciones
        //{
        //    get
        //    {
        //        return GetCollection<CxCTransaccion>(nameof(Transacciones));
        //    }
        //}


        #endregion


        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}