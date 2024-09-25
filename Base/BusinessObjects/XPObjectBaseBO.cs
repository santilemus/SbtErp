using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// Para heredar las clases de XPObject, con las propiedades de creación y modificación 
    /// </summary>
    [NonPersistent]
    public abstract class XPObjectBaseBO : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public XPObjectBaseBO(Session session)
              : base(session)
        {
        }

        private bool isDefaultPropertyAttributeInit;
        private XPMemberInfo defaultPropertyMemberInfo;
        [Size(25), Persistent(@"UsuarioCrea"), DbType("varchar(25)"), NonCloneable(), ModelDefault("AllowEdit", "False")]
        string usuarioCrea;
        [Persistent(@"FechaCrea"), DbType("datetime"), NonCloneable, ModelDefault("AllowEdit", "False")]
        DateTime? fechaCrea = DateTime.Now;
        [Size(25), Persistent(@"UsuarioMod"), DbType("varchar(25)"), NonCloneable, ModelDefault("AllowEdit", "False")]
        private string usuarioMod;
        [Persistent(@"FechaMod"), DbType("datetime")]
        private DateTime? fechaMod;

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (string.IsNullOrEmpty(SecuritySystem.CurrentUserName))
                usuarioCrea = Session.ServiceProvider.GetRequiredService<ISecurityStrategyBase>().UserName;
            else
                usuarioCrea = SecuritySystem.CurrentUserName;
            fechaCrea = DateTime.Now;

            if (this.ClassInfo.FindMember("Empresa") != null)
            {
                int id = ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid;
                var emp = Session.GetObjectByKey<Empresa>(id);
                this["Empresa"] = emp;
            }
            if (this.GetType().GetProperty("Moneda") != null)
            {
                Moneda moneda;
                var parametro = Session.GetObjectByKey<Constante>("MONEDA DEFECTO");
                if (parametro != null)
                {
                    moneda = Session.GetObjectByKey<Moneda>(parametro.Valor.Trim());
                    this["Moneda"] = moneda;
                    this["ValorMoneda"] = moneda?.FactorCambio;
                }
            }
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (Session is not NestedUnitOfWork && (Session.DataLayer != null) && (Session.ObjectLayer is SecuredSessionObjectLayer))
            {
                if (!Session.IsNewObject(this))
                {
                    fechaMod = DateTime.Now;
                    if (string.IsNullOrEmpty(SecuritySystem.CurrentUserName))
                        usuarioMod = Session.ServiceProvider.GetRequiredService<ISecurityStrategyBase>().UserName;
                    else
                        usuarioMod = SecuritySystem.CurrentUserName;
                }
            }
        }

        /*
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
        */

        #region Propiedades

        /// <summary>
        /// Usuario que creó el registro
        /// </summary>
        [DevExpress.Xpo.DisplayName(@"Usuario Creó"), VisibleInListView(false), Browsable(false),
            PersistentAlias("usuarioCrea"), NonCloneable(), ModelDefault("AllowEdit", "False")]
        [Delayed(true)]
        public string UsuarioCrea
        {
            get => usuarioCrea;
        }

        /// <summary>
        /// Fecha y hora de creación del registro
        /// </summary>
        [PersistentAlias("fechaCrea")]
        [DevExpress.Xpo.DisplayName(@"Fecha Creación"), ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G"),
            Browsable(false)]
        [Delayed(true)]
        public DateTime? FechaCrea
        {
            get => fechaCrea;
        }

        /// <summary>
        /// Usuario que realizó la última modificación
        /// </summary>
        [DevExpress.Xpo.DisplayName(@"Usuario Modificó"), Browsable(false), PersistentAlias("usuarioMod")]
        [Delayed(true)]
        public string UsuarioMod
        {
            get => usuarioMod;
        }

        /// <summary>
        /// Fecha y Hora de la última modificación
        /// </summary>
        [DevExpress.Xpo.DisplayName(@"Fecha Modificación"), ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G"),
            PersistentAlias("fechaMod"), Browsable(false)]
        [Delayed(true)]
        public DateTime? FechaMod
        {
            get => fechaMod;
        }
        #endregion Propiedades

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

        [Action(Caption = "Información de Auditoría", ImageName = "BO_Contact", ToolTip = "Mostrar la Información de Auditoría del Objeto")]
        public void MostrarAuditInfo(AuditObjectInfo AuditParameters)
        {

        }

        #endregion Metodos
    }

}