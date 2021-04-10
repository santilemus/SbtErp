using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// BO con las propiedades y funcionalidad comun para implementar los encabezados (datos generales) de documentos, cuando
    /// se heredan de XPCustomObject. En este caso se debe implementar el Oid. Es util en los casos, donde se necesita un Oid
    /// mas grande que un entero de 32 bits, o cuya logica para calcularla es diferente a un identity o secuencia. 
    /// Otro caso es cuando se llama distinto (no es Oid), porque corresponde a una tabla que ya existía previamente
    /// </summary>
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [NonPersistent]
    public abstract class XPCustomBaseDoc : XPCustomBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public XPCustomBaseDoc(Session session)
            : base(session)
        {
        }

        /// <summary>
        /// Retornar el siguiente correlativo de documento para un Persistent Object. El correlativo es por empresa
        /// </summary>
        /// <returns>Entero que representa un correlativo de documento en un objeto persistente</returns>
        protected virtual int CorrelativoDoc()
        {
            string sCriteria = "Empresa.Oid == ? && GetYear(Fecha) == ?";
            using (UnitOfWork uow = new UnitOfWork(Session.DataLayer, null))
            {
                object max = uow.Evaluate(this.GetType(), CriteriaOperator.Parse("Max(Numero) + 1"), CriteriaOperator.Parse(sCriteria, Empresa.Oid, Fecha));
                return Convert.ToInt32(max ?? 1);
            }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            if (this.GetType().GetProperty("Fecha") != null)
                Fecha = DateTime.Now;
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading && !IsSaving && string.Compare(propertyName, "Moneda", StringComparison.Ordinal) == 0 && oldValue != newValue)
                ValorMoneda = Moneda.FactorCambio;
        }

        protected override void OnSaving()
        {
            if (!(Session is NestedUnitOfWork) && Session.IsNewObject(this))
                Numero = CorrelativoDoc();
            base.OnSaving();
        }

        /// <summary>
        /// Invocar cuando se anula un documento, para agregar informacion de comentario, fecha, usuario que anulo y guardar los cambios
        /// En los BO heredados, debe realizar antes las acciones especificas de cada caso e invocar al final este metdo
        /// </summary>
        /// <param name="AnularParams">Parametros para realizar la anulacion</param>
        public virtual void Anular(AnularParametros AnularParams)
        {
            comentario += $"{Environment.NewLine}{AnularParams.Comentario}";
            fechaAnula = DateTime.Now;
            usuarioAnulo = DevExpress.ExpressApp.SecuritySystem.CurrentUserName;
        }

        #region Propiedades
        [Browsable(false)]
        private Empresa empresa;
#if (Firebird)
        [DbType("DM_ENTERO_CORTO"), Persistent("COD_EMP")]
#else
        [DbType("smallint"), Persistent("Empresa")]
#endif
        [Index(0), ModelDefault("AllowEdit", "False")]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }

        /// <summary>
        /// Correlativo por tipo de documento.
        /// </summary>
        [Browsable(false)]
        private int numero;
#if (Firebird)
        [DbType("DM_ENTERO"), Persistent("NUMERO")]
#else
        [DbType("int"), Persistent("Numero")]
#endif
        [Index(1), XafDisplayName("Número"), ModelDefault("AllowEdit", "False"), RuleRequiredField("XPCustomBaseDocs.Numero_Requerido", "Save"),
            Indexed("Empresa", Unique = false), NonCloneable]
        public int Numero
        {
            get => numero;
            set => SetPropertyValue(nameof(Numero), ref numero, value);
        }

        private DateTime fecha;
#if (Firebird)
        [DbType("DM_FECHA"), Persistent("FECHA")]
#else
        [DbType("datetime"), Persistent(nameof(Fecha))]
#endif
        [XafDisplayName("Fecha"), Index(2), RuleRequiredField("XPCustomBaseDocs.Fecha_Requerida", "Save"), Indexed("Empresa", Unique = false)]
        [VisibleInListView(true), VisibleInLookupListView(true)]
        public DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
        }

        private Moneda moneda;
#if (Firebird)
        [DbType("DM_CODIGO03"), Persistent("COD_MONEDA")]
#else
        [DbType("varchar(3)"), Persistent(nameof(Moneda))]
#endif
        [Index(3), XafDisplayName("Moneda"), RuleRequiredField("XPCustomBaseDocs.Moneda_Requerida", DefaultContexts.Save)]
        [ExplicitLoading]
        public Moneda Moneda
        {
            get => moneda;
            set => SetPropertyValue(nameof(Moneda), ref moneda, value);
        }

        [Browsable(false)]
        private decimal valorMoneda;
#if (Firebird)
        [DbType("DM_DINERO122"), Persistent("VAL_MONE")]
#else
        [DbType("numeric(12, 2)"), Persistent(nameof(ValorMoneda))]
#endif
        [XafDisplayName("Valor Moneda"), Index(4), ModelDefault("AllowEdit", "False")]
        public decimal ValorMoneda
        {
            get => valorMoneda;
            set => SetPropertyValue(nameof(ValorMoneda), ref valorMoneda, value);
        }

        [NonCloneable]
#if (Firebird)
        [DbType("DM_FECHA_HORA"), Persistent("FECHA_ANULA")]
#else
        [DbType("datetime"), Persistent(nameof(FechaAnula))]
#endif
        private DateTime? fechaAnula;
        [PersistentAlias("fechaAnula")]
        [XafDisplayName("Fecha Anulación"), ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G"), Index(98), ModelDefault("AllowEdit", "False"),
            Browsable(false)]
        public DateTime? FechaAnula
        {
            get => fechaAnula;
        }

        [NonCloneable]
#if (Firebird)
        [DbType("DM_DESCRIPCION25"), Persistent("USUARIO_ANULO")]
#else
        [DbType("varchar(25)"), Persistent(nameof(UsuarioAnulo))]
#endif
        private string usuarioAnulo;
        [PersistentAlias("usuarioAnulo")]
        [Size(25), Index(99), XafDisplayName("Usuario Anuló"), ModelDefault("AllowEdit", "False"), Browsable(false)]
        public string UsuarioAnulo
        {
            get => usuarioAnulo;
        }

        [Browsable(false)]

        private string comentario;
#if (Firebird)
        [DbType("DM_DESCRIPCION250"), Persistent("COMENTARIO")]
#else
        [DbType("varchar(250)"), Persistent(nameof(Comentario))]
#endif
        [Size(250), Index(100), XafDisplayName("Comentario"), ModelDefault("AllowEdit", "False"), NonCloneable]
        public string Comentario
        {
            get => comentario;
            set => SetPropertyValue(nameof(Comentario), ref comentario, value);
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}