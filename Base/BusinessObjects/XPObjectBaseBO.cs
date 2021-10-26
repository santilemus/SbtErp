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
    /// Para heredar las clases de XPObject, con las propiedades de creación y modificación 
    /// </summary>
    [NonPersistent]
    public abstract class XPObjectBaseBO : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public XPObjectBaseBO(Session session)
              : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (this.ClassInfo.FindMember("Empresa") != null)
            {
                this["Empresa"] = Session.GetObjectByKey<Empresa>(SesionDataHelper.ObtenerValor("OidEmpresa"));
                if (this.GetType().GetProperty("Moneda") != null)
                {
                    this["Moneda"] = (this["Empresa"] as Empresa).MonedaDefecto;
                    if (this.GetType().GetProperty("ValorMoneda") != null)
                        this["ValorMoneda"] = (this["Empresa"] as Empresa).MonedaDefecto.FactorCambio;
                }
            }
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
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

        [Size(25), Persistent(@"UsuarioCrea"), DbType("varchar(25)"), NonCloneable(), ModelDefault("AllowEdit", "False")]
        string usuarioCrea = DevExpress.ExpressApp.SecuritySystem.CurrentUserName;
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

        [Persistent(@"FechaCrea"), DbType("datetime"), NonCloneable, ModelDefault("AllowEdit", "False")]
        DateTime? fechaCrea = DateTime.Now;
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

        [Size(25), Persistent(@"UsuarioMod"), DbType("varchar(25)"), NonCloneable, ModelDefault("AllowEdit", "False")]
        private string usuarioMod;
        /// <summary>
        /// Usuario que realizó la última modificación
        /// </summary>

        [DevExpress.Xpo.DisplayName(@"Usuario Modificó"), Browsable(false), PersistentAlias("usuarioMod")]
        [Delayed(true)]
        public string UsuarioMod
        {
            get => usuarioMod;
        }

        [Persistent(@"FechaMod"), DbType("datetime")]
        private DateTime? fechaMod;
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

    /// <summary>
    /// Implementar validacion en codigo para propiedades del BO XPObjectBaseBO
    /// </summary>
    /// <remarks>
    /// Ver https://docs.devexpress.com/eXpressAppFramework/113051/concepts/extra-modules/validation/implement-custom-rules
    /// Ver el ejemplo original en CodeRuleObject.cs en: Components\eXpressApp Framework\FeatureCenter\CS\FeatureCenter.Module\Validation
    /// </remarks>

    //[CodeRule]
    //public class XPObjectBaseBOIsValidRule: RuleBase<XPObjectBaseBO>
    //{
    //    private string usedProperty = "";
    //    protected override bool IsValidInternal(XPObjectBaseBO target, out string errorMessageTemplate)
    //    {
    //        if (target.FechaCrea == null || target.FechaCrea < DateTime.Now)
    //        {
    //            errorMessageTemplate = "Fecha de Creacion debe ser ingresada";
    //            usedProperty = "FechaCrea";
    //            return false;
    //        }
    //        else
    //        {
    //            errorMessageTemplate = "solo para que no de error";
    //            return false; // igual solo para que no de error, esto es un ejemplo de validaciones con codigo
    //        }
    //        // otra validacion
    //        //usedProperty = "TitleOfCourtesy";
    //        //if (target.Sex == Sex.Male)
    //        //{
    //        //    errorMessageTemplate = "Title of Courtesy for males must be 'Mr'.";
    //        //    return target.TitleOfCourtesy == TitleOfCourtesy.Mr;
    //        //}
    //        //else
    //        //{
    //        //    errorMessageTemplate = "Title of Courtesy for a woman must be 'Ms', if she is not marred; otherwise, 'Mrs'.";
    //        //    return target.TitleOfCourtesy == TitleOfCourtesy.Mrs || target.TitleOfCourtesy == TitleOfCourtesy.Ms;
    //        //}
    //    }
    //    public override System.Collections.ObjectModel.ReadOnlyCollection<string> UsedProperties
    //    {
    //        get
    //        {
    //            if (string.IsNullOrEmpty(usedProperty))
    //            {
    //                return base.UsedProperties;
    //            }
    //            return new List<string>(new string[] { usedProperty }).AsReadOnly();
    //        }
    //    }
    //    public XPObjectBaseBOIsValidRule() : base("", "Save")
    //    {
    //        Properties.SkipNullOrEmptyValues = false;
    //    }
    //    public XPObjectBaseBOIsValidRule(IRuleBaseProperties properties)
    //        : base(properties)
    //    {
    //    }
    //}
}