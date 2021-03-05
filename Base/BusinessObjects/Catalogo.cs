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
using DevExpress.ExpressApp.SystemModule;

namespace SBT.Apps.Contabilidad.BusinessObjects
{
    /// <summary>
    /// BO Correspondiente al Catalogo de cuentas contable
    /// </summary>
    /// <remarks>
    /// PENDIENTE. Si agregamos la columna periodo. La idea de este catalogo es que no sea necesario estar replicando cada año.
    /// Por eso es de evaluar si agregamos el período, para saber en que año se creo la cuenta.
    /// </remarks>
    [DefaultClassOptions, CreatableItem(false)]
    [ImageName("CatalogoContable")]
    [ModelDefault("Caption", "Catálogo Contable"), NavigationItem("Contabilidad"), DefaultProperty("CodCuenta"), Persistent("ConCatalogo"), 
        DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None), 
        ListViewFilter("Catálogo de la Empresa de la Sesion", "Empresa.Codigo = EmpresaActualOid()")]
    //[Indices("CodCuenta")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Catalogo : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Catalogo(Session session)
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
            if (Padre != null)
                nivelCta = Padre.Nivel + 1;
            else
                nivelCta = 1;
        }

        #region Propiedades

        Empresa empresa;
        string codigoCuenta;
        string nombre;
        Catalogo padre;
        ETipoCuentaCatalogo tipoCuenta = ETipoCuentaCatalogo.Activo;
        bool ctaResumen = true;
        bool ctaMayor;
        ETipoSaldoCuenta tipoSaldoCta = ETipoSaldoCuenta.Deudor;
        [Persistent(nameof(Nivel)), DbType("smallint")]
        int nivelCta = 1;
        bool activa = true;

#if Firebird
        [DbType("DM_ENTERO_CORTO"), Persistent("COD_EMP")]
#else
        [DbType("smallint"), Persistent("Empresa")]
#endif
        //[Association("Empresa-Catalogos")]
        [XafDisplayName("Empresa"), Index(1), RuleRequiredField("Catalogo.Empresa_Requerida", "Save")]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }

#if Firebird
        [DbType("DM_CODIGO20"), Persistent("COD_CTA_PADRE")]
#else
        [DbType("varchar(20)"), Persistent("CtaPadre")]
#endif
        [Size(20), XafDisplayName("Código Cuenta"), RuleRequiredField("Catalogo.CuentaPadre_Requerido", "Save"), Index(2),
            VisibleInLookupListView(true), ImmediatePostData(true)]
        [Association("Padre-Cuentas"), DataSourceCriteria("CtaResumen == True and CodCuenta != '@This.CodCuenta'")]
        public Catalogo Padre
        {
            get => padre;
            set => SetPropertyValue(nameof(Padre), ref padre, value);
        }


#if Firebird
        [DbType("DM_CODIGO20"), Persistent("COD_CUENTA")]
#else
        [DbType("varchar(20)"), Persistent(nameof(CodigoCuenta))]
#endif
        [Size(20), XafDisplayName("Código Cuenta"), RuleRequiredField("Catalogo.CodCuenta_Requerido", "Save"), Index(3),
            VisibleInLookupListView(true), NonCloneable]
        public string CodigoCuenta
        {
            get => codigoCuenta;
            set => SetPropertyValue(nameof(CodigoCuenta), ref codigoCuenta, value);
        }

#if Firebird
        [DbType("DM_DESCRIPCION150"), Persistent("NOMBRE")]
#else
        [DbType("varchar(150)"), Persistent("Nombre")]
#endif
        [Size(150), XafDisplayName("Nombre"), RuleRequiredField("Catalogo.Nombre_Requerido", DefaultContexts.Save), Index(4),
            VisibleInLookupListView(true)]
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        [XafDisplayName("Nivel"), VisibleInLookupListView(false)]
        [PersistentAlias("Iif(!IsNull([Padre]), [Padre].Nivel + 1, 1)")]
        public System.Int16 Nivel
        {
            get { return Convert.ToInt16(EvaluateAlias("Nivel")); }
        }

#if Firebird
        [DbType("DM_ENTERO_CORTO"), Persistent("TIPO_CUENTA")]
#else
        [DbType("smallint"), Persistent("TipoCuenta")]
#endif
        [XafDisplayName("Tipo Cuenta"), Index(6), VisibleInLookupListView(true), RuleRequiredField("Catalogo.TipoCuenta_Requerido", "Save")]
        public ETipoCuentaCatalogo TipoCuenta
        {
            get => tipoCuenta;
            set => SetPropertyValue(nameof(TipoCuenta), ref tipoCuenta, value);
        }

#if Firebird
        [DbType("DM_BOOLEAN"), Persistent("CTA_RESUMEN")]
        [ValueConverter(typeof(ToBooleanDataType))]
#else
        [DbType("bit"), Persistent("CtaResumen")]
#endif
        [XafDisplayName("Cuenta Resumen"), Index(7), VisibleInLookupListView(false), RuleRequiredField("Catalogo.CtaResumen_Requerido", DefaultContexts.Save)]
        public bool CtaResumen
        {
            get => ctaResumen;
            set => SetPropertyValue(nameof(CtaResumen), ref ctaResumen, value);
        }

#if Firebird
        [DbType("DM_BOOLEAN"), Persistent("CTA_MAYOR")]
        [ValueConverter(typeof(ToBooleanDataType))]
#else
        [DbType("bit"), Persistent("CtaMayor")]
#endif
        [XafDisplayName("Cuenta Mayor"), Index(8), VisibleInLookupListView(false), RuleRequiredField("Catalogo.CtaMayor_Requerido", DefaultContexts.Save)]
        public bool CtaMayor
        {
            get => ctaMayor;
            set => SetPropertyValue(nameof(CtaMayor), ref ctaMayor, value);
        }

#if Firebird
        [DbType("DM_ENTERO_CORTO"), Persistent("TIPO_SALDO")]
#else
        [DbType("smallint"), Persistent(nameof(TipoSaldoCta))]
#endif
        [XafDisplayName("Tipo Saldo"), Index(9), VisibleInLookupListView(false), RuleRequiredField("Catalogo.TipoSaldo_Requerido", "Save")]
        public ETipoSaldoCuenta TipoSaldoCta
        {
            get => tipoSaldoCta;
            set => SetPropertyValue(nameof(TipoSaldoCta), ref tipoSaldoCta, value);
        }

#if Firebird
        [DbType("DM_BOOLEAN"), Persistent("ACTIVA")]
        [ValueConverter(typeof(ToBooleanDataType))]
#else
        [DbType("bit"), Persistent("Activa")]
#endif
        [XafDisplayName("Cuenta Activa"), Index(10), VisibleInLookupListView(false), RuleRequiredField("Catalogo.Activa", "Save")]
        public bool Activa
        {
            get => activa;
            set => SetPropertyValue(nameof(Activa), ref activa, value);
        }

        #endregion

        #region Colecciones
        [Association("Padre-Cuentas")]
        public XPCollection<Catalogo> Cuentas
        {
            get
            {
                return GetCollection<Catalogo>(nameof(Cuentas));
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