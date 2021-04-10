using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    [ModelDefault("Caption", "Contacto"), NavigationItem(false), DefaultProperty(nameof(Nombre)), Persistent("PersonaContacto")]
    [ImageName(nameof(PersonaContacto))]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class PersonaContacto : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public PersonaContacto(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        #region Propiedades

        bool activo = true;
        string direccion;
        string telefono;
        string nombre;
        Persona contacto;
        Persona persona;

        [Association("Persona-Contactos"), XafDisplayName("Persona"), Persistent("Persona"), DbType("int")]
        public Persona Persona
        {
            get => persona;
            set => SetPropertyValue(nameof(Persona), ref persona, value);
        }


        [XafDisplayName("Contacto"), Persistent("Contacto"), DbType("int")]
        [ImmediatePostData(true)]
        public Persona Contacto
        {
            get => contacto;
            set
            {
                bool changed = SetPropertyValue(nameof(Contacto), ref contacto, value);
                if (!IsLoading && !IsSaving && changed)
                {
                    Nombre = value != null ? value.Nombre + " " + value.Apellido : string.Empty;
                    Direccion = value != null ? value.Direccion : string.Empty;
                    if (value.Telefonos.Count > 0)
                        Telefono = value != null ? value.Telefonos.FirstOrDefault<PersonaTelefono>().Telefono.Numero : string.Empty;
                }
            }
        }


        [Size(80), DbType("varchar(80)"), Persistent("Nombre"), XafDisplayName("Nombre Contacto")]
        [Appearance("", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any",
            Criteria = "[Contacto] Is not Null", TargetItems = "Nombre")]
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }


        [Size(150), DbType("varchar(150)"), Persistent("Direccion"), XafDisplayName("Dirección")]
        public string Direccion
        {
            get => direccion;
            set => SetPropertyValue(nameof(Direccion), ref direccion, value);
        }

        [Size(14), DbType("varchar(25)"), Persistent("Telefono"), XafDisplayName("Teléfono")]
        public string Telefono
        {
            get => telefono;
            set => SetPropertyValue(nameof(Telefono), ref telefono, value);
        }

        [DbType("bit"), Persistent("Activo"), XafDisplayName("Activo"), RuleRequiredField("PersonaContacto.Activo_Requerido", "Save")]
        public bool Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}