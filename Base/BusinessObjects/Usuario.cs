using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DevExpress.Persistent.Validation;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// BO con los usuarios del sistema. Evaluar agregar en la relacion la informacion del Empleado vinculado al usuario
    /// para que en los documentos como la impresion de las Partidas Contables, se pueda mostrar el nombre del empleado
    /// </summary>
    [DefaultClassOptions(), DefaultProperty("UserName"), DevExpress.ExpressApp.Model.ModelDefault("Caption", "Usuarios - Empresa"),
        NavigationItem(false), CreatableItem(false)]
    [Persistent(@"Usuario")]
    [RuleCriteria("Usuario_Prevent_delete_logged_in_user", DefaultContexts.Delete, "[Oid] != CurrentUserId()", 
        "No puede borrar el usuario con el cual inicio la sesion actual. Por favor, ingreso con otra cuenta de usuario y reintente la operación.")]
    public class Usuario : PermissionPolicyUser
    {
        public Usuario(Session session) : base(session) { }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        private Empresa empresa;

        [Association("Empresa-Usuarios"), XafDisplayName("Empresa"), VisibleInListView(true), VisibleInDetailView(true), Index(0)]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }

        EmpresaUnidad agencia;
        [DataSourceCriteria("Empresa = '@This.Empresa'"), XafDisplayName("Agencia"), Persistent("Agencia"), Index(1)]
        public EmpresaUnidad Agencia
        {
            get => agencia;
            set => SetPropertyValue(nameof(Agencia), ref agencia, value);
        }
    }

}
