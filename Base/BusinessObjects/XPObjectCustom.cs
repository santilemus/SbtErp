using DevExpress.ExpressApp;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using System;
using System.ComponentModel;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// Hereda de XPObject, e incluye la asignacion y la moneda por defecto cuando existen las propiedades en el BO.
    /// Ademas se reescribe OnDeleting para evitar borrar objetos cuando hay referencias en otros BO.
    /// Incluye propiedad para leer o asignar valores a propiedades del BO por su nombre
    /// </summary>
    /// 
    [NonPersistent]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public abstract class XPObjectCustom : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public XPObjectCustom(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (this.ClassInfo.FindMember("Empresa") != null)
            {
                int id = ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid;
                var emp = Session.GetObjectByKey<Empresa>(id);
                this["Empresa"] = emp;
                if (this.GetType().GetProperty("Moneda") != null && emp != null)
                {
                    (this["Moneda"]) = (this["Empresa"] as Empresa).MonedaDefecto;
                    if (this.GetType().GetProperty("ValorMoneda") != null)
                        this["ValorMoneda"] = (this["Empresa"] as Empresa).MonedaDefecto.FactorCambio;
                }
            }

            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
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

        #endregion Metodos

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}