using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// BO con las propiedades y funcionalidad comun para implementar los encabezados (datos generales) de documentos, cuando
    /// se heredan de XPObject. En este caso el Oid es un entero de 32 bits
    /// </summary>
    [NonPersistent]
    public abstract class XPOBaseDoc : XPObjectBaseBO
    {
        private Empresa empresa;
        private int? numero;
        private DateTime fecha;
        private Moneda moneda;
        private decimal valorMoneda;
        [NonCloneable]
#if (Firebird)
        [DbType("DM_FECHA_HORA"), Persistent("FECHA_ANULA")]
#else
        [DbType("datetime"), Persistent(nameof(FechaAnula))]
#endif
        private DateTime? fechaAnula;
        [NonCloneable]
#if (Firebird)
        [DbType("DM_DESCRIPCION25"), Persistent("USUARIO_ANULO")]
#else
        [DbType("varchar(25)"), Persistent(nameof(UsuarioAnulo))]
#endif
        private string usuarioAnulo;
        private string comentario;

        public XPOBaseDoc(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        /// <summary>
        /// Retornar el siguiente correlativo de documento para un Persistent Object. El correlativo es por empresa y año
        /// </summary>
        /// <returns>Entero que representa un correlativo de documento en un objeto persistente</returns>
        /// <remarks>
        /// 28/04/2021. De ser necesario debe reescribirlo en cada caso en las clases heredadas
        /// </remarks>
        protected virtual int CorrelativoDoc()
        {
            string sCriteria = "[Empresa.Oid] == ? && GetYear([Fecha]) == ?";
            object max = Session.Evaluate(this.GetType(), CriteriaOperator.Parse("Max([Numero]) + 1"), CriteriaOperator.Parse(sCriteria, Empresa.Oid, Fecha.Year));
            return Convert.ToInt32(max ?? 1);
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
            if ((Session is not NestedUnitOfWork) && (Session.DataLayer != null) && Session.IsNewObject(this) &&
                (Session.ObjectLayer is SecuredSessionObjectLayer) && (Numero == null || Numero <= 0))
            {
                Numero = CorrelativoDoc();
            }
            base.OnSaving();
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
            if (string.IsNullOrEmpty(SecuritySystem.CurrentUserName))
                usuarioAnulo = Session.ServiceProvider.GetRequiredService<ISecurityStrategyBase>().UserName;
            else
                usuarioAnulo = DevExpress.ExpressApp.SecuritySystem.CurrentUserName;
        }

        #region Propiedades

#if (Firebird)
        [DbType("DM_ENTERO_CORTO"), Persistent("COD_EMP")]
#else
        [DbType("smallint"), Persistent(nameof(Empresa))]
#endif
        [Index(0), ModelDefault("AllowEdit", "False"), XafDisplayName("Empresa")]
        [Browsable(false)]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }

#if (Firebird)
        [DbType("DM_ENTERO_LARGO"), Persistent("NUMERO")]
#else
        [DbType("int"), Persistent("Numero")]
#endif
        [Index(1), XafDisplayName("Número"), RuleRequiredField("XPOBaseDocs.Numero_Requerido", "Save"), NonCloneable]
        [ModelDefault("AllowEdit", "False")]
        public int? Numero
        {
            get => numero;
            set => SetPropertyValue(nameof(Numero), ref numero, value);
        }

#if (Firebird)
        [DbType("DM_FECHA"), Persistent("FECHA")]
#else
        [DbType("datetime"), Persistent("Fecha")]
#endif
        [XafDisplayName("Fecha"), Index(2), RuleRequiredField("", "Save")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy}"), ModelDefault("EditMask", "dd/MM/yyyy")]
        public DateTime Fecha
        {
            get => fecha;
            set
            {
                var oldfecha = fecha;
                bool changed = SetPropertyValue(nameof(Fecha), ref fecha, value);
                if (!IsLoading && !IsSaving && changed)
                    DoFechaChange(true, oldfecha);
            }
        }

#if (Firebird)
        [DbType("DM_CODIGO03"), Persistent("COD_MONEDA")]
#else
        [DbType("varchar(3)"), Persistent("Moneda")]
#endif
        //[Association("Moneda-Docs"), 
        [Index(3), XafDisplayName("Moneda"), RuleRequiredField("XPOBaseDoc.Moneda_Requerida", DefaultContexts.Save)]
        [ExplicitLoading]
        public Moneda Moneda
        {
            get => moneda;
            set => SetPropertyValue(nameof(Moneda), ref moneda, value);
        }

#if (Firebird)
        [DbType("DM_DINERO122"), Persistent("VAL_MONE")]
#else
        [DbType("numeric(12, 2)"), Persistent(nameof(ValorMoneda))]
#endif
        [XafDisplayName("Valor Moneda"), Index(4), ModelDefault("AllowEdit", "False"), Browsable(false)]
        public decimal ValorMoneda
        {
            get => valorMoneda;
            set => SetPropertyValue(nameof(ValorMoneda), ref valorMoneda, value);
        }


        [XafDisplayName("Fecha Anulación"), ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G"), Index(98), ModelDefault("AllowEdit", "False")]
        [PersistentAlias("fechaAnula"), Browsable(false)]
        [Delayed(true)]
        public DateTime? FechaAnula
        {
            get => fechaAnula;
        }

        [Size(25), Index(99), XafDisplayName("Usuario Anuló"), ModelDefault("AllowEdit", "False"),
            PersistentAlias("usuarioAnulo"), Browsable(false)]
        [Delayed(true)]
        public string UsuarioAnulo
        {
            get => usuarioAnulo;
        }

#if (Firebird)
        [DbType("DM_DESCRIPCION250"), Persistent("COMENTARIO")]
#else
        [DbType("varchar(250)"), Persistent(nameof(Comentario))]
#endif
        [Size(250), Index(100), XafDisplayName("Comentario")]
        [ModelDefault("AllowEdit", "False")]
        [Delayed(true)]
        [NonCloneable]
        public string Comentario
        {
            get => comentario;
            set => SetPropertyValue(nameof(Comentario), ref comentario, value);
        }

        #endregion

        #region Metodos
        /// <summary>
        /// Metodo que se dispara cuando cambia la propiedad fecha
        /// </summary>
        /// <param name="forceChangeEvents"></param>
        protected virtual void DoFechaChange(bool forceChangeEvents, DateTime oldValue)
        {
            if (forceChangeEvents)
                OnChanged(nameof(Fecha), oldValue, Fecha);
        }

        #endregion
    }

}