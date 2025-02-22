﻿using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace SBT.Apps.Banco.Module.BusinessObjects
{
    /// <summary>
    /// Bancos. BO Clasificacion de las transacciones de bancos, para tener una identificacion mas amplia que la proporcionada
    /// por el enum EBancoTipoTransaccion 
    /// </summary>
    [DefaultClassOptions, NavigationItem("Banco"), ModelDefault("Caption", "Clasificación Transacción"),
        Persistent(nameof(BancoTipoTransaccion)), DefaultProperty("Nombre")]
    [ImageName("BancoClasificacionTransac")]
    [CreatableItem(false)]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class BancoTipoTransaccion : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public BancoTipoTransaccion(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            Activa = true;
        }

        #region Propiedades

        bool activa = true;
        EBancoTipoTransaccion tipo = EBancoTipoTransaccion.Abono;
        string nombre;

        [Size(60), DbType("varchar(60)"), Persistent("Nombre"), XafDisplayName("Nombre"),
            RuleRequiredField("BancoClasificacionTransac.Nombre_Requerido", "Save")]
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        [DbType("smallint"), Persistent("Tipo"), XafDisplayName("Tipo Transaccion")]
        public EBancoTipoTransaccion Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        [DbType("bit"), Persistent("Activa"), XafDisplayName("Activa")]
        public bool Activa
        {
            get => activa;
            set => SetPropertyValue(nameof(Activa), ref activa, value);
        }
        /// <summary>
        /// Para facilitar realizar las operaciones de calculo de saldos. Las operaciones positivas aumentan el saldo de la cuenta
        /// afectada por la transaccion, las negativas lo disminuyen  
        /// </summary>
        [PersistentAlias("Iif(Tipo = 1 Or Tipo = 2, 1, -1)"), Browsable(false)]
        public int Operacion
        {
            get { return Convert.ToInt16(EvaluateAlias(nameof(Operacion))); }
        }

        #endregion

        #region Colecciones
        //[Association("BancoClasificacionTransac-Transacciones"), XafDisplayName("Transacciones")]
        //public XPCollection<BancoTransaccion> Transacciones
        //{
        //    get
        //    {
        //        return GetCollection<BancoTransaccion>(nameof(Transacciones));
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