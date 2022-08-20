﻿using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace SBT.Apps.Contabilidad.Module.BusinessObjects
{
    /// <summary>
    /// Contabilidad. BO correspondientes al encabezado de las partidas contables
    /// </summary>
    [NavigationItem("Contabilidad"), ModelDefault("Caption", "Partida Contable"), XafDefaultProperty(nameof(Numero)), CreatableItem(false)]
    [ImageName(nameof(Partida)), MapInheritance(MapInheritanceType.OwnTable)]
    [VisibleInReports(true)]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    [Persistent("ConPartida")]
    [Appearance("Partida_Incompleta", AppearanceItemType = "ViewItem", TargetItems = "*",
        Criteria = "([TotalDebe] Is Null || [TotalDebe] == 0) || ([TotalHaber] Is Null || [TotalHaber] == 0) || ([TotalDebe] != [TotalHaber])",
        FontColor = "Red", Context = "ListView", Priority = 1)]
    [Appearance("Partida.TransaccionPartidas Ocultar", Criteria = "This is not null && (IsNewObject(This) || [BancoTransaccionPartidas].Count() > 0)", 
            TargetItems = nameof(BancoTransaccionPartidas), AppearanceItemType = "ViewItem",
            Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "DetailView")]

    [Appearance("Partida.Mayorizada", Criteria = "[Mayorizada] == True", TargetItems = "*", Context = "DetailView", Enabled = false)]
    [Appearance("Partida.Mayorizada2", Criteria = "[Mayorizada] == True", TargetItems = "Delete", Context = "Any", AppearanceItemType = "Action", Enabled = false)]

    [ListViewFilter("Todos", "")]
    [ListViewFilter("Partidas de Días Abiertos", "[Mayorizada] == False")]
    [ListViewFilter("Partidas Incompletas", "([TotalDebe] Is Null || [TotalDebe] == 0) || ([TotalHaber] Is Null || [TotalHaber] == 0) || ([TotalDebe] != [TotalHaber])")]
    [ListViewFilter("Partidas Ultimo 30 Días", "[Fecha] >= ADDDAYS(LocalDateTimeToday(), -30)")]
    [ListViewFilter("Partidas del Período", "GetYear([Fecha]) == GetYear(LocalDateTimeToday())", true)]
    [ListViewFilter("Partidas del Período Anterior", "(GetYear([Fecha])) == (GetYear(LocalDateTimeToday()) - 1)")]
    [RuleCriteria("Partida Cuadre", DefaultContexts.Save, "[Detalles][].Count() > 0 && [TotalDebe] == [TotalHaber]", 
        "Debe cuadrar la Partida Contable y asegurarse que no este vacía, antes de guardar", UsedProperties = "TotalDebe,TotalHaber")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Partida : XPOBaseDoc
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Partida(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            mayorizada = false;
            elaboro = null;
            Numero = -1;
            Oid = -1;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        protected override void OnLoaded()
        {
            Reset();
            base.OnLoaded();
        }

        private void Reset()
        {
            totalDebe = null;
            totalHaber = null;
        }

        #region Propiedades

        Periodo periodo;
        ETipoPartida tipo = ETipoPartida.Diario;
        int? presupuesto;
        string concepto;
        [Persistent(nameof(Elaboro))]
        SBT.Apps.Empleado.Module.BusinessObjects.Empleado elaboro;
        //[Persistent(nameof(TotalHaber))]
        decimal? totalHaber;
        //[Persistent(nameof(TotalDebe))]
        decimal? totalDebe;
        [Persistent(nameof(Mayorizada)), DbType("bit")]
        bool mayorizada;

        [DbType("int"), Persistent("Periodo"), XafDisplayName("Período"), VisibleInLookupListView(false), VisibleInListView(false)]
        [RuleRequiredField("Partida.Periodo_Requerido", "Save", SkipNullOrEmptyValues = false)]
        public Periodo Periodo
        {
            get => periodo;
            set => SetPropertyValue(nameof(Periodo), ref periodo, value);
        }

        [DbType("smallint"), Persistent("Tipo"), XafDisplayName("Tipo"), RuleRequiredField("Partida.Tipo_Requerido", DefaultContexts.Save)]
        public ETipoPartida Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        [Size(150), DbType("varchar(150)"), Persistent("Concepto"), XafDisplayName("Concepto")]
        public string Concepto
        {
            get => concepto;
            set => SetPropertyValue(nameof(Concepto), ref concepto, value);
        }

        /// <summary>
        /// Presupuesto que sera afectado con la transaccion contable
        /// </summary>
        /// <remarks>
        ///  Esta columna es por compatibilidad con sistema contable creado en delphi. Cuando se incorpore el modulo de presupuestos
        ///  debera cambiar el clasificacion para corresponder al BO de Presupuestos
        /// </remarks>
        [DbType("int"), Persistent("Presupuesto"), XafDisplayName("Presupuesto"), Browsable(false)]
        public int? Presupuesto
        {
            get => presupuesto;
            set => SetPropertyValue(nameof(Presupuesto), ref presupuesto, value);
        }


        [XafDisplayName("Elaborado Por"), PersistentAlias("elaboro"), VisibleInListView(false), VisibleInLookupListView(false)]
        public SBT.Apps.Empleado.Module.BusinessObjects.Empleado Elaboro
        {
            get => elaboro;
        }

        /// <summary>
        /// Calcula los totales a partir del detalle y ademas guarda en el master (encabezado de la partida)
        /// los totales
        /// </summary>
        /// <remarks>
        /// Se implemento de acuerdo a lo expuesto en
        /// https://docs.devexpress.com/eXpressAppFramework/113179/task-based-help/business-model-design/express-persistent-objects-xpo/how-to-calculate-a-property-value-based-on-values-from-a-detail-collection
        /// https://supportcenter.devexpress.com/ticket/details/KA18699/how-to-implement-dependent-and-calculated-properties-in-xpo
        /// </remarks>
        //[PersistentAlias(nameof(totalDebe))]
        [XafDisplayName("Debe")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [RuleValueComparison("Partida.TotalDebe = TotalHaber", DefaultContexts.Save, ValueComparisonType.Equals, "[TotalHaber]", ParametersMode.Expression, 
            CustomMessageTemplate = "La Partida {TargetObject.Numero} no esta cuadrada")]
        public decimal? TotalDebe
        {
            get
            {
                if (!IsLoading && !IsSaving && totalDebe == null)
                    UpdateTotDebe(false);
                return totalDebe;

            }
        }

        //[PersistentAlias(nameof(totalHaber))]
        [XafDisplayName("Haber")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal? TotalHaber
        {
            get
            {
                if (!IsLoading && !IsSaving && totalHaber == null)
                    UpdateTotHaber(false);
                return totalHaber;
            }
        }

        [PersistentAlias(nameof(mayorizada)), XafDisplayName("Mayorizada")]
        public bool Mayorizada
        {
            get { return mayorizada; }
        }

        //[Appearance("Partida.TransaccionPartidas Mostrar", TargetItems = nameof(BancoTransaccionPartidas), AppearanceItemType = "ViewItem",
        //    Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "DetailView")]
        //private bool HideBancoTransaccionPartidasCollection()
        //{
        //    return (Session.IsNewObject(this) || BancoTransaccionPartidas.Count == 0);
        //}

        #endregion

        #region Colecciones
        [Association("Partida-Detalles"), DevExpress.Xpo.Aggregated, XafDisplayName("Detalle")]
        public XPCollection<PartidaDetalle> Detalles
        {
            get
            {
                return GetCollection<PartidaDetalle>(nameof(Detalles));
            }
        }

        [Association("Partida-BancoTransaccionPartidas"), XafDisplayName("Banco Transacciones")]
        public XPCollection<BancoTransaccionPartida> BancoTransaccionPartidas => GetCollection<BancoTransaccionPartida>(nameof(BancoTransaccionPartidas));

        #endregion

        #region Metodos
        public void UpdateTotDebe(bool forceChangeEvents)
        {
            decimal? oldTotalDebe = totalDebe;
            totalDebe = Convert.ToDecimal(Evaluate(CriteriaOperator.Parse("[Detalles].Sum([ValorDebe])")));
            if (forceChangeEvents)
                OnChanged(nameof(TotalDebe), oldTotalDebe, totalDebe);
        }

        public void UpdateTotHaber(bool forceChangeEvents)
        {
            decimal? oldTotalHaber = totalHaber;
            totalHaber = Convert.ToDecimal(Evaluate(CriteriaOperator.Parse("[Detalles].Sum([ValorHaber])")));
            if (forceChangeEvents)
                OnChanged(nameof(TotalHaber), oldTotalHaber, totalHaber);
        }

        public void DoCierreMes(DateTime AFecha)
        {
            Session.BeginTransaction();
            try
            {
                Session.ExecuteNonQuery("update ConCierre set MesCerrado = 1 where Empresa = @Empresa And Month(FechaCierre) = @Mes And Year(FechaCierre) = @Anio",
                    new string[] { "@Empresa", "@Mes", "@Anio" }, new object[] { ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid, AFecha.Month, AFecha.Year });
                if (Session.InTransaction)
                    Session.CommitTransaction();
            }
            catch (Exception e)
            {
                Session.RollbackTransaction();
                throw new UserFriendlyException(e);
            }
        }

        public bool ExistenDiasAbiertos(int AMes, int AAnio)
        {
            var obj = Session.Evaluate(typeof(Partida), CriteriaOperator.Parse("Count()"),
                CriteriaOperator.Parse("[Empresa] = ? And GetMonth([Fecha]) = ? And GetYear([Fecha]) = ? And [Mayorizada] = True",
                ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid, AMes, AAnio));
            return Convert.ToInt32(obj) > 0;
        }

        public bool ExistePartidaDe(int APeriodo, ETipoPartida ATipo)
        {
            var obj = Session.Evaluate<Partida>(CriteriaOperator.Parse("Count()"),
                CriteriaOperator.Parse("[Empresa] = ? And [Periodo] = ? And [Tipo] = ?", ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid, APeriodo, (int)ATipo));
            return Convert.ToInt32(obj) > 0;
        }

        [Browsable(false)]
        [RuleFromBoolProperty("Partida.Tipo No Valido", DefaultContexts.Save,
            "Tipo {TargetObject.Tipo} no es válido porque ya existe",
            SkipNullOrEmptyValues = false, UsedProperties = "Tipo", TargetCriteria = "[Periodo.Oid] = GetYear([Fecha]) And [Tipo] In ('Diario', 'Liquidacion', 'Cierre')")]
        public bool TipoPartidaValida
        {
            get
            {
                switch (Tipo)
                {
                    case ETipoPartida.Apertura:
                        return !ExistePartidaDe(Periodo.Oid, ETipoPartida.Apertura);
                    case ETipoPartida.Cierre:
                        return !ExistePartidaDe(Periodo.Oid, ETipoPartida.Cierre);
                    default:
                        // cuando son partidas de Diario, Ingreso, Egreso o Liquidacion no debe existir la liquidacion en el periodo
                        return !ExistePartidaDe(Periodo.Oid, ETipoPartida.Liquidacion);
                }
            }
        }

        [Browsable(false)]
        [RuleFromBoolProperty("Partida.Fecha No Valida", DefaultContexts.Save, "La Fecha no es valida ya son días cerrados o no pertenece al período",
            SkipNullOrEmptyValues = false)] //, UsedProperties = "[Fecha]")]
        public bool FechaEsValida
        {
            get
            {
                if (Fecha == null || Periodo == null)
                    return false;
                var obj = Session.Evaluate<CierreDiario>(CriteriaOperator.Parse("Count()"),
                    CriteriaOperator.Parse("[Empresa] == ? && [FechaCierre] == ? && [DiaCerrado] = True", Empresa.Oid, Fecha));
                return Convert.ToInt32(obj) == 0 && Fecha.Year == Periodo.Oid;
            }
        }
        #endregion

        protected override void OnSaving()
        {
            if (!(Session is NestedUnitOfWork) && (Session.DataLayer != null) && Session.IsNewObject(this) &&
                (Session.ObjectLayer is SimpleObjectLayer) && (Numero == null || Numero == 0))
            {
                Empleado.Module.BusinessObjects.Empleado empleado = (((Usuario)SecuritySystem.CurrentUser).GetMemberValue("Empleado") as Empleado.Module.BusinessObjects.Empleado);
                if (empleado != null)
                    elaboro = Session.GetObjectByKey<Empleado.Module.BusinessObjects.Empleado>(empleado.Oid);
            }
            base.OnSaving();
        }

        protected override void DoFechaChange(bool forceChangeEvents, DateTime oldValue)
        {
            Periodo pp = Session.GetObjectByKey<Periodo>(Fecha.Year);
            if (pp != null)
                Periodo = pp;
            base.DoFechaChange(forceChangeEvents, oldValue);           
        }

        [Action(Caption = "Test", ImageName = "Attention", AutoCommit = false)]
        public void Test()
        {
            // saldo de octubre de la cuenta '11'
            CriteriaOperator formula = CriteriaOperator.Parse("SaldoDeCuenta([This], [Empresa.Oid], [Periodo.Oid], 10, '11')");
            ExpressionEvaluator eval = new ExpressionEvaluator(TypeDescriptor.GetProperties(this), formula);
            var resultado = Convert.ToDecimal(eval.Evaluate(this));
        }

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}