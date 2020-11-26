using DevExpress.Data.Filtering;
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
    [DefaultClassOptions, NavigationItem("Contabilidad"), ModelDefault("Caption", "Partida Contable"), XafDefaultProperty(nameof(Numero))]
    [ImageName(nameof(Partida)), MapInheritance(MapInheritanceType.OwnTable)]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    [Persistent("ConPartida")]
    [Appearance("Partida_Incompleta", AppearanceItemType = "ViewItem", TargetItems = "*",
        Criteria = "([TotalDebe] Is Null || [TotalDebe] == 0) || ([TotalHaber] Is Null || [TotalHaber] == 0) || ([TotalDebe] != [TotalHaber])",
        FontColor = "Red", Context = "ListView", Priority = 1)]
    [ListViewFilter("Todos", "")]
    [ListViewFilter("Partidas de Días Abiertos", "[Mayorizada] == False")]
    [ListViewFilter("Partidas Incompletas", "([TotalDebe] Is Null || [TotalDebe] == 0) || ([TotalHaber] Is Null || [TotalHaber] == 0) || ([TotalDebe] != [TotalHaber])")]
    [ListViewFilter("Partidas Ultimo 30 Días", "[Fecha] >= ADDDAYS(LocalDateTimeToday(), -30)")]
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
            
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        #region Propiedades

        Periodo periodo;
        ETipoPartida tipo = ETipoPartida.Diario;
        int? presupuesto;
        string concepto;
        [Persistent(nameof(Elaboro))]
        SBT.Apps.Empleado.Module.BusinessObjects.Empleado elaboro = null;
        [Persistent(nameof(TotalHaber))]
        decimal? totalHaber = null;
        [Persistent(nameof(TotalDebe))]
        decimal? totalDebe = null;
        [Persistent(nameof(Mayorizada)), DbType("bit")]
        bool mayorizada = false;

        [DbType("int"), Persistent("Periodo"), XafDisplayName("Período"), VisibleInLookupListView(false), VisibleInListView(false)]
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
        [PersistentAlias(nameof(totalDebe)), XafDisplayName("Debe")]
        public decimal? TotalDebe
        {
            get
            {
                if (!IsLoading && !IsSaving && totalDebe == null)
                    UpdateTotDebe(false);
                return totalDebe;

            }
        }

        [PersistentAlias(nameof(totalHaber)), XafDisplayName("Haber")]
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
        #endregion

        #region Metodos
        public void UpdateTotDebe(bool forceChangeEvents)
        {
            decimal? oldTotalDebe = totalDebe;
            decimal tempDebe = 0.0m;
            foreach (PartidaDetalle detalle in Detalles)
                tempDebe += detalle.ValorDebe;
            totalDebe = tempDebe;
            if (forceChangeEvents)
                OnChanged(nameof(TotalDebe), oldTotalDebe, totalDebe);
        }

        public void UpdateTotHaber(bool forceChangeEvents)
        {
            decimal? oldTotalHaber = totalHaber;
            decimal tempHaber = 0.0m;
            foreach (PartidaDetalle detalle in Detalles)
                tempHaber += detalle.ValorHaber;
            totalHaber = tempHaber;
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
            "Tipo de Partida no es válido porque ya existe o porque ya existe una partida de liquidación",
            SkipNullOrEmptyValues = false, UsedProperties = "Tipo", TargetCriteria = "[Tipo] <= 4")]
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
                if (Fecha == null)
                    return false;
                var obj = Session.Evaluate<CierreDiario>(CriteriaOperator.Parse("Count()"),
                    CriteriaOperator.Parse("[Empresa] == ? && [FechaCierre] == ? && [DiaCerrado] = True", Empresa.Oid, Fecha));
                return Convert.ToInt32(obj) == 0 && Fecha.Year == Periodo.Oid;
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