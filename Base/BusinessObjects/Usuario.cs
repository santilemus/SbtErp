﻿using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.XtraRichEdit.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// BO con los usuarios del sistema. Evaluar agregar en la relacion la informacion del Empleado vinculado al usuario
    /// para que en los documentos como la impresion de las Partidas Contables, se pueda mostrar el nombre del empleado
    /// </summary>
    [DefaultClassOptions(), DefaultProperty("UserName"), DevExpress.ExpressApp.Model.ModelDefault("Caption", "Usuarios - Empresa"),
        NavigationItem(false), CreatableItem(false)]
    [DevExpress.Xpo.MapInheritance(MapInheritanceType.ParentTable)]
    [RuleCriteria("Usuario_Prevent_delete_logged_in_user", DefaultContexts.Delete, "[Oid] != CurrentUserId()",
        "No puede borrar el usuario con el cual inicio la sesion actual. Por favor, ingreso con otra cuenta de usuario y reintente la operación.")]
    public class Usuario : PermissionPolicyUser, IObjectSpaceLink, ISecurityUserWithLoginInfo
    {
        public Usuario(Session session) : base(session) { }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        public ISecurityUserLoginInfo CreateUserLoginInfo(string loginProviderName, string providerUserKey)
        {
            throw new System.NotImplementedException();
        }

        private Empresa empresa;

        [Association("Empresa-Usuarios"), XafDisplayName("Empresa"), VisibleInListView(true), VisibleInDetailView(true), Index(0)]
        [ImmediatePostData(true)]
        //[ExplicitLoading]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }

        EmpresaUnidad agencia;
        private int caja;

        [DataSourceCriteria("Empresa = '@This.Empresa'"), XafDisplayName("Agencia"), Persistent("Agencia"), Index(1)]
        //[ExplicitLoading(1)]
        public EmpresaUnidad Agencia
        {
            get => agencia;
            set => SetPropertyValue(nameof(Agencia), ref agencia, value);
        }

        public int Caja
        {
            get => caja;
            set => SetPropertyValue(nameof(Caja), ref caja, value);
        }

        /// <summary>
        /// Validar que la caja exista. Pendiente de implementar solo se deja para que permita pasar
        /// </summary>
        [Browsable(false)]
        [RuleFromBoolProperty("Usuario.Caja No Valida", DefaultContexts.Save, "Caja {TargetObject.Caja} no existe o no está activa")]
        public bool CajaEsValida
        {
           get
            {
                if (Agencia == null)
                    return false;
                // revisar esta implementacion. Evaluar pasar Cajas de Facturacion a este module o realizar una consulta directa a la base aquí
                var oidCaja = Session.ExecuteScalar(string.Format("select Oid from FacCaja where NoCaja = {0} And Agencia = {1} And Activa = 1 And GCRecord is null",
                                                    Caja, Agencia.Oid));
                return oidCaja != null; 
            }
        }

        IObjectSpace IObjectSpaceLink.ObjectSpace { get; set; }


        [Browsable(false)]
        [DevExpress.Xpo.Aggregated, Association("User-LoginInfo")]
        public XPCollection<ApplicationUserLoginInfo> LoginInfo
        {
            get { return GetCollection<ApplicationUserLoginInfo>(nameof(LoginInfo)); }
        }

        IEnumerable<ISecurityUserLoginInfo> UserLogins => LoginInfo.OfType<ISecurityUserLoginInfo>();
        IEnumerable<ISecurityUserLoginInfo> IOAuthSecurityUser.UserLogins => LoginInfo.OfType<ISecurityUserLoginInfo>();

        ISecurityUserLoginInfo ISecurityUserWithLoginInfo.CreateUserLoginInfo(string loginProviderName, string providerUserKey)
        {
            ApplicationUserLoginInfo result = ((IObjectSpaceLink)this).ObjectSpace.CreateObject<ApplicationUserLoginInfo>();
            result.LoginProviderName = loginProviderName;
            result.ProviderUserKey = providerUserKey;
            result.User = this;
            return result;
        }
    }

}
