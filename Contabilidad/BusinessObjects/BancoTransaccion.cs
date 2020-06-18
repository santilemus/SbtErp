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
using SBT.Apps.Empleado.Module.BusinessObjects;
using SBT.Apps.Tercero.Module.BusinessObjects;

namespace SBT.Apps.Banco.BusinessObjects
{
    /// <summary>
    /// Banco. BO para las transacciones de bancos
    /// </summary>

    [DefaultClassOptions, ModelDefault("Caption", "Transacción Bancaria"), NavigationItem("Banco"), Persistent("BanTransaccion"),
        DefaultProperty("Concepto")]  // revisar si es la propiedad por default adecuada
    [RuleCombinationOfPropertiesIsUnique("BancoTransaccion.Empresa_Clasificacion_NumeroPortipo", DefaultContexts.Save,
        "Empresa,Clasificacion,NumeroPorTipo", CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction, SkipNullOrEmptyValues = false)]
    [RuleCriteria("BancoTransaccion Cuenta.Empresa = Transaccion.Empresa", DefaultContexts.Save, "[NumeroCuenta.Empresa] = [Empresa]", 
        SkipNullOrEmptyValues = false, InvertResult = true)]
    [ImageName(nameof(BancoTransaccion))]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class BancoTransaccion : XPOBaseDoc
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public BancoTransaccion(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (Clasificacion.Tipo == EBancoTipoTransaccion.Cheque && Session.IsNewObject(this) && chequera != null)
            {
                if (ChequeNo >= chequera.NumeroInicio && ChequeNo < chequera.NumeroFin)
                    chequera.NumeroActual++;
                chequera.Save();
            }
        }

        protected override void OnLoading()
        {
            base.OnLoading();
            chequera = null;
        }

        #region Propiedades
        BancoCuenta numeroCuenta;
        BancoClasificacionTransac clasificacion;
        [DbType("int"), Persistent("ChequeNo")]
        int chequeNo;
        [Size(8), DbType("varchar(8)"), Persistent("Serie")]
        string serie;
        SBT.Apps.Tercero.Module.BusinessObjects.Tercero proveedor;
        string beneficiario;
        decimal monto = 0.0m;
        [DbType("int"), Persistent("NumeroPorTipo")]
        int? numeroPorTipo = null;
        EBancoTransaccionEstado estado = EBancoTransaccionEstado.Digitado;
        [Persistent("Partida")]
        SBT.Apps.Contabilidad.Module.BusinessObjects.Partida partida = null;
        // otros fields
        BancoChequera chequera;

        [Association("BancoCuenta-Transacciones"), Persistent("NumeroCuenta"), XafDisplayName("Número Cuenta")]
        public BancoCuenta NumeroCuenta
        {
            get => numeroCuenta;
            set => SetPropertyValue(nameof(NumeroCuenta), ref numeroCuenta, value);
        }

        [Persistent("Proveedor"), XafDisplayName("Proveedor")]
        public SBT.Apps.Tercero.Module.BusinessObjects.Tercero Proveedor
        {
            get => proveedor;
            set
            {
                var changed = SetPropertyValue(nameof(Proveedor), ref proveedor, value);
                if (!IsLoading && !IsSaving && changed)
                    Beneficiario = value.Nombre;
            }
        }

        [PersistentAlias(nameof(serie)), XafDisplayName("No Serie")]
        public string Serie
        {
            get => serie;
        }

        //[Association("BancoClasificacionTransac-Transacciones")]
        [Persistent("Clasificacion"), XafDisplayName("Clasificacion"), RuleRequiredField("BancoTransaccion.Tipo_Requerido", "Save")]
        [ToolTip("Clasificación de Transacciones de bancos", "Bancos", DevExpress.Utils.ToolTipIconType.Information)]
        public BancoClasificacionTransac Clasificacion
        {
            get => clasificacion;
            set
            {
                var changed = SetPropertyValue(nameof(Clasificacion), ref clasificacion, value);
                if (!IsLoading && !IsSaving && changed && value.Tipo == EBancoTipoTransaccion.Cheque)
                {
                    chequera = Session.FindObject<BancoChequera>(
                        DevExpress.Data.Filtering.CriteriaOperator.Parse("NumeroCuenta = ? And FechaFin Is Null", NumeroCuenta));
                    if (chequera != null)
                        serie = chequera.Serie;
                    if (chequera != null && Session.IsNewObject(this))
                        chequeNo = chequera.NumeroActual;
                }
            }
        }

        [XafDisplayName("Número x Clasificación"), PersistentAlias(nameof(numeroPorTipo))]
        public int? NumeroPorTipo
        {
            get => numeroPorTipo;
        }

        [XafDisplayName("Cheque No")]
        [PersistentAlias(nameof(chequeNo))]
        public int ChequeNo
        {
            get => chequeNo;
        }

        [Size(150), DbType("varchar(150)"), Persistent("Beneficiario"), XafDisplayName("Paguese A")]
        public string Beneficiario
        {
            get => beneficiario;
            set => SetPropertyValue(nameof(Beneficiario), ref beneficiario, value);
        }

        [DbType("money"), Persistent("Monto"), XafDisplayName("Monto")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [RuleValueComparison("BancoTransaccion.Monto >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = false)]
        public decimal Monto
        {
            get => monto;
            set => SetPropertyValue(nameof(Monto), ref monto, value);
        }

        [DbType("smallint"), Persistent("Estado"), XafDisplayName("Estado")]
        public EBancoTransaccionEstado Estado
        {
            get => estado;
            set => SetPropertyValue(nameof(Estado), ref estado, value);
        }

        [PersistentAlias(nameof(partida)), XafDisplayName("Partida")]
        public SBT.Apps.Contabilidad.Module.BusinessObjects.Partida Partida
        {
            get => partida;
        }

        [PersistentAlias("[Partida.Oid]"), XafDisplayName("Partida No")]
        public int PartidaNumero
        {
            get { return Convert.ToInt32(EvaluateAlias("PartidaNumero")); }
        }       

        [PersistentAlias("Iif([Clasificacion.Tipo] = 1 Or [Clasificacion.Tipo] = 2, [Monto], 0)")]
        [XafDisplayName("Abono"), ModelDefault("DisplayFormat", "{0:N2}")]
        public decimal Abono
        {
            get { return Convert.ToDecimal(EvaluateAlias("Abono")); }
        }


        [PersistentAlias("Iif([Clasificacion.Tipo] = 3 Or [Clasificacion.Tipo] = 4, [Monto], 0)")]
        [XafDisplayName("Cargo"), ModelDefault("DisplayFormat", "{0:N2}")]
        public decimal Cargo
        {
            get { return Convert.ToDecimal(EvaluateAlias("Cargo")); }
        }
        #endregion

        #region Colecciones
        [Association("BancoTransaccion-Detalles"), DevExpress.Xpo.Aggregated, XafDisplayName("Detalle")]
        public XPCollection<BancoTransaccionDetalle> Detalles
        {
            get
            {
                return GetCollection<BancoTransaccionDetalle>(nameof(Detalles));
            }
        }

        [Association("BancoTransaccion-Conciliaciones"), XafDisplayName("Conciliaciones")]
        public XPCollection<BancoConciliacionDetalle> Conciliaciones
        {
            get
            {
                return GetCollection<BancoConciliacionDetalle>(nameof(Conciliaciones));
            }
        }
        #endregion
        [Action(Caption = "Entregar Cheque", ConfirmationMessage = "Esta Seguro?", TargetObjectsCriteria = "Clasificacion.Tipo = 3",
            SelectionDependencyType = MethodActionSelectionDependencyType.RequireSingleObject, ImageName = "Attention", AutoCommit = true,
            ToolTip = "Marcar el cheque seleccionado como entregado")]
        public void Entregado()
        {
            Estado = EBancoTransaccionEstado.Entregado;
            Save();

        }

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}