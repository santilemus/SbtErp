using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// Para heredar los clases de XPCustomObject, con las propiedades de creación y modificación 
    /// </summary>
    [NonPersistent]
    public abstract class XPCustomBaseBO : XPCustomObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public XPCustomBaseBO(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            if (this.ClassInfo.FindMember("Empresa") != null)
            {
                this["Empresa"] = this.Session.GetObjectByKey<Empresa>(SesionDataHelper.ObtenerValor("OidEmpresa"));
            }
            if (this.GetType().GetProperty("Moneda") != null)
            {
                Constante constante = Session.GetObjectByKey<Constante>("MONEDA DEFECTO");
                Moneda moneda = null;
                if (constante != null)
                    moneda = this.Session.GetObjectByKey<Moneda>(constante.Valor.Trim());
                if (moneda == null)
                {
                    this["Moneda"] = moneda;
                    if (this.GetType().GetProperty("ValorMoneda") != null)
                        this["ValorMoneda"] = moneda.FactorCambio;
                }
            }
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (!Session.IsNewObject(this))
            {
                fechaMod = DateTime.Now;
                usuarioMod = DevExpress.ExpressApp.SecuritySystem.CurrentUserName;
            }
        }

        /// <summary>
        /// Evitar que se borren objetos, cuando existen referencias a ellos en objetos relacionados
        /// </summary>
        protected override void OnDeleting()
        {
            base.OnDeleting();
            if (Session.CollectReferencingObjects(this).Count > 0)
            {
                string usadoPor = string.Empty;
                int i = 0;
                foreach (var a in Session.CollectReferencingObjects(this))
                {
                    i++;
                    usadoPor = $"{usadoPor}{a.GetType()}:{a}; ";
                    if (i > 2) break;
                }
                throw new Exception($"No puede borrar, Existen objetos, que usan el objeto que esta intentando eliminar : {ToString()}\r\n{usadoPor}");
            }
        }

        private bool isDefaultPropertyAttributeInit;
        private XPMemberInfo defaultPropertyMemberInfo;

        #region Propiedades

        [Size(25), Persistent(@"UsuarioCrea"), DbType("varchar(25)"), NonCloneable, ModelDefault("AllowEdit", "False")]
        string usuarioCrea = DevExpress.ExpressApp.SecuritySystem.CurrentUserName;
        /// <summary>
        /// Usuario que creó el registro
        /// </summary>
        [DevExpress.Xpo.DisplayName(@"Usuario Creó"), VisibleInListView(false), Browsable(false)]
        public string UsuarioCrea
        {
            get { return usuarioCrea; }
        }

        [Persistent(@"FechaCrea"), DbType("datetime"), NonCloneable, ModelDefault("AllowEdit", "False")]
        DateTime? fechaCrea = DateTime.Now;
        /// <summary>
        /// Fecha y hora de creación del registro
        /// </summary>
        [DevExpress.Xpo.DisplayName(@"Fecha Creación"), ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G"),
            Browsable(false), PersistentAlias("fechaCrea")]
        public DateTime? FechaCrea
        {
            get { return fechaCrea; }
        }
        [Size(25)]
        [Persistent(@"UsuarioMod"), DbType("varchar(25)"), NonCloneable, ModelDefault("AllowEdit", "False")]
        string usuarioMod;
        /// <summary>
        /// Usuario que realizó la última modificación
        /// </summary>
        [DevExpress.Xpo.DisplayName(@"Usuario Modificó"), Browsable(false)]
        public string UsuarioMod
        {
            get { return usuarioMod; }
        }
        [Persistent(@"FechaMod"), DbType("datetime"), NonCloneable, ModelDefault("AllowEdit", "False")]
        DateTime? fechaMod;
        /// <summary>
        /// Fecha y Hora de la última modificación
        /// </summary>
        [DevExpress.Xpo.DisplayName(@"Fecha Modificación"), ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G"), Browsable(false)]
        public DateTime? FechaMod
        {
            get { return fechaMod; }
        }

        #endregion

        #region Metodos

        public override string ToString()
        {
            if (!isDefaultPropertyAttributeInit)
            {
                DefaultPropertyAttribute attrib = XafTypesInfo.Instance.FindTypeInfo(
                    GetType()).FindAttribute<DefaultPropertyAttribute>();
                if (attrib != null)
                    defaultPropertyMemberInfo = ClassInfo.FindMember(attrib.Name);
                isDefaultPropertyAttributeInit = true;
            }
            if (defaultPropertyMemberInfo != null)
            {
                object obj = defaultPropertyMemberInfo.GetValue(this);
                if (obj != null)
                    return obj.ToString();
            }
            return base.ToString();
        }

        /// <summary>
        /// Extender funcionalidad de la clase, para acceder a una propiedad por su nombre
        /// Ejemplo: myObject["Nombre"] = "San"
        /// </summary>
        /// <param name="propertyName">Nombre de la propiedad cuyo valor se va a asignar o leer</param>
        /// <returns>El valor de la propiedad que corresponde a propertyName</returns>
        /// <remarks>
        /// Ver https://supportcenter.devexpress.com/ticket/details/a755/how-to-access-the-property-of-a-class-by-its-name-in-c
        /// </remarks>
        public object this[string propertyName]
        {
            get => ClassInfo.GetMember(propertyName).GetValue(this);
            set => ClassInfo.GetMember(propertyName).SetValue(this, value);
        }

        protected int CorrelativoSQ(string SQName)
        {
            using (UnitOfWork uow = new UnitOfWork(this.Session.DataLayer, null))
            {
#if (Firebird)
                return Convert.ToInt32(uow.ExecuteScalar("select next value for " + SQName + " from RDB$DATABASE"));
#else
                return Convert.ToInt32(uow.ExecuteScalar($"select next value for {SQName}"));
#endif
            }
        }

        protected int Correlativo(string sSQL)
        {
            using (UnitOfWork uow = new UnitOfWork(this.Session.DataLayer, null))
            {
                return Convert.ToInt32(uow.ExecuteScalar(sSQL));
            }
        }

        [Action(Caption = "Información de Auditoría", ImageName = "BO_Contact", ToolTip = "Mostrar la Información de Auditoría del Objeto")]
        public void MostrarAuditInfo(AuditObjectInfo AuditParameters)
        {

        }

        #endregion

    }
}