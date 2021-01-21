using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System;
using System.ComponentModel;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// BO con las propiedades y funcionalidad comun para implementar los encabezados (datos generales) de documentos, cuando
    /// se heredan de XPObject. En este caso el Oid es un entero de 32 bits
    /// </summary>
    [NonPersistent, MemberDesignTimeVisibility(false)]
    [RuleIsReferenced("XPOBaseDoc.Empresa_Referencia", DefaultContexts.Save, typeof(Empresa), nameof(Empresa),
        CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
    public abstract class XPOBaseDoc : XPObjectBaseBO
    {

        public XPOBaseDoc(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        /// <summary>
        /// Retornar el siguiente correlativo de documento para un Persistent Object. El correlativo es por empresa y año
        /// </summary>
        /// <returns>Entero que representa un correlativo de documento en un objeto persistente</returns>
        protected virtual int CorrelativoDoc()
        {          
            string sCriteria = "Empresa.Oid == ? && GetYear(Fecha) == ?";
            //if (GetType().GetProperty("Caja") != null)
            //    sCriteria += " && Caja.Oid == ?";
            object max = Session.Evaluate(this.GetType(), CriteriaOperator.Parse("Max(Numero)"), CriteriaOperator.Parse(sCriteria, Empresa.Oid, Fecha));
            return (max != null) ? Convert.ToInt32(max): 1;
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading && !IsSaving && string.Compare(propertyName, "Moneda", StringComparison.Ordinal) == 0 && oldValue != newValue)
            {
                ValorMoneda = Moneda.FactorCambio;
            }
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (!(Session is NestedUnitOfWork) && Session.IsNewObject(this))
                Numero = CorrelativoDoc();
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (this.GetType().GetProperty("Fecha") != null)
                Fecha = DateTime.Now;
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
        [DbType("smallint"), Persistent(nameof(Empresa))]
#endif
        [Index(0), ModelDefault("AllowEdit", "False"), XafDisplayName("Empresa")]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }

        [Browsable(false)]
        private int numero;
#if (Firebird)
        [DbType("DM_ENTERO_LARGO"), Persistent("NUMERO")]
#else
        [DbType("int"), Persistent("Numero")]
#endif
        [Index(1), XafDisplayName("Número"), ModelDefault("AllowEdit", "False"), RuleRequiredField("XPOBaseDocs.Numero_Requerido", "Save"),
            NonCloneable]
        public int Numero
        {
            get => numero;
            set => SetPropertyValue(nameof(Numero), ref numero, value);
        }

        private DateTime fecha;
#if (Firebird)
        [DbType("DM_FECHA"), Persistent("FECHA")]
#else
        [DbType("datetime"), Persistent("Fecha")]
#endif
        [XafDisplayName("Fecha"), Index(2), RuleRequiredField("", "Save")]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
        }

        private Moneda moneda;
#if (Firebird)
        [DbType("DM_CODIGO03"), Persistent("COD_MONEDA")]
#else
        [DbType("varchar(3)"), Persistent("Moneda")]
#endif
        //[Association("Moneda-Docs"), 
        [Index(3), XafDisplayName("Moneda"), RuleRequiredField("XPOBaseDoc.Moneda_Requerida", DefaultContexts.Save)]
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
        [XafDisplayName("Fecha Anulación"), ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G"), Index(98), ModelDefault("AllowEdit", "False")]
        [PersistentAlias("fechaAnula"), Browsable(false)]
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
        [Size(25), Index(99), XafDisplayName("Usuario Anuló"), ModelDefault("AllowEdit", "False"),
            PersistentAlias("usuarioAnulo"), Browsable(false)]
        public string UsuarioAnulo
        {
            get => usuarioAnulo;
        }

        [NonCloneable]
        private string comentario;
#if (Firebird)
        [DbType("DM_DESCRIPCION250"), Persistent("COMENTARIO")]
#else
        [DbType("varchar(250)"), Persistent(nameof(Comentario))]
#endif
        [Size(250), Index(100), XafDisplayName("Comentario"), ModelDefault("AllowEdit", "False"), Browsable(false)]
        public string Comentario
        {
            get => comentario;
            set => SetPropertyValue(nameof(Comentario), ref comentario, value);
        }

        #endregion
    }

}